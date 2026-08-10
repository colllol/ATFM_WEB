param(
    [switch]$InstallDependencies
)

$ErrorActionPreference = "Stop"
$projectRoot = Split-Path -Parent $PSScriptRoot
$sourcePath = Join-Path $PSScriptRoot "FlightTrackingApiPython"
$buildPath = Join-Path $PSScriptRoot "build\FlightTrackingApi"
$distPath = Join-Path $PSScriptRoot "dist"
$packagePath = Join-Path $distPath "ATFM-FlightTrackingApi-package"
$zipPath = $packagePath + ".zip"
$specPath = Join-Path $sourcePath "FlightTrackingApi.spec"

if ($InstallDependencies) {
    python -m pip install -r (Join-Path $sourcePath "requirements.txt")
    if ($LASTEXITCODE -ne 0) {
        throw "Cài dependency thất bại (exit code $LASTEXITCODE)."
    }
}

New-Item -ItemType Directory -Force -Path $buildPath, $distPath | Out-Null
Push-Location $sourcePath
try {
    python -m PyInstaller --noconfirm --clean --workpath $buildPath --distpath $distPath $specPath
    if ($LASTEXITCODE -ne 0) {
        throw "Build ATFM-FlightTrackingApi.exe thất bại (exit code $LASTEXITCODE)."
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
Copy-Item -LiteralPath (Join-Path $distPath "ATFM-FlightTrackingApi.exe") -Destination $packagePath
Copy-Item -LiteralPath (Join-Path $sourcePath "FlightTrackingApi.example.json") `
    -Destination (Join-Path $packagePath "FlightTrackingApi.local.json")
Copy-Item -LiteralPath (Join-Path $sourcePath "README.md") -Destination $packagePath
Compress-Archive -Path (Join-Path $packagePath "*") -DestinationPath $zipPath -CompressionLevel Optimal

Write-Host "Build thành công:"
Write-Host "  $distPath\ATFM-FlightTrackingApi.exe"
Write-Host "  $zipPath"
