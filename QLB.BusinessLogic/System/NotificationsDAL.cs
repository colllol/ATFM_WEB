using System.Data;
using Oracle.DataAccess.Client;
using ShareDLL;

namespace QLB.BusinessLogic
{
    public class NotificationsDAL
    {
        public DataSet GetState(long userId)
        {
            return new oDataProvider().ExecuteDatase(
                "NOTIFICATION_PKG",
                "GET_STATE",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId });
        }

        public DataSet GetPage(long userId, int status, int pageIndex)
        {
            return new oDataProvider().ExecuteDatase(
                "NOTIFICATION_PKG",
                "GET_PAGE",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_STATUS", OracleDbType.Int64) { Value = status },
                new OracleParameter("P_PAGE_INDEX", OracleDbType.Int64) { Value = pageIndex },
                new OracleParameter("P_PAGE_SIZE", OracleDbType.Int64) { Value = 100 });
        }

        public long MarkRead(long userId, long id)
        {
            return new oDataProvider().ExecuteNonQuery(
                "NOTIFICATION_PKG",
                "MARK_READ",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_ID", OracleDbType.Int64) { Value = id },
                new OracleParameter("P_UPDATED_COUNT", OracleDbType.Int64, ParameterDirection.Output));
        }

        public long MarkAllRead(long userId)
        {
            return new oDataProvider().ExecuteNonQuery(
                "NOTIFICATION_PKG",
                "MARK_ALL_READ",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_UPDATED_COUNT", OracleDbType.Int64, ParameterDirection.Output));
        }

        public long Create(string title, string content, string userIds)
        {
            return new oDataProvider().ExecuteNonQuery(
                "NOTIFICATION_PKG",
                "CREATE_NOTIFICATION",
                new OracleParameter("P_TITLE", OracleDbType.NVarchar2) { Value = title },
                new OracleParameter("P_CONTENT", OracleDbType.NVarchar2) { Value = content },
                new OracleParameter("P_USER_IDS", OracleDbType.Varchar2)
                {
                    Value = string.IsNullOrWhiteSpace(userIds)
                        ? (object)System.DBNull.Value
                        : userIds
                },
                new OracleParameter("P_ID", OracleDbType.Int64, ParameterDirection.Output));
        }
    }
}
