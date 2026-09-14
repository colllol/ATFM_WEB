using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using Oracle.DataAccess.Client;
using ShareDLL;

namespace QLB.BusinessLogic
{
    public class NotificationsDAL
    {
        public DataSet GetState(long userId)
        {
            return ExecuteDataSet(
                "AI_NOTIFICATION_PKG.GET_STATE",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
        }

        public DataSet GetPage(long userId, int status, int pageIndex, string source = "ALL")
        {
            return ExecuteDataSet(
                "AI_NOTIFICATION_PKG.GET_PAGE",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_STATUS", OracleDbType.Int64) { Value = status },
                new OracleParameter("P_PAGE_INDEX", OracleDbType.Int64) { Value = pageIndex },
                new OracleParameter("P_PAGE_SIZE", OracleDbType.Int64) { Value = 100 },
                new OracleParameter("P_SOURCE_TYPE", OracleDbType.Varchar2) { Value = source },
                new OracleParameter("P_DATA_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                new OracleParameter("P_TOTAL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
        }

        public long MarkRead(long userId, long id)
        {
            return ExecuteUpdate(
                "AI_NOTIFICATION_PKG.MARK_READ",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_ID", OracleDbType.Int64) { Value = id });
        }

        public long MarkAllRead(long userId, string source = "ALL")
        {
            return ExecuteUpdate(
                "AI_NOTIFICATION_PKG.MARK_ALL_READ",
                new OracleParameter("P_USER_ID", OracleDbType.Int64) { Value = userId },
                new OracleParameter("P_SOURCE_TYPE", OracleDbType.Varchar2) { Value = source });
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
                OracleParameter output = new OracleParameter("P_UPDATED_COUNT", OracleDbType.Int64, ParameterDirection.Output);
                command.Parameters.Add(output);
                connection.Open();
                // AI_NOTIFICATION_PKG does not commit: the API owns this transaction.
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    long updated = Convert.ToInt64(output.Value.ToString(), CultureInfo.InvariantCulture);
                    if (updated < 0) throw new InvalidOperationException("Invalid notification update count.");
                    transaction.Commit();
                    return updated;
                }
            }
        }

        private static OracleConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectDB"];
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ConfigurationErrorsException("Missing AppSettings ConnectDB.");
            return new OracleConnection(connectionString);
        }
    }
}
