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

$testNewtonsoft = Resolve-TestDependency @('packages\Newtonsoft.Json.10.0.3\lib\net45\Newtonsoft.Json.dll', 'prjApplication\bin\Newtonsoft.Json.dll')
$testInfo = Resolve-TestDependency @('prjInfo\bin\Debug\prjInfo.dll', 'prjApplication\bin\prjInfo.dll', 'prjInfo\bin\Release\prjInfo.dll')
$testTempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$testBuildDirectory = Join-Path $testTempRoot ('atfm-notification-proxy-tests-' + [Guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $testBuildDirectory
$testExitCode = 1
try {
    Copy-Item -LiteralPath $testNewtonsoft -Destination $testBuildDirectory
    Copy-Item -LiteralPath $testInfo -Destination $testBuildDirectory
    $testExecutable = Join-Path $testBuildDirectory 'NotificationProxyTests.exe'
    $testCompilerArgs = @(
        '/nologo', '/target:exe', '/platform:anycpu', '/optimize+',
        ('/out:' + $testExecutable),
        '/reference:System.dll', '/reference:System.Core.dll', '/reference:System.Data.dll',
        '/reference:System.Configuration.dll', '/reference:System.Web.dll', '/reference:System.Xml.dll',
        ('/reference:' + $testNewtonsoft), ('/reference:' + $testInfo),
        (Join-Path $testRepoRoot 'prjApplication\Handlers\Notification.ashx.cs'),
        (Join-Path $PSScriptRoot 'test_notification_proxy.cs')
    )
    & $testCompiler @testCompilerArgs
    if ($LASTEXITCODE -ne 0) { throw 'Notification proxy test compilation failed.' }
    & $testExecutable
    $testExitCode = $LASTEXITCODE
}
finally {
    $testResolvedBuild = [IO.Path]::GetFullPath($testBuildDirectory)
    $testExpectedPrefix = $testTempRoot.TrimEnd('\') + '\atfm-notification-proxy-tests-'
    if (-not $testResolvedBuild.StartsWith($testExpectedPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Refusing to remove a build path outside the test temporary directory.'
    }
    Remove-Item -LiteralPath $testResolvedBuild -Recurse -Force
}
if ($testExitCode -ne 0) { throw 'Notification proxy regression tests failed.' }
