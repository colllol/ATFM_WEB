using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class AdsBPerformanceDAL
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

        public AdsBPerformanceResponse GetData(AdsBPerformanceRequest request)
        {
            DateTime from;
            DateTime to;
            ValidateRequest(request, out from, out to);
            string permType = NormalizeFilter(request.PermType);
            string oper = NormalizeFilter(request.Oper);
            List<LogItem> items = LoadLogs(from, to, permType, oper);

            List<AdsBPerformanceTrend> trend = items
                .GroupBy(x => x.FlightDate.Date)
                .OrderBy(x => x.Key)
                .Select(x => new AdsBPerformanceTrend
                {
                    Key = x.Key.ToString("yyyy-MM-dd"),
                    Label = x.Key.ToString("dd/MM"),
                    Ld = x.Count(y => String.Equals(y.PermType, "LD", StringComparison.OrdinalIgnoreCase)),
                    Of = x.Count(y => String.Equals(y.PermType, "O/F", StringComparison.OrdinalIgnoreCase))
                }).ToList();

            return new AdsBPerformanceResponse
            {
                Source = "T_TRACKS_LOG",
                ServerTime = DateTime.Now.ToString("HH:mm:ss"),
                Total = items.Count,
                Ld = items.Count(x => String.Equals(x.PermType, "LD", StringComparison.OrdinalIgnoreCase)),
                Of = items.Count(x => String.Equals(x.PermType, "O/F", StringComparison.OrdinalIgnoreCase)),
                OperatorCount = items.Select(x => x.Oper).Where(x => !String.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
                Trend = trend,
                Rows = items.Select((item, index) => new AdsBPerformanceRow
                {
                    No = index + 1,
                    Id = item.Id,
                    Callsign = item.Callsign,
                    Oper = item.Oper,
                    PermType = item.PermType,
                    FromAirp = item.FromAirp,
                    ToAirp = item.ToAirp,
                    Etd = item.Etd,
                    Eta = item.Eta,
                    Status = item.Status,
                    StatusText = item.Status == 1 ? "VVHN" : (item.Status == 2 ? "VVHM" : "Không xác định"),
                    Date = item.FlightDate.ToString("dd/MM/yyyy"),
                    UpdatedAtUtc = item.UpdatedAtUtc
                }).ToList()
            };
        }

        public List<string> GetOperators(AdsBPerformanceRequest request)
        {
            DateTime from;
            DateTime to;
            ValidateRequest(request, out from, out to);
            var result = new List<string>();
            using (var connection = CreateConnection())
            using (var command = new OracleCommand(BuildSql(true), connection))
            {
                BindFilters(command, from, to, NormalizeFilter(request.PermType), null);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) result.Add(Convert.ToString(reader[0]).Trim());
            }
            return result;
        }

        private static List<LogItem> LoadLogs(DateTime from, DateTime to, string permType, string oper)
        {
            var result = new List<LogItem>();
            using (var connection = CreateConnection())
            using (var command = new OracleCommand(BuildSql(false), connection))
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

        private static OracleConnection CreateConnection()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectDB"];
            if (String.IsNullOrWhiteSpace(connectionString))
                throw new ConfigurationErrorsException("Thiếu cấu hình AppSettings ConnectDB.");
            return new OracleConnection(connectionString);
        }

        private static string BuildSql(bool operatorsOnly)
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
            command.Parameters.Add("toDate", OracleDbType.Date).Value = to.Date.AddDays(1);
            command.Parameters.Add("permType", OracleDbType.Varchar2).Value = permType == null ? (object)DBNull.Value : permType;
            if (command.CommandText.IndexOf(":oper", StringComparison.Ordinal) >= 0)
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = oper == null ? (object)DBNull.Value : oper;
        }

        private static string NormalizeFilter(string value)
        {
            string normalized = (value ?? String.Empty).Trim().ToUpperInvariant();
            return normalized.Length == 0 || normalized == "ALL" ? null : normalized;
        }

        private static void ValidateRequest(AdsBPerformanceRequest request, out DateTime from, out DateTime to)
        {
            if (request == null) throw new ArgumentException("Dữ liệu yêu cầu không được để trống.");
            if (!DateTime.TryParseExact(request.FromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(request.ToDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc phải đúng định dạng yyyy-MM-dd.");
            if (from > to) throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
            if ((to - from).TotalDays > 3660) throw new ArgumentException("Khoảng lọc tối đa là 10 năm.");
        }
    }
}
