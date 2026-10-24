[CmdletBinding(SupportsShouldProcess = $true)]
param
(
    [Parameter(Mandatory = $true)]
    [string]$BackupPath,

    [string]$TargetPath,

    [switch]$NoAppOffline
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Example:
#   powershell -ExecutionPolicy Bypass -File .\Deploy\Rollback-EditPermNo.ps1 `
#       -BackupPath 'D:\WebSites\ATFM_WEB_Backups\EditPermNo_20260811_150000'

$BackupPath = [System.IO.Path]::GetFullPath($BackupPath)
$manifestPath = Join-Path $BackupPath 'manifest.json'

if (-not (Test-Path -LiteralPath $manifestPath -PathType Leaf)) {
    throw "Rollback manifest not found: $manifestPath"
}

$manifest = Get-Content -LiteralPath $manifestPath -Raw | ConvertFrom-Json
if ([string]::IsNullOrWhiteSpace($TargetPath)) {
    $TargetPath = [string]$manifest.TargetPath
}
$TargetPath = [System.IO.Path]::GetFullPath($TargetPath)

if (-not (Test-Path -LiteralPath $TargetPath -PathType Container)) {
    throw "Target website directory not found: $TargetPath"
}

$offlinePath = Join-Path $TargetPath 'app_offline.htm'
$offlineCreated = $false

if (-not $PSCmdlet.ShouldProcess($TargetPath, "Rollback from $BackupPath")) {
    Write-Host 'WhatIf completed; the website was not changed.' -ForegroundColor Yellow
    return
}

try {
    if (-not $NoAppOffline) {
        if (Test-Path -LiteralPath $offlinePath) {
            throw "app_offline.htm already exists: $offlinePath"
        }

        'ATFM_WEB rollback is in progress. Please try again shortly.' |
            Set-Content -LiteralPath $offlinePath -Encoding UTF8
        $offlineCreated = $true
    }

    foreach ($file in $manifest.Files) {
        $targetFile = Join-Path $TargetPath ([string]$file.RelativePath)
        $backupFile = Join-Path $BackupPath ([string]$file.RelativePath)

        if ([bool]$file.ExistedBefore) {
            if (-not (Test-Path -LiteralPath $backupFile -PathType Leaf)) {
                throw "Backup file is missing: $backupFile"
            }

            New-Item -ItemType Directory -Path (Split-Path -Parent $targetFile) -Force | Out-Null
            Copy-Item -LiteralPath $backupFile -Destination $targetFile -Force
            Write-Host "RESTORE  $($file.RelativePath)" -ForegroundColor Green
        }
        elseif (Test-Path -LiteralPath $targetFile -PathType Leaf) {
            Remove-Item -LiteralPath $targetFile -Force
            Write-Host "REMOVE   $($file.RelativePath)" -ForegroundColor Green
        }
    }

    Write-Host "Rollback completed successfully from: $BackupPath" -ForegroundColor Green
}
finally {
    if ($offlineCreated -and (Test-Path -LiteralPath $offlinePath -PathType Leaf)) {
        Remove-Item -LiteralPath $offlinePath -Force
    }
}
