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
$dotenvPath = Join-Path $publishDirectory ".env"

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
if (-not (Test-Path -LiteralPath $dotenvPath)) {
    throw "Thiếu tệp .env sau khi publish. Kiểm tra target CopyDotEnvToPublishOutput trong FlightTrackingApi.csproj."
}

Add-Type -AssemblyName System.IO.Compression.FileSystem
for ($attempt = 1; $attempt -le 3; $attempt++) {
    try {
        [System.IO.Compression.ZipFile]::CreateFromDirectory(
            $publishDirectory,
            $zipPath,
            [System.IO.Compression.CompressionLevel]::Optimal,
            $false)
        break
    }
    catch [System.IO.IOException] {
        if ($attempt -eq 3) {
            throw
        }

        # Antivirus can briefly lock newly published runtime files on Windows.
        if (Test-Path -LiteralPath $zipPath) {
            Remove-Item -LiteralPath $zipPath -Force
        }
        Start-Sleep -Seconds 2
    }
}
Write-Host "Đã tạo gói: $zipPath"
Write-Host "Tệp cấu hình: $dotenvPath"
