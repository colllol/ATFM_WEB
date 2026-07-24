using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;

namespace prjApplication.SLOTS
{
    public partial class EmailReports : Page
    {
        private const string DefaultEndpoint = "http://192.168.100.131:8080/api/reports/emails";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetEmails()
        {
            string endpoint = ConfigurationManager.AppSettings["EmailReports.ApiUrl"] ?? DefaultEndpoint;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;
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
                throw new InvalidOperationException(
                    "Không thể kết nối API báo cáo email từ máy chủ ATFM." +
                    (String.IsNullOrWhiteSpace(detail) ? String.Empty : " Chi tiết: " + detail),
                    ex);
            }
        }

        private static string ReadErrorResponse(WebException exception)
        {
            if (exception.Response == null)
                return exception.Status == WebExceptionStatus.Timeout
                    ? "Kết nối đến API đã hết thời gian chờ."
                    : exception.Message;

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
