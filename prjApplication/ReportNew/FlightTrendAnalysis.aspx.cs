using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using prjBusinessLogic;

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
        public static object GetTrend(string fromDate, string toDate, string airport, string oper, string period, int yearsBack)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string mode = String.Equals(period, "month", StringComparison.OrdinalIgnoreCase) ? "month" : "day";
            if (mode == "day" && (to - from).TotalDays > 366)
                throw new ArgumentException("Ch? ?? theo ng?y h? tr? t?i ?a 367 ng?y. H?y ch?n theo th?ng cho kho?ng d?i h?n.");

            int selectedYears = Math.Max(1, Math.Min(10, yearsBack <= 0 ? 1 : yearsBack));
            string selectedAirport = NormalizeFilter(airport);
            string selectedOper = NormalizeFilter(oper);
            bool currentDay = from.Date == DateTime.Today && to.Date == DateTime.Today;
            PeriodCounts current = LoadCounts(from, to, selectedAirport, selectedOper, mode);
            var labels = BuildLabels(from, to, mode);
            var currentValues = BuildValues(current, from, labels.Count, mode);
            var comparisonPeriods = new List<object>();
            PeriodCounts closestPrevious = null;

            for (int years = 1; years <= selectedYears; years++)
            {
                DateTime compareFrom = from.AddYears(-years);
                DateTime compareTo = to.AddYears(-years);
                PeriodCounts comparison = LoadCounts(compareFrom, compareTo, selectedAirport, selectedOper, mode);
                if (closestPrevious == null) closestPrevious = comparison;

                comparisonPeriods.Add(new {
                    year = compareFrom.Year,
                    from = compareFrom.ToString("dd/MM/yyyy"),
                    to = compareTo.ToString("dd/MM/yyyy"),
                    total = comparison.Total,
                    finished = comparison.Finished,
                    delay = comparison.Delay,
                    values = BuildValues(comparison, compareFrom, labels.Count, mode)
                });
            }

            int difference = current.Total - closestPrevious.Total;
            double changePercent = closestPrevious.Total == 0
                ? (current.Total == 0 ? 0 : 100)
                : Math.Round(difference * 100.0 / closestPrevious.Total, 1);

            return new {
                source = currentDay ? "T_DAY_FLIGHTS_GOINGON" : "T_FINISHED_FLIGHTS",
                period = mode,
                yearsBack = selectedYears,
                currentFrom = from.ToString("dd/MM/yyyy"),
                currentTo = to.ToString("dd/MM/yyyy"),
                previousFrom = from.AddYears(-1).ToString("dd/MM/yyyy"),
                previousTo = to.AddYears(-1).ToString("dd/MM/yyyy"),
                currentTotal = current.Total,
                previousTotal = closestPrevious.Total,
                currentFinished = current.Finished,
                currentDelay = current.Delay,
                difference = difference,
                changePercent = changePercent,
                labels = labels,
                current = currentValues,
                comparisonPeriods = comparisonPeriods
            };
        }

        [WebMethod]
        public static object GetOperators(string fromDate, string toDate)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            DataTable data = new clsResuftAPI().GetTableApiExtension(
                "FLIGHT_STATUS_PKG", "GET_OPERATORS",
                new { P_FROM_DATE = fromDate, P_TO_DATE = toDate });
            if (data == null)
                throw new InvalidOperationException("Không lấy được danh sách hãng khai thác từ API.");
            var values = new List<string>();
            foreach (DataRow row in data.Rows)
                values.Add(Convert.ToString(row["OPER_ID"]));
            return values;
        }

        // from/to la khoang ngay bao gom ca ngay ket thuc (theo P_TO_DATE cua package).
        private static PeriodCounts LoadCounts(DateTime from, DateTime to, string airport, string oper, string mode)
        {
            var result = new PeriodCounts();
            DateTime today = DateTime.Today;
            if (from.Date < today && to.Date >= today)
            {
                AppendCounts(result, from, today.AddDays(-1), airport, oper, mode, false);
                AppendCounts(result, today, today, airport, oper, mode, true);
                return result;
            }

            AppendCounts(result, from, to, airport, oper, mode, from.Date == today && to.Date == today);
            return result;
        }

        private static void AppendCounts(PeriodCounts result, DateTime from, DateTime to, string airport, string oper, string mode, bool currentDay)
        {
            if (from.Date > to.Date) return;
            DataTable data = FlightStatusRate.LoadStatus(
                from.ToString("yyyy-MM-dd"), to.ToString("yyyy-MM-dd"), oper, airport, currentDay);
            foreach (DataRow row in data.Rows)
            {
                string state = Convert.ToString(row["FLIGHT_STATE"]);
                if (state != "FINISHED" && !state.StartsWith("DELAY", StringComparison.Ordinal)) continue;
                if (state == "FINISHED") result.Finished++;
                else result.Delay++;
                string key = BucketKey(Convert.ToDateTime(row["FLIGHTDATE"], CultureInfo.InvariantCulture), mode);
                int value;
                result.Buckets.TryGetValue(key, out value);
                result.Buckets[key] = value + 1;
            }
        }

        private static List<string> BuildLabels(DateTime from, DateTime to, string mode)
        {
            var labels = new List<string>();
            DateTime cursor = mode == "month" ? new DateTime(from.Year, from.Month, 1) : from.Date;
            DateTime end = mode == "month" ? new DateTime(to.Year, to.Month, 1) : to.Date;
            while (cursor <= end)
            {
                labels.Add(mode == "month" ? cursor.ToString("MM/yyyy") : cursor.ToString("dd/MM"));
                cursor = mode == "month" ? cursor.AddMonths(1) : cursor.AddDays(1);
            }
            return labels;
        }

        private static List<int> BuildValues(PeriodCounts counts, DateTime from, int count, string mode)
        {
            var values = new List<int>();
            DateTime cursor = mode == "month" ? new DateTime(from.Year, from.Month, 1) : from.Date;
            for (int index = 0; index < count; index++)
            {
                values.Add(GetCount(counts.Buckets, BucketKey(cursor, mode)));
                cursor = mode == "month" ? cursor.AddMonths(1) : cursor.AddDays(1);
            }
            return values;
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
            if (from > to || to > DateTime.Today)
                throw new ArgumentException("Khoảng ngày phải hợp lệ và không vượt quá ngày hiện tại.");
        }
    }
}
