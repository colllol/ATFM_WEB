using System;
using System.Data;
using QLB.API.Models;
using QLB.API.Common;
using QLB.BusinessLogic;

namespace QLB.API.Data
{
    public class NotificationsRepository
    {
        public ReponseReportEntity GetState()
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                DataSet data = new NotificationsDAL().GetState();
                if (data.Tables.Count == 0)
                    throw new InvalidOperationException("NOTIFICATION_PKG.GET_STATE returned no result table.");

                DataTable rows = data.Tables[0];
                EnsureColumns(rows, "ID", "TITLE", "CONTENT", "DATETIME", "STATUS", "UNREAD_COUNT");
                int unreadCount = rows.Rows.Count > 0
                    ? Convert.ToInt32(rows.Rows[0]["UNREAD_COUNT"])
                    : 0;

                if (rows.Columns.Contains("UNREAD_COUNT"))
                    rows.Columns.Remove("UNREAD_COUNT");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = unreadCount;
                response.ListValue = rows;
                response.SumRecord = rows.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository.GetState: " + ex);
                response.Code = oMessage.ExceptionCode;
                response.Message = oMessage.ExceptionMessage;
            }
            return response;
        }

        public ReponseReportEntity GetPage(int status, int pageIndex)
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                DataSet data = new NotificationsDAL().GetPage(status, pageIndex);
                if (data.Tables.Count < 2 || data.Tables[1].Rows.Count == 0)
                    throw new InvalidOperationException("NOTIFICATION_PKG.GET_PAGE returned an invalid result.");

                DataTable rows = data.Tables[0];
                EnsureColumns(rows, "ID", "TITLE", "CONTENT", "DATETIME", "STATUS");
                EnsureColumns(data.Tables[1], "TOTAL_COUNT", "UNREAD_COUNT");
                DataRow totals = data.Tables[1].Rows[0];
                int totalCount = Convert.ToInt32(totals["TOTAL_COUNT"]);
                int unreadCount = Convert.ToInt32(totals["UNREAD_COUNT"]);

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = unreadCount;
                response.ListValue = rows;
                response.SumRecord = totalCount.ToString();
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository.GetPage: " + ex);
                response.Code = oMessage.ExceptionCode;
                response.Message = oMessage.ExceptionMessage;
            }
            return response;
        }

        public ReponseReportEntity MarkRead(long id)
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                long updated = new NotificationsDAL().MarkRead(id);
                if (updated < 0)
                    throw new InvalidOperationException("NOTIFICATION_PKG.MARK_READ did not return an update count.");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = updated;
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository.MarkRead: " + ex);
                response.Code = oMessage.ExceptionCode;
                response.Message = oMessage.ExceptionMessage;
            }
            return response;
        }

        public ReponseReportEntity MarkAllRead()
        {
            ReponseReportEntity response = new ReponseReportEntity();
            try
            {
                long updated = new NotificationsDAL().MarkAllRead();
                if (updated < 0)
                    throw new InvalidOperationException("NOTIFICATION_PKG.MARK_ALL_READ did not return an update count.");

                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = updated;
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository.MarkAllRead: " + ex);
                response.Code = oMessage.ExceptionCode;
                response.Message = oMessage.ExceptionMessage;
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
    }
}
