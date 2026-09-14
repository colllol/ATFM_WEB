// Runs production validation and HTTP code without a database or real API key.
// Execute: powershell -ExecutionPolicy Bypass -File tests/test_ai_notification_service.ps1
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using QLB.API.Data;

internal static class AiNotificationServiceTests
{
    private static int passed;
    private static int failed;
    private const string SyntheticKey = "synthetic-test-key-not-a-credential";
    private static readonly MethodInfo GetJsonMethod = typeof(AiNotificationService).GetMethod("GetJson", BindingFlags.NonPublic | BindingFlags.Static);

    private static int Main()
    {
        TestValidation();
        TestRecipientNormalization();
        TestHttp();
        Console.WriteLine("TOTAL: {0} passed, {1} failed (no database calls)", passed, failed);
        return failed == 0 ? 0 : 1;
    }

    private static void Check(string name, Action test)
    {
        try { test(); passed++; Console.WriteLine("PASS " + name); }
        catch (Exception ex) { failed++; Console.WriteLine("FAIL " + name + ": " + ex.GetType().Name + " - " + ex.Message); }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static JObject Item(long id)
    {
        return new JObject(
            new JProperty("id", id), new JProperty("job_id", "1a786a9a-c5f3-4ae5-a6f8-c65fc773496b"),
            new JProperty("recipient_id", "user@example.com"), new JProperty("type", "query.completed"),
            new JProperty("title", "Complete"), new JProperty("message", "Result ready"), new JProperty("read_at", null),
            new JProperty("created_at", "2026-09-13T04:46:29.883208+00:00"));
    }

    private static JObject Page(long lastId, params JObject[] items)
    {
        return new JObject(new JProperty("items", new JArray(items)), new JProperty("last_id", lastId), new JProperty("unread_count", 0));
    }

    private static void Reject(string name, Action<JObject> mutate)
    {
        Check(name, delegate {
            const long cursor = 10;
            long persistedCursor = cursor;
            JObject page = Page(11, Item(11));
            mutate(page);
            bool rejected = false;
            try { persistedCursor = AiNotificationService.ValidatePage(page, cursor); }
            catch { rejected = true; }
            Assert(rejected, "Invalid page was accepted.");
            Assert(persistedCursor == cursor, "Cursor advanced on an invalid page.");
        });
    }

    private static void TestValidation()
    {
        Check("incremental pages preserve cursor and accept gaps", delegate {
            long cursor = AiNotificationService.ValidatePage(Page(12, Item(2), Item(12)), 0);
            Assert(cursor == 12, "First cursor did not advance to the last imported ID.");
            cursor = AiNotificationService.ValidatePage(Page(15, Item(15)), cursor);
            Assert(cursor == 15, "Second cursor was incorrect.");
            Assert(AiNotificationService.ValidatePage(Page(0), cursor) == 15, "Empty page reset the cursor.");
            Assert(AiNotificationService.ValidatePage(Page(15), cursor) == 15, "Empty page changed cursor.");
        });
        Check("full page contains exactly 100 validated notifications", delegate {
            List<JObject> items = new List<JObject>();
            for (int id = 1; id <= 100; id++) items.Add(Item(id));
            Assert(AiNotificationService.ValidatePage(Page(100, items.ToArray()), 0) == 100, "Full page cursor mismatch.");
        });
        Check("all supported notification types accept provider global read state", delegate {
            foreach (string type in new[] { "query.completed", "query.failed", "query.cancelled" }) {
                JObject item = Item(11); item["type"] = type; item["read_at"] = "2026-09-13T08:00:00+00:00";
                Assert(AiNotificationService.ValidatePage(Page(11, item), 10) == 11, "Supported event was dropped because read_at was set.");
            }
        });
        Reject("reject missing items", p => p.Remove("items"));
        Reject("reject non-array items", p => p["items"] = new JObject());
        Reject("reject malformed item", p => p["items"] = new JArray("text"));
        Reject("reject oversized page", p => { JArray a = new JArray(); for (int i = 11; i <= 111; i++) a.Add(Item(i)); p["items"] = a; p["last_id"] = 111; });
        Reject("reject unordered IDs without advancing cursor", p => { p["items"] = new JArray(Item(12), Item(11)); p["last_id"] = 11; });
        Reject("reject duplicate IDs without advancing cursor", p => p["items"] = new JArray(Item(11), Item(11)));
        Reject("reject replay of cursor ID", p => { p["items"] = new JArray(Item(10)); p["last_id"] = 10; });
        Reject("reject zero ID", p => p["items"][0]["id"] = 0);
        Reject("reject negative ID", p => p["items"][0]["id"] = -1);
        Reject("reject string ID", p => p["items"][0]["id"] = "11");
        Reject("reject fractional ID", p => p["items"][0]["id"] = 11.5);
        Reject("reject missing ID", p => ((JObject)p["items"][0]).Remove("id"));
        Reject("reject overflowing ID", p => p["items"][0]["id"] = JToken.Parse("9223372036854775808"));
        Reject("reject invalid job UUID", p => p["items"][0]["job_id"] = "invalid");
        Reject("reject empty job UUID", p => p["items"][0]["job_id"] = Guid.Empty.ToString());
        Reject("reject missing job UUID", p => ((JObject)p["items"][0]).Remove("job_id"));
        Reject("reject object job UUID", p => p["items"][0]["job_id"] = new JObject());
        Reject("reject missing recipient", p => ((JObject)p["items"][0]).Remove("recipient_id"));
        Reject("reject blank recipient", p => p["items"][0]["recipient_id"] = "   ");
        Reject("reject non-string recipient", p => p["items"][0]["recipient_id"] = 1);
        Reject("reject recipient beyond 320 characters", p => p["items"][0]["recipient_id"] = new string('a', 321));
        Reject("reject unsupported notification type", p => p["items"][0]["type"] = "query.running");
        Reject("reject missing notification type", p => ((JObject)p["items"][0]).Remove("type"));
        Reject("reject invalid date", p => p["items"][0]["created_at"] = "not-a-date");
        Reject("reject missing date", p => ((JObject)p["items"][0]).Remove("created_at"));
        Reject("reject last_id ahead of imported items", p => p["last_id"] = 12);
        Reject("reject last_id behind imported items", p => p["last_id"] = 10);
        Reject("reject missing last_id", p => p.Remove("last_id"));
        Reject("reject non-integer last_id", p => p["last_id"] = "11");
        Reject("reject empty page jumping cursor", p => { p["items"] = new JArray(); p["last_id"] = 11; });
        Reject("reject empty page with negative last_id", p => { p["items"] = new JArray(); p["last_id"] = -1; });
    }

    private static void TestRecipientNormalization()
    {
        Check("email recipients are trimmed and lowercased", delegate { Assert(AiNotificationService.NormalizeRecipient(" User@Example.COM ") == "user@example.com", "Email normalization differs."); });
        Check("guest recipients remain case-sensitive", delegate { Assert(AiNotificationService.NormalizeRecipient("Guest-A") != AiNotificationService.NormalizeRecipient("guest-a"), "Guest identities were collapsed."); });
        Check("guest identifiers retain exact characters", delegate { Assert(AiNotificationService.NormalizeRecipient(" Guest-A ") == " Guest-A ", "Guest identity was rewritten."); });
        Check("null recipient remains null", delegate { Assert(AiNotificationService.NormalizeRecipient(null) == null, "Null recipient changed."); });
    }

    private static JObject GetJson(string url, int limit)
    {
        try { return (JObject)GetJsonMethod.Invoke(null, new object[] { url, SyntheticKey, limit }); }
        catch (TargetInvocationException ex) { throw ex.InnerException; }
    }

    private static void ExpectStatus(Action action, int status)
    {
        try { action(); }
        catch (AiIntegrationException ex) { Assert(ex.StatusCode == status, "Expected status " + status + ", got " + ex.StatusCode); return; }
        throw new InvalidOperationException("Expected rejection with HTTP " + status);
    }

    private static void TestHttp()
    {
        Check("backend GET sends synthetic key and parses UTF-8 JSON", delegate {
            using (LoopbackServer server = new LoopbackServer(200, "{\"message\":\"K\u1ebft qu\u1ea3\"}")) {
                JObject value = GetJson(server.Url, 4096);
                Assert((string)value["message"] == "K\u1ebft qu\u1ea3", "UTF-8 content was corrupted.");
                Assert(server.Method == "GET" && server.Key == SyntheticKey && server.Accept == "application/json", "Backend request headers or method differ.");
                Assert(server.Requests == 1, "Unexpected request count.");
            }
        });
        Check("redirects are not followed or given the API key", delegate {
            using (LoopbackServer server = new LoopbackServer(302, "{}")) {
                ExpectStatus(() => GetJson(server.Url, 4096), 502);
                Assert(server.Requests == 1, "Redirect endpoint received another request.");
            }
        });
        foreach (int status in new[] { 401, 404, 409, 410, 500, 503 }) {
            int expected = status == 401 || status == 503 ? 503 : status == 500 ? 502 : status;
            Check("upstream HTTP " + status + " maps to " + expected, delegate {
                using (LoopbackServer server = new LoopbackServer(status, "{\"detail\":\"synthetic provider error\"}")) ExpectStatus(() => GetJson(server.Url, 4096), expected);
            });
        }
        foreach (string body in new[] { "", "not JSON", "[]", "{\"broken\":", "{} {}", "{} trailing" }) {
            Check("reject malformed JSON: " + (body.Length == 0 ? "empty" : body), delegate {
                using (LoopbackServer server = new LoopbackServer(200, body)) ExpectStatus(() => GetJson(server.Url, 4096), 502);
            });
        }
        Check("response at exact byte limit succeeds", delegate {
            using (LoopbackServer server = new LoopbackServer(200, "{}")) Assert(GetJson(server.Url, 2) != null, "Exact limit was rejected.");
        });
        Check("response beyond byte limit is rejected", delegate {
            using (LoopbackServer server = new LoopbackServer(200, "{}")) ExpectStatus(() => GetJson(server.Url, 1), 502);
        });
        Check("size limit counts UTF-8 bytes rather than characters", delegate {
            string body = "{\"value\":\"\u00e9\"}";
            using (LoopbackServer server = new LoopbackServer(200, body)) ExpectStatus(() => GetJson(server.Url, body.Length), 502);
        });
        Check("decompressed payload is subject to the size limit", delegate {
            using (LoopbackServer server = new LoopbackServer(200, "{\"value\":\"" + new string('a', 4096) + "\"}", true)) ExpectStatus(() => GetJson(server.Url, 1024), 502);
        });
        Check("excessive JSON nesting is rejected", delegate {
            string body = "{}";
            for (int i = 0; i < 70; i++) body = "{\"nested\":" + body + "}";
            using (LoopbackServer server = new LoopbackServer(200, body)) ExpectStatus(() => GetJson(server.Url, 4096), 502);
        });
    }

    private sealed class LoopbackServer : IDisposable
    {
        private readonly HttpListener listener;
        private readonly Task worker;
        private readonly byte[] body;
        private readonly int status;
        private readonly bool gzip;
        internal string Url { get; private set; }
        internal string Key { get; private set; }
        internal string Method { get; private set; }
        internal string Accept { get; private set; }
        internal int Requests;

        internal LoopbackServer(int responseStatus, string responseBody, bool compress = false)
        {
            status = responseStatus; gzip = compress;
            body = Encoding.UTF8.GetBytes(responseBody);
            if (compress) {
                using (MemoryStream output = new MemoryStream()) {
                    using (GZipStream stream = new GZipStream(output, CompressionMode.Compress, true)) stream.Write(body, 0, body.Length);
                    body = output.ToArray();
                }
            }
            TcpListener portProbe = new TcpListener(IPAddress.Loopback, 0);
            portProbe.Start();
            int port = ((IPEndPoint)portProbe.LocalEndpoint).Port;
            portProbe.Stop();
            Url = "http://127.0.0.1:" + port + "/";
            listener = new HttpListener(); listener.Prefixes.Add(Url); listener.Start();
            worker = Task.Run((Action)Serve);
        }

        private void Serve()
        {
            try {
                while (listener.IsListening) {
                    HttpListenerContext context = listener.GetContext();
                    Interlocked.Increment(ref Requests);
                    Key = context.Request.Headers["X-API-Key"];
                    Method = context.Request.HttpMethod;
                    Accept = context.Request.Headers["Accept"];
                    context.Response.StatusCode = status;
                    context.Response.ContentType = "application/json; charset=utf-8";
                    if (gzip) context.Response.Headers["Content-Encoding"] = "gzip";
                    if (status == 302) context.Response.RedirectLocation = Url + "redirect-target";
                    context.Response.ContentLength64 = body.Length;
                    context.Response.OutputStream.Write(body, 0, body.Length);
                    context.Response.Close();
                }
            }
            catch (HttpListenerException) { }
            catch (ObjectDisposedException) { }
            catch (IOException) { }
        }

        public void Dispose()
        {
            listener.Close();
            Assert(worker.Wait(3000), "Loopback listener did not stop.");
        }
    }
}
