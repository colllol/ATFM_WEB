param(
    [ValidateSet("win-x64", "linux-x64")]
    [string]$Runtime = "win-x64",
    [switch]$FrameworkDependent
)

$ErrorActionPreference = "Stop"
$projectDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path
$artifactDirectory = Join-Path $projectDirectory "artifacts"
$publishDirectory = Join-Path $artifactDirectory ("FlightTrackingApi-" + $Runtime)
$zipPath = $publishDirectory + ".zip"

if (Test-Path -LiteralPath $publishDirectory) {
    Remove-Item -LiteralPath $publishDirectory -Recurse -Force
}
if (Test-Path -LiteralPath $zipPath) {
    Remove-Item -LiteralPath $zipPath -Force
}

$selfContained = if ($FrameworkDependent) { "false" } else { "true" }
dotnet publish (Join-Path $projectDirectory "FlightTrackingApi.csproj") `
    --configuration Release `
    --runtime $Runtime `
    --self-contained $selfContained `
    --output $publishDirectory

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish thất bại với mã $LASTEXITCODE."
}

Compress-Archive -Path (Join-Path $publishDirectory "*") -DestinationPath $zipPath -CompressionLevel Optimal
Write-Host "Đã tạo gói: $zipPath"
