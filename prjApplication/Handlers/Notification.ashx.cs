using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using prjInfo;

namespace prjApplication.Handlers
{
    public class NotificationHandler : IHttpHandler, IReadOnlySessionState
    {
        private const string CurrentUserSessionKey = "ATFM_CURRENT_USER";
        private const string EmailNotificationSource = "EMAIL_API";
        private static readonly object EmailSyncStateLock = new object();
        private static DateTime _lastEmailSyncAttemptUtc = DateTime.MinValue;
        private static bool _emailSyncInProgress;

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

            int status = -1;
            int page = 1;
            long notificationId = 0;
            if (isGet && string.Equals(action, "state", StringComparison.OrdinalIgnoreCase))
            {
            }
            else if (isGet && string.Equals(action, "list", StringComparison.OrdinalIgnoreCase))
            {
                if (!int.TryParse(context.Request["status"], out status)
                    || (status != -1 && status != 0 && status != 1)
                    || !int.TryParse(context.Request["page"], out page)
                    || page < 1 || page > 1000000)
                {
                    WriteError(context, 400, "Bộ lọc hoặc trang không hợp lệ.");
                    return;
                }
            }
            else if (isPost && string.Equals(action, "markRead", StringComparison.OrdinalIgnoreCase))
            {
                if (!long.TryParse(context.Request["id"], out notificationId) || notificationId <= 0)
                {
                    WriteError(context, 400, "Mã thông báo không hợp lệ.");
                    return;
                }
            }
            else if (isPost && string.Equals(action, "markAllRead", StringComparison.OrdinalIgnoreCase))
            {
            }
            else
            {
                WriteError(context, 400, "Thao tác thông báo không hợp lệ.");
                return;
            }

            try
            {
                if (isGet)
                    TrySynchronizeEmailNotifications();

                object response;
                if (string.Equals(action, "state", StringComparison.OrdinalIgnoreCase))
                    response = GetState(sessionUser.UserID);
                else if (string.Equals(action, "list", StringComparison.OrdinalIgnoreCase))
                    response = GetPage(sessionUser.UserID, status, page);
                else if (string.Equals(action, "markRead", StringComparison.OrdinalIgnoreCase))
                    response = MarkRead(sessionUser.UserID, notificationId);
                else
                    response = MarkAllRead(sessionUser.UserID);

                context.Response.Write(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("[NotificationHandler] " + ex);
                WriteError(context, 500, "Không thể xử lý dữ liệu thông báo.");
            }
        }

        private static void TrySynchronizeEmailNotifications()
        {
            lock (EmailSyncStateLock)
            {
                if (_emailSyncInProgress
                    || DateTime.UtcNow.Subtract(_lastEmailSyncAttemptUtc) < TimeSpan.FromSeconds(15))
                    return;

                _emailSyncInProgress = true;
                _lastEmailSyncAttemptUtc = DateTime.UtcNow;
            }

            try
            {
                SynchronizeEmailNotifications();
            }
            catch (Exception ex)
            {
                // API email khong duoc lam gian doan cac thong bao noi bo dang co.
                System.Diagnostics.Trace.TraceWarning("[NotificationHandler][EmailSync] " + ex);
            }
            finally
            {
                lock (EmailSyncStateLock)
                    _emailSyncInProgress = false;
            }
        }

        private static void SynchronizeEmailNotifications()
        {
            string endpoint = ConfigurationManager.AppSettings["APIEmail"];
            if (string.IsNullOrWhiteSpace(endpoint))
                return;

            string separator = endpoint.IndexOf('?') >= 0 ? "&" : "?";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                endpoint + separator + "page=0&size=100");
            request.Method = "GET";
            request.Accept = "application/json";
            request.Timeout = 5000;
            request.ReadWriteTimeout = 5000;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            EmailReportPage reportPage;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                reportPage = JsonConvert.DeserializeObject<EmailReportPage>(reader.ReadToEnd());
            }

            if (reportPage == null || reportPage.Content == null || reportPage.Content.Count == 0)
                return;

            // API tra mot dong cho moi tep dinh kem. Gop theo Message-ID de moi email
            // chi tao mot thong bao va uu tien dong co tep da luu/loi xu ly ro rang nhat.
            Dictionary<string, EmailReportItem> emails = new Dictionary<string, EmailReportItem>(StringComparer.OrdinalIgnoreCase);
            for (int index = 0; index < reportPage.Content.Count; index++)
            {
                EmailReportItem item = reportPage.Content[index];
                if (item == null)
                    continue;

                string sourceKey = BuildEmailSourceKey(item);
                EmailReportItem selected;
                if (!emails.TryGetValue(sourceKey, out selected)
                    || GetEmailRowPriority(item) > GetEmailRowPriority(selected))
                    emails[sourceKey] = item;
            }

            using (OracleConnection connection = CreateConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (KeyValuePair<string, EmailReportItem> email in emails)
                            UpsertEmailNotification(connection, transaction, email.Key, email.Value);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void UpsertEmailNotification(
            OracleConnection connection,
            OracleTransaction transaction,
            string sourceKey,
            EmailReportItem item)
        {
            const string sql = @"
MERGE INTO T_NOTIFICATION TARGET
USING (
    SELECT :P_SOURCE_TYPE AS SOURCE_TYPE,
           :P_SOURCE_KEY AS SOURCE_KEY
    FROM DUAL
) SOURCE
ON (TARGET.SOURCE_TYPE = SOURCE.SOURCE_TYPE
    AND TARGET.SOURCE_KEY = SOURCE.SOURCE_KEY)
WHEN MATCHED THEN
    UPDATE SET TARGET.TITLE = :P_TITLE,
               TARGET.CONTENT = :P_CONTENT,
               TARGET.DATETIME = :P_DATETIME
    WHERE TARGET.TITLE <> :P_TITLE
       OR TARGET.CONTENT <> :P_CONTENT
       OR TARGET.DATETIME <> :P_DATETIME
WHEN NOT MATCHED THEN
    INSERT (TITLE, CONTENT, DATETIME, TARGET_TYPE, SOURCE_TYPE, SOURCE_KEY)
    VALUES (:P_TITLE, :P_CONTENT, :P_DATETIME, 0, :P_SOURCE_TYPE, :P_SOURCE_KEY)";

            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.Transaction = transaction;
                command.BindByName = true;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 10;
                command.Parameters.Add("P_SOURCE_TYPE", OracleDbType.Varchar2, 30).Value = EmailNotificationSource;
                command.Parameters.Add("P_SOURCE_KEY", OracleDbType.Varchar2, 80).Value = sourceKey;
                command.Parameters.Add("P_TITLE", OracleDbType.NVarchar2, 250).Value = BuildEmailTitle(item);
                command.Parameters.Add("P_CONTENT", OracleDbType.NVarchar2, 2000).Value = BuildEmailContent(item);
                command.Parameters.Add("P_DATETIME", OracleDbType.TimeStamp).Value = ParseReceivedAt(item.ReceivedAt);
                command.ExecuteNonQuery();
            }
        }

        private static string BuildEmailSourceKey(EmailReportItem item)
        {
            string identity = !string.IsNullOrWhiteSpace(item.MessageId)
                ? "message:" + item.MessageId.Trim()
                : "report:" + item.Id.ToString(CultureInfo.InvariantCulture);

            using (SHA256 algorithm = SHA256.Create())
            {
                byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(identity));
                StringBuilder value = new StringBuilder(hash.Length * 2);
                for (int index = 0; index < hash.Length; index++)
                    value.Append(hash[index].ToString("x2", CultureInfo.InvariantCulture));
                return value.ToString();
            }
        }

        private static int GetEmailRowPriority(EmailReportItem item)
        {
            int priority = 0;
            if (!string.IsNullOrWhiteSpace(item.StoredFileName)) priority += 100;
            if (!string.IsNullOrWhiteSpace(item.ErrorMessage)) priority += 50;
            if (string.Equals(item.ProcessingStatus, "FAILED", StringComparison.OrdinalIgnoreCase)
                || string.Equals(item.ProcessingStatus, "QUARANTINED", StringComparison.OrdinalIgnoreCase)
                || string.Equals(item.ProcessingStatus, "BLOCKED", StringComparison.OrdinalIgnoreCase))
                priority += 20;
            if (!string.IsNullOrWhiteSpace(item.AttachmentName)) priority += 10;
            return priority;
        }

        private static string BuildEmailTitle(EmailReportItem item)
        {
            string subject = string.IsNullOrWhiteSpace(item.Subject)
                ? "(Không có tiêu đề)"
                : item.Subject.Trim();
            return Truncate("Email mới: " + subject, 250);
        }

        private static string BuildEmailContent(EmailReportItem item)
        {
            string attachment = !string.IsNullOrWhiteSpace(item.StoredFileName)
                ? item.StoredFileName.Trim()
                : (!string.IsNullOrWhiteSpace(item.AttachmentName) ? item.AttachmentName.Trim() : "Không có");
            StringBuilder content = new StringBuilder();
            content.AppendLine("Người gửi: " + DisplayValue(item.Sender));
            content.AppendLine("Tệp đính kèm: " + attachment);
            content.AppendLine("Thời gian: " + DisplayValue(item.ReceivedAt));
            content.AppendLine("Trạng thái xử lý: " + DisplayValue(item.ProcessingStatus));
            content.AppendLine("Trạng thái xác nhận: " + DisplayValue(item.AcknowledgementStatus));
            content.Append("Thông báo lỗi: " + DisplayValue(item.ErrorMessage));
            return Truncate(content.ToString(), 2000);
        }

        private static string DisplayValue(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? "Không có" : value.Trim();
        }

        private static string Truncate(string value, int maximumLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maximumLength)
                return value;
            return value.Substring(0, maximumLength);
        }

        private static DateTime ParseReceivedAt(string value)
        {
            DateTime result;
            if (!string.IsNullOrWhiteSpace(value)
                && DateTime.TryParse(
                    value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
                    out result))
                return result;
            return DateTime.Now;
        }

        private sealed class EmailReportPage
        {
            public List<EmailReportItem> Content { get; set; }
        }

        private sealed class EmailReportItem
        {
            public long Id { get; set; }
            public string MessageId { get; set; }
            public string Sender { get; set; }
            public string Subject { get; set; }
            public string ReceivedAt { get; set; }
            public string AttachmentName { get; set; }
            public string StoredFileName { get; set; }
            public string ErrorMessage { get; set; }
            public string ProcessingStatus { get; set; }
            public string AcknowledgementStatus { get; set; }
        }

        private static object GetState(long userId)
        {
            DataSet data = ExecuteDataSet(
                "NOTIFICATION_PKG.GET_STATE",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                });
            DataTable rows = data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
            int unreadCount = rows.Rows.Count > 0 && rows.Columns.Contains("UNREAD_COUNT")
                ? Convert.ToInt32(rows.Rows[0]["UNREAD_COUNT"], CultureInfo.InvariantCulture)
                : 0;
            if (rows.Columns.Contains("UNREAD_COUNT"))
                rows.Columns.Remove("UNREAD_COUNT");

            return Success(rows, unreadCount, rows.Rows.Count);
        }

        private static object GetPage(long userId, int status, int page)
        {
            DataSet data = ExecuteDataSet(
                "NOTIFICATION_PKG.GET_PAGE",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_STATUS", OracleDbType.Int32) { Value = status },
                new OracleParameter("P_PAGE_INDEX", OracleDbType.Int32) { Value = page },
                new OracleParameter("P_PAGE_SIZE", OracleDbType.Int32) { Value = 100 },
                new OracleParameter("P_DATA_CURSOR", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                },
                new OracleParameter("P_TOTAL_CURSOR", OracleDbType.RefCursor)
                {
                    Direction = ParameterDirection.Output
                });

            DataTable rows = data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
            DataTable totals = data.Tables.Count > 1 ? data.Tables[1] : new DataTable();
            int totalCount = totals.Rows.Count > 0
                ? Convert.ToInt32(totals.Rows[0]["TOTAL_COUNT"], CultureInfo.InvariantCulture)
                : 0;
            int unreadCount = totals.Rows.Count > 0
                ? Convert.ToInt32(totals.Rows[0]["UNREAD_COUNT"], CultureInfo.InvariantCulture)
                : 0;
            return Success(rows, unreadCount, totalCount);
        }

        private static object MarkRead(long userId, long notificationId)
        {
            long updated = ExecuteUpdate(
                "NOTIFICATION_PKG.MARK_READ",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_ID", OracleDbType.Int64) { Value = notificationId });
            return Success(null, updated, 0);
        }

        private static object MarkAllRead(long userId)
        {
            long updated = ExecuteUpdate(
                "NOTIFICATION_PKG.MARK_ALL_READ",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId });
            return Success(null, updated, 0);
        }

        private static DataSet ExecuteDataSet(string procedureName, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = CreateConnection())
            using (OracleCommand command = new OracleCommand(procedureName, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                command.BindByName = true;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 15;
                command.Parameters.AddRange(parameters);
                DataSet data = new DataSet();
                adapter.Fill(data);
                return data;
            }
        }

        private static long ExecuteUpdate(string procedureName, params OracleParameter[] parameters)
        {
            using (OracleConnection connection = CreateConnection())
            using (OracleCommand command = new OracleCommand(procedureName, connection))
            {
                command.BindByName = true;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 15;
                command.Parameters.AddRange(parameters);
                OracleParameter output = new OracleParameter(
                    "P_UPDATED_COUNT",
                    OracleDbType.Int64,
                    ParameterDirection.Output);
                command.Parameters.Add(output);
                connection.Open();
                command.ExecuteNonQuery();
                return Convert.ToInt64(output.Value.ToString(), CultureInfo.InvariantCulture);
            }
        }

        private static OracleConnection CreateConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["SlotsOracle"];
            if (settings == null || string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new ConfigurationErrorsException("Missing SlotsOracle connection string.");
            return new OracleConnection(settings.ConnectionString);
        }

        private static object Success(DataTable rows, long value, int totalCount)
        {
            return new
            {
                Code = "00",
                Message = "Get Data Success!",
                Value = value,
                ListValue = rows,
                SumRecord = totalCount.ToString(CultureInfo.InvariantCulture)
            };
        }

        private static void WriteError(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.Write(JsonConvert.SerializeObject(new { Code = "-99", Message = message }));
        }
    }
}
