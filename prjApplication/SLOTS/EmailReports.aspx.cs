using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using prjBusinessLogic;
using prjInfo;

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

                PermMasterSc permission = new PermMasterScDAL().GetOneObject(targetPermId.ToString());
                if (permission == null)
                    throw new InvalidOperationException("Không tìm thấy dữ liệu số phép bay trong database.");

                string oper = !String.IsNullOrWhiteSpace(permission.OPER_NAME)
                    ? permission.OPER_NAME
                    : permission.OPER_ID;
                return new { targetPermId = targetPermId, hasDatabaseData = true, oper = oper ?? String.Empty };
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object SendReport(string recipient, string format, List<EmailReportRow> rows)
        {
            MailAddress to;
            try
            {
                to = new MailAddress((recipient ?? String.Empty).Trim());
            }
            catch (FormatException)
            {
                throw new ArgumentException("Địa chỉ Gmail người nhận không hợp lệ.");
            }

            if (rows == null || rows.Count == 0)
                throw new ArgumentException("Không có dữ liệu để tạo báo cáo.");
            if (rows.Count > 500)
                throw new ArgumentException("Mỗi báo cáo được gửi tối đa 500 dòng.");

            format = String.Equals(format, "word", StringComparison.OrdinalIgnoreCase) ? "word" : "excel";
            string extension = format == "word" ? ".doc" : ".xls";
            string mediaType = format == "word" ? "application/msword" : "application/vnd.ms-excel";
            string fileName = "Bao_cao_Email_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + extension;
            byte[] report = BuildReport(rows);

            using (MailMessage message = new MailMessage())
            using (MemoryStream stream = new MemoryStream(report))
            using (Attachment attachment = new Attachment(stream, fileName, mediaType))
            using (SmtpClient client = new SmtpClient())
            {
                string fromAddress = ConfigurationManager.AppSettings["EmailReportFrom"];
                SmtpSection smtpSettings = ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
                if (String.IsNullOrWhiteSpace(fromAddress) && smtpSettings != null)
                    fromAddress = smtpSettings.Network.UserName;
                if (String.IsNullOrWhiteSpace(fromAddress))
                    throw new InvalidOperationException("Chưa cấu hình tài khoản gửi Gmail trong Web.config.");

                message.From = new MailAddress(fromAddress);
                message.To.Add(to);
                message.Subject = "Báo cáo trạng thái dữ liệu email ATFM";
                message.Body = "Báo cáo được tạo tự động từ trang SLOTS/EmailReports.aspx. Dòng có ký hiệu [!] là email không có dữ liệu trong database.";
                message.Attachments.Add(attachment);
                client.EnableSsl = true;
                client.Send(message);
            }

            return new { success = true, message = "Đã gửi báo cáo tới " + to.Address + "." };
        }

        private static byte[] BuildReport(IList<EmailReportRow> rows)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<html><head><meta charset=\"utf-8\"><style>body{font-family:Arial}table{border-collapse:collapse;width:100%}th,td{border:1px solid #777;padding:7px}th{background:#dceef8}.missing{color:#b4232c;background:#fff0f1;font-weight:bold}</style></head><body>");
            html.Append("<h2>BÁO CÁO TRẠNG THÁI DỮ LIỆU EMAIL</h2><table><thead><tr><th>STT</th><th>OPER (Tên hãng)</th><th>Tên file</th><th>Trạng thái</th></tr></thead><tbody>");
            for (int i = 0; i < rows.Count; i++)
            {
                EmailReportRow row = rows[i] ?? new EmailReportRow();
                bool hasData = row.HasDatabaseData;
                string state = hasData
                    ? (String.IsNullOrWhiteSpace(row.Status) ? "CÓ DỮ LIỆU" : row.Status + " - CÓ DỮ LIỆU DB")
                    : String.Equals(row.DatabaseState, "error", StringComparison.OrdinalIgnoreCase)
                        ? "[?] KHÔNG KIỂM TRA ĐƯỢC DATABASE"
                        : "[!] KHÔNG CÓ DỮ LIỆU TRONG DATABASE";
                html.Append("<tr").Append(hasData ? String.Empty : " class=\"missing\"").Append("><td>")
                    .Append(i + 1).Append("</td><td>").Append(WebEncode(row.Oper, "--"))
                    .Append("</td><td>").Append(WebEncode(row.FileName, "--"))
                    .Append("</td><td>").Append(WebEncode(state, "--")).Append("</td></tr>");
            }
            html.Append("</tbody></table></body></html>");
            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(html.ToString())).ToArray();
        }

        private static string WebEncode(string value, string fallback)
        {
            return System.Web.HttpUtility.HtmlEncode(String.IsNullOrWhiteSpace(value) ? fallback : value.Trim());
        }

        public sealed class EmailReportRow
        {
            public string Oper { get; set; }
            public string FileName { get; set; }
            public string Status { get; set; }
            public bool HasDatabaseData { get; set; }
            public string DatabaseState { get; set; }
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
