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
$packageConfigPath = Join-Path $packagePath "FlightTrackingApi.local.json"
$stagingPath = Join-Path $buildPath "package"

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

if (Test-Path -LiteralPath $zipPath) {
    Remove-Item -LiteralPath $zipPath -Force
}
if (Test-Path -LiteralPath $stagingPath) {
    Remove-Item -LiteralPath $stagingPath -Recurse -Force
}

New-Item -ItemType Directory -Force -Path $packagePath, $stagingPath | Out-Null
Copy-Item -LiteralPath (Join-Path $distPath "ATFM-FlightTrackingApi.exe") -Destination $packagePath -Force
Copy-Item -LiteralPath (Join-Path $sourcePath "README.md") -Destination $packagePath -Force
if (-not (Test-Path -LiteralPath $packageConfigPath)) {
    Copy-Item -LiteralPath (Join-Path $sourcePath "FlightTrackingApi.example.json") -Destination $packageConfigPath
}

# ZIP phat hanh luon dung config mau; khong dong goi nham mat khau trong config local dang chay.
Copy-Item -LiteralPath (Join-Path $distPath "ATFM-FlightTrackingApi.exe") -Destination $stagingPath
Copy-Item -LiteralPath (Join-Path $sourcePath "FlightTrackingApi.example.json") `
    -Destination (Join-Path $stagingPath "FlightTrackingApi.local.json")
Copy-Item -LiteralPath (Join-Path $sourcePath "README.md") -Destination $stagingPath
Compress-Archive -Path (Join-Path $stagingPath "*") -DestinationPath $zipPath -CompressionLevel Optimal

Write-Host "Build thành công:"
Write-Host "  $distPath\ATFM-FlightTrackingApi.exe"
Write-Host "  $zipPath"
