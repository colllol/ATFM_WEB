using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class ReportAirportDAL
    {
        public ReportAirportChartResponse GetChartData(ReportAirportRangeRequest request)
        {
            DateTime from, to;
            ValidateRange(request, out from, out to);
            return new ReportAirportChartResponse {
                Source = "T_FINISHED_FLIGHTS", FromDate = request.FromDate, ToDate = request.ToDate,
                Flights = LoadFlights(from, to, null)
            };
        }

        public ReportAirportSummaryResponse GetSummary(ReportAirportSummaryRequest request)
        {
            DateTime from, to;
            ValidateRange(request, out from, out to);
            string airport = NormalizeAirport(request.Airport, true);
            var values = new Dictionary<string, ReportAirportMetricDto>(StringComparer.OrdinalIgnoreCase);
            foreach (ReportAirportFlightDto flight in LoadFlights(from, to, airport).Where(IsCompletedOrDelayed)) {
                if (IsVietnamAirport(flight.FromAirp) && (airport == null || flight.FromAirp == airport)) GetMetric(values, flight.FromAirp).Departures++;
                if (IsVietnamAirport(flight.ToAirp) && (airport == null || flight.ToAirp == airport)) GetMetric(values, flight.ToAirp).Arrivals++;
            }
            List<ReportAirportMetricDto> airports = values.Values.Select(x => { x.Total = x.Departures + x.Arrivals; return x; })
                .OrderByDescending(x => x.Total).ThenBy(x => x.Code).ToList();
            ReportAirportMetricDto peak = airports.FirstOrDefault();
            return new ReportAirportSummaryResponse {
                Source = "T_FINISHED_FLIGHTS", Airports = airports, AirportCount = airports.Count,
                TotalDepartures = airports.Sum(x => x.Departures), TotalArrivals = airports.Sum(x => x.Arrivals),
                PeakAirport = peak == null ? String.Empty : peak.Code, PeakTotal = peak == null ? 0 : peak.Total
            };
        }

        public ReportAirportDetailsResponse GetDetails(ReportAirportDetailsRequest request)
        {
            DateTime from, to;
            ValidateRange(request, out from, out to);
            string airport = NormalizeAirport(request.Airport, false);
            if (!IsVietnamAirport(airport)) throw new ArgumentException("Mã sân bay cần xem chi tiết không hợp lệ.");
            string movement = (request.Movement ?? String.Empty).Trim().ToLowerInvariant();
            if (movement != "departure" && movement != "arrival") throw new ArgumentException("Loại cất/hạ cánh không hợp lệ.");
            int page = Math.Max(1, request.Page), pageSize = Math.Max(25, Math.Min(500, request.PageSize));
            List<ReportAirportFlightDto> filtered = LoadFlights(from, to, airport).Where(IsCompletedOrDelayed)
                .Where(x => movement == "departure" ? x.FromAirp == airport : x.ToAirp == airport).ToList();
            int totalPages = Math.Max(1, (int)Math.Ceiling(filtered.Count / (double)pageSize));
            page = Math.Min(page, totalPages);
            List<ReportAirportFlightDto> rows = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            for (int index = 0; index < rows.Count; index++) rows[index].Stt = (page - 1) * pageSize + index + 1;
            return new ReportAirportDetailsResponse { Airport = airport, Movement = movement, Page = page, PageSize = pageSize, Total = filtered.Count, TotalPages = totalPages, Rows = rows };
        }

        private static List<ReportAirportFlightDto> LoadFlights(DateTime from, DateTime to, string airport)
        {
            var result = new List<ReportAirportFlightDto>();
            using (var connection = CreateConnection())
            using (var command = new OracleCommand(FlightAnalyticsDAL.BuildStatusSql(false), connection)) {
                command.BindByName = true; command.CommandTimeout = 120;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from.Date;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.Date.AddDays(1);
                command.Parameters.Add("permType", OracleDbType.Varchar2).Value = "LD";
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = DBNull.Value;
                command.Parameters.Add("airport", OracleDbType.Varchar2).Value = airport == null ? (object)DBNull.Value : airport;
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader()) while (reader.Read()) result.Add(new ReportAirportFlightDto {
                    FlightDate = DateValue(reader, "FLIGHTDATE"), Callsign = Text(reader, "FLIGHTNBR"), Oper = Text(reader, "OPER_ID"),
                    Registration = Text(reader, "REGISTRATION"), PermType = Text(reader, "PERMTYPE"), FromAirp = Airport(reader, "FROM_AIRP"),
                    ToAirp = Airport(reader, "TO_AIRP"), AtdDay = Text(reader, "ATDDAY"), AtaDay = Text(reader, "ATADAY"),
                    EobtDay = Text(reader, "EOBTDAY"), Status = Text(reader, "FLIGHT_STATE")
                });
            }
            return result;
        }

        private static string BuildSql()
        {
            return @"WITH fpl AS (
 SELECT FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION, FROM_AIRP, TO_AIRP, EOBTDATE, EOBT,
        ROW_NUMBER() OVER (PARTITION BY TRUNC(FLIGHTDATE), UPPER(TRIM(REGISTRATION)), UPPER(TRIM(FLIGHTNBR)) ORDER BY FLIGHT_ID DESC) rn
 FROM (SELECT FLIGHT_ID,FLIGHTDATE,FLIGHTNBR,REGISTRATION,FROM_AIRP,TO_AIRP,EOBTDATE,EOBT FROM T_DAY_FLIGHTS_GOINGON
       UNION ALL SELECT FLIGHT_ID,FLIGHTDATE,FLIGHTNBR,REGISTRATION,FROM_AIRP,TO_AIRP,EOBTDATE,EOBT FROM T_DAY_FLIGHTS_GOINGON032024
       UNION ALL SELECT FLIGHT_ID,FLIGHTDATE,FLIGHTNBR,REGISTRATION,FROM_AIRP,TO_AIRP,EOBTDATE,EOBT FROM T_T_DAY_FLIGHTS_GOINGON)
), src AS (
 SELECT f.*, COALESCE(NULLIF(TRIM(p.EOBTDATE),''),NULLIF(TRIM(p.EOBT),'')) EOBTDAY,
        CASE WHEN REGEXP_LIKE(TRIM(COALESCE(p.EOBTDATE,p.EOBT)), '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN TRUNC(f.FLIGHTDATE)+TO_NUMBER(SUBSTR(TRIM(COALESCE(p.EOBTDATE,p.EOBT)),1,2))/24+TO_NUMBER(SUBSTR(TRIM(COALESCE(p.EOBTDATE,p.EOBT)),3,2))/1440 END planned_ts,
        CASE WHEN REGEXP_LIKE(TRIM(f.ATD), '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN TRUNC(f.FLIGHTDATE)+TO_NUMBER(SUBSTR(TRIM(f.ATD),1,2))/24+TO_NUMBER(SUBSTR(TRIM(f.ATD),3,2))/1440 END actual_ts
 FROM T_FINISHED_FLIGHTS f LEFT JOIN fpl p ON p.rn=1 AND TRUNC(p.FLIGHTDATE)=TRUNC(f.FLIGHTDATE)
  AND UPPER(TRIM(p.REGISTRATION))=UPPER(TRIM(f.REGISTRATION)) AND UPPER(TRIM(p.FLIGHTNBR))=UPPER(TRIM(f.FLIGHTNBR))
 WHERE f.PERMTYPE='LD' AND f.OPER_ID IS NOT NULL AND f.FLIGHTDATE>=:fromDate AND f.FLIGHTDATE<:toDate
 AND (:airport IS NULL OR UPPER(TRIM(f.FROM_AIRP))=:airport OR UPPER(TRIM(f.TO_AIRP))=:airport)
)
SELECT FLIGHTDATE,FLIGHTNBR,OPER_ID,REGISTRATION,PERMTYPE,FROM_AIRP,TO_AIRP,NULLIF(TRIM(ATD),'') ATDDAY,NULLIF(TRIM(ATA),'') ATADAY,EOBTDAY,
 CASE WHEN FROM_AIRP LIKE 'VV%' AND TO_AIRP IS NOT NULL AND ATD IS NOT NULL AND actual_ts-planned_ts>=60/1440 THEN 'DELAY_60_PLUS'
      WHEN FROM_AIRP LIKE 'VV%' AND TO_AIRP IS NOT NULL AND ATD IS NOT NULL AND actual_ts-planned_ts>=30/1440 THEN 'DELAY_30_59'
      WHEN FROM_AIRP LIKE 'VV%' AND TO_AIRP IS NOT NULL AND ATD IS NOT NULL AND actual_ts-planned_ts>=15/1440 THEN 'DELAY_15_29'
      WHEN ATD IS NOT NULL AND ATA IS NOT NULL AND FROM_AIRP IS NOT NULL AND TO_AIRP IS NOT NULL THEN 'FINISHED' ELSE 'CANCEL' END FLIGHT_STATE
FROM src ORDER BY FLIGHTDATE,FLIGHTNBR";
        }

        private static OracleConnection CreateConnection() { string value = ConfigurationManager.AppSettings["ConnectDB"]; if (String.IsNullOrWhiteSpace(value)) throw new ConfigurationErrorsException("Thiếu cấu hình AppSettings ConnectDB."); return new OracleConnection(value); }
        private static void ValidateRange(ReportAirportRangeRequest request, out DateTime from, out DateTime to) { if (request == null) throw new ArgumentException("Dữ liệu yêu cầu không được để trống."); if (!DateTime.TryParseExact(request.FromDate,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out from)||!DateTime.TryParseExact(request.ToDate,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out to)) throw new ArgumentException("Ngày lọc phải đúng định dạng yyyy-MM-dd."); if(from>to) throw new ArgumentException("Từ ngày không được lớn hơn đến ngày."); }
        private static string NormalizeAirport(string value, bool allowAll) { string result=(value??String.Empty).Trim().ToUpperInvariant(); return allowAll&&(result.Length==0||result=="ALL")?null:result; }
        private static bool IsVietnamAirport(string value) { return !String.IsNullOrEmpty(value)&&value.StartsWith("VV",StringComparison.Ordinal); }
        private static bool IsCompletedOrDelayed(ReportAirportFlightDto value) { return value.Status=="FINISHED"||value.Status.StartsWith("DELAY",StringComparison.Ordinal); }
        private static ReportAirportMetricDto GetMetric(IDictionary<string,ReportAirportMetricDto> values,string code) { ReportAirportMetricDto result; if(!values.TryGetValue(code,out result)){result=new ReportAirportMetricDto{Code=code};values.Add(code,result);}return result; }
        private static string Text(OracleDataReader reader,string name) { return reader[name]==DBNull.Value?String.Empty:Convert.ToString(reader[name]).Trim(); }
        private static string Airport(OracleDataReader reader,string name) { return Text(reader,name).ToUpperInvariant(); }
        private static string DateValue(OracleDataReader reader,string name) { return reader[name]==DBNull.Value?String.Empty:Convert.ToDateTime(reader[name],CultureInfo.InvariantCulture).ToString("yyyy-MM-dd",CultureInfo.InvariantCulture); }
    }
}
