using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class FlightTrendAnalysis : ReportPageBase
    {
        private sealed class PeriodCounts
        {
            public readonly Dictionary<string, int> Buckets = new Dictionary<string, int>(StringComparer.Ordinal);
            public int Finished { get; set; }
            public int Delay { get; set; }
            public int Total { get { return Finished + Delay; } }
        }

        [WebMethod]
        public static object GetTrend(string fromDate, string toDate, string airport, string oper, string period)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string mode = String.Equals(period, "month", StringComparison.OrdinalIgnoreCase) ? "month" : "day";
            if (mode == "day" && (to - from).TotalDays > 366)
                throw new ArgumentException("Chế độ theo ngày hỗ trợ tối đa 367 ngày. Hãy chọn theo tháng cho khoảng dài hơn.");

            string selectedAirport = NormalizeFilter(airport);
            string selectedOper = NormalizeFilter(oper);
            DateTime compareFrom = from.AddYears(-1);
            DateTime compareTo = to.AddYears(-1);
            PeriodCounts current = LoadCounts(from, to, selectedAirport, selectedOper, mode);
            PeriodCounts previous = LoadCounts(compareFrom, compareTo, selectedAirport, selectedOper, mode);
            var labels = new List<string>();
            var currentValues = new List<int>();
            var previousValues = new List<int>();
            DateTime cursor = mode == "month" ? new DateTime(from.Year, from.Month, 1) : from.Date;
            DateTime end = mode == "month" ? new DateTime(to.Year, to.Month, 1) : to.Date;

            while (cursor <= end)
            {
                string currentKey = BucketKey(cursor, mode);
                string previousKey = BucketKey(cursor.AddYears(-1), mode);
                labels.Add(mode == "month" ? cursor.ToString("MM/yyyy") : cursor.ToString("dd/MM"));
                currentValues.Add(GetCount(current.Buckets, currentKey));
                previousValues.Add(GetCount(previous.Buckets, previousKey));
                cursor = mode == "month" ? cursor.AddMonths(1) : cursor.AddDays(1);
            }

            int difference = current.Total - previous.Total;
            double changePercent = previous.Total == 0
                ? (current.Total == 0 ? 0 : 100)
                : Math.Round(difference * 100.0 / previous.Total, 1);

            return new {
                source = "T_FINISHED_FLIGHTS",
                period = mode,
                currentFrom = from.ToString("dd/MM/yyyy"),
                currentTo = to.ToString("dd/MM/yyyy"),
                previousFrom = compareFrom.ToString("dd/MM/yyyy"),
                previousTo = compareTo.ToString("dd/MM/yyyy"),
                currentTotal = current.Total,
                previousTotal = previous.Total,
                currentFinished = current.Finished,
                currentDelay = current.Delay,
                difference = difference,
                changePercent = changePercent,
                labels = labels,
                current = currentValues,
                previous = previousValues
            };
        }

        [WebMethod]
        public static object GetOperators(string fromDate, string toDate)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            var values = new List<string>();
            const string sql = @"SELECT DISTINCT UPPER(TRIM(OPER_ID)) OPER_ID
                                   FROM T_FINISHED_FLIGHTS
                                  WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL
                                    AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate
                                  ORDER BY OPER_ID";
            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
                connection.Open();
                using (var reader = command.ExecuteReader())
                    while (reader.Read()) values.Add(Convert.ToString(reader["OPER_ID"]));
            }
            return values;
        }

        private static PeriodCounts LoadCounts(DateTime from, DateTime to, string airport, string oper, string mode)
        {
            var result = new PeriodCounts();
            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(FlightStatusRate.BuildHistoricalStatusSql(), connection))
            {
                command.BindByName = true;
                command.CommandTimeout = 120;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = oper == null ? (object)DBNull.Value : oper;
                command.Parameters.Add("airport", OracleDbType.Varchar2).Value = airport == null ? (object)DBNull.Value : airport;
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string state = Convert.ToString(reader["FLIGHT_STATE"]);
                        if (state != "FINISHED" && !state.StartsWith("DELAY", StringComparison.Ordinal)) continue;
                        if (state == "FINISHED") result.Finished++;
                        else result.Delay++;
                        string key = BucketKey(Convert.ToDateTime(reader["FLIGHTDATE"], CultureInfo.InvariantCulture), mode);
                        int value;
                        result.Buckets.TryGetValue(key, out value);
                        result.Buckets[key] = value + 1;
                    }
                }
            }
            return result;
        }

        private static string BucketKey(DateTime value, string mode)
        {
            return mode == "month" ? value.ToString("yyyy-MM") : value.ToString("yyyy-MM-dd");
        }

        private static int GetCount(IDictionary<string, int> values, string key)
        {
            int value;
            return values.TryGetValue(key, out value) ? value : 0;
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
        }
    }
}
