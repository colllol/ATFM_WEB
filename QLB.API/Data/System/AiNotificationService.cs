using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.DataAccess.Client;

namespace QLB.API.Data
{
    // The integration read_at/unread_count belong to the provider. ATFM uses only
    // T_NOTIFICATION_READ and never acknowledges the provider's global read state.
    internal static class AiNotificationService
    {
        private const int PageSize = 100;
        private static readonly object SyncLock = new object();
        private static bool syncing;
        private static DateTime lastAttempt = DateTime.MinValue;

        internal static void QueueSynchronization()
        {
            lock (SyncLock)
            {
                if (syncing || DateTime.UtcNow - lastAttempt < TimeSpan.FromSeconds(15)) return;
                syncing = true;
                lastAttempt = DateTime.UtcNow;
            }
            try
            {
                HostingEnvironment.QueueBackgroundWorkItem(token =>
                {
                    try
                    {
                        for (int page = 0; page < 5 && !token.IsCancellationRequested; page++)
                            if (!SynchronizePage(page == 0)) break;
                    }
                    catch (Exception ex)
                    {
                        // Do not log upstream payloads, keys, or recipient/job data.
                        global::System.Diagnostics.Trace.TraceWarning("[AI notification sync] " + ex.GetType().Name);
                    }
                    finally { lock (SyncLock) syncing = false; }
                });
            }
            catch (Exception ex)
            {
                lock (SyncLock) syncing = false;
                global::System.Diagnostics.Trace.TraceWarning("[AI notification queue] " + ex.GetType().Name);
            }
        }

        // Page, targets and durable cursor are committed together. NOWAIT prevents
        // competing IIS workers from importing the same page or waiting on HTTP I/O.
        internal static bool SynchronizePage(bool checkInterval)
        {
            using (OracleConnection connection = CreateConnection())
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    long cursor;
                    using (OracleCommand command = Command(connection, transaction,
                        "SELECT LAST_ID, CASE WHEN LAST_POLL_AT > SYS_EXTRACT_UTC(SYSTIMESTAMP) - INTERVAL '15' SECOND THEN 1 ELSE 0 END AS RECENT FROM T_AI_NOTIFICATION_SYNC WHERE SOURCE_NAME = 'NL2SQL' FOR UPDATE NOWAIT"))
                    {
                        try
                        {
                            using (OracleDataReader reader = command.ExecuteReader())
                            {
                                if (!reader.Read()) throw new InvalidOperationException("Missing AI sync state.");
                                cursor = Convert.ToInt64(reader.GetValue(0), CultureInfo.InvariantCulture);
                                if (checkInterval && Convert.ToInt32(reader.GetValue(1), CultureInfo.InvariantCulture) == 1) return false;
                            }
                        }
                        catch (OracleException ex) { if (ex.Number == 54) return false; throw; }
                    }

                    Uri endpoint;
                    string key;
                    bool configured = TryConfiguration(out endpoint, out key);
                    JObject page = configured ? GetJson(endpoint.AbsoluteUri + "?after_id=" + cursor.ToString(CultureInfo.InvariantCulture) + "&limit=100", key, 4 * 1024 * 1024) : null;
                    long nextCursor = cursor;
                    JArray items = null;
                    if (page != null)
                    {
                        nextCursor = ValidatePage(page, cursor);
                        items = (JArray)page["items"];
                        foreach (JObject item in items) Upsert(connection, transaction, item);
                    }
                    using (OracleCommand reconcile = Command(connection, transaction, "AI_NOTIFICATION_PKG.RECONCILE_TARGETS"))
                    {
                        reconcile.CommandType = CommandType.StoredProcedure;
                        reconcile.ExecuteNonQuery();
                    }
                    using (OracleCommand update = Command(connection, transaction,
                        "UPDATE T_AI_NOTIFICATION_SYNC SET LAST_ID = :P_ID, LAST_POLL_AT = SYS_EXTRACT_UTC(SYSTIMESTAMP) WHERE SOURCE_NAME = 'NL2SQL'"))
                    {
                        update.Parameters.Add("P_ID", OracleDbType.Int64).Value = nextCursor;
                        update.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    return items != null && items.Count == PageSize;
                }
            }
        }

        internal static long ValidatePage(JObject page, long cursor)
        {
            try { return ValidatePageCore(page, cursor); }
            catch (ArgumentException) { throw InvalidPayload(); }
            catch (FormatException) { throw InvalidPayload(); }
            catch (OverflowException) { throw InvalidPayload(); }
            catch (InvalidCastException) { throw InvalidPayload(); }
        }

        private static long ValidatePageCore(JObject page, long cursor)
        {
            if (page == null || cursor < 0) throw InvalidPayload();
            JArray items = page["items"] as JArray;
            if (items == null || items.Count > PageSize || page["last_id"] == null || page["last_id"].Type != JTokenType.Integer)
                throw InvalidPayload();
            long previous = cursor;
            foreach (JToken token in items)
            {
                JObject item = token as JObject;
                Guid job;
                DateTimeOffset created;
                if (item == null || item["id"] == null || item["id"].Type != JTokenType.Integer
                    || (long)item["id"] <= previous
                    || !Guid.TryParse((string)item["job_id"], out job) || job == Guid.Empty
                    || item["recipient_id"] == null || item["recipient_id"].Type != JTokenType.String
                    || string.IsNullOrWhiteSpace((string)item["recipient_id"]) || ((string)item["recipient_id"]).Length > 320
                    || !DateTimeOffset.TryParse((string)item["created_at"], CultureInfo.InvariantCulture, DateTimeStyles.None, out created)
                    || !IsNotificationType((string)item["type"])) throw InvalidPayload();
                previous = (long)item["id"];
            }
            long lastId = (long)page["last_id"];
            // An empty page may report 0; it must never advance past unprocessed data.
            if ((items.Count > 0 && lastId != previous) || (items.Count == 0 && (lastId < 0 || lastId > cursor))) throw InvalidPayload();
            return previous;
        }

        private static bool IsNotificationType(string type)
        {
            return type == "query.completed" || type == "query.failed" || type == "query.cancelled";
        }

        private static void Upsert(OracleConnection connection, OracleTransaction transaction, JObject item)
        {
            const string sql = @"MERGE INTO T_NOTIFICATION N
USING (SELECT :P_HASH SOURCE_HASH FROM DUAL) S ON (N.SOURCE_HASH = S.SOURCE_HASH)
WHEN MATCHED THEN UPDATE SET N.TITLE=:P_TITLE, N.CONTENT=:P_CONTENT, N.DATETIME=:P_DATE,
 N.TARGET_TYPE=1, N.SOURCE_TYPE='AI_QUERY', N.SOURCE_KEY=:P_JOB, N.AI_RECIPIENT_ID=:P_RECIPIENT
WHEN NOT MATCHED THEN INSERT (TITLE, CONTENT, DATETIME, TARGET_TYPE, SOURCE_TYPE, SOURCE_KEY, SOURCE_HASH, AI_RECIPIENT_ID)
 VALUES (:P_TITLE, :P_CONTENT, :P_DATE, 1, 'AI_QUERY', :P_JOB, :P_HASH, :P_RECIPIENT)";
            string type = (string)item["type"];
            string title = (string)item["title"];
            if (string.IsNullOrWhiteSpace(title)) title = type == "query.completed" ? "Truy vấn AI đã hoàn thành" : type == "query.failed" ? "Truy vấn AI thất bại" : "Truy vấn AI đã hủy";
            using (OracleCommand command = Command(connection, transaction, sql))
            {
                command.Parameters.Add("P_HASH", OracleDbType.Varchar2, 80).Value = "AI_QUERY:" + ((long)item["id"]).ToString(CultureInfo.InvariantCulture);
                command.Parameters.Add("P_TITLE", OracleDbType.NVarchar2, 250).Value = Truncate(title, 250);
                command.Parameters.Add("P_CONTENT", OracleDbType.NVarchar2, 2000).Value = Truncate(string.IsNullOrWhiteSpace((string)item["message"]) ? title : (string)item["message"], 2000);
                command.Parameters.Add("P_DATE", OracleDbType.TimeStamp).Value = DateTimeOffset.Parse((string)item["created_at"], CultureInfo.InvariantCulture).LocalDateTime;
                command.Parameters.Add("P_JOB", OracleDbType.Varchar2, 80).Value = Guid.Parse((string)item["job_id"]).ToString();
                command.Parameters.Add("P_RECIPIENT", OracleDbType.Varchar2, 320).Value = NormalizeRecipient((string)item["recipient_id"]);
                command.ExecuteNonQuery();
            }
        }

        internal static JObject GetJob(long userId, long notificationId, bool result)
        {
            try { return GetJobCore(userId, notificationId, result); }
            catch (ArgumentException) { throw InvalidPayload(); }
            catch (FormatException) { throw InvalidPayload(); }
            catch (OverflowException) { throw InvalidPayload(); }
            catch (InvalidCastException) { throw InvalidPayload(); }
        }

        private static JObject GetJobCore(long userId, long notificationId, bool result)
        {
            string jobId;
            string recipient;
            using (OracleConnection connection = CreateConnection())
            using (OracleCommand command = Command(connection, null, @"SELECT N.SOURCE_KEY, N.AI_RECIPIENT_ID
FROM T_NOTIFICATION N
WHERE N.ID=:P_ID AND N.SOURCE_TYPE='AI_QUERY' AND N.TARGET_TYPE=1
AND EXISTS (SELECT 1 FROM T_NOTIFICATION_TARGET T WHERE T.NOTIFICATION_ID=N.ID AND T.USER_ID=:P_USER)
AND AI_NOTIFICATION_PKG.CAN_RECEIVE(N.AI_RECIPIENT_ID, :P_USER)=1"))
            {
                command.Parameters.Add("P_ID", OracleDbType.Int64).Value = notificationId;
                command.Parameters.Add("P_USER", OracleDbType.Int64).Value = userId;
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) throw new AiIntegrationException(404, "Không tìm thấy thông báo AI hoặc bạn không có quyền xem.");
                    jobId = Convert.ToString(reader.GetValue(0), CultureInfo.InvariantCulture);
                    recipient = Convert.ToString(reader.GetValue(1), CultureInfo.InvariantCulture);
                }
            }
            Guid parsed;
            if (!Guid.TryParse(jobId, out parsed) || parsed == Guid.Empty) throw InvalidPayload();
            Uri endpoint;
            string key;
            if (!TryConfiguration(out endpoint, out key)) throw new AiIntegrationException(503, "Chưa cấu hình kết nối dịch vụ thông báo AI.");
            string jobUrl = new Uri(endpoint, "query-jobs/" + parsed.ToString()).AbsoluteUri;
            JObject job = GetJson(jobUrl, key, 4 * 1024 * 1024);
            Guid returnedId;
            if (!Guid.TryParse((string)job["id"], out returnedId) || returnedId != parsed
                || !string.Equals(NormalizeRecipient((string)job["requested_by"]), recipient, StringComparison.Ordinal))
                throw new AiIntegrationException(403, "Thông tin người nhận của kết quả AI không khớp.");
            if (!result) return job;
            if ((string)job["status"] != "succeeded") throw new AiIntegrationException(409, "Job AI chưa hoàn thành thành công.");
            if (job["result_available"] == null || job["result_available"].Type != JTokenType.Boolean || !(bool)job["result_available"])
                throw new AiIntegrationException(410, "Kết quả AI đã hết hạn hoặc không còn khả dụng.");
            // Never follow result_url supplied by the remote service.
            JObject data = GetJson(jobUrl + "/result", key, 64 * 1024 * 1024);
            if (!Guid.TryParse((string)data["job_id"], out returnedId) || returnedId != parsed
                || !(data["columns"] is JArray) || !(data["rows"] is JArray)) throw InvalidPayload();
            return data;
        }

        internal static string NormalizeRecipient(string recipient)
        {
            if (recipient == null) return null;
            return recipient.IndexOf('@') >= 0 ? recipient.Trim().ToLowerInvariant() : recipient;
        }

        private static bool TryConfiguration(out Uri endpoint, out string key)
        {
            key = Environment.GetEnvironmentVariable("QUERY_JOB_API_KEY");
            if (string.IsNullOrWhiteSpace(key)) key = ConfigurationManager.AppSettings["APIAIKey"];
            string address = ConfigurationManager.AppSettings["APIAINotifications"];
            endpoint = null;
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(address)) return false;
            if (!Uri.TryCreate(address.Trim().TrimEnd('/'), UriKind.Absolute, out endpoint)
                || (endpoint.Scheme != "http" && endpoint.Scheme != "https")
                || !string.IsNullOrEmpty(endpoint.Query) || !string.IsNullOrEmpty(endpoint.Fragment)
                || !string.IsNullOrEmpty(endpoint.UserInfo)
                || !endpoint.AbsolutePath.EndsWith("/api/integration/notifications", StringComparison.Ordinal))
                throw new AiIntegrationException(503, "Cấu hình địa chỉ dịch vụ thông báo AI không hợp lệ.");
            key = key.Trim();
            return true;
        }

        private static JObject GetJson(string url, string key, int maximumBytes)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Headers["X-API-Key"] = key;
            request.AllowAutoRedirect = false;
            request.Timeout = 5000;
            request.ReadWriteTimeout = 15000;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK) throw InvalidPayload();
                    using (Stream stream = response.GetResponseStream())
                    using (MemoryStream buffer = new MemoryStream())
                    {
                        byte[] block = new byte[8192];
                        int count;
                        while ((count = stream.Read(block, 0, block.Length)) > 0)
                        {
                            if (buffer.Length + count > maximumBytes) throw new AiIntegrationException(502, "Dữ liệu AI vượt quá giới hạn tải cho phép.");
                            buffer.Write(block, 0, count);
                        }
                        buffer.Position = 0;
                        using (StreamReader reader = new StreamReader(buffer, Encoding.UTF8, true))
                        using (JsonTextReader json = new JsonTextReader(reader) { DateParseHandling = DateParseHandling.None, MaxDepth = 64 })
                        {
                            JObject value = JObject.Load(json);
                            if (json.Read()) throw InvalidPayload();
                            return value;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = ex.Response as HttpWebResponse)
                {
                    int status = response == null ? 503 : (int)response.StatusCode;
                    if (status == 404) throw new AiIntegrationException(404, "Job hoặc kết quả AI không còn tồn tại.");
                    if (status == 409) throw new AiIntegrationException(409, "Job AI chưa hoàn thành thành công.");
                    if (status == 410) throw new AiIntegrationException(410, "Kết quả AI đã hết hạn.");
                    throw new AiIntegrationException(status == 401 || status == 503 ? 503 : 502, "Không thể kết nối dịch vụ AI. Vui lòng thử lại sau.");
                }
            }
            catch (JsonException) { throw InvalidPayload(); }
        }

        private static AiIntegrationException InvalidPayload() { return new AiIntegrationException(502, "Dữ liệu phản hồi từ dịch vụ AI không hợp lệ."); }
        private static string Truncate(string value, int length) { return value.Length <= length ? value : value.Substring(0, length); }
        private static OracleCommand Command(OracleConnection connection, OracleTransaction transaction, string sql)
        {
            return new OracleCommand(sql, connection) { BindByName = true, CommandTimeout = 15, Transaction = transaction };
        }
        private static OracleConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectDB"];
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ConfigurationErrorsException("Missing backend ConnectDB.");
            return new OracleConnection(connectionString);
        }
    }

    internal sealed class AiIntegrationException : Exception
    {
        internal int StatusCode { get; private set; }
        internal AiIntegrationException(int statusCode, string message) : base(message) { StatusCode = statusCode; }
    }
}
