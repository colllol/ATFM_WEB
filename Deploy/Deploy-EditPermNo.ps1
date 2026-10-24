[CmdletBinding(SupportsShouldProcess = $true)]
param
(
    [Parameter(Mandatory = $true)]
    [string]$TargetPath,

    [string]$PublishOutput,

    [string]$BackupRoot,

    [string]$MsBuildPath,

    [switch]$SkipPublish,

    [switch]$AllowDirtyWorktree,

    [switch]$NoAppOffline
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Examples:
#   powershell -ExecutionPolicy Bypass -File .\Deploy\Deploy-EditPermNo.ps1 `
#       -TargetPath '\\SERVER\D$\WebSites\ATFM_WEB'
#
#   powershell -ExecutionPolicy Bypass -File .\Deploy\Deploy-EditPermNo.ps1 `
#       -TargetPath 'D:\WebSites\ATFM_WEB' -SkipPublish
#
# Use -AllowDirtyWorktree only when other pending source changes are intended
# to be compiled into prjApplication.dll as part of this deployment.
#
# The script publishes Release unless -SkipPublish is specified. It deploys only:
#   Permission\Edit_PermNo.aspx
#   Permission\Edit_PermNO4Mail.aspx
#   bin\prjApplication.dll
# Existing target files are backed up and restored automatically on failure.

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$projectPath = Join-Path $repoRoot 'prjApplication\prjApplication.csproj'

if ([string]::IsNullOrWhiteSpace($PublishOutput)) {
    $PublishOutput = Join-Path $repoRoot 'prjApplication\bin\Release\PublishOutput'
}
$PublishOutput = [System.IO.Path]::GetFullPath($PublishOutput)
$TargetPath = [System.IO.Path]::GetFullPath($TargetPath)

if ([string]::IsNullOrWhiteSpace($BackupRoot)) {
    $targetParent = Split-Path -Parent $TargetPath
    $BackupRoot = Join-Path $targetParent 'ATFM_WEB_Backups'
}
$BackupRoot = [System.IO.Path]::GetFullPath($BackupRoot)

function Resolve-MsBuild
{
    param([string]$RequestedPath)

    if (-not [string]::IsNullOrWhiteSpace($RequestedPath)) {
        if (-not (Test-Path -LiteralPath $RequestedPath -PathType Leaf)) {
            throw "MSBuild not found: $RequestedPath"
        }
        return [System.IO.Path]::GetFullPath($RequestedPath)
    }

    $candidates = @(
        'C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\MSBuild.exe',
        'C:\Program Files (x86)\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe'
    )

    foreach ($candidate in $candidates) {
        if (Test-Path -LiteralPath $candidate -PathType Leaf) {
            return $candidate
        }
    }

    $command = Get-Command msbuild.exe -ErrorAction SilentlyContinue
    if ($null -ne $command) {
        return $command.Source
    }

    throw 'MSBuild not found. Specify -MsBuildPath.'
}

if (-not $SkipPublish) {
    if (-not $AllowDirtyWorktree -and (Test-Path -LiteralPath (Join-Path $repoRoot '.git'))) {
        $gitCommand = Get-Command git.exe -ErrorAction SilentlyContinue
        if ($null -ne $gitCommand) {
            $allowedDirtyPaths = @(
                'NHAT_KY_THAY_DOI.txt',
                'prjApplication/Permission/Edit_PermNo.aspx',
                'prjApplication/Permission/Edit_PermNo.aspx.cs',
                'prjApplication/Permission/Edit_PermNO4Mail.aspx',
                'Deploy/Deploy-EditPermNo.ps1',
                'Deploy/Rollback-EditPermNo.ps1'
            )
            $unexpectedDirtyPaths = New-Object System.Collections.Generic.List[string]
            $gitStatus = @(& $gitCommand.Source -C $repoRoot status --porcelain --untracked-files=all)
            if ($LASTEXITCODE -ne 0) {
                throw 'Unable to inspect Git worktree before publishing.'
            }

            foreach ($statusLine in $gitStatus) {
                if ([string]::IsNullOrWhiteSpace($statusLine) -or $statusLine.Length -lt 4) {
                    continue
                }

                $dirtyPath = $statusLine.Substring(3).Trim('"').Replace('\', '/')
                if ($dirtyPath -notin $allowedDirtyPaths) {
                    $unexpectedDirtyPaths.Add($dirtyPath)
                }
            }

            if ($unexpectedDirtyPaths.Count -gt 0) {
                $messageLines = New-Object System.Collections.Generic.List[string]
                $messageLines.Add(
                    'Publishing was stopped because unrelated pending changes would be compiled into prjApplication.dll:')
                foreach ($unexpectedPath in $unexpectedDirtyPaths) {
                    $messageLines.Add("  - $unexpectedPath")
                }
                $messageLines.Add(
                    'Commit/stash those files, or explicitly use -AllowDirtyWorktree.')
                throw ($messageLines -join [Environment]::NewLine)
            }
        }
    }

    $resolvedMsBuild = Resolve-MsBuild -RequestedPath $MsBuildPath
    Write-Host "Publishing Release with: $resolvedMsBuild" -ForegroundColor Cyan

    & $resolvedMsBuild `
        $projectPath `
        '/t:Build' `
        '/p:Configuration=Release' `
        '/p:DeployOnBuild=true' `
        '/p:PublishProfile=FolderProfile' `
        '/m' `
        '/v:minimal'

    if ($LASTEXITCODE -ne 0) {
        throw "Publish failed. MSBuild exit code: $LASTEXITCODE"
    }
}

if (-not (Test-Path -LiteralPath $PublishOutput -PathType Container)) {
    throw "PublishOutput directory not found: $PublishOutput"
}
if (-not (Test-Path -LiteralPath $TargetPath -PathType Container)) {
    throw "Target website directory not found: $TargetPath"
}
if ($TargetPath.TrimEnd('\') -eq $PublishOutput.TrimEnd('\')) {
    throw 'TargetPath must be different from PublishOutput.'
}

$relativeFiles = @(
    'Permission\Edit_PermNo.aspx',
    'Permission\Edit_PermNO4Mail.aspx',
    'bin\prjApplication.dll'
)

foreach ($relativePath in $relativeFiles) {
    $sourcePath = Join-Path $PublishOutput $relativePath
    if (-not (Test-Path -LiteralPath $sourcePath -PathType Leaf)) {
        throw "Required file is missing from PublishOutput: $relativePath"
    }
}

$timestamp = Get-Date -Format 'yyyyMMdd_HHmmss'
$backupPath = Join-Path $BackupRoot "EditPermNo_$timestamp"
$offlinePath = Join-Path $TargetPath 'app_offline.htm'
$offlineCreated = $false
$fileStates = New-Object System.Collections.Generic.List[object]

if (-not $PSCmdlet.ShouldProcess($TargetPath, 'Deploy Edit_PermNo and Edit_PermNO4Mail')) {
    Write-Host 'WhatIf completed; the website was not changed.' -ForegroundColor Yellow
    return
}

New-Item -ItemType Directory -Path $backupPath -Force | Out-Null

try {
    if (-not $NoAppOffline) {
        if (Test-Path -LiteralPath $offlinePath) {
            throw "app_offline.htm already exists. Check another deployment: $offlinePath"
        }

        @'
<!DOCTYPE html>
<html><head><meta charset="utf-8"><title>ATFM maintenance</title></head>
<body><h3>ATFM_WEB is being updated. Please try again shortly.</h3></body></html>
'@ | Set-Content -LiteralPath $offlinePath -Encoding UTF8
        $offlineCreated = $true
    }

    foreach ($relativePath in $relativeFiles) {
        $sourcePath = Join-Path $PublishOutput $relativePath
        $targetFile = Join-Path $TargetPath $relativePath
        $backupFile = Join-Path $backupPath $relativePath
        $targetExisted = Test-Path -LiteralPath $targetFile -PathType Leaf

        $state = [pscustomobject]@{
            RelativePath = $relativePath
            ExistedBefore = $targetExisted
            SourceSha256 = (Get-FileHash -LiteralPath $sourcePath -Algorithm SHA256).Hash
        }
        $fileStates.Add($state)

        if ($targetExisted) {
            New-Item -ItemType Directory -Path (Split-Path -Parent $backupFile) -Force | Out-Null
            Copy-Item -LiteralPath $targetFile -Destination $backupFile -Force
        }

        New-Item -ItemType Directory -Path (Split-Path -Parent $targetFile) -Force | Out-Null
        Copy-Item -LiteralPath $sourcePath -Destination $targetFile -Force

        $targetHash = (Get-FileHash -LiteralPath $targetFile -Algorithm SHA256).Hash
        if ($targetHash -ne $state.SourceSha256) {
            throw "Checksum mismatch after deployment: $relativePath"
        }

        Write-Host "OK  $relativePath" -ForegroundColor Green
    }

    $manifest = [pscustomobject]@{
        CreatedAt = (Get-Date).ToString('o')
        TargetPath = $TargetPath
        PublishOutput = $PublishOutput
        Files = $fileStates
        OracleScript = 'Database\Oracle\20260811_PERM_NO_APPROVAL_PKG.sql'
    }
    $manifest | ConvertTo-Json -Depth 5 |
        Set-Content -LiteralPath (Join-Path $backupPath 'manifest.json') -Encoding UTF8

    Write-Host ''
    Write-Host 'Web deployment completed successfully.' -ForegroundColor Green
    Write-Host "Backup: $backupPath"
    Write-Host 'If Accepted PermNo is not installed, also run:'
    Write-Host '  Database\Oracle\20260811_PERM_NO_APPROVAL_PKG.sql' -ForegroundColor Yellow
}
catch {
    Write-Warning "Deployment failed; starting automatic rollback: $($_.Exception.Message)"

    foreach ($state in $fileStates) {
        $targetFile = Join-Path $TargetPath $state.RelativePath
        $backupFile = Join-Path $backupPath $state.RelativePath

        if ($state.ExistedBefore -and (Test-Path -LiteralPath $backupFile -PathType Leaf)) {
            Copy-Item -LiteralPath $backupFile -Destination $targetFile -Force
        }
        elseif (-not $state.ExistedBefore -and (Test-Path -LiteralPath $targetFile -PathType Leaf)) {
            Remove-Item -LiteralPath $targetFile -Force
        }
    }

    throw
}
finally {
    if ($offlineCreated -and (Test-Path -LiteralPath $offlinePath -PathType Leaf)) {
        Remove-Item -LiteralPath $offlinePath -Force
    }
}
