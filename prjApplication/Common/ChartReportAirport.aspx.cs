using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Services;

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
            DateTime today = DateTime.Today;
            bool currentDay = from.Date == today && to.Date == today;
            foreach (System.Data.DataRow row in ReportNew.FlightStatusRate.LoadStatus(fromDate, toDate, null, null, currentDay).Rows)
            {
                var flight = new AirportFlight
                {
                    FlightDate = GetDateValue(row, "FLIGHTDATE"),
                    Callsign = GetStringValue(row, "FLIGHTNBR"),
                    Oper = GetStringValue(row, "OPER_ID"),
                    Registration = GetStringValue(row, "REGISTRATION"),
                    PermType = GetStringValue(row, "PERMTYPE"),
                    FromAirp = NormalizeAirport(GetStringValue(row, "FROM_AIRP")),
                    ToAirp = NormalizeAirport(GetStringValue(row, "TO_AIRP")),
                    AtdDay = GetStringValue(row, "ATDDAY"),
                    AtaDay = GetStringValue(row, "ATADAY"),
                    EobtDay = GetStringValue(row, "EOBTDAY"),
                    Status = GetStringValue(row, "FLIGHT_STATE")
                };
                if (!currentDay && flight.Status == "CANCEL")
                    continue;
                flights.Add(flight);
            }

            if (!currentDay)
            {
                // Trang nay hien thi tat ca chuyen huy nen goi voi P_FILTER_PERMTYPE=0.
                System.Data.DataTable cancelData = new global::clsResuftAPI().GetTableApiExtension(
                    "FLIGHT_STATUS_PKG", "GET_CANCELLED",
                    new { P_FROM_DATE = fromDate, P_TO_DATE = toDate, P_FILTER_PERMTYPE = 0 });
                if (cancelData == null)
                    throw new InvalidOperationException("Không lấy được danh sách chuyến hủy từ API.");

                foreach (System.Data.DataRow row in cancelData.Rows)
                {
                    flights.Add(new AirportFlight
                    {
                        FlightDate = GetDateValue(row, "FLIGHTDATE"),
                        Callsign = GetStringValue(row, "FLIGHTNBR"),
                        Oper = GetStringValue(row, "OPER_ID"),
                        Registration = GetStringValue(row, "REGISTRATION"),
                        PermType = GetStringValue(row, "PERMTYPE"),
                        FromAirp = NormalizeAirport(GetStringValue(row, "FROM_AIRP")),
                        ToAirp = NormalizeAirport(GetStringValue(row, "TO_AIRP")),
                        AtdDay = GetStringValue(row, "ATDDAY"),
                        AtaDay = GetStringValue(row, "ATADAY"),
                        EobtDay = GetStringValue(row, "EOBTDAY"),
                        Status = "CANCEL"
                    });
                }
            }

            return new
            {
                source = !currentDay
                    ? "T_FINISHED_FLIGHTS + ATFM.T_DAY_FLIGHTS_CANCEL"
                    : "ATFM.T_DAY_FLIGHTS_GOINGON",
                fromDate = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                toDate = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                flights = flights
            };
        }

        private static string GetStringValue(System.Data.DataRow row, string column)
        {
            return row[column] == DBNull.Value ? String.Empty : Convert.ToString(row[column]).Trim();
        }

        private static string GetDateValue(System.Data.DataRow row, string column)
        {
            if (row[column] == DBNull.Value) return String.Empty;
            DateTime value;
            if (row[column] is DateTime) value = (DateTime)row[column];
            else if (!DateTime.TryParse(Convert.ToString(row[column]), out value)) return String.Empty;
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
