using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class FlightPlanDailyComparison : ReportPageBase
    {
        private sealed class FlightRow
        {
            public string Callsign { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string Eta { get; set; }
            public string FlightType { get; set; }
            public string OperId { get; set; }
            public string PermId { get; set; }
            public string PermNbr { get; set; }
        }

        private sealed class DetailFlight
        {
            public string callsign { get; set; }
            public string fromAirp { get; set; }
            public string toAirp { get; set; }
            public string etd { get; set; }
            public string eta { get; set; }
            public string flightType { get; set; }
            public string permNbr { get; set; }
        }

        private sealed class AirportCount
        {
            public string airport { get; set; }
            public int date1 { get; set; }
            public int date2 { get; set; }
        }

        private sealed class ComparisonDay
        {
            public DateTime Date { get; set; }
            public List<FlightRow> Flights { get; set; }
            public int Total { get { return Flights.Count; } }
            public int Seasonal { get { return CountDistinctByType("SC"); } }
            public int AdHoc { get { return CountDistinctByType("NO"); } }
            public int SeasonalPermits { get { return CountDistinctPermitsByType("SC"); } }
            public int AdHocPermits { get { return CountDistinctPermitsByType("NO"); } }
            private int CountDistinctByType(string flightType)
            {
                return Flights.Where(x => String.Equals(x.FlightType, flightType, StringComparison.OrdinalIgnoreCase))
                    .GroupBy(Key, StringComparer.OrdinalIgnoreCase)
                    .Count();
            }
            private int CountDistinctPermitsByType(string flightType)
            {
                return Flights.Where(x => String.Equals(x.FlightType, flightType, StringComparison.OrdinalIgnoreCase))
                    .Select(x => (x.PermId ?? String.Empty).Trim().ToUpperInvariant())
                    .Where(x => x.Length > 0)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count();
            }
        }

        [WebMethod]
        public static object GetComparison(string date1, string airport, string oper)
        {
            DateTime first = ParseDate(date1);
            if (first.Date > DateTime.Today) throw new ArgumentException("Ngày so sánh 1 không được vượt quá ngày hiện tại.");
            DateTime second = first.Date.AddDays(-7);
            string selectedAirport = NormalizeFilter(airport);
            string selectedOper = NormalizeFilter(oper);
            ComparisonDay day1 = LoadDay(first.Date, selectedAirport, selectedOper);
            ComparisonDay day2 = LoadDay(second, selectedAirport, selectedOper);
            Dictionary<string, FlightRow> map1 = day1.Flights.GroupBy(Key).ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);
            Dictionary<string, FlightRow> map2 = day2.Flights.GroupBy(Key).ToDictionary(x => x.Key, x => x.First(), StringComparer.OrdinalIgnoreCase);
            List<FlightRow> only1 = map1.Where(x => !map2.ContainsKey(x.Key)).Select(x => x.Value).OrderBy(x => x.Callsign).ToList();
            List<FlightRow> only2 = map2.Where(x => !map1.ContainsKey(x.Key)).Select(x => x.Value).OrderBy(x => x.Callsign).ToList();
            return new
            {
                date1 = first.ToString("yyyy-MM-dd"), date2 = second.ToString("yyyy-MM-dd"), airport = selectedAirport ?? "ALL", oper = selectedOper ?? "ALL",
                day1 = new { total = day1.Total, seasonal = day1.Seasonal, adHoc = day1.AdHoc, seasonalPermits = day1.SeasonalPermits, adHocPermits = day1.AdHocPermits },
                day2 = new { total = day2.Total, seasonal = day2.Seasonal, adHoc = day2.AdHoc, seasonalPermits = day2.SeasonalPermits, adHocPermits = day2.AdHocPermits },
                permits = new { scDate1 = DistinctPermitDetails(day1.Flights, "SC"), scDate2 = DistinctPermitDetails(day2.Flights, "SC"), noDate1 = DistinctPermitDetails(day1.Flights, "NO"), noDate2 = DistinctPermitDetails(day2.Flights, "NO") },
                airportChart = BuildAirportChart(day1.Flights, day2.Flights),
                differences = new { total = only1.Count + only2.Count, onlyDate1 = only1.Count, onlyDate2 = only2.Count, date1 = only1.Select(ToDetail).ToList(), date2 = only2.Select(ToDetail).ToList() }
            };
        }

        [WebMethod]
        public static object GetAirports()
        {
            const string sql = @"SELECT AIRPORT FROM (
                                   SELECT DISTINCT UPPER(TRIM(FROM_AIRP)) AIRPORT FROM T_DAY_FLIGHTS WHERE FROM_AIRP IS NOT NULL
                                   UNION SELECT DISTINCT UPPER(TRIM(TO_AIRP)) AIRPORT FROM T_DAY_FLIGHTS WHERE TO_AIRP IS NOT NULL
                                 ) WHERE AIRPORT IS NOT NULL ORDER BY AIRPORT";
            List<string> values = new List<string>();
            using (OracleConnection connection = CreateConnection())
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader()) while (reader.Read()) values.Add(Convert.ToString(reader["AIRPORT"], CultureInfo.InvariantCulture).Trim());
            }
            return values;
        }

        [WebMethod]
        public static object GetOperators()
        {
            const string sql = @"SELECT DISTINCT UPPER(TRIM(OPER_ID)) OPER_ID FROM T_DAY_FLIGHTS WHERE OPER_ID IS NOT NULL ORDER BY OPER_ID";
            List<string> values = new List<string>();
            using (OracleConnection connection = CreateConnection())
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader()) while (reader.Read()) values.Add(Text(reader["OPER_ID"]));
            }
            return values;
        }

        private static ComparisonDay LoadDay(DateTime date, string airport, string oper)
        {
            const string sql = @"SELECT FLIGHTNBR, FROM_AIRP, TO_AIRP, ETD, ETA, FLIGHT_TYPE, OPER_ID, PERMNBR, PERM_ID
                                   FROM T_DAY_FLIGHTS
                                  WHERE FLIGHTDATE >= :flightDate AND FLIGHTDATE < :nextDate
                                    AND PERMNBR IS NOT NULL AND UPPER(TRIM(PERMNBR)) <> 'NOPERM'
                                    AND (:airport IS NULL OR UPPER(TRIM(FROM_AIRP)) = :airport OR UPPER(TRIM(TO_AIRP)) = :airport)
                                    AND (:oper IS NULL OR UPPER(TRIM(OPER_ID)) = :oper)";
            List<FlightRow> flights = new List<FlightRow>();
            using (OracleConnection connection = CreateConnection())
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                command.Parameters.Add("flightDate", OracleDbType.Date).Value = date;
                command.Parameters.Add("nextDate", OracleDbType.Date).Value = date.AddDays(1);
                command.Parameters.Add("airport", OracleDbType.Varchar2).Value = airport == null ? (object)DBNull.Value : airport;
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = oper == null ? (object)DBNull.Value : oper;
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) flights.Add(new FlightRow { Callsign = Text(reader["FLIGHTNBR"]), FromAirp = Text(reader["FROM_AIRP"]), ToAirp = Text(reader["TO_AIRP"]), Etd = Text(reader["ETD"]), Eta = Text(reader["ETA"]), FlightType = Text(reader["FLIGHT_TYPE"]).ToUpperInvariant(), OperId = Text(reader["OPER_ID"]).ToUpperInvariant(), PermNbr = Text(reader["PERMNBR"]), PermId = Text(reader["PERM_ID"]) });
                }
            }
            return new ComparisonDay { Date = date, Flights = flights };
        }

        private static OracleConnection CreateConnection() { return new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString); }
        private static string Text(object value) { return value == null || value == DBNull.Value ? String.Empty : Convert.ToString(value, CultureInfo.InvariantCulture).Trim(); }
        private static string Key(FlightRow row) { return String.Join("|", new[] { row.Callsign, row.FromAirp, row.ToAirp, row.Etd, row.Eta }.Select(x => (x ?? String.Empty).Trim().ToUpperInvariant())); }
        private static List<AirportCount> BuildAirportChart(IEnumerable<FlightRow> first, IEnumerable<FlightRow> second)
        {
            Dictionary<string, int> firstCounts = CountAirports(first);
            Dictionary<string, int> secondCounts = CountAirports(second);
            return firstCounts.Keys.Union(secondCounts.Keys, StringComparer.OrdinalIgnoreCase)
                .Select(code => new AirportCount { airport = code, date1 = firstCounts.ContainsKey(code) ? firstCounts[code] : 0, date2 = secondCounts.ContainsKey(code) ? secondCounts[code] : 0 })
                .OrderByDescending(x => x.date1 + x.date2).ThenBy(x => x.airport).Take(24).ToList();
        }
        private static Dictionary<string, int> CountAirports(IEnumerable<FlightRow> flights)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (FlightRow flight in flights)
            {
                HashSet<string> airports = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (!String.IsNullOrWhiteSpace(flight.FromAirp)) airports.Add(flight.FromAirp.Trim().ToUpperInvariant());
                if (!String.IsNullOrWhiteSpace(flight.ToAirp)) airports.Add(flight.ToAirp.Trim().ToUpperInvariant());
                foreach (string airport in airports) counts[airport] = counts.ContainsKey(airport) ? counts[airport] + 1 : 1;
            }
            return counts;
        }
        private static DetailFlight ToDetail(FlightRow row) { return new DetailFlight { callsign = row.Callsign, fromAirp = row.FromAirp, toAirp = row.ToAirp, etd = row.Etd, eta = row.Eta, flightType = row.FlightType, permNbr = row.PermNbr }; }
        private static List<DetailFlight> DistinctPermitDetails(IEnumerable<FlightRow> flights, string flightType)
        {
            return flights.Where(x => String.Equals(x.FlightType, flightType, StringComparison.OrdinalIgnoreCase))
                .GroupBy(x => (x.PermId ?? String.Empty).Trim(), StringComparer.OrdinalIgnoreCase)
                .Where(x => x.Key.Length > 0)
                .Select(x => ToDetail(x.First()))
                .OrderBy(x => x.permNbr)
                .ToList();
        }
        private static string NormalizeFilter(string value) { string result = (value ?? String.Empty).Trim().ToUpperInvariant(); return result.Length == 0 || result == "ALL" ? null : result; }
        private static DateTime ParseDate(string value) { DateTime result; if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) throw new ArgumentException("Ngày so sánh không hợp lệ."); return result.Date; }
    }
}
