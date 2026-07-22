using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;

namespace prjApplication.SLOTS
{
    public partial class AIReports : Page
    {
        private const string DefaultEndpoint = "http://172.29.79.49:8000/api/reports/ai";
        private const string DefaultApiKey = "atfm";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetReports(int limit, int offset)
        {
            limit = Math.Max(1, Math.Min(limit, 500));
            offset = Math.Max(0, offset);
            string endpoint = ConfigurationManager.AppSettings["AIReports.ApiUrl"] ?? DefaultEndpoint;
            string apiKey = ConfigurationManager.AppSettings["AIReports.ApiKey"] ?? DefaultApiKey;
            string separator = endpoint.Contains("?") ? "&" : "?";
            string requestUrl = endpoint + separator + "limit=" + limit + "&offset=" + offset;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers["X-API-Key"] = apiKey;
            request.Timeout = 45000;
            request.ReadWriteTimeout = 45000;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                string detail = ReadErrorResponse(ex);
                throw new InvalidOperationException("Không thể tải báo cáo AI từ dịch vụ nội bộ." +
                    (string.IsNullOrWhiteSpace(detail) ? string.Empty : " Chi tiết: " + detail), ex);
            }
        }

        private static string ReadErrorResponse(WebException exception)
        {
            if (exception.Response == null) return exception.Message;
            try
            {
                using (Stream stream = exception.Response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    string content = reader.ReadToEnd();
                    return content.Length > 500 ? content.Substring(0, 500) : content;
                }
            }
            catch
            {
                return exception.Message;
            }
        }
    }
}
