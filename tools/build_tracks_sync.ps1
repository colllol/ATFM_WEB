param(
    [switch]$InstallDependencies
)

$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$buildPath = Join-Path $PSScriptRoot "build"
$distPath = Join-Path $PSScriptRoot "dist"
$guiSpec = Join-Path $projectRoot "ATFM-TracksSync.spec"
$cliSpec = Join-Path $projectRoot "ATFM-TracksSync-CLI.spec"
$apiSourcePath = Join-Path $PSScriptRoot "FlightTrackingApiPython"
$apiSpec = Join-Path $apiSourcePath "FlightTrackingApi.spec"
$apiBuildPath = Join-Path $buildPath "FlightTrackingApi"
$packagePath = Join-Path $distPath "ATFM-TracksSync-package"
$zipPath = $packagePath + ".zip"

if ($InstallDependencies) {
    python -m pip install -r (Join-Path $PSScriptRoot "TracksSyncPython\requirements.txt")
    if ($LASTEXITCODE -ne 0) {
        throw "Cai dependency that bai (exit code $LASTEXITCODE)."
    }
}

New-Item -ItemType Directory -Force -Path $buildPath, $distPath | Out-Null

Push-Location $projectRoot
try {
    python -m PyInstaller --noconfirm --clean --workpath $buildPath --distpath $distPath $guiSpec
    if ($LASTEXITCODE -ne 0) {
        throw "Build ATFM-TracksSync.exe that bai (exit code $LASTEXITCODE)."
    }

    python -m PyInstaller --noconfirm --clean --workpath $buildPath --distpath $distPath $cliSpec
    if ($LASTEXITCODE -ne 0) {
        throw "Build ATFM-TracksSync-CLI.exe that bai (exit code $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

Push-Location $apiSourcePath
try {
    python -m PyInstaller --noconfirm --clean --workpath $apiBuildPath --distpath $distPath $apiSpec
    if ($LASTEXITCODE -ne 0) {
        throw "Build ATFM-FlightTrackingApi.exe that bai (exit code $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

if (Test-Path -LiteralPath $packagePath) {
    Remove-Item -LiteralPath $packagePath -Recurse -Force
}
if (Test-Path -LiteralPath $zipPath) {
    Remove-Item -LiteralPath $zipPath -Force
}

New-Item -ItemType Directory -Path $packagePath | Out-Null
Copy-Item -LiteralPath (Join-Path $distPath "ATFM-TracksSync.exe") -Destination $packagePath
Copy-Item -LiteralPath (Join-Path $distPath "ATFM-FlightTrackingApi.exe") -Destination $packagePath
Copy-Item -LiteralPath (Join-Path $PSScriptRoot "TracksSyncPython\TracksSync.sample.json") `
    -Destination (Join-Path $packagePath "TracksSync.local.json")
Copy-Item -LiteralPath (Join-Path $PSScriptRoot "TracksSyncPython\README.md") -Destination $packagePath
Compress-Archive -Path (Join-Path $packagePath "*") -DestinationPath $zipPath -CompressionLevel Optimal

Write-Host "Build thanh cong:"
Write-Host "  $distPath\ATFM-TracksSync.exe"
Write-Host "  $distPath\ATFM-TracksSync-CLI.exe"
Write-Host "  $distPath\ATFM-FlightTrackingApi.exe"
Write-Host "  $zipPath"
