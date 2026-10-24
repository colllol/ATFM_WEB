$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$buildPath = Join-Path $PSScriptRoot "build"
$distPath = Join-Path $PSScriptRoot "dist"
$specPath = Join-Path $projectRoot "ATFM-ADS-B-Tools.spec"

New-Item -ItemType Directory -Force -Path $buildPath, $distPath | Out-Null
Push-Location $projectRoot
try {
    python -m PyInstaller --noconfirm --clean --workpath $buildPath --distpath $distPath $specPath
    if ($LASTEXITCODE -ne 0) {
        throw "Build ATFM-ADS-B-Tools.exe that bai (exit code $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

Write-Host "Build thanh cong: $distPath\ATFM-ADS-B-Tools.exe"
Write-Host "Dat TracksSync.local.json canh EXE de dung chung cau hinh PostgreSQL/Oracle."
Write-Host "Neu can cau hinh API rieng, dat FlightTrackingApi.local.json canh EXE."
