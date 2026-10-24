// Exercises the production ASP.NET handler with synthetic sessions and loopback HTTP.
// Run tests/test_notification_proxy.ps1; no real backend, database or keys are used.
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;
using Newtonsoft.Json.Linq;
using prjApplication.Handlers;
using prjInfo;

internal static class NotificationProxyTests
{
    private const string BackendKey = "synthetic-backend-key-not-a-credential";
    private const string BackendSecret = "synthetic-private-database-details";
    private const string Success = "{\"Code\":\"00\",\"Value\":2,\"AIUnreadCount\":1,\"ListValue\":[]}";
    private static int passed;
    private static int failed;

    private static int Main()
    {
        using (Backend backend = new Backend())
        {
            Configure(backend.Url, BackendKey);
            TestAuthentication(backend);
            TestRoutes(backend);
            TestAiCompatibility(backend);
            TestValidation(backend);
            TestBackendFailures(backend);
            TestConfiguration(backend);
        }
        Console.WriteLine("TOTAL: {0} passed, {1} failed; synthetic loopback only, no database or live configuration", passed, failed);
        return failed == 0 ? 0 : 1;
    }

    private static void Check(string name, Action action)
    {
        try { action(); passed++; Console.WriteLine("PASS " + name); }
        catch (Exception ex) { failed++; Console.WriteLine("FAIL " + name + ": " + ex.GetType().Name + " - " + ex.Message); }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    private static void Configure(string address, string key)
    {
        // The runner's executable and configuration live in its unique temporary directory.
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings.Remove("ApplicationPath.API");
        config.AppSettings.Settings.Remove("APIKey");
        config.AppSettings.Settings.Add("ApplicationPath.API", address);
        config.AppSettings.Settings.Add("APIKey", key);
        config.Save(ConfigurationSaveMode.Modified);
        ConfigurationManager.RefreshSection("appSettings");
    }

    private sealed class Result
    {
        internal int Status;
        internal string Body;
        internal bool SuppressRedirect;
        internal string ContentType;
    }

    private static Result Invoke(string query, string method = "GET", string xhr = "XMLHttpRequest",
        bool authenticated = true, bool session = true, int userId = 42,
        string sessionName = "atfm-test-user", string identityName = "atfm-test-user", string body = "")
    {
        using (StringWriter output = new StringWriter())
        {
            Request worker = new Request(query, method, xhr, body, output);
            HttpContext context = new HttpContext(worker);
            context.User = new GenericPrincipal(new GenericIdentity(authenticated ? identityName : "", authenticated ? "SyntheticTest" : ""), new string[0]);
            if (session)
            {
                SessionStateItemCollection items = new SessionStateItemCollection();
                items["ATFM_CURRENT_USER"] = new T_Users { UserID = userId, UserName = sessionName };
                HttpSessionStateContainer state = new HttpSessionStateContainer("synthetic-session", items,
                    new HttpStaticObjectsCollection(), 20, true, HttpCookieMode.UseCookies, SessionStateMode.InProc, true);
                SessionStateUtility.AddHttpSessionStateToContext(context, state);
            }
            new NotificationHandler().ProcessRequest(context);
            Result result = new Result { Status = context.Response.StatusCode,
                SuppressRedirect = context.Response.SuppressFormsAuthenticationRedirect,
                ContentType = context.Response.ContentType };
            context.Response.Flush();
            result.Body = output.ToString();
            Assert(!result.Body.Contains(BackendKey), "Browser response exposed the backend API key.");
            Assert(!result.Body.Contains(BackendSecret), "Browser response exposed backend error details.");
            Assert(result.ContentType == "application/json", "Response is not JSON.");
            Assert(result.SuppressRedirect, "Authentication errors may redirect to HTML login.");
            return result;
        }
    }

    private static void Reject(Backend backend, string name, string query, int status = 400,
        string method = "GET", string xhr = "XMLHttpRequest")
    {
        Check(name, delegate {
            int before = backend.Requests;
            Result result = Invoke(query, method, xhr);
            Assert(result.Status == status, "Unexpected local rejection status: " + result.Status);
            if (method == "HEAD") Assert(result.Body.Length == 0, "ASP.NET must omit the HEAD response body.");
            else Assert((string)JObject.Parse(result.Body)["Code"] == "-99", "Local rejection lacks error envelope.");
            Assert(backend.Requests == before, "Invalid browser request reached the backend.");
        });
    }

    private static void TestAuthentication(Backend backend)
    {
        Check("anonymous request returns 401 without backend traffic", delegate {
            int before = backend.Requests;
            Assert(Invoke("action=state", authenticated: false).Status == 401, "Anonymous access accepted.");
            Assert(backend.Requests == before, "Anonymous request forwarded.");
        });
        Check("missing session returns 401 without backend traffic", delegate {
            int before = backend.Requests;
            Assert(Invoke("action=state", session: false).Status == 401, "Missing session accepted.");
            Assert(backend.Requests == before, "Missing session forwarded.");
        });
        Check("invalid session user ID returns 401 without backend traffic", delegate {
            int before = backend.Requests;
            Assert(Invoke("action=state", userId: 0).Status == 401, "Zero session ID accepted.");
            Assert(backend.Requests == before, "Invalid session forwarded.");
        });
        Check("identity and session mismatch returns 401 without backend traffic", delegate {
            int before = backend.Requests;
            Assert(Invoke("action=state", identityName: "another-user").Status == 401, "Mismatched identity accepted.");
            Assert(backend.Requests == before, "Mismatched identity forwarded.");
        });
    }

    private static void Route(Backend backend, string name, string query, string method,
        string path, string expectedQuery, string body = "")
    {
        Check(name, delegate {
            backend.Reply(200, Success);
            int before = backend.Requests;
            Result result = Invoke(query, method, body: body);
            Assert(result.Status == 200 && (string)JObject.Parse(result.Body)["Code"] == "00", "Successful backend envelope changed.");
            int expectedRequests = path == "MarkAllRead" && expectedQuery == "&source=AI_QUERY" ? 2 : 1;
            Assert(backend.Requests == before + expectedRequests, "Wrong backend request count.");
            Assert(backend.LastPath == "/api/Notifications/" + path, "Wrong API operation: " + backend.LastPath);
            Assert(backend.LastQuery == "?userId=42" + expectedQuery, "Forwarded query differs: " + backend.LastQuery);
            Assert(backend.LastMethod == method, "Wrong backend method.");
            Assert(backend.LastKey == BackendKey && backend.LastAccept == "application/json", "Server headers omitted backend key or JSON accept.");
            Assert(backend.LastBody == "", "Browser body was forwarded to the backend.");
        });
    }

    private static void TestAiCompatibility(Backend backend)
    {
        const string legacy = "{\"Code\":\"00\",\"Value\":356,\"SumRecord\":\"3789\",\"ListValue\":[{\"ID\":1,\"SOURCE_TYPE\":\"email\"}]}";
        Check("legacy general notifications remain available", delegate {
            backend.Reply(200, legacy);
            Assert(Invoke("action=list&status=-1&page=1&source=ALL").Status == 200, "General inbox was blocked.");
        });
        Check("legacy API cannot show general rows in the AI inbox", delegate {
            backend.Reply(200, legacy);
            Result result = Invoke("action=list&status=-1&page=1&source=AI_QUERY");
            Assert(result.Status == 503, "Legacy unfiltered response was accepted.");
            JObject body = JObject.Parse(result.Body);
            Assert((string)body["Code"] == "-99" && body["ListValue"] == null, "General rows leaked into AI inbox.");
        });
        Check("empty legacy response does not claim AI support", delegate {
            backend.Reply(200, "{\"Code\":\"00\",\"Value\":0,\"ListValue\":[]}");
            Assert(Invoke("action=list&status=-1&page=1&source=AI_QUERY").Status == 503, "Missing AI contract accepted.");
        });
        Check("AI counter alone cannot hide mixed-source rows", delegate {
            backend.Reply(200, legacy.Replace("\"Value\":356", "\"Value\":356,\"AIUnreadCount\":2"));
            Assert(Invoke("action=list&status=-1&page=1&source=AI_QUERY").Status == 503, "Mixed-source AI page accepted.");
        });
        Check("supported empty AI inbox remains successful", delegate {
            backend.Reply(200, Success);
            Assert(Invoke("action=list&status=-1&page=1&source=AI_QUERY").Status == 200, "Valid empty AI page rejected.");
        });
        Check("AI counter alias and actual AI row are accepted", delegate {
            backend.Reply(200, "{\"Code\":\"00\",\"AI_UNREAD_COUNT\":1,\"ListValue\":[{\"ID\":2,\"SOURCE_TYPE\":\"AI\"}]}");
            Result result = Invoke("action=list&status=-1&page=1&source=AI_QUERY");
            Assert(result.Status == 200 && ((JArray)JObject.Parse(result.Body)["ListValue"]).Count == 1, "Valid AI row rejected.");
        });
        Check("AI mark-all never posts to an API that ignores source", delegate {
            backend.Reply(200, legacy);
            int before = backend.Requests;
            Assert(Invoke("action=markAllRead&source=AI_QUERY", "POST").Status == 503, "Legacy AI mark-all accepted.");
            Assert(backend.Requests == before + 1 && backend.LastMethod == "GET"
                && backend.LastPath == "/api/Notifications/GetPage", "Unsafe POST reached legacy API.");
        });
        Check("failed AI capability check prevents mark-all mutation", delegate {
            backend.Reply(503, "{}");
            int before = backend.Requests;
            Assert(Invoke("action=markAllRead&source=AI_QUERY", "POST").Status == 503, "Failed preflight accepted.");
            Assert(backend.Requests == before + 1 && backend.LastMethod == "GET", "POST followed failed preflight.");
        });
    }

    private static void TestRoutes(Backend backend)
    {
        Route(backend, "GET state forwards session ID and general plus AI counters", "action=state&userId=999", "GET", "GetState", "");
        Route(backend, "GET list forwards validated AI filters and page", "action=list&status=0&page=2&source=ai_query&userId=999", "GET", "GetPage", "&status=0&page=2&source=AI_QUERY");
        Route(backend, "GET aiJob uses local notification ID", "action=aiJob&id=123&userId=999", "GET", "GetAiJob", "&id=123");
        Route(backend, "GET aiResult uses local notification ID", "action=aiResult&id=123", "GET", "GetAiResult", "&id=123");
        Route(backend, "POST markRead ignores spoofed form user ID", "action=markRead&id=123", "POST", "MarkRead", "&id=123", "userId=999&APIKey=attacker-key");
        Route(backend, "POST markAllRead forwards AI source", "action=markAllRead&source=AI_QUERY&userId=999", "POST", "MarkAllRead", "&source=AI_QUERY");
        Route(backend, "omitted action defaults to state", "", "GET", "GetState", "");
        Route(backend, "list accepts all-status filter and first page", "action=list&status=-1&page=1", "GET", "GetPage", "&status=-1&page=1&source=ALL");
        Route(backend, "list accepts read filter and maximum page", "action=list&status=1&page=1000000", "GET", "GetPage", "&status=1&page=1000000&source=ALL");
        Route(backend, "POST markAllRead defaults to all sources", "action=markAllRead", "POST", "MarkAllRead", "&source=ALL");
        Check("case insensitive authenticated identity matches session", delegate {
            backend.Reply(200, Success);
            Assert(Invoke("action=STATE", identityName: "ATFM-TEST-USER").Status == 200, "Case insensitive name/action was rejected.");
        });
    }

    private static void TestValidation(Backend backend)
    {
        Reject(backend, "POST requires XMLHttpRequest header", "action=markAllRead", 403, "POST", null);
        Reject(backend, "POST rejects a different XMLHttpRequest header value", "action=markRead&id=1", 403, "POST", "xmlhttprequest");
        foreach (string method in new[] { "PUT", "DELETE", "PATCH", "HEAD" })
            Reject(backend, method + " cannot access notification proxy", "action=state", 405, method);
        foreach (string action in new[] { "markRead&id=1", "markAllRead", "unknown" })
            Reject(backend, "GET cannot perform " + action, "action=" + action);
        foreach (string action in new[] { "state", "list&status=-1&page=1", "aiJob&id=1", "aiResult&id=1", "unknown" })
            Reject(backend, "POST cannot perform " + action, "action=" + action, 400, "POST");
        foreach (string source in new[] { "OTHER", "", "AI_QUERY%26userId%3D999", "AI_QUERY,ALL" })
            Reject(backend, "invalid source is rejected: " + source, "action=state&source=" + source);
        foreach (string status in new[] { "-2", "2", "text", "", "2147483648" })
            Reject(backend, "invalid list status is rejected: " + status, "action=list&page=1&status=" + status);
        Reject(backend, "missing list status is rejected", "action=list&page=1");
        foreach (string page in new[] { "0", "-1", "1000001", "text", "", "2147483648" })
            Reject(backend, "invalid list page is rejected: " + page, "action=list&status=-1&page=" + page);
        Reject(backend, "missing list page is rejected", "action=list&status=-1");
        foreach (string id in new[] { "0", "-1", "text", "", "9223372036854775808", "1%26userId%3D999" })
        {
            Reject(backend, "invalid aiJob notification ID is rejected: " + id, "action=aiJob&id=" + id);
            Reject(backend, "invalid markRead notification ID is rejected: " + id, "action=markRead&id=" + id, 400, "POST");
        }
        Reject(backend, "missing aiResult notification ID is rejected", "action=aiResult");
    }

    private static void TestBackendFailures(Backend backend)
    {
        foreach (int code in new[] { 401, 403, 404, 409, 410, 500, 503, 302 })
        {
            int status = code;
            int expected = code == 401 || code == 503 ? 503 : code == 500 || code == 302 ? 502 : code;
            Check("backend HTTP " + status + " becomes " + expected + " with private error body removed", delegate {
                backend.Reply(status, "{\"Message\":\"" + BackendSecret + "\",\"APIKey\":\"" + BackendKey + "\"}");
                int before = backend.Requests;
                Result result = Invoke("action=aiJob&id=1");
                Assert(result.Status == expected, "Unexpected forwarded HTTP status: " + result.Status);
                Assert((string)JObject.Parse(result.Body)["Code"] == "-99", "Failure lacks local error envelope.");
                Assert(backend.Requests == before + 1, "Backend redirect was followed or request repeated.");
            });
        }
        foreach (string body in new[] { "", "not JSON", "[]", "{\"Code\":", "{\"Code\":\"00\"} {}", "{\"Code\":\"00\"} trailing", "{}",
            "{\"Code\":\"-99\",\"Message\":\"" + BackendSecret + "\"}" })
        {
            string payload = body;
            Check("invalid backend response is sanitized: " + (body.Length == 0 ? "empty" : body), delegate {
                backend.Reply(200, payload);
                Assert(Invoke("action=state").Status == 502, "Invalid backend response accepted.");
            });
        }
        Check("oversized backend state body is rejected without exposing details", delegate {
            backend.Reply(200, "{\"Code\":\"00\",\"Message\":\"" + new string('x', 4 * 1024 * 1024) + "\"}");
            Assert(Invoke("action=state").Status == 502, "Oversized state response accepted.");
        });
        Check("deeply nested backend JSON is rejected", delegate {
            string nested = "{}";
            for (int i = 0; i < 70; i++) nested = "{\"nested\":" + nested + "}";
            backend.Reply(200, "{\"Code\":\"00\",\"Data\":" + nested + "}");
            Assert(Invoke("action=state").Status == 502, "Deep backend JSON accepted.");
        });
    }

    private static void TestConfiguration(Backend backend)
    {
        foreach (string address in new[] { "", "file:///private", backend.Url + "?injected=1", backend.Url + "#fragment", "http://user:secret@127.0.0.1/" })
        {
            string invalid = address;
            Check("invalid backend configuration is rejected: " + address, delegate {
                Configure(invalid, BackendKey);
                int before = backend.Requests;
                Assert(Invoke("action=state").Status == 503, "Invalid backend address accepted.");
                Assert(backend.Requests == before, "Invalid configuration caused HTTP request.");
            });
        }
        Check("missing backend key returns 503 without network", delegate {
            Configure(backend.Url, "");
            int before = backend.Requests;
            Assert(Invoke("action=state").Status == 503, "Missing backend key accepted.");
            Assert(backend.Requests == before, "Request made without backend key.");
        });
        Configure(backend.Url, BackendKey);
    }

    private sealed class Request : SimpleWorkerRequest
    {
        private readonly string method;
        private readonly string xhr;
        private readonly byte[] body;
        internal Request(string query, string verb, string header, string form, TextWriter output)
            : base("/", AppDomain.CurrentDomain.BaseDirectory, "Notification.ashx", query, output)
        { method = verb; xhr = header; body = Encoding.UTF8.GetBytes(form); }
        public override string GetHttpVerbName() { return method; }
        public override string GetUnknownRequestHeader(string name)
        { return name.Equals("X-Requested-With", StringComparison.OrdinalIgnoreCase) ? xhr : null; }
        public override string[][] GetUnknownRequestHeaders()
        { return xhr == null ? new string[0][] : new[] { new[] { "X-Requested-With", xhr } }; }
        public override string GetKnownRequestHeader(int index)
        {
            if (index == HeaderContentType) return "application/x-www-form-urlencoded";
            if (index == HeaderContentLength) return body.Length.ToString();
            return base.GetKnownRequestHeader(index);
        }
        public override byte[] GetPreloadedEntityBody() { return body; }
        public override int GetTotalEntityBodyLength() { return body.Length; }
        public override bool IsEntireEntityBodyIsPreloaded() { return true; }
    }

    private sealed class Backend : IDisposable
    {
        private readonly HttpListener listener = new HttpListener();
        private readonly Task worker;
        private int responseStatus = 200;
        private byte[] responseBody = Encoding.UTF8.GetBytes(Success);
        internal string Url;
        internal int Requests;
        internal string LastPath, LastQuery, LastMethod, LastKey, LastAccept, LastBody;
        internal Backend()
        {
            TcpListener probe = new TcpListener(IPAddress.Loopback, 0);
            probe.Start();
            int port = ((IPEndPoint)probe.LocalEndpoint).Port;
            probe.Stop();
            Url = "http://127.0.0.1:" + port + "/";
            listener.Prefixes.Add(Url);
            listener.Start();
            worker = Task.Run((Action)Serve);
        }
        internal void Reply(int status, string body)
        { responseStatus = status; responseBody = Encoding.UTF8.GetBytes(body); }
        private void Serve()
        {
            try
            {
                while (listener.IsListening)
                {
                    HttpListenerContext request = listener.GetContext();
                    Interlocked.Increment(ref Requests);
                    LastPath = request.Request.Url.AbsolutePath;
                    LastQuery = request.Request.Url.Query;
                    LastMethod = request.Request.HttpMethod;
                    LastKey = request.Request.Headers["X-API-Key"];
                    LastAccept = request.Request.Headers["Accept"];
                    using (StreamReader reader = new StreamReader(request.Request.InputStream)) LastBody = reader.ReadToEnd();
                    request.Response.StatusCode = responseStatus;
                    request.Response.ContentType = "application/json";
                    if (responseStatus == 302) request.Response.RedirectLocation = Url + "redirect-target";
                    request.Response.ContentLength64 = responseBody.Length;
                    try { request.Response.OutputStream.Write(responseBody, 0, responseBody.Length); }
                    catch (HttpListenerException) { }
                    finally { request.Response.Close(); }
                }
            }
            catch (HttpListenerException) { }
            catch (ObjectDisposedException) { }
        }
        public void Dispose()
        {
            listener.Close();
            Assert(worker.Wait(3000), "Synthetic backend did not stop.");
        }
    }
}
