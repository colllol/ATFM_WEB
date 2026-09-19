$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$buildPath = Join-Path $PSScriptRoot "build"
$distPath = Join-Path $PSScriptRoot "dist"
$specPath = Join-Path $projectRoot "ATFM-Unified-Tools.spec"

New-Item -ItemType Directory -Force -Path $buildPath, $distPath | Out-Null

Push-Location $projectRoot
try {
    python -m PyInstaller --noconfirm --clean --workpath $buildPath --distpath $distPath $specPath
    if ($LASTEXITCODE -ne 0) {
        throw "Build ATFM-Data-Tools.exe that bai (exit code $LASTEXITCODE)."
    }
}
finally {
    Pop-Location
}

Write-Host "Build thanh cong: $distPath\ATFM-Data-Tools.exe"
