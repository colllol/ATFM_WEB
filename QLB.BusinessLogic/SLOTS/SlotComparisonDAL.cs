using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class SlotComparisonDAL
    {
        private static readonly HashSet<string> ResultTypes = new HashSet<string>(new[] { "KQ1", "KQ2", "KQ3", "KQ4" });

        public SlotComparisonBootstrapResponse GetBootstrap()
        {
            using (OracleConnection connection = CreateConnection())
            {
                connection.Open();
                DateTime date = DateTime.Today;
                using (var command = CreateCommand(connection, "SELECT MAX(COMPARE_DATE) FROM (SELECT TRUNC(\"Date\") COMPARE_DATE FROM T_KHH INTERSECT SELECT TRUNC(FLIGHT_DATE) FROM T_SLOT_AERO)"))
                { object value = command.ExecuteScalar(); if (value != null && value != DBNull.Value) date = Convert.ToDateTime(value, CultureInfo.InvariantCulture); }
                var operators = new List<string>();
                using (var command = CreateCommand(connection, "SELECT DISTINCT CASE WHEN UPPER(\"OPER\")='VNA' THEN 'HVN' ELSE UPPER(\"OPER\") END OPER FROM T_KHH WHERE \"OPER\" IS NOT NULL ORDER BY OPER"))
                using (var reader = command.ExecuteReader()) while (reader.Read()) operators.Add(Convert.ToString(reader[0]).Trim());
                return new SlotComparisonBootstrapResponse { DefaultDate = date.ToString("yyyy-MM-dd"), Operators = operators };
            }
        }

        public SlotComparisonResponse GetResults(SlotComparisonRequest request)
        {
            if (request == null) throw new ArgumentException("Dữ liệu yêu cầu không được để trống.");
            DateTime date;
            if (!DateTime.TryParseExact(request.CompareDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                throw new ArgumentException("Ngày đối chiếu phải đúng định dạng yyyy-MM-dd.");
            string type = (request.ResultType ?? String.Empty).Trim().ToUpperInvariant();
            if (!ResultTypes.Contains(type)) throw new ArgumentException("Loại kết quả chỉ chấp nhận KQ1, KQ2, KQ3 hoặc KQ4.");
            string oper = (request.Oper ?? String.Empty).Trim().ToUpperInvariant();
            bool all = oper.Length == 0 || oper == "ALL";
            if (all) oper = "ALL";
            int pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            int pageSize = request.PageSize <= 0 ? 100 : Math.Min(request.PageSize, 8000);

            using (OracleConnection connection = CreateConnection())
            {
                connection.Open();
                Dictionary<string, int> summary = LoadSummary(connection, date.Date, oper, all);
                string from = " FROM T_SLOT_COMPARE_RESULT r INNER JOIN T_SLOT_COMPARE_RUN h ON h.ID=r.RUN_ID " +
                    "WHERE h.COMPARE_DATE=:compareDate AND r.RESULT_TYPE=:resultType" + (all ? String.Empty : " AND r.OPER=:oper");
                int total;
                using (var command = CreateCommand(connection, "SELECT COUNT(*)" + from))
                { AddBaseParameters(command, date, type, oper, all); total = Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture); }
                int totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
                pageIndex = Math.Min(pageIndex, totalPages);
                int start = ((pageIndex - 1) * pageSize) + 1, end = pageIndex * pageSize;
                string sql = "SELECT ID,RESULT_TYPE,FLIGHT_DATE,OPER,CALLSIGN,FROM_AIRP,TO_AIRP,ETD,REMARK,SOURCE_REF FROM (" +
                    "SELECT r.ID,r.RESULT_TYPE,r.FLIGHT_DATE,r.OPER,r.CALLSIGN,r.FROM_AIRP,r.TO_AIRP,r.ETD,r.REMARK,r.SOURCE_REF," +
                    "ROW_NUMBER() OVER(ORDER BY r.OPER,r.CALLSIGN,r.FROM_AIRP,r.TO_AIRP,r.ID) RN" + from +
                    ") WHERE RN BETWEEN :rowStart AND :rowEnd";
                var rows = new List<SlotComparisonRow>();
                using (var command = CreateCommand(connection, sql))
                {
                    AddBaseParameters(command, date, type, oper, all);
                    command.Parameters.Add("rowStart", OracleDbType.Int32).Value = start;
                    command.Parameters.Add("rowEnd", OracleDbType.Int32).Value = end;
                    using (var reader = command.ExecuteReader()) while (reader.Read()) rows.Add(new SlotComparisonRow
                    {
                        Id = Convert.ToInt64(reader["ID"], CultureInfo.InvariantCulture),
                        ResultType = Text(reader["RESULT_TYPE"]), FlightDate = Convert.ToDateTime(reader["FLIGHT_DATE"]).ToString("yyyy-MM-dd"),
                        Oper = Text(reader["OPER"]), Callsign = Text(reader["CALLSIGN"]), FromAirp = Text(reader["FROM_AIRP"]),
                        ToAirp = Text(reader["TO_AIRP"]), Etd = Text(reader["ETD"]), Remark = Text(reader["REMARK"]), SourceRef = Text(reader["SOURCE_REF"])
                    });
                }
                return new SlotComparisonResponse { CompareDate = date.ToString("yyyy-MM-dd"), Oper = oper, ResultType = type,
                    Summary = summary, Rows = rows, PageIndex = pageIndex, PageSize = pageSize, TotalRecords = total, TotalPages = totalPages };
            }
        }

        private static Dictionary<string, int> LoadSummary(OracleConnection connection, DateTime date, string oper, bool all)
        {
            var result = new Dictionary<string, int> { { "KQ1", 0 }, { "KQ2", 0 }, { "KQ3", 0 }, { "KQ4", 0 } };
            string sql = "SELECT r.RESULT_TYPE,COUNT(*) TOTAL FROM T_SLOT_COMPARE_RESULT r INNER JOIN T_SLOT_COMPARE_RUN h ON h.ID=r.RUN_ID " +
                "WHERE h.COMPARE_DATE=:compareDate" + (all ? String.Empty : " AND r.OPER=:oper") + " GROUP BY r.RESULT_TYPE";
            using (var command = CreateCommand(connection, sql))
            {
                command.Parameters.Add("compareDate", OracleDbType.Date).Value = date;
                if (!all) command.Parameters.Add("oper", OracleDbType.Varchar2).Value = oper;
                using (var reader = command.ExecuteReader()) while (reader.Read())
                { string key = Text(reader[0]); if (result.ContainsKey(key)) result[key] = Convert.ToInt32(reader[1], CultureInfo.InvariantCulture); }
            }
            return result;
        }

        private static void AddBaseParameters(OracleCommand command, DateTime date, string type, string oper, bool all)
        {
            command.Parameters.Add("compareDate", OracleDbType.Date).Value = date.Date;
            command.Parameters.Add("resultType", OracleDbType.Varchar2).Value = type;
            if (!all) command.Parameters.Add("oper", OracleDbType.Varchar2).Value = oper;
        }

        private static string Text(object value) { return value == null || value == DBNull.Value ? String.Empty : Convert.ToString(value).Trim(); }
        private static OracleCommand CreateCommand(OracleConnection connection, string sql)
        { return new OracleCommand(sql, connection) { BindByName = true, CommandTimeout = 90 }; }
        private static OracleConnection CreateConnection()
        {
            string value = ConfigurationManager.AppSettings["ConnectDB"];
            if (String.IsNullOrWhiteSpace(value)) throw new ConfigurationErrorsException("Thiếu cấu hình AppSettings ConnectDB.");
            return new OracleConnection(value);
        }
    }
}
