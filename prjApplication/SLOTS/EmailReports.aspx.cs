using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Configuration;

namespace prjApplication.SLOTS
{
    public partial class EmailReports : Page
    {
        // string DefaultEndpoint = ConfigurationManager.AppSettings["APIEmail"];
        // string DefaultJobsEndpoint = ConfigurationManager.AppSettings["APIEmailJobs"];

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetEmails(string query, string processingStatus, string fromDate, string toDate, int page, int size)
        {
            string endpoint = ConfigurationManager.AppSettings["APIEmail"];
            var parameters = new List<string>();
            AddParameter(parameters, "query", query);
            AddParameter(parameters, "processingStatus", processingStatus);
            AddParameter(parameters, "from", fromDate);
            AddParameter(parameters, "to", toDate);
            parameters.Add("page=" + Math.Max(0, page));
            parameters.Add("size=" + Math.Max(1, Math.Min(size, 100)));

            string separator = endpoint.Contains("?") ? "&" : "?";
            string requestUrl = endpoint + separator + String.Join("&", parameters);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetTargetPermId(string syncJobId)
        {
            if (String.IsNullOrWhiteSpace(syncJobId))
                throw new ArgumentException("Email không có syncJobId.");

            syncJobId = syncJobId.Trim();
            if (syncJobId.Length > 200)
                throw new ArgumentException("syncJobId không hợp lệ.");

            string endpoint = ConfigurationManager.AppSettings["APIEmailJobs"];
            if (!endpoint.EndsWith("/", StringComparison.Ordinal))
                endpoint += "/";

            string requestUrl = endpoint + Uri.EscapeDataString(syncJobId);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Timeout = 30000;
            request.ReadWriteTimeout = 30000;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            try
            {
                string content;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    content = reader.ReadToEnd();
                }

                object payload = new JavaScriptSerializer().DeserializeObject(content);
                object rawTargetPermId = FindProperty(payload, "targetPermId");
                long targetPermId;
                if (rawTargetPermId == null ||
                    !Int64.TryParse(Convert.ToString(rawTargetPermId), out targetPermId) ||
                    targetPermId <= 0)
                {
                    throw new InvalidOperationException("API job không trả về targetPermId hợp lệ.");
                }

                return new { targetPermId = targetPermId };
            }
            catch (WebException ex)
            {
                string detail = ReadErrorResponse(ex);
                throw new InvalidOperationException(
                    "Không thể lấy thông tin job từ máy chủ ATFM." +
                    (String.IsNullOrWhiteSpace(detail) ? String.Empty : " Chi tiết: " + detail),
                    ex);
            }
        }

        private static void AddParameter(ICollection<string> parameters, string name, string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
                parameters.Add(name + "=" + Uri.EscapeDataString(value.Trim()));
        }

        private static object FindProperty(object value, string propertyName)
        {
            IDictionary<string, object> dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
            {
                foreach (KeyValuePair<string, object> pair in dictionary)
                {
                    if (String.Equals(pair.Key, propertyName, StringComparison.OrdinalIgnoreCase))
                        return pair.Value;
                }

                foreach (KeyValuePair<string, object> pair in dictionary)
                {
                    object nestedValue = FindProperty(pair.Value, propertyName);
                    if (nestedValue != null)
                        return nestedValue;
                }
            }

            object[] items = value as object[];
            if (items != null)
            {
                foreach (object item in items)
                {
                    object nestedValue = FindProperty(item, propertyName);
                    if (nestedValue != null)
                        return nestedValue;
                }
            }

            return null;
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
