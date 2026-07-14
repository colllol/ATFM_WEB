using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.Common
{
    public partial class ChartReportAirport : System.Web.UI.Page
    {
        private sealed class AirportFlight
        {
            public string FlightDate { get; set; }
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

        [WebMethod]
        public static object GetData(string fromDate, string toDate)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc không hợp lệ.");

            if (from > to)
                throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");

            var flights = new List<AirportFlight>();
            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(ReportNew.FlightStatusRate.BuildHistoricalStatusSql(), connection))
            {
                command.BindByName = true;
                command.CommandTimeout = 120;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = DBNull.Value;
                command.Parameters.Add("airport", OracleDbType.Varchar2).Value = DBNull.Value;

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flights.Add(new AirportFlight
                        {
                            FlightDate = GetDateValue(reader, "FLIGHTDATE"),
                            Callsign = GetStringValue(reader, "FLIGHTNBR"),
                            Oper = GetStringValue(reader, "OPER_ID"),
                            Registration = GetStringValue(reader, "REGISTRATION"),
                            PermType = GetStringValue(reader, "PERMTYPE"),
                            FromAirp = NormalizeAirport(GetStringValue(reader, "FROM_AIRP")),
                            ToAirp = NormalizeAirport(GetStringValue(reader, "TO_AIRP")),
                            AtdDay = GetStringValue(reader, "ATDDAY"),
                            AtaDay = GetStringValue(reader, "ATADAY"),
                            EobtDay = GetStringValue(reader, "EOBTDAY"),
                            Status = GetStringValue(reader, "FLIGHT_STATE")
                        });
                    }
                }
            }

            return new
            {
                source = "T_FINISHED_FLIGHTS",
                fromDate = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                toDate = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                flights = flights
            };
        }

        private static string GetStringValue(OracleDataReader reader, string column)
        {
            return reader[column] == DBNull.Value ? String.Empty : Convert.ToString(reader[column]).Trim();
        }

        private static string GetDateValue(OracleDataReader reader, string column)
        {
            if (reader[column] == DBNull.Value) return String.Empty;
            DateTime value;
            if (reader[column] is DateTime) value = (DateTime)reader[column];
            else if (!DateTime.TryParse(Convert.ToString(reader[column]), out value)) return String.Empty;
            return value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        private static string NormalizeAirport(string value)
        {
            return (value ?? String.Empty).Trim().ToUpperInvariant();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
