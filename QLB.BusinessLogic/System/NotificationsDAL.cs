using System.Data;
using Oracle.DataAccess.Client;
using ShareDLL;

namespace QLB.BusinessLogic
{
    public class NotificationsDAL
    {
        public DataSet GetState()
        {
            return new oDataProvider().ExecuteDatase(
                "NOTIFICATION_PKG", "GET_STATE", new OracleParameter[0]);
        }

        public DataSet GetPage(int status, int pageIndex)
        {
            return new oDataProvider().ExecuteDatase(
                "NOTIFICATION_PKG",
                "GET_PAGE",
                new OracleParameter("P_STATUS", OracleDbType.Int64) { Value = status },
                new OracleParameter("P_PAGE_INDEX", OracleDbType.Int64) { Value = pageIndex },
                new OracleParameter("P_PAGE_SIZE", OracleDbType.Int64) { Value = 100 });
        }

        public long MarkRead(long id)
        {
            return new oDataProvider().ExecuteNonQuery(
                "NOTIFICATION_PKG",
                "MARK_READ",
                new OracleParameter("P_ID", OracleDbType.Int64) { Value = id },
                new OracleParameter("P_UPDATED_COUNT", OracleDbType.Int64, ParameterDirection.Output));
        }

        public long MarkAllRead()
        {
            return new oDataProvider().ExecuteNonQuery(
                "NOTIFICATION_PKG",
                "MARK_ALL_READ",
                new OracleParameter("P_UPDATED_COUNT", OracleDbType.Int64, ParameterDirection.Output));
        }
    }
}
