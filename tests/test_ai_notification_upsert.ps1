param([switch] $RunLive)

# Default: compile the production service and the harness without opening Oracle.
# Opt in: powershell -ExecutionPolicy Bypass -File tests/test_ai_notification_upsert.ps1 -RunLive
# All synthetic writes are rolled back. Oracle identity/sequence numbers may have gaps.
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
$testOracle = Resolve-TestDependency @('packages\Oracle.ManagedDataAccess.12.2.1100\lib\net40\Oracle.ManagedDataAccess.dll', 'prjApplication\bin\Oracle.ManagedDataAccess.dll')
$testTempRoot = [IO.Path]::GetFullPath([IO.Path]::GetTempPath())
$testBuildDirectory = Join-Path $testTempRoot ('atfm-ai-upsert-tests-' + [Guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $testBuildDirectory
$testExitCode = 1
$testHarness = @'
using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;
using prjApplication.Handlers;

internal static class AiNotificationUpsertTests
{
    private const long Event1 = 900000000000001;
    private const long Event2 = 900000000000002;
    private const string Hashes = "('AI_QUERY:900000000000001','AI_QUERY:900000000000002')";
    private const string Recipient = "atfm-upsert-test@example.invalid";
    private static readonly MethodInfo Upsert = typeof(AiNotificationService).GetMethod("Upsert", BindingFlags.NonPublic | BindingFlags.Static);
    private static int passed;
    private static string phase = "initialization";

    private static int Main(string[] args)
    {
        try
        {
            Check(Upsert != null, "production Upsert is available by reflection");
            if (args.Length != 2 || args[0] != "--live")
            {
                Console.WriteLine("COMPILE PASS: production service compiled; live Oracle checks skipped (use -RunLive).");
                return 0;
            }
            phase = "read local SlotsOracle configuration";
            XmlDocument config = new XmlDocument { XmlResolver = null };
            config.Load(args[1]);
            XmlElement entry = config.SelectSingleNode("/configuration/connectionStrings/add[@name='SlotsOracle']") as XmlElement;
            if (entry == null || string.IsNullOrWhiteSpace(entry.GetAttribute("connectionString")))
                throw new InvalidOperationException("Missing local connection configuration.");
            // The connection string stays in memory; never print it or pass it on the command line.
            RunLive(entry.GetAttribute("connectionString"));
            Console.WriteLine("TOTAL: {0} passed; rollback verified; no notification, target, read, cursor or policy changes committed.", passed);
            return 0;
        }
        catch (Exception ex)
        {
            Exception failure = ex is TargetInvocationException && ex.InnerException != null ? ex.InnerException : ex;
            OracleException oracle = failure as OracleException;
            // Exception messages from drivers/configuration can contain connection details.
            Console.WriteLine("FAIL during {0}: {1}{2}", phase, failure.GetType().Name,
                oracle == null ? "" : " (Oracle error " + oracle.Number.ToString(CultureInfo.InvariantCulture) + ")");
            return 1;
        }
    }

    private static void Check(bool condition, string name)
    {
        phase = name;
        if (!condition) throw new InvalidOperationException("Assertion failed.");
        passed++;
        Console.WriteLine("PASS " + name);
    }

    private static OracleCommand Command(OracleConnection connection, OracleTransaction transaction, string text)
    {
        return new OracleCommand(text, connection) { BindByName = true, CommandTimeout = 15, Transaction = transaction };
    }

    private static long Number(OracleConnection connection, OracleTransaction transaction, string sql)
    {
        using (OracleCommand command = Command(connection, transaction, sql))
            return Convert.ToInt64(command.ExecuteScalar(), CultureInfo.InvariantCulture);
    }

    private static string Text(OracleConnection connection, OracleTransaction transaction, string sql)
    {
        using (OracleCommand command = Command(connection, transaction, sql))
            return Convert.ToString(command.ExecuteScalar(), CultureInfo.InvariantCulture);
    }

    private static string SyncState(OracleConnection connection, OracleTransaction transaction)
    {
        return Text(connection, transaction, "SELECT TO_CHAR(LAST_ID) || '|' || AUDIENCE_MODE || '|' || NVL(TO_CHAR(LAST_POLL_AT, 'YYYYMMDDHH24MISSFF9'), 'NULL') FROM T_AI_NOTIFICATION_SYNC WHERE SOURCE_NAME='NL2SQL'");
    }

    private static JObject Item(long id, string job)
    {
        return new JObject(new JProperty("id", id), new JProperty("job_id", job),
            new JProperty("recipient_id", "  ATFM-Upsert-Test@Example.Invalid  "),
            new JProperty("type", "query.completed"), new JProperty("title", "Synthetic rollback test"),
            new JProperty("message", "Synthetic result ready"),
            new JProperty("created_at", "2026-09-13T04:46:29.883208+00:00"),
            new JProperty("read_at", "2026-09-13T05:00:00+00:00"));
    }

    private static void Import(OracleConnection connection, OracleTransaction transaction, JObject item)
    {
        phase = "invoke production Upsert";
        Upsert.Invoke(null, new object[] { connection, transaction, item });
    }

    private static long MarkRead(OracleConnection connection, OracleTransaction transaction, long user, long notification)
    {
        using (OracleCommand command = Command(connection, transaction, "AI_NOTIFICATION_PKG.MARK_READ"))
        {
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("P_USER_ID", OracleDbType.Int64).Value = user;
            command.Parameters.Add("P_ID", OracleDbType.Int64).Value = notification;
            OracleParameter changed = command.Parameters.Add("P_UPDATED_COUNT", OracleDbType.Int64);
            changed.Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            return long.Parse(changed.Value.ToString(), CultureInfo.InvariantCulture);
        }
    }

    private static void RunLive(string connectionString)
    {
        long local1 = 0;
        long local2 = 0;
        bool ownsSyntheticIds = false;
        Exception failure = null;
        using (OracleConnection connection = new OracleConnection(connectionString))
        {
            phase = "open Oracle";
            connection.Open();
            using (OracleTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    phase = "lock sync coordinator without modifying it";
                    Number(connection, transaction, "SELECT LAST_ID FROM T_AI_NOTIFICATION_SYNC WHERE SOURCE_NAME='NL2SQL' FOR UPDATE NOWAIT");
                    string syncBefore = SyncState(connection, transaction);
                    Check(Text(connection, transaction, "SELECT AUDIENCE_MODE FROM T_AI_NOTIFICATION_SYNC WHERE SOURCE_NAME='NL2SQL'") == "ALL_USERS", "live policy is ALL_USERS (never changed by test)");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_HASH IN " + Hashes) == 0, "synthetic source IDs do not collide with existing notifications");
                    ownsSyntheticIds = true;

                    // Reconciliation is a global procedure. Refuse to invoke it while real
                    // rows are out of sync so this test changes only its uncommitted rows.
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_TYPE='AI_QUERY' AND TARGET_TYPE<>1") == 0, "existing AI notification target types need no update");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION_TARGET T JOIN T_NOTIFICATION N ON N.ID=T.NOTIFICATION_ID WHERE N.SOURCE_TYPE='AI_QUERY' AND AI_NOTIFICATION_PKG.CAN_RECEIVE(N.AI_RECIPIENT_ID,T.USER_ID)=0") == 0, "existing AI targets need no deletion");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION N CROSS JOIN (SELECT DISTINCT USERID FROM T_USERS WHERE USERACTIVE=1 AND USERID>0) U WHERE N.SOURCE_TYPE='AI_QUERY' AND AI_NOTIFICATION_PKG.CAN_RECEIVE(N.AI_RECIPIENT_ID,U.USERID)=1 AND NOT EXISTS (SELECT 1 FROM T_NOTIFICATION_TARGET T WHERE T.NOTIFICATION_ID=N.ID AND T.USER_ID=U.USERID)") == 0, "existing AI targets need no insertion");

                    long activeUsers = Number(connection, transaction, "SELECT COUNT(DISTINCT USERID) FROM T_USERS WHERE USERACTIVE=1 AND USERID>0");
                    Check(activeUsers >= 2, "at least two active users exist for read isolation checks");
                    long user1 = Number(connection, transaction, "SELECT MIN(USERID) FROM T_USERS WHERE USERACTIVE=1 AND USERID>0");
                    long user2 = Number(connection, transaction, "SELECT MIN(USERID) FROM T_USERS WHERE USERACTIVE=1 AND USERID>" + user1.ToString(CultureInfo.InvariantCulture));
                    string job = Guid.NewGuid().ToString();
                    JObject first = Item(Event1, job);
                    Import(connection, transaction, first);
                    local1 = Number(connection, transaction, "SELECT ID FROM T_NOTIFICATION WHERE SOURCE_HASH='AI_QUERY:900000000000001'");
                    first["title"] = "Synthetic replay updated";
                    Import(connection, transaction, first);
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_HASH='AI_QUERY:900000000000001'") == 1, "same event replay is idempotent");
                    Check(Number(connection, transaction, "SELECT ID FROM T_NOTIFICATION WHERE SOURCE_HASH='AI_QUERY:900000000000001'") == local1, "replay preserves the local notification ID");
                    Check(Text(connection, transaction, "SELECT TITLE FROM T_NOTIFICATION WHERE SOURCE_HASH='AI_QUERY:900000000000001'") == "Synthetic replay updated", "replay updates notification content through production MERGE");
                    Import(connection, transaction, Item(Event2, job));
                    local2 = Number(connection, transaction, "SELECT ID FROM T_NOTIFICATION WHERE SOURCE_HASH='AI_QUERY:900000000000002'");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_HASH IN " + Hashes + " AND SOURCE_KEY='" + job + "'") == 2, "two distinct events for one job are retained");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_HASH IN " + Hashes + " AND TARGET_TYPE=1 AND SOURCE_TYPE='AI_QUERY'") == 2, "both imports are targeted AI notifications");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_HASH IN " + Hashes + " AND AI_RECIPIENT_ID='" + Recipient + "'") == 2, "recipient email is trimmed and normalized");
                    string localIds = "(" + local1.ToString(CultureInfo.InvariantCulture) + "," + local2.ToString(CultureInfo.InvariantCulture) + ")";
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION_READ WHERE NOTIFICATION_ID IN " + localIds) == 0, "provider read_at creates no ATFM per-user read records");
                    phase = "reconcile only synthetic targets in the open transaction";
                    using (OracleCommand reconcile = Command(connection, transaction, "AI_NOTIFICATION_PKG.RECONCILE_TARGETS"))
                    {
                        reconcile.CommandType = CommandType.StoredProcedure;
                        reconcile.ExecuteNonQuery();
                    }
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION_TARGET WHERE NOTIFICATION_ID IN " + localIds) == activeUsers * 2, "ALL_USERS gives each synthetic event one target per active user");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION_TARGET T WHERE NOTIFICATION_ID IN " + localIds + " AND NOT EXISTS (SELECT 1 FROM T_USERS U WHERE U.USERID=T.USER_ID AND U.USERACTIVE=1 AND U.USERID>0)") == 0, "no inactive user receives a synthetic target");
                    Check(MarkRead(connection, transaction, user1, local1) == 1, "marking synthetic event read changes only the selected user");
                    Check(MarkRead(connection, transaction, user1, local1) == 0, "repeating local markRead is idempotent");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION_READ WHERE NOTIFICATION_ID=" + local1.ToString(CultureInfo.InvariantCulture) + " AND USER_ID=" + user2.ToString(CultureInfo.InvariantCulture)) == 0, "another active user still has the first event unread");
                    Check(Number(connection, transaction, "SELECT COUNT(*) FROM T_NOTIFICATION_READ WHERE NOTIFICATION_ID=" + local2.ToString(CultureInfo.InvariantCulture)) == 0, "another event for the same job stays unread");
                    Check(SyncState(connection, transaction) == syncBefore, "production upsert and reconciliation leave cursor, poll time and policy unchanged");
                }
                catch (Exception ex) { failure = ex; }
                finally
                {
                    try { transaction.Rollback(); }
                    catch (Exception ex) { phase = "rollback synthetic transaction"; failure = ex; }
                }
            }
        }

        // Use a fresh session after rollback, including when any assertion failed.
        if (ownsSyntheticIds)
        {
            string failedPhase = phase;
            using (OracleConnection verify = new OracleConnection(connectionString))
            {
                phase = "verify rollback in a fresh Oracle session";
                verify.Open();
                Check(Number(verify, null, "SELECT COUNT(*) FROM T_NOTIFICATION WHERE SOURCE_HASH IN " + Hashes) == 0, "rollback leaves zero synthetic notifications");
                string localIds = "(" + local1.ToString(CultureInfo.InvariantCulture) + "," + local2.ToString(CultureInfo.InvariantCulture) + ")";
                Check(Number(verify, null, "SELECT COUNT(*) FROM T_NOTIFICATION_TARGET WHERE NOTIFICATION_ID IN " + localIds) == 0, "rollback leaves zero synthetic targets");
                Check(Number(verify, null, "SELECT COUNT(*) FROM T_NOTIFICATION_READ WHERE NOTIFICATION_ID IN " + localIds) == 0, "rollback leaves zero synthetic read records");
            }
            if (failure != null) phase = failedPhase;
        }
        if (failure != null) throw failure;
    }
}
'@

try {
    Copy-Item -LiteralPath $testNewtonsoft -Destination $testBuildDirectory
    Copy-Item -LiteralPath $testOracle -Destination $testBuildDirectory
    $testHarnessPath = Join-Path $testBuildDirectory 'AiNotificationUpsertTests.cs'
    [IO.File]::WriteAllText($testHarnessPath, $testHarness, [Text.UTF8Encoding]::new($false))
    $testExecutable = Join-Path $testBuildDirectory 'AiNotificationUpsertTests.exe'
    $testCompilerArgs = @(
        '/nologo', '/target:exe', '/platform:anycpu', '/optimize+',
        ('/out:' + $testExecutable),
        '/reference:System.dll', '/reference:System.Core.dll', '/reference:System.Data.dll',
        '/reference:System.Configuration.dll', '/reference:System.Web.dll', '/reference:System.Xml.dll',
        ('/reference:' + $testNewtonsoft), ('/reference:' + $testOracle),
        (Join-Path $testRepoRoot 'prjApplication\Handlers\AiNotificationService.cs'), $testHarnessPath
    )
    & $testCompiler @testCompilerArgs
    if ($LASTEXITCODE -ne 0) { throw 'AI notification upsert test compilation failed.' }
    if ($RunLive) {
        & $testExecutable '--live' (Join-Path $testRepoRoot 'prjApplication\Web.config')
    } else {
        & $testExecutable
    }
    $testExitCode = $LASTEXITCODE
}
finally {
    $testResolvedBuild = [IO.Path]::GetFullPath($testBuildDirectory)
    $testExpectedPrefix = $testTempRoot.TrimEnd('\') + '\atfm-ai-upsert-tests-'
    if (-not $testResolvedBuild.StartsWith($testExpectedPrefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Refusing to remove a build path outside the test temporary directory.'
    }
    Remove-Item -LiteralPath $testResolvedBuild -Recurse -Force
}
exit $testExitCode
