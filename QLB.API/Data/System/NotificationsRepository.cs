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
                DataTable rows = data.Tables.Count > 0 ? data.Tables[0] : new DataTable();
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
                LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository.GetState: " + ex.Message);
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
                response.Code = oMessage.CodeSussess;
                response.Message = oMessage.GetDataSussess;
                response.Value = updated < 0 ? 0 : updated;
            }
            catch (Exception ex)
            {
                LogAPI.LogToFile(LogFileType.EXCEPTION, "NotificationsRepository.MarkAllRead: " + ex.Message);
                response.Code = oMessage.ExceptionCode;
                response.Message = oMessage.ExceptionMessage;
            }
            return response;
        }
    }
}
