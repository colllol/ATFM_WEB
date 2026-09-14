using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;

namespace QLB.API.Data
{
    internal static class EmailNotificationService
    {
        private static readonly object EmailSyncStateLock = new object();
        private static DateTime _lastEmailSyncAttemptUtc = DateTime.MinValue;
        private static bool _emailSyncInProgress;
        internal static void TrySynchronize()
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
                global::System.Diagnostics.Trace.TraceWarning("[Email notification sync] " + ex.GetType().Name);
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

                string sourceHash = BuildEmailSourceHash(item);
                EmailReportItem selected;
                if (!emails.TryGetValue(sourceHash, out selected)
                    || GetEmailRowPriority(item) > GetEmailRowPriority(selected))
                    emails[sourceHash] = item;
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
            string sourceHash,
            EmailReportItem item)
        {
            const string sql = @"
MERGE INTO T_NOTIFICATION TARGET
USING (
    SELECT :P_SOURCE_HASH AS SOURCE_HASH
    FROM DUAL
) SOURCE
ON (TARGET.SOURCE_HASH = SOURCE.SOURCE_HASH)
WHEN MATCHED THEN
    UPDATE SET TARGET.TITLE = :P_TITLE,
               TARGET.CONTENT = :P_CONTENT,
               TARGET.DATETIME = :P_DATETIME,
               TARGET.SOURCE_TYPE = :P_SOURCE_TYPE,
               TARGET.SOURCE_KEY = :P_SOURCE_KEY
    WHERE TARGET.TITLE <> :P_TITLE
       OR TARGET.CONTENT <> :P_CONTENT
       OR TARGET.DATETIME <> :P_DATETIME
       OR NVL(TARGET.SOURCE_TYPE, '~') <> NVL(:P_SOURCE_TYPE, '~')
       OR NVL(TARGET.SOURCE_KEY, '~') <> NVL(:P_SOURCE_KEY, '~')
WHEN NOT MATCHED THEN
    INSERT (TITLE, CONTENT, DATETIME, TARGET_TYPE, SOURCE_TYPE, SOURCE_KEY, SOURCE_HASH)
    VALUES (:P_TITLE, :P_CONTENT, :P_DATETIME, 0, :P_SOURCE_TYPE, :P_SOURCE_KEY, :P_SOURCE_HASH)";

            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.Transaction = transaction;
                command.BindByName = true;
                command.CommandType = CommandType.Text;
                command.CommandTimeout = 10;
                command.Parameters.Add("P_SOURCE_HASH", OracleDbType.Varchar2, 80).Value = sourceHash;
                command.Parameters.Add("P_SOURCE_TYPE", OracleDbType.Varchar2, 30).Value =
                    (object)Truncate(NullIfBlank(item.SourceType), 30) ?? DBNull.Value;
                command.Parameters.Add("P_SOURCE_KEY", OracleDbType.Varchar2, 80).Value =
                    (object)Truncate(NullIfBlank(item.SourceKey), 80) ?? DBNull.Value;
                command.Parameters.Add("P_TITLE", OracleDbType.NVarchar2, 250).Value = BuildEmailTitle(item);
                command.Parameters.Add("P_CONTENT", OracleDbType.NVarchar2, 2000).Value = BuildEmailContent(item);
                command.Parameters.Add("P_DATETIME", OracleDbType.TimeStamp).Value = ParseReceivedAt(item.ReceivedAt);
                command.ExecuteNonQuery();
            }
        }

        private static string NullIfBlank(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string BuildEmailSourceHash(EmailReportItem item)
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

            [JsonProperty("source_type")]
            public string SourceType { get; set; }

            [JsonProperty("source_key")]
            public string SourceKey { get; set; }
        }

        private static OracleConnection CreateConnection()
        {
            string connection = ConfigurationManager.AppSettings["ConnectDB"];
            if (string.IsNullOrWhiteSpace(connection)) throw new ConfigurationErrorsException("Missing ConnectDB.");
            return new OracleConnection(connection);
        }
    }
}
