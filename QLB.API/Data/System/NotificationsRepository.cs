using System;
using System.Data;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Oracle.DataAccess.Client;
using QLB.API.Models;
using QLB.API.Common;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class NotificationsRepository
    {
        public NotificationResponse GetState(long userId)
        {
            NotificationResponse response = new NotificationResponse();
            try
            {
                DataSet data = new NotificationsDAL().GetState(userId);
                if (data.Tables.Count == 0)
                    throw new InvalidOperationException("AI_NOTIFICATION_PKG.GET_STATE returned no result table.");

                DataTable rows = data.Tables[0];
                EnsureColumns(rows, "ID", "TITLE", "CONTENT", "DATETIME", "SOURCE_TYPE", "SOURCE_KEY", "STATUS", "UNREAD_COUNT", "AI_UNREAD_COUNT");
                int unreadCount = rows.Rows.Count > 0
                    ? Convert.ToInt32(rows.Rows[0]["UNREAD_COUNT"], CultureInfo.InvariantCulture)
                    : 0;

                // Counts are computed over all visible unread rows by the package,
                // before the bell's 50-row limit is applied.
                response.AIUnreadCount = rows.Rows.Count > 0
                    ? Convert.ToInt32(rows.Rows[0]["AI_UNREAD_COUNT"], CultureInfo.InvariantCulture)
                    : 0;

                if (rows.Columns.Contains("UNREAD_COUNT"))
                    rows.Columns.Remove("UNREAD_COUNT");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = unreadCount;
                response.ListValue = rows;
                response.SumRecord = rows.Rows.Count.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                SetFailure(response, "GetState", ex);
            }
            return response;
        }

        public NotificationResponse GetPage(long userId, int status, int pageIndex, string source = "ALL")
        {
            NotificationResponse response = new NotificationResponse();
            try
            {
                DataSet data = new NotificationsDAL().GetPage(userId, status, pageIndex, source);
                if (data.Tables.Count < 2 || data.Tables[1].Rows.Count == 0)
                    throw new InvalidOperationException("AI_NOTIFICATION_PKG.GET_PAGE returned an invalid result.");

                DataTable rows = data.Tables[0];
                EnsureColumns(rows, "ID", "TITLE", "CONTENT", "DATETIME", "SOURCE_TYPE", "SOURCE_KEY", "STATUS");
                EnsureColumns(data.Tables[1], "TOTAL_COUNT", "UNREAD_COUNT", "AI_UNREAD_COUNT");
                DataRow totals = data.Tables[1].Rows[0];
                int totalCount = Convert.ToInt32(totals["TOTAL_COUNT"], CultureInfo.InvariantCulture);
                int unreadCount = Convert.ToInt32(totals["UNREAD_COUNT"], CultureInfo.InvariantCulture);
                response.AIUnreadCount = Convert.ToInt32(totals["AI_UNREAD_COUNT"], CultureInfo.InvariantCulture);

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = unreadCount;
                response.ListValue = rows;
                response.SumRecord = totalCount.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                SetFailure(response, "GetPage", ex);
            }
            return response;
        }

        public ReponseReportEntity MarkRead(long userId, long id)
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                long updated = new NotificationsDAL().MarkRead(userId, id);
                if (updated < 0)
                    throw new InvalidOperationException("AI_NOTIFICATION_PKG.MARK_READ did not return an update count.");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = updated;
                response.SumRecord = "0";
            }
            catch (Exception ex)
            {
                SetFailure(response, "MarkRead", ex);
            }
            return response;
        }

        public ReponseReportEntity MarkAllRead(long userId, string source = "ALL")
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                long updated = new NotificationsDAL().MarkAllRead(userId, source);
                if (updated < 0)
                    throw new InvalidOperationException("AI_NOTIFICATION_PKG.MARK_ALL_READ did not return an update count.");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = updated;
                response.SumRecord = "0";
            }
            catch (Exception ex)
            {
                SetFailure(response, "MarkAllRead", ex);
            }
            return response;
        }

        public ReponseReportEntity Create(string title, string content, string userIds)
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                long id = new NotificationsDAL().Create(title, content, userIds);
                if (id <= 0)
                    throw new InvalidOperationException("NOTIFICATION_PKG.CREATE_NOTIFICATION did not return an ID.");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = id;
            }
            catch (Exception ex)
            {
                SetFailure(response, "Create", ex);
            }
            return response;
        }

        private static void EnsureColumns(DataTable table, params string[] columnNames)
        {
            foreach (string columnName in columnNames)
            {
                if (!table.Columns.Contains(columnName))
                    throw new InvalidOperationException("Notification result is missing column " + columnName + ".");
            }
        }

        public JObject GetAiJob(long userId, long id, bool result)
        {
            // The service validates local notification visibility before requesting
            // any upstream job or result; the caller never supplies a remote job ID.
            return AiNotificationService.GetJob(userId, id, result);
        }

        private static void SetFailure(ReponseReportEntity response, string operation, Exception exception)
        {
            OracleException oracle = exception as OracleException;
            if (oracle != null && oracle.Number == 20141)
                throw new AiIntegrationException(403, "Tài khoản không hoạt động hoặc không hợp lệ.");
            if (oracle != null && oracle.Number == 20142)
                throw new AiIntegrationException(400, "Nguồn thông báo không hợp lệ.");

            // Do not put provider payloads, connection strings or SQL into responses/logs.
            LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository." + operation + ": " + exception.GetType().Name);
            response.Code = oMessage.ExceptionCode;
            response.Message = "Không thể xử lý dữ liệu thông báo.";
        }
    }
}
