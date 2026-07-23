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

        public long MarkAllRead()
        {
            return new oDataProvider().ExecuteNonQuery(
                "NOTIFICATION_PKG",
                "MARK_ALL_READ",
                new OracleParameter("P_UPDATED_COUNT", OracleDbType.Int64, ParameterDirection.Output));
        }
    }
}
