param()

$ErrorActionPreference = 'Stop'
$testRepoRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..')).Path
$testCompiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
if (-not (Test-Path -LiteralPath $testCompiler)) {
    $testCompiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe'
}
if (-not (Test-Path -LiteralPath $testCompiler)) { throw 'The .NET Framework C# compiler was not found.' }

function Resolve-TestDependency([string[]] $Candidates) {
    foreach ($candidate in $Candidates) {
        $dependency = Join-Path $testRepoRoot $candidate
        if (Test-Path -LiteralPath $dependency) { return $dependency }
    }
    throw ('Missing test dependency: ' + ($Candidates -join ', '))
}

$testNewtonsoft = Resolve-TestDependency @('packages\Newtonsoft.Json.10.0.3\lib\net45\Newtonsoft.Json.dll', 'prjApplication\bin\Newtonsoft.Json.dll', 'Lib\Newtonsoft.Json.dll')
$testOracle = Resolve-TestDependency @('packages\Oracle.ManagedDataAccess.12.2.1100\lib\net40\Oracle.ManagedDataAccess.dll', 'prjApplication\bin\Oracle.ManagedDataAccess.dll')
$testTempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$testBuildDirectory = Join-Path $testTempRoot ('atfm-ai-notification-tests-' + [Guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $testBuildDirectory
$testExitCode = 1
try {
    Copy-Item -LiteralPath $testNewtonsoft -Destination $testBuildDirectory
    Copy-Item -LiteralPath $testOracle -Destination $testBuildDirectory
    $testExecutable = Join-Path $testBuildDirectory 'AiNotificationServiceTests.exe'
    $testCompilerArgs = @(
        '/nologo', '/target:exe', '/platform:anycpu', '/optimize+',
        ('/out:' + $testExecutable),
        '/reference:System.dll', '/reference:System.Core.dll', '/reference:System.Data.dll',
        '/reference:System.Configuration.dll', '/reference:System.Web.dll',
        ('/reference:' + $testNewtonsoft), ('/reference:' + $testOracle),
        (Join-Path $testRepoRoot 'prjApplication\Handlers\AiNotificationService.cs'),
        (Join-Path $PSScriptRoot 'test_ai_notification_service.cs')
    )
    & $testCompiler @testCompilerArgs
    if ($LASTEXITCODE -ne 0) { throw 'AI notification test compilation failed.' }
    & $testExecutable
    $testExitCode = $LASTEXITCODE
}
finally {
    # Only remove this runner's verified temporary build directory, using one shell.
    $testResolvedBuild = [IO.Path]::GetFullPath($testBuildDirectory)
    $testExpectedPrefix = $testTempRoot.TrimEnd('\') + '\atfm-ai-notification-tests-'
    if (-not $testResolvedBuild.StartsWith($testExpectedPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Refusing to remove a build path outside the test temporary directory.'
    }
    Remove-Item -LiteralPath $testResolvedBuild -Recurse -Force
}
exit $testExitCode
