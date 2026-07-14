using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class AirportTakeoffLanding : ReportPageBase
    {
        private sealed class AirportMetric
        {
            public string Code { get; set; }
            public int Departures { get; set; }
            public int Arrivals { get; set; }
        }

        [WebMethod]
        public static object GetSummary(string fromDate, string toDate, string airport)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string selectedAirport = NormalizeAirport(airport, true);
            var metrics = new Dictionary<string, AirportMetric>(StringComparer.OrdinalIgnoreCase);

            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = CreateCommand(connection, from, to, selectedAirport))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string state = Convert.ToString(reader["FLIGHT_STATE"]);
                        if (!IsCompletedOrDelayed(state)) continue;

                        string fromAirport = NormalizeAirport(Convert.ToString(reader["FROM_AIRP"]), false);
                        string toAirport = NormalizeAirport(Convert.ToString(reader["TO_AIRP"]), false);
                        if (IsVietnamAirport(fromAirport) && (selectedAirport == null || fromAirport == selectedAirport))
                            GetMetric(metrics, fromAirport).Departures++;
                        if (IsVietnamAirport(toAirport) && (selectedAirport == null || toAirport == selectedAirport))
                            GetMetric(metrics, toAirport).Arrivals++;
                    }
                }
            }

            var airports = metrics.Values
                .OrderByDescending(item => item.Departures + item.Arrivals)
                .ThenBy(item => item.Code)
                .Select(item => new {
                    code = item.Code,
                    departures = item.Departures,
                    arrivals = item.Arrivals,
                    total = item.Departures + item.Arrivals
                })
                .ToList();
            int totalDepartures = airports.Sum(item => item.departures);
            int totalArrivals = airports.Sum(item => item.arrivals);
            var peak = airports.FirstOrDefault();

            return new {
                source = "T_FINISHED_FLIGHTS",
                totalDepartures = totalDepartures,
                totalArrivals = totalArrivals,
                airportCount = airports.Count,
                peakAirport = peak == null ? String.Empty : peak.code,
                peakTotal = peak == null ? 0 : peak.total,
                airports = airports
            };
        }

        [WebMethod]
        public static object GetDetails(string fromDate, string toDate, string airport, string movement, int page, int pageSize)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string selectedAirport = NormalizeAirport(airport, false);
            if (!IsVietnamAirport(selectedAirport))
                throw new ArgumentException("Mã sân bay cần xem chi tiết không hợp lệ.");

            string movementKey = (movement ?? String.Empty).Trim().ToLowerInvariant();
            if (movementKey != "departure" && movementKey != "arrival")
                throw new ArgumentException("Loại cất/hạ cánh không hợp lệ.");
            page = Math.Max(1, page);
            pageSize = Math.Max(25, Math.Min(500, pageSize));
            int start = (page - 1) * pageSize;
            int total = 0;
            var rows = new List<object>(pageSize);

            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = CreateCommand(connection, from, to, selectedAirport))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string state = Convert.ToString(reader["FLIGHT_STATE"]);
                        if (!IsCompletedOrDelayed(state)) continue;
                        string fromAirport = NormalizeAirport(Convert.ToString(reader["FROM_AIRP"]), false);
                        string toAirport = NormalizeAirport(Convert.ToString(reader["TO_AIRP"]), false);
                        bool matches = movementKey == "departure"
                            ? fromAirport == selectedAirport
                            : toAirport == selectedAirport;
                        if (!matches) continue;

                        if (total >= start && rows.Count < pageSize)
                        {
                            rows.Add(new {
                                stt = total + 1,
                                callsign = Convert.ToString(reader["FLIGHTNBR"]),
                                oper = Convert.ToString(reader["OPER_ID"]),
                                registration = Convert.ToString(reader["REGISTRATION"]),
                                permType = Convert.ToString(reader["PERMTYPE"]),
                                fromAirp = fromAirport,
                                toAirp = toAirport,
                                atdDay = Convert.ToString(reader["ATDDAY"]),
                                ataDay = Convert.ToString(reader["ATADAY"]),
                                eobtDay = Convert.ToString(reader["EOBTDAY"]),
                                status = state
                            });
                        }
                        total++;
                    }
                }
            }

            int totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
            return new {
                airport = selectedAirport,
                movement = movementKey,
                page = Math.Min(page, totalPages),
                pageSize = pageSize,
                total = total,
                totalPages = totalPages,
                rows = rows
            };
        }

        private static OracleCommand CreateCommand(OracleConnection connection, DateTime from, DateTime to, string airport)
        {
            var command = new OracleCommand(FlightStatusRate.BuildHistoricalStatusSql(), connection);
            command.BindByName = true;
            command.CommandTimeout = 120;
            command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
            command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
            command.Parameters.Add("oper", OracleDbType.Varchar2).Value = DBNull.Value;
            command.Parameters.Add("airport", OracleDbType.Varchar2).Value = airport == null ? (object)DBNull.Value : airport;
            return command;
        }

        private static void ParseDateRange(string fromDate, string toDate, out DateTime from, out DateTime to)
        {
            if (!DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc không hợp lệ.");
            if (from > to)
                throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
        }

        private static string NormalizeAirport(string value, bool allowAll)
        {
            string normalized = (value ?? String.Empty).Trim().ToUpperInvariant();
            if (allowAll && (normalized.Length == 0 || normalized == "ALL")) return null;
            return normalized;
        }

        private static bool IsVietnamAirport(string code)
        {
            return !String.IsNullOrEmpty(code) && code.StartsWith("VV", StringComparison.Ordinal);
        }

        private static bool IsCompletedOrDelayed(string state)
        {
            return state == "FINISHED" || state.StartsWith("DELAY", StringComparison.Ordinal);
        }

        private static AirportMetric GetMetric(IDictionary<string, AirportMetric> metrics, string code)
        {
            AirportMetric metric;
            if (!metrics.TryGetValue(code, out metric))
            {
                metric = new AirportMetric { Code = code };
                metrics.Add(code, metric);
            }
            return metric;
        }
    }
}
