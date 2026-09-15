using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using prjInfo;

namespace prjApplication.Handlers
{
    // Same-origin adapter. Database access, synchronization and the NL2SQL key
    // belong to QLB.API. The browser cannot select the forwarded ATFM user ID.
    public class NotificationHandler : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.SuppressFormsAuthenticationRedirect = true;

            T_Users user = context.Session == null ? null : context.Session["ATFM_CURRENT_USER"] as T_Users;
            if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated
                || user == null || user.UserID <= 0
                || !string.Equals(user.UserName, context.User.Identity.Name, StringComparison.OrdinalIgnoreCase))
            {
                WriteError(context, 401, "Phiên đăng nhập không hợp lệ.");
                return;
            }

            bool isGet = string.Equals(context.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase);
            bool isPost = string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase);
            if (!isGet && !isPost)
            {
                WriteError(context, 405, "Phương thức không được hỗ trợ.");
                return;
            }
            if (isPost && !string.Equals(context.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
            {
                WriteError(context, 403, "Yêu cầu không hợp lệ.");
                return;
            }

            string action = context.Request["action"] ?? "state";
            string source = (context.Request["source"] ?? "ALL").ToUpperInvariant();
            if (source != "ALL" && source != "AI_QUERY")
            {
                WriteError(context, 400, "Nguồn thông báo không hợp lệ.");
                return;
            }

            string operation;
            string query = "userId=" + user.UserID.ToString(CultureInfo.InvariantCulture);
            long id;
            int status, page;
            if (isGet && action.Equals("state", StringComparison.OrdinalIgnoreCase)) operation = "GetState";
            else if (isGet && action.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(context.Request["status"], out status) || (status != -1 && status != 0 && status != 1)
                    || !int.TryParse(context.Request["page"], out page) || page < 1 || page > 1000000)
                {
                    WriteError(context, 400, "Bộ lọc hoặc trang không hợp lệ.");
                    return;
                }
                operation = "GetPage";
                query += "&status=" + status.ToString(CultureInfo.InvariantCulture) + "&page=" + page.ToString(CultureInfo.InvariantCulture) + "&source=" + source;
            }
            else if ((isGet && (action.Equals("aiJob", StringComparison.OrdinalIgnoreCase) || action.Equals("aiResult", StringComparison.OrdinalIgnoreCase)))
                || (isPost && action.Equals("markRead", StringComparison.OrdinalIgnoreCase)))
            {
                if (!long.TryParse(context.Request["id"], out id) || id <= 0)
                {
                    WriteError(context, 400, "Mã thông báo không hợp lệ.");
                    return;
                }
                operation = isPost ? "MarkRead" : action.Equals("aiJob", StringComparison.OrdinalIgnoreCase) ? "GetAiJob" : "GetAiResult";
                query += "&id=" + id.ToString(CultureInfo.InvariantCulture);
            }
            else if (isPost && action.Equals("markAllRead", StringComparison.OrdinalIgnoreCase))
            {
                operation = "MarkAllRead";
                query += "&source=" + source;
            }
            else
            {
                WriteError(context, 400, "Thao tác thông báo không hợp lệ.");
                return;
            }

            try
            {
                // Older API deployments ignore source, including on MarkAllRead.
                // Verify the AI contract before a scoped mutation reaches them.
                if (operation == "MarkAllRead" && source == "AI_QUERY"
                    && ReadBackend(context, "GetPage", "userId=" + user.UserID.ToString(CultureInfo.InvariantCulture)
                        + "&status=-1&page=1&source=AI_QUERY", false, source) == null)
                    return;

                JObject response = ReadBackend(context, operation, query, isPost, source);
                if (response != null) context.Response.Write(response.ToString(Formatting.None));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("[Notification API proxy] " + ex.GetType().Name);
                WriteError(context, 502, "Không thể kết nối backend thông báo. Vui lòng thử lại sau.");
            }
        }

        private static JObject ReadBackend(HttpContext context, string operation, string query, bool isPost, string source)
        {
            string address = ConfigurationManager.AppSettings["ApplicationPath.API"];
            string apiKey = ConfigurationManager.AppSettings["APIKey"];
            Uri backend;
            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(address)
                || !Uri.TryCreate(address.Trim().TrimEnd('/') + "/", UriKind.Absolute, out backend)
                || (backend.Scheme != "http" && backend.Scheme != "https")
                || !string.IsNullOrEmpty(backend.UserInfo) || !string.IsNullOrEmpty(backend.Query) || !string.IsNullOrEmpty(backend.Fragment))
            {
                WriteError(context, 503, "Chưa cấu hình kết nối backend thông báo.");
                return null;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(backend, "api/Notifications/" + operation + "?" + query));
            request.Method = isPost ? "POST" : "GET";
            request.Accept = "application/json";
            request.Headers["X-API-Key"] = apiKey;
            request.AllowAutoRedirect = false;
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            if (isPost) request.ContentLength = 0;
            HttpWebResponse response;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
                if (response == null) throw;
            }
            using (response)
            {
                int status = (int)response.StatusCode;
                if (status != 200)
                {
                    // Upstream errors may contain configuration details. Only
                    // expose our local messages and meaningful HTTP statuses.
                    if (status == 404) WriteError(context, 404, "Không tìm thấy thông báo hoặc yêu cầu AI.");
                    else if (status == 409) WriteError(context, 409, "Job AI chưa hoàn thành thành công.");
                    else if (status == 410) WriteError(context, 410, "Kết quả AI đã hết hạn hoặc không còn khả dụng.");
                    else if (status == 403) WriteError(context, 403, "Bạn không có quyền xem yêu cầu AI này.");
                    else WriteError(context, status == 401 || status == 503 ? 503 : 502, "Backend thông báo chưa sẵn sàng. Vui lòng thử lại sau.");
                    return null;
                }
                using (Stream stream = response.GetResponseStream())
                using (MemoryStream buffer = new MemoryStream())
                {
                    int maximumBytes = operation == "GetAiResult" ? 72 * 1024 * 1024 : 4 * 1024 * 1024;
                    byte[] block = new byte[8192];
                    int count;
                    while ((count = stream.Read(block, 0, block.Length)) > 0)
                    {
                        if (buffer.Length + count > maximumBytes) throw new InvalidDataException("Notification response exceeds size limit.");
                        buffer.Write(block, 0, count);
                    }
                    buffer.Position = 0;
                    using (StreamReader reader = new StreamReader(buffer, Encoding.UTF8, true))
                    using (JsonTextReader json = new JsonTextReader(reader) { MaxDepth = 66, DateParseHandling = DateParseHandling.None })
                    {
                        JObject value = JObject.Load(json);
                        if (json.Read()) throw new InvalidDataException("Trailing response content.");
                        if ((string)value["Code"] != "00")
                        {
                            WriteError(context, 502, "Backend không thể xử lý dữ liệu thông báo.");
                            return null;
                        }
                        if (operation == "GetPage" && source == "AI_QUERY" && !SupportsAiFilter(value))
                        {
                            WriteError(context, 503, "Dịch vụ thông báo AI chưa sẵn sàng. Vui lòng liên hệ quản trị viên để cập nhật dịch vụ.");
                            return null;
                        }
                        return value;
                    }
                }
            }
        }

        private static bool SupportsAiFilter(JObject response)
        {
            JToken count = response["AIUnreadCount"] ?? response["AI_UNREAD_COUNT"];
            JArray rows = response["ListValue"] as JArray;
            if (count == null || count.Type != JTokenType.Integer || (long)count < 0 || rows == null)
                return false;
            foreach (JToken row in rows)
            {
                JObject item = row as JObject;
                if (item == null || !string.Equals((string)item["SOURCE_TYPE"], "AI_QUERY", StringComparison.OrdinalIgnoreCase))
                    return false;
            }
            return true;
        }

        private static void WriteError(HttpContext context, int status, string message)
        {
            context.Response.StatusCode = status;
            context.Response.Write(JsonConvert.SerializeObject(new { Code = "-99", Message = message }));
        }
    }
}
