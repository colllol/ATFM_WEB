using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class FlightOperationOverview : ReportPageBase
    {
        private sealed class FlightRow
        {
            public string Callsign { get; set; }
            public string Oper { get; set; }
            public string Registration { get; set; }
            public string PermType { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string AtdDay { get; set; }
            public string AtaDay { get; set; }
            public string EobtDay { get; set; }
            public string Status { get; set; }
        }

        private sealed class AirportMetric
        {
            public string Code { get; set; }
            public int Departures { get; set; }
            public int Arrivals { get; set; }
        }

        [WebMethod]
        public static object GetData(string fromDate, string toDate, string airport)
        {
            DateTime from;
            DateTime to;
            ParseDateRange(fromDate, toDate, out from, out to);
            string selectedAirport = NormalizeAirport(airport, true);
            var flights = new List<FlightRow>();
            var metrics = new Dictionary<string, AirportMetric>(StringComparer.OrdinalIgnoreCase);
            int finished = 0;
            int delay = 0;

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

                        if (state == "FINISHED") finished++;
                        else delay++;

                        string fromAirport = NormalizeAirport(Convert.ToString(reader["FROM_AIRP"]), false);
                        string toAirport = NormalizeAirport(Convert.ToString(reader["TO_AIRP"]), false);
                        flights.Add(new FlightRow {
                            Callsign = Convert.ToString(reader["FLIGHTNBR"]),
                            Oper = Convert.ToString(reader["OPER_ID"]),
                            Registration = Convert.ToString(reader["REGISTRATION"]),
                            PermType = Convert.ToString(reader["PERMTYPE"]),
                            FromAirp = fromAirport,
                            ToAirp = toAirport,
                            AtdDay = Convert.ToString(reader["ATDDAY"]),
                            AtaDay = Convert.ToString(reader["ATADAY"]),
                            EobtDay = Convert.ToString(reader["EOBTDAY"]),
                            Status = state
                        });

                        if (IsVietnamAirport(fromAirport) && (selectedAirport == null || fromAirport == selectedAirport))
                            GetMetric(metrics, fromAirport).Departures++;
                        if (IsVietnamAirport(toAirport) && (selectedAirport == null || toAirport == selectedAirport))
                            GetMetric(metrics, toAirport).Arrivals++;
                    }
                }
            }

            var airportRows = metrics.Values
                .OrderByDescending(item => item.Departures + item.Arrivals)
                .ThenBy(item => item.Code)
                .Select(item => new {
                    code = item.Code,
                    departures = item.Departures,
                    arrivals = item.Arrivals,
                    total = item.Departures + item.Arrivals
                })
                .ToList();

            return new {
                source = "T_FINISHED_FLIGHTS",
                total = finished + delay,
                finished = finished,
                delay = delay,
                airportCount = airportRows.Count,
                totalDepartures = airportRows.Sum(item => item.departures),
                totalArrivals = airportRows.Sum(item => item.arrivals),
                airports = airportRows,
                flights = flights.Select(item => new {
                    callsign = item.Callsign,
                    oper = item.Oper,
                    registration = item.Registration,
                    permType = item.PermType,
                    fromAirp = item.FromAirp,
                    toAirp = item.ToAirp,
                    atdDay = item.AtdDay,
                    ataDay = item.AtaDay,
                    eobtDay = item.EobtDay,
                    status = item.Status
                }).ToList()
            };
        }

        private static OracleCommand CreateCommand(OracleConnection connection, DateTime from, DateTime to, string airport)
        {
            var command = new OracleCommand(FlightStatusRate.BuildHistoricalStatusSql(), connection);
            command.BindByName = true;
            command.CommandTimeout = 120;
            command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
            command.Parameters.Add("toDate", OracleDbType.Date).Value = to;
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
