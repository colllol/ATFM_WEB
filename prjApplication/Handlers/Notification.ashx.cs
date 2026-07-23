using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using prjInfo;

namespace prjApplication.Handlers
{
    public class NotificationHandler : IHttpHandler, IReadOnlySessionState
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

            string method = context.Request.HttpMethod;
            string action = context.Request["action"] ?? "state";
            bool isGet = string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase);
            bool isPost = string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase);

            if (!isGet && !isPost)
            {
                WriteError(context, 405, "Phương thức không được hỗ trợ.");
                return;
            }

            if (isPost
                && !string.Equals(context.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
            {
                WriteError(context, 403, "Yêu cầu không hợp lệ.");
                return;
            }

            string apiPath;
            bool sendPost;
            if (isGet && string.Equals(action, "state", StringComparison.OrdinalIgnoreCase))
            {
                apiPath = "api/Notifications/GetState";
                sendPost = false;
            }
            else if (isGet && string.Equals(action, "list", StringComparison.OrdinalIgnoreCase))
            {
                int status;
                int page;
                if (!int.TryParse(context.Request["status"], out status)
                    || (status != -1 && status != 0 && status != 1)
                    || !int.TryParse(context.Request["page"], out page)
                    || page < 1 || page > 1000000)
                {
                    WriteError(context, 400, "Bộ lọc hoặc trang không hợp lệ.");
                    return;
                }

                apiPath = string.Format(
                    CultureInfo.InvariantCulture,
                    "api/Notifications/GetPage?status={0}&page={1}",
                    status,
                    page);
                sendPost = false;
            }
            else if (isPost && string.Equals(action, "markRead", StringComparison.OrdinalIgnoreCase))
            {
                long id;
                if (!long.TryParse(context.Request["id"], out id) || id <= 0)
                {
                    WriteError(context, 400, "Mã thông báo không hợp lệ.");
                    return;
                }

                apiPath = string.Format(
                    CultureInfo.InvariantCulture,
                    "api/Notifications/MarkRead?id={0}",
                    id);
                sendPost = true;
            }
            else if (isPost && string.Equals(action, "markAllRead", StringComparison.OrdinalIgnoreCase))
            {
                apiPath = "api/Notifications/MarkAllRead";
                sendPost = true;
            }
            else
            {
                WriteError(context, 400, "Thao tác thông báo không hợp lệ.");
                return;
            }

            try
            {
                HttpResponseMessage apiResponse = sendPost
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
            client.Timeout = TimeSpan.FromSeconds(15);
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
