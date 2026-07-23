using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using prjInfo;

namespace prjApplication.Handlers
{
    public class NotificationHandler : IHttpHandler, IRequiresSessionState
    {
        private const string CurrentUserSessionKey = "ATFM_CURRENT_USER";
        private static readonly Lazy<HttpClient> ApiClient = new Lazy<HttpClient>(CreateApiClient);

        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();

            T_Users sessionUser = context.Session[CurrentUserSessionKey] as T_Users;
            if (context.User == null || context.User.Identity == null
                || !context.User.Identity.IsAuthenticated || sessionUser == null
                || !string.Equals(sessionUser.UserName, context.User.Identity.Name, StringComparison.OrdinalIgnoreCase))
            {
                WriteError(context, 401, "Phiên đăng nhập không hợp lệ.");
                return;
            }

            bool markAllRead = string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase)
                && string.Equals(context.Request["action"], "markAllRead", StringComparison.OrdinalIgnoreCase);

            if (!string.Equals(context.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)
                && !markAllRead)
            {
                WriteError(context, 405, "Phương thức không được hỗ trợ.");
                return;
            }

            if (string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(context.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
            {
                WriteError(context, 403, "Yêu cầu không hợp lệ.");
                return;
            }

            string apiPath = markAllRead
                ? "api/Notifications/MarkAllRead"
                : "api/Notifications/GetState";

            try
            {
                HttpResponseMessage apiResponse = markAllRead
                    ? ApiClient.Value.PostAsync(apiPath, new StringContent("{}", Encoding.UTF8, "application/json")).Result
                    : ApiClient.Value.GetAsync(apiPath).Result;
                string body = apiResponse.Content.ReadAsStringAsync().Result;

                if (!apiResponse.IsSuccessStatusCode)
                {
                    WriteError(context, 502, "Không thể tải dữ liệu thông báo.");
                    return;
                }

                context.Response.Write(body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[NotificationHandler] " + ex.Message);
                WriteError(context, 502, "Không thể kết nối dịch vụ thông báo.");
            }
        }

        private static HttpClient CreateApiClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApplicationPath.API"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string apiKey = Environment.GetEnvironmentVariable("ATFM_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                apiKey = ConfigurationManager.AppSettings["APIKey"];
            if (!string.IsNullOrWhiteSpace(apiKey))
                client.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            return client;
        }

        private static void WriteError(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.Write(JsonConvert.SerializeObject(new { Code = "-99", Message = message }));
        }
    }
}
