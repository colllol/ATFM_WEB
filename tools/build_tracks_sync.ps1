$ErrorActionPreference = "Stop"

$projectRoot = Split-Path -Parent $PSScriptRoot
$buildPath = Join-Path $PSScriptRoot "build"
$distPath = Join-Path $PSScriptRoot "dist"
$guiSpec = Join-Path $projectRoot "ATFM-TracksSync.spec"
$cliSpec = Join-Path $projectRoot "ATFM-TracksSync-CLI.spec"

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

Write-Host "Build thanh cong:"
Write-Host "  $distPath\ATFM-TracksSync.exe"
Write-Host "  $distPath\ATFM-TracksSync-CLI.exe"
