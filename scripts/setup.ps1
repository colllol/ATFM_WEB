param(
    [switch]$Build
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$solution = Join-Path $root 'ATFM.sln'
$msbuildCandidates = @(
    "$env:ProgramFiles\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe",
    "$env:ProgramFiles\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe",
    "$env:ProgramFiles\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe",
    "${env:ProgramFiles(x86)}\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
)

$msbuild = $msbuildCandidates | Where-Object { Test-Path $_ } | Select-Object -First 1
if (-not $msbuild) {
    throw 'Không tìm thấy MSBuild 2022. Cài Visual Studio 2022/Build Tools với workload ASP.NET and web development.'
}

$net48Release = Get-ItemPropertyValue -Path 'HKLM:\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full' -Name Release -ErrorAction SilentlyContinue
if (-not $net48Release -or $net48Release -lt 528040) {
    throw '.NET Framework 4.8 Developer Pack chưa được cài đặt.'
}

Write-Host "MSBuild: $msbuild"
Write-Host '.NET Framework 4.8: OK'

& $msbuild $solution /t:Restore /p:RestorePackagesConfig=true /v:minimal
if ($LASTEXITCODE -ne 0) { throw 'Khôi phục NuGet thất bại.' }

if ($Build) {
    & $msbuild $solution /t:Build /p:Configuration=Debug /m /v:minimal
    if ($LASTEXITCODE -ne 0) { throw 'Build thất bại. Xem mục xử lý lỗi trong README.md.' }
}

Write-Host 'Hoàn tất kiểm tra và khôi phục package.' -ForegroundColor Green

