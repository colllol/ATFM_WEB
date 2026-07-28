using System;
using System.Configuration;
using System.Data;
using System.Globalization;
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
