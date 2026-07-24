using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class AdsBPerformanceReport : ReportPageBase
    {
        private sealed class LogItem
        {
            public long Id { get; set; }
            public string Callsign { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string Eta { get; set; }
            public int Status { get; set; }
            public DateTime FlightDate { get; set; }
            public string UpdatedAtUtc { get; set; }
            public string PermType { get; set; }
            public string Oper { get; set; }
        }

        [WebMethod]
        public static object GetData(string fromDate, string toDate, string permType, string oper)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string selectedPermType = NormalizeFilter(permType);
            string selectedOper = NormalizeFilter(oper);
            List<LogItem> items = LoadLogs(from, to, selectedPermType, selectedOper);

            var trend = items.GroupBy(x => x.FlightDate.Date)
                .OrderBy(group => group.Key)
                .Select(group => new
                {
                    key = group.Key.ToString("yyyy-MM-dd"),
                    label = group.Key.ToString("dd/MM"),
                    ld = group.Count(x => String.Equals(x.PermType, "LD", StringComparison.OrdinalIgnoreCase)),
                    of = group.Count(x => String.Equals(x.PermType, "O/F", StringComparison.OrdinalIgnoreCase))
                }).ToList();
            int ld = items.Count(x => String.Equals(x.PermType, "LD", StringComparison.OrdinalIgnoreCase));
            int of = items.Count(x => String.Equals(x.PermType, "O/F", StringComparison.OrdinalIgnoreCase));

            return new
            {
                source = "T_TRACKS_LOG",
                serverTime = DateTime.Now.ToString("HH:mm:ss"),
                total = items.Count,
                ld = ld,
                of = of,
                operatorCount = items.Select(x => x.Oper).Where(x => !String.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
                trend = trend,
                rows = items.Select((item, index) => new
                {
                    no = index + 1,
                    id = item.Id,
                    callsign = item.Callsign,
                    oper = item.Oper,
                    permType = item.PermType,
                    fromAirp = item.FromAirp,
                    toAirp = item.ToAirp,
                    etd = item.Etd,
                    eta = item.Eta,
                    status = item.Status,
                    statusText = item.Status == 1 ? "VVHN" : (item.Status == 2 ? "VVHM" : "Không xác định"),
                    date = item.FlightDate.ToString("dd/MM/yyyy"),
                    updatedAtUtc = item.UpdatedAtUtc
                }).ToList()
            };
        }

        [WebMethod]
        public static object GetOperators(string fromDate, string toDate, string permType)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string selectedPermType = NormalizeFilter(permType);
            var values = new List<string>();
            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(BuildBaseSql(true), connection))
            {
                BindFilters(command, from, to, selectedPermType, null);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) values.Add(Convert.ToString(reader[0]));
            }
            return values;
        }

        private static List<LogItem> LoadLogs(DateTime from, DateTime to, string permType, string oper)
        {
            var result = new List<LogItem>();
            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(BuildBaseSql(false), connection))
            {
                BindFilters(command, from, to, permType, oper);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new LogItem
                        {
                            Id = Convert.ToInt64(reader["TRLOG_ID"], CultureInfo.InvariantCulture),
                            Callsign = Convert.ToString(reader["CALLSIGN"]).Trim(),
                            FromAirp = Convert.ToString(reader["FROM_AIRP"]).Trim(),
                            ToAirp = Convert.ToString(reader["TO_AIRP"]).Trim(),
                            Etd = Convert.ToString(reader["ETD"]).Trim(),
                            Eta = Convert.ToString(reader["ETA"]).Trim(),
                            Status = reader["STATUS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["STATUS"], CultureInfo.InvariantCulture),
                            FlightDate = Convert.ToDateTime(reader["LOG_DATE"], CultureInfo.InvariantCulture),
                            UpdatedAtUtc = Convert.ToString(reader["UPDATED_AT_UTC"]).Trim(),
                            PermType = Convert.ToString(reader["PERMTYPE"]).Trim().ToUpperInvariant(),
                            Oper = Convert.ToString(reader["OPER_ID"]).Trim().ToUpperInvariant()
                        });
                    }
                }
            }
            return result;
        }

        private static string BuildBaseSql(bool operatorsOnly)
        {
            const string baseQuery = @"
                WITH LOG_DATA AS (
                    SELECT L.TRLOG_ID, L.CALLSIGN, L.FROM_AIRP, L.TO_AIRP,
                           L.ETD, L.ETA, L.STATUS, L.UPDATED_AT_UTC,
                           UPPER(TRIM(L.PERMTYPE)) PERMTYPE,
                           TO_DATE(L.""DATE"", 'DD-MM-YYYY') LOG_DATE,
                           (SELECT MAX(UPPER(TRIM(D.OPER_ID)))
                              FROM T_DAY_FLIGHTS_GOINGON D
                             WHERE UPPER(TRIM(D.FLIGHTNBR)) = UPPER(TRIM(L.CALLSIGN))
                               AND TRUNC(D.FLIGHTDATE) = TO_DATE(L.""DATE"", 'DD-MM-YYYY')
                               AND (UPPER(TRIM(D.PERMTYPE)) = 'O/F'
                                    OR (TRIM(D.FROM_AIRP) IS NOT NULL AND TRIM(D.TO_AIRP) IS NOT NULL))) OPER_ID
                      FROM T_TRACKS_LOG L
                     WHERE REGEXP_LIKE(L.""DATE"", '^[0-9]{2}-[0-9]{2}-[0-9]{4}$')
                ) ";
            if (operatorsOnly)
            {
                return baseQuery + @"
                    SELECT DISTINCT OPER_ID
                      FROM LOG_DATA
                     WHERE LOG_DATE >= :fromDate AND LOG_DATE < :toDate
                       AND (:permType IS NULL OR PERMTYPE = :permType)
                       AND OPER_ID IS NOT NULL
                     ORDER BY OPER_ID";
            }
            return baseQuery + @"
                SELECT TRLOG_ID, CALLSIGN, FROM_AIRP, TO_AIRP, ETD, ETA,
                       STATUS, UPDATED_AT_UTC, PERMTYPE, LOG_DATE, OPER_ID
                  FROM LOG_DATA
                 WHERE LOG_DATE >= :fromDate AND LOG_DATE < :toDate
                   AND (:permType IS NULL OR PERMTYPE = :permType)
                   AND (:oper IS NULL OR OPER_ID = :oper)
                 ORDER BY LOG_DATE, CALLSIGN, TRLOG_ID";
        }

        private static void BindFilters(OracleCommand command, DateTime from, DateTime to, string permType, string oper)
        {
            command.BindByName = true;
            command.CommandTimeout = 90;
            command.Parameters.Add("fromDate", OracleDbType.Date).Value = from.Date;
            command.Parameters.Add("toDate", OracleDbType.Date).Value = to.Date;
            command.Parameters.Add("permType", OracleDbType.Varchar2).Value = permType == null ? (object)DBNull.Value : permType;
            if (command.CommandText.IndexOf(":oper", StringComparison.Ordinal) >= 0)
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = oper == null ? (object)DBNull.Value : oper;
        }

        private static string NormalizeFilter(string value)
        {
            string normalized = (value ?? String.Empty).Trim().ToUpperInvariant();
            return normalized.Length == 0 || normalized == "ALL" ? null : normalized;
        }

        private static void ParseDateRange(string fromDate, string toDate, out DateTime from, out DateTime to)
        {
            if (!DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc không hợp lệ.");
            if (from > to) throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
            if ((to - from).TotalDays > 3660) throw new ArgumentException("Khoảng lọc tối đa là 10 năm.");
        }
    }
}
