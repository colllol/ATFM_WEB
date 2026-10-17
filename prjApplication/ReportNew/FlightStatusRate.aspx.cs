using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.Services;
using prjBusinessLogic;

namespace prjApplication.ReportNew
{
    public partial class FlightStatusRate : ReportPageBase
    {
        [WebMethod]
        public static object GetData(string fromDate, string toDate, string oper, string airport, bool currentDay)
        {
            try
            {
                return GetDataCore(fromDate, toDate, oper, airport, currentDay);
            }
            catch (Exception ex)
            {
                Exception rootError = ex.GetBaseException();
                System.Diagnostics.Trace.TraceError(
                    "FlightStatusRate.GetData failed: {0}",
                    ex);

                return new
                {
                    Code = "99",
                    Message = "FlightStatusRate.GetData: " + rootError.Message
                };
            }
        }

        private static object GetDataCore(string fromDate, string toDate, string oper, string airport, bool currentDay)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc không hợp lệ.");

            if (from > to || to > DateTime.Today)
                throw new ArgumentException("Khoảng ngày phải hợp lệ và không vượt quá ngày hiện tại.");

            currentDay = from.Date == DateTime.Today && to.Date == DateTime.Today;
            string table = currentDay ? "T_DAY_FLIGHTS_GOINGON" : "T_FINISHED_FLIGHTS";
            var flights = new List<object>();
            int finished = 0, cancel = 0, delay = 0, wait = 0;

            DataTable statusData = LoadStatus(fromDate, toDate, NormalizeFilter(oper), NormalizeFilter(airport), currentDay);
            foreach (DataRow row in statusData.Rows)
            {
                string state = Convert.ToString(row["FLIGHT_STATE"]);
                // Với kỳ quá khứ, danh sách hủy phải lấy duy nhất từ bảng chuyên biệt bên dưới.
                if (!currentDay && state == "CANCEL")
                    continue;
                if (state == "FINISHED") finished++;
                else if (state == "CANCEL") cancel++;
                else if (state.StartsWith("DELAY", StringComparison.Ordinal)) delay++;
                else wait++;
                flights.Add(new {
                    callsign = Text(row["FLIGHTNBR"]),
                    oper = Text(row["OPER_ID"]),
                    registration = Text(row["REGISTRATION"]),
                    permType = Text(row["PERMTYPE"]),
                    fromAirp = Text(row["FROM_AIRP"]),
                    toAirp = Text(row["TO_AIRP"]),
                    atdDay = Text(row["ATDDAY"]),
                    ataDay = Text(row["ATADAY"]),
                    eobtDay = Text(row["EOBTDAY"]),
                    status = state
                });
            }

            if (!currentDay)
            {
                DataTable cancelData = new clsResuftAPI().GetTableApiExtension(
                    "FLIGHT_STATUS_PKG", "GET_CANCELLED",
                    new { P_FROM_DATE = fromDate, P_TO_DATE = toDate });
                if (cancelData == null)
                    throw new InvalidOperationException("Không lấy được danh sách chuyến hủy từ API.");

                foreach (DataRow row in cancelData.Rows)
                {
                    cancel++;
                    flights.Add(new {
                        callsign = Text(row["FLIGHTNBR"]), oper = Text(row["OPER_ID"]),
                        registration = Text(row["REGISTRATION"]), permType = Text(row["PERMTYPE"]),
                        fromAirp = Text(row["FROM_AIRP"]), toAirp = Text(row["TO_AIRP"]),
                        atdDay = Text(row["ATDDAY"]), ataDay = Text(row["ATADAY"]),
                        eobtDay = Text(row["EOBTDAY"]), status = "CANCEL"
                    });
                }
                table += " + T_DAY_FLIGHTS_CANCEL";
            }

            return new {
                source = table,
                total = finished + cancel + delay + wait,
                finished = finished,
                cancel = cancel,
                delay = delay,
                wait = wait,
                flights = flights
            };
        }

        [WebMethod]
        public static object GetOperators(string fromDate, string toDate, bool currentDay)
        {
            DateTime from, to;
            if (!DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc không hợp lệ.");
            if (from > to || to > DateTime.Today)
                throw new ArgumentException("Khoảng ngày phải hợp lệ và không vượt quá ngày hiện tại.");

            DataTable data = new clsResuftAPI().GetTableApiExtension(
                "FLIGHT_STATUS_PKG", "GET_OPERATORS",
                new { P_FROM_DATE = fromDate, P_TO_DATE = toDate });
            if (data == null)
                throw new InvalidOperationException("Không lấy được danh sách hãng khai thác từ API.");

            var values = new List<string>();
            foreach (DataRow row in data.Rows) values.Add(Text(row["OPER_ID"]));
            return values;
        }

        // Danh sach chuyen bay kem trang thai phan loai tu FLIGHT_STATUS_PKG.GET_STATUS.
        // fromDate/toDate dang yyyy-MM-dd, toDate bao gom ca ngay ket thuc.
        internal static DataTable LoadStatus(string fromDate, string toDate, string oper, string airport, bool currentDay)
        {
            DataTable data = new clsResuftAPI().GetTableApiExtension(
                "FLIGHT_STATUS_PKG", "GET_STATUS",
                new
                {
                    P_FROM_DATE = fromDate,
                    P_TO_DATE = toDate,
                    P_OPER = oper,
                    P_AIRPORT = airport,
                    P_CURRENT_DAY = currentDay ? 1 : 0
                });
            if (data == null)
                throw new InvalidOperationException("Không lấy được dữ liệu trạng thái chuyến bay từ API.");
            return data;
        }

        internal static string NormalizeFilter(string value)
        {
            string normalized = (value ?? String.Empty).Trim().ToUpperInvariant();
            return normalized.Length == 0 || normalized == "ALL" ? null : normalized;
        }

        internal static string Text(object value)
        {
            return value == null || value == DBNull.Value ? String.Empty : Convert.ToString(value).Trim();
        }
    }
}
