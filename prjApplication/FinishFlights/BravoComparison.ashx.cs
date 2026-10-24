using System;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using prjInfo;

namespace prjApplication.FinishFlights
{
    public sealed class BravoComparisonHandler : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();

            T_Users user = context.Session == null ? null : context.Session[Login.CurrentUserSessionKey] as T_Users;
            if (user == null)
            {
                WriteError(context, 401, "Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.");
                return;
            }
            if (!String.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase)
                || !String.Equals(context.Request.Headers["X-Requested-With"], "XMLHttpRequest", StringComparison.Ordinal))
            {
                WriteError(context, 405, "Phương thức không được hỗ trợ.");
                return;
            }

            try
            {
                object result = ListFinishedFlights.GetBravoComparison(
                    context.Request.Form["fromDate"], context.Request.Form["toDate"],
                    context.Request.Form["comparisonType"], context.Request.Form["region"]);
                context.Response.Write(JsonConvert.SerializeObject(result));
            }
            catch (ArgumentException ex) { WriteError(context, 400, ex.Message); }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[BravoComparison] " + ex);
                WriteError(context, 500, "Không thể tải dữ liệu đối chiếu Bravo.");
            }
        }

        private static void WriteError(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.Write(JsonConvert.SerializeObject(new { message = message }));
        }
    }
}
