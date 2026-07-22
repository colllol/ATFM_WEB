using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class FlightAnalyticsDAL
    {
        public ChartReportResponse GetDashboard(FlightAnalyticsRequest request)
        {
            return new ChartReportResponse { Status = GetStatus(request), Overview = GetOverview(request), Trend = GetTrend(request) };
        }

        public FlightStatusRateResponse GetStatus(FlightAnalyticsRequest request)
        {
            DateTime from, to; Validate(request, out from, out to);
            var rows = Load(from, to, Filter(request.Oper), Filter(request.Airport), request.CurrentDay, "LD");
            return new FlightStatusRateResponse { Source = request.CurrentDay ? "T_DAY_FLIGHTS_GOINGON" : "T_FINISHED_FLIGHTS", Total = rows.Count, Finished = rows.Count(x => x.Status == "FINISHED"), Cancel = rows.Count(x => x.Status == "CANCEL"), Delay = rows.Count(x => x.Status.StartsWith("DELAY", StringComparison.Ordinal)), Wait = rows.Count(x => x.Status == "WAIT"), Flights = rows };
        }

        public List<string> GetOperators(FlightAnalyticsRequest request)
        {
            DateTime from, to; Validate(request, out from, out to); var result = new List<string>();
            const string sql = @"SELECT OPER_ID FROM (SELECT UPPER(TRIM(OPER_ID)) OPER_ID FROM T_DAY_FLIGHTS_GOINGON WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate UNION SELECT UPPER(TRIM(OPER_ID)) FROM T_FINISHED_FLIGHTS WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate) ORDER BY OPER_ID";
            using (var c = Connection()) using (var cmd = new OracleCommand(sql, c)) { cmd.BindByName = true; cmd.Parameters.Add("fromDate", OracleDbType.Date).Value = from; cmd.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1); c.Open(); using (var r = cmd.ExecuteReader()) while (r.Read()) result.Add(Convert.ToString(r[0])); }
            return result;
        }

        public FlightOperationOverviewResponse GetOverview(FlightAnalyticsRequest request)
        {
            DateTime from, to; Validate(request, out from, out to); var rows = Load(from, to, null, Filter(request.Airport), false, "LD").Where(Completed).ToList();
            var airports = AirportRows(rows);
            return new FlightOperationOverviewResponse { Source="T_FINISHED_FLIGHTS", Total=rows.Count, Finished=rows.Count(x=>x.Status=="FINISHED"), Delay=rows.Count(x=>x.Status.StartsWith("DELAY")), AirportCount=airports.Count, TotalDepartures=airports.Sum(x=>x.Departures), TotalArrivals=airports.Sum(x=>x.Arrivals), Airports=airports, Flights=rows };
        }

        public FlightTrendResponse GetTrend(FlightAnalyticsRequest request)
        {
            DateTime from,to; Validate(request,out from,out to); string mode=String.Equals(request.Period,"month",StringComparison.OrdinalIgnoreCase)?"month":"day"; if(mode=="day"&&(to-from).TotalDays>366) throw new ArgumentException("Chế độ theo ngày hỗ trợ tối đa 367 ngày.");
            DateTime pf=from.AddYears(-1), pt=to.AddYears(-1); var current=Load(from,to,Filter(request.Oper),Filter(request.Airport),false,"LD").Where(Completed).ToList(); var previous=Load(pf,pt,Filter(request.Oper),Filter(request.Airport),false,"LD").Where(Completed).ToList();
            var cg=current.GroupBy(x=>Bucket(DateTime.ParseExact(x.FlightDate,"yyyy-MM-dd",CultureInfo.InvariantCulture),mode)).ToDictionary(x=>x.Key,x=>x.Count()); var pg=previous.GroupBy(x=>Bucket(DateTime.ParseExact(x.FlightDate,"yyyy-MM-dd",CultureInfo.InvariantCulture),mode)).ToDictionary(x=>x.Key,x=>x.Count());
            var labels=new List<string>(); var cv=new List<int>(); var pv=new List<int>(); DateTime cursor=mode=="month"?new DateTime(from.Year,from.Month,1):from.Date,end=mode=="month"?new DateTime(to.Year,to.Month,1):to.Date; while(cursor<=end){ labels.Add(mode=="month"?cursor.ToString("MM/yyyy"):cursor.ToString("dd/MM")); int v; cv.Add(cg.TryGetValue(Bucket(cursor,mode),out v)?v:0); pv.Add(pg.TryGetValue(Bucket(cursor.AddYears(-1),mode),out v)?v:0); cursor=mode=="month"?cursor.AddMonths(1):cursor.AddDays(1); }
            int diff=current.Count-previous.Count; return new FlightTrendResponse { Source="T_FINISHED_FLIGHTS",Period=mode,CurrentFrom=from.ToString("dd/MM/yyyy"),CurrentTo=to.ToString("dd/MM/yyyy"),PreviousFrom=pf.ToString("dd/MM/yyyy"),PreviousTo=pt.ToString("dd/MM/yyyy"),CurrentTotal=current.Count,PreviousTotal=previous.Count,CurrentFinished=current.Count(x=>x.Status=="FINISHED"),CurrentDelay=current.Count(x=>x.Status.StartsWith("DELAY")),Difference=diff,ChangePercent=previous.Count==0?(current.Count==0?0:100):Math.Round(diff*100.0/previous.Count,1),Labels=labels,Current=cv,Previous=pv };
        }

        public CivilFlightSummaryResponse GetCivil(FlightAnalyticsRequest request)
        {
            DateTime from,to; Validate(request,out from,out to); var rows=Load(from,to,Filter(request.Oper),Filter(request.Airport),request.CurrentDay,"LD"); int finished=rows.Count(x=>x.Status=="FINISHED"), delay=rows.Count(x=>x.Status.StartsWith("DELAY"));
            // “Nội địa” dùng cùng quy tắc nghiệp vụ hiện hữu: cả hai đầu rỗng hoặc mang mã ICAO Việt Nam VV*.
            Func<FlightAnalyticsRow,bool> domestic=x=>(String.IsNullOrWhiteSpace(x.FromAirp)||x.FromAirp.StartsWith("VV"))&&(String.IsNullOrWhiteSpace(x.ToAirp)||x.ToAirp.StartsWith("VV"));
            return new CivilFlightSummaryResponse { Source=request.CurrentDay?"T_DAY_FLIGHTS_GOINGON":"T_FINISHED_FLIGHTS",Total=rows.Count,Domestic=rows.Count(domestic),International=rows.Count(x=>!domestic(x)),Finished=finished,Delay=delay,Cancel=rows.Count(x=>x.Status=="CANCEL"),Wait=rows.Count(x=>x.Status=="WAIT"),OnTimePercent=finished+delay==0?0:Math.Round(finished*100.0/(finished+delay),1),Airports=AirportRows(rows.Where(Completed).ToList()),Flights=rows };
        }

        public MilitaryFlightResponse GetMilitary(FlightAnalyticsRequest request)
        {
            DateTime from,to; Validate(request,out from,out to); var rows=Load(from,to,Filter(request.Oper),Filter(request.Airport),request.CurrentDay,"O/F");
            // Dữ liệu nguồn không có cột loại nhiệm vụ chuẩn hóa; VIP được nhận diện bảo thủ từ số phép/callsign, phần còn lại là quân sự.
            int vip=rows.Count(x=>(x.Callsign??"").IndexOf("VIP",StringComparison.OrdinalIgnoreCase)>=0);
            return new MilitaryFlightResponse { Source=request.CurrentDay?"T_DAY_FLIGHTS_GOINGON":"T_FINISHED_FLIGHTS",Total=rows.Count,Vip=vip,Military=rows.Count-vip,Airports=AirportRows(rows),Flights=rows };
        }

        private static List<FlightAnalyticsRow> Load(DateTime from,DateTime to,string oper,string airport,bool current,string permType)
        {
            var list=new List<FlightAnalyticsRow>(); string sql=BuildStatusSql(current);
            using(var c=Connection()) using(var cmd=new OracleCommand(sql,c)){cmd.BindByName=true;cmd.CommandTimeout=120;cmd.Parameters.Add("fromDate",OracleDbType.Date).Value=from;cmd.Parameters.Add("toDate",OracleDbType.Date).Value=to.AddDays(1);cmd.Parameters.Add("oper",OracleDbType.Varchar2).Value=(object)oper??DBNull.Value;cmd.Parameters.Add("airport",OracleDbType.Varchar2).Value=(object)airport??DBNull.Value;cmd.Parameters.Add("permType",OracleDbType.Varchar2).Value=permType;c.Open();using(var r=cmd.ExecuteReader())while(r.Read())list.Add(new FlightAnalyticsRow{Callsign=S(r,"FLIGHTNBR"),Oper=S(r,"OPER_ID"),Registration=S(r,"REGISTRATION"),PermType=S(r,"PERMTYPE"),FromAirp=S(r,"FROM_AIRP"),ToAirp=S(r,"TO_AIRP"),AtdDay=S(r,"ATDDAY"),AtaDay=S(r,"ATADAY"),EobtDay=S(r,"EOBTDAY"),Status=S(r,"FLIGHT_STATE"),FlightDate=Convert.ToDateTime(r["FLIGHTDATE"]).ToString("yyyy-MM-dd")});} return list;
        }

        // Shared/public so other report DALs can reuse the exact status classification contract.
        public static string BuildStatusSql(bool current)
        {
            string auxiliary = current ? String.Empty : BuildHistoricalFplCtes();
            string fromClause = current ? "T_DAY_FLIGHTS_GOINGON f" : @"T_FINISHED_FLIGHTS f
                    LEFT JOIN historical_fpl day_fpl ON day_fpl.finished_rowid=ROWIDTOCHAR(f.ROWID) AND day_fpl.match_order=1";
            string planned = current
                ? BuildEobtFallbackSql("f.EOBTDATE", "f.EOBT", "NULL")
                : BuildEobtFallbackSql("day_fpl.EOBTDATE", "day_fpl.EOBT", "NULL");
            string plannedTimestamp = BuildFlightTimestampSql("planned_raw");
            string actualTimestamp = BuildFlightTimestampSql("actual_departure_raw");
            return @"WITH " + auxiliary + @"source_rows AS (
 SELECT f.*," + planned + @" planned_raw,NULLIF(TRIM(f.ATD),'') actual_departure_raw,
 CASE WHEN (NULLIF(TRIM(f.FROM_AIRP),'') IS NULL OR UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%') AND (NULLIF(TRIM(f.TO_AIRP),'') IS NULL OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%') THEN 1 ELSE 0 END domestic,
 CASE WHEN NULLIF(TRIM(f.FROM_AIRP),'') IS NOT NULL THEN 1 ELSE 0 END has_from,
 CASE WHEN NULLIF(TRIM(f.TO_AIRP),'') IS NOT NULL THEN 1 ELSE 0 END has_to,
 CASE WHEN NULLIF(TRIM(f.ATD),'') IS NOT NULL THEN 1 ELSE 0 END has_atd,
 CASE WHEN NULLIF(TRIM(f.ATA),'') IS NOT NULL THEN 1 ELSE 0 END has_ata,
 CASE WHEN NULLIF(TRIM(f.FROM_AIRP),'') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' THEN 1 ELSE 0 END departing_vietnam,
 CASE WHEN NULLIF(TRIM(f.FROM_AIRP),'') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_from,
 CASE WHEN NULLIF(TRIM(f.TO_AIRP),'') IS NOT NULL AND UPPER(TRIM(f.TO_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_to
 FROM " + fromClause + @" WHERE f.PERMTYPE=:permType AND f.OPER_ID IS NOT NULL AND f.FLIGHTDATE>=:fromDate AND f.FLIGHTDATE<:toDate
 AND (:oper IS NULL OR UPPER(TRIM(f.OPER_ID))=:oper) AND (:airport IS NULL OR UPPER(TRIM(f.FROM_AIRP))=:airport OR UPPER(TRIM(f.TO_AIRP))=:airport)
), parsed_rows AS (SELECT s.*," + plannedTimestamp + @" planned_timestamp," + actualTimestamp + @" actual_departure_timestamp FROM source_rows s),
metric_rows AS (SELECT p.*,CASE WHEN planned_timestamp IS NOT NULL AND actual_departure_timestamp IS NOT NULL AND actual_departure_timestamp>=planned_timestamp-(6/24) AND actual_departure_timestamp<=planned_timestamp+1 THEN ROUND((actual_departure_timestamp-planned_timestamp)*1440) END delay_minutes FROM parsed_rows p)
SELECT FLIGHTDATE,FLIGHTNBR,OPER_ID,REGISTRATION,PERMTYPE,FROM_AIRP,TO_AIRP,NULLIF(TRIM(ATD),'') ATDDAY,NULLIF(TRIM(ATA),'') ATADAY,planned_raw EOBTDAY,
 CASE WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=60 THEN 'DELAY_60_PLUS'
 WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=30 THEN 'DELAY_30_59'
 WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=15 THEN 'DELAY_15_29'
 WHEN (domestic=1 AND has_from=1 AND has_to=1 AND has_atd=1 AND has_ata=1) OR (domestic=0 AND ((foreign_from=1 AND has_to=1 AND has_ata=1) OR (foreign_to=1 AND has_from=1 AND has_atd=1))) THEN 'FINISHED'
 WHEN domestic=0 AND ((foreign_to=1 AND (has_from=0 OR has_atd=0)) OR (foreign_from=1 AND (has_to=0 OR has_ata=0))) THEN 'CANCEL'
 WHEN " + (current ? "1" : "0") + @"=1 AND domestic=1 AND departing_vietnam=1 AND has_to=1 AND planned_timestamp IS NOT NULL AND has_atd=0 THEN 'WAIT'
 WHEN domestic=1 AND (has_from=0 OR has_to=0 OR has_atd=0 OR has_ata=0) THEN 'CANCEL' ELSE 'CANCEL' END FLIGHT_STATE
FROM metric_rows ORDER BY FLIGHTDATE,FLIGHTNBR";
        }

        private static string BuildFlightTimestampSql(string expression)
        {
            string value = "TRIM(" + expression + ")";
            return @"CASE WHEN REGEXP_LIKE(" + value + @",'^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN
 CASE WHEN SUBSTR(" + value + @",1,2)=TO_CHAR(FLIGHTDATE,'DD') THEN TRUNC(FLIGHTDATE) WHEN SUBSTR(" + value + @",1,2)=TO_CHAR(FLIGHTDATE+1,'DD') THEN TRUNC(FLIGHTDATE)+1 WHEN SUBSTR(" + value + @",1,2)=TO_CHAR(FLIGHTDATE-1,'DD') THEN TRUNC(FLIGHTDATE)-1 END
 +TO_NUMBER(SUBSTR(" + value + @",3,2))/24+TO_NUMBER(SUBSTR(" + value + @",5,2))/1440
 WHEN REGEXP_LIKE(" + value + @",'^([01][0-9]|2[0-3])[0-5][0-9]$') THEN TRUNC(FLIGHTDATE)+TO_NUMBER(SUBSTR(" + value + @",1,2))/24+TO_NUMBER(SUBSTR(" + value + @",3,2))/1440 END";
        }

        private static string BuildEobtFallbackSql(string dateExpression, string timeExpression, string fallback)
        {
            return @"CASE WHEN REGEXP_LIKE(TRIM(" + dateExpression + @"),'^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN TRIM(" + dateExpression + @")
 WHEN REGEXP_LIKE(TRIM(" + timeExpression + @"),'^((0[1-9]|[12][0-9]|3[01]))?([01][0-9]|2[0-3])[0-5][0-9]$') THEN TRIM(" + timeExpression + @") ELSE " + fallback + " END";
        }

        private static string BuildHistoricalFplCtes()
        {
            return @"historical_fpl_source AS (
 SELECT 1 source_priority,FLIGHT_ID,FLIGHTDATE,FLIGHTNBR,REGISTRATION,FROM_AIRP,TO_AIRP,EOBTDATE,EOBT FROM T_DAY_FLIGHTS_GOINGON WHERE FLIGHTDATE>=TRUNC(:fromDate) AND FLIGHTDATE<TRUNC(:toDate)
 UNION ALL SELECT 2,FLIGHT_ID,FLIGHTDATE,FLIGHTNBR,REGISTRATION,FROM_AIRP,TO_AIRP,EOBTDATE,EOBT FROM T_DAY_FLIGHTS_GOINGON032024 WHERE FLIGHTDATE>=TRUNC(:fromDate) AND FLIGHTDATE<TRUNC(:toDate)
 UNION ALL SELECT 3,FLIGHT_ID,FLIGHTDATE,FLIGHTNBR,REGISTRATION,FROM_AIRP,TO_AIRP,EOBTDATE,EOBT FROM T_T_DAY_FLIGHTS_GOINGON WHERE FLIGHTDATE>=TRUNC(:fromDate) AND FLIGHTDATE<TRUNC(:toDate)
), historical_fpl_candidates AS (
 SELECT ROWIDTOCHAR(f.ROWID) finished_rowid,d.EOBTDATE,d.EOBT,d.source_priority,d.FLIGHT_ID source_flight_id,
 CASE WHEN UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f.FLIGHTNBR)) AND NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f.FROM_AIRP)),'#') AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f.TO_AIRP)),'#') THEN 0 WHEN UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f.FLIGHTNBR)) THEN 1 WHEN NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f.FROM_AIRP)),'#') AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f.TO_AIRP)),'#') THEN 2 ELSE 3 END match_rank
 FROM T_FINISHED_FLIGHTS f JOIN historical_fpl_source d ON d.FLIGHTDATE>=TRUNC(f.FLIGHTDATE) AND d.FLIGHTDATE<TRUNC(f.FLIGHTDATE)+1 AND UPPER(TRIM(d.REGISTRATION))=UPPER(TRIM(f.REGISTRATION))
 WHERE f.PERMTYPE=:permType AND f.OPER_ID IS NOT NULL AND f.REGISTRATION IS NOT NULL AND f.FLIGHTDATE>=:fromDate AND f.FLIGHTDATE<:toDate
 AND (:oper IS NULL OR UPPER(TRIM(f.OPER_ID))=:oper) AND (:airport IS NULL OR UPPER(TRIM(f.FROM_AIRP))=:airport OR UPPER(TRIM(f.TO_AIRP))=:airport)
 AND COALESCE(TRIM(d.EOBTDATE),TRIM(d.EOBT)) IS NOT NULL AND (d.FLIGHT_ID=f.FLIGHT_ID OR UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f.FLIGHTNBR)) OR (NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f.FROM_AIRP)),'#') AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f.TO_AIRP)),'#')))
), historical_fpl AS (SELECT c.*,ROW_NUMBER() OVER(PARTITION BY c.finished_rowid ORDER BY c.match_rank,CASE WHEN REGEXP_LIKE(TRIM(c.EOBTDATE),'^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN 0 ELSE 1 END,c.source_priority,c.source_flight_id) match_order FROM historical_fpl_candidates c), ";
        }
        private static List<AirportTrafficRow> AirportRows(List<FlightAnalyticsRow> rows){return rows.SelectMany(x=>new[]{new{Code=x.FromAirp,D=1,A=0},new{Code=x.ToAirp,D=0,A=1}}).Where(x=>!String.IsNullOrEmpty(x.Code)&&x.Code.StartsWith("VV")).GroupBy(x=>x.Code).Select(g=>new AirportTrafficRow{Code=g.Key,Departures=g.Sum(x=>x.D),Arrivals=g.Sum(x=>x.A),Total=g.Count()}).OrderByDescending(x=>x.Total).ThenBy(x=>x.Code).ToList();}
        private static bool Completed(FlightAnalyticsRow x){return x.Status=="FINISHED"||x.Status.StartsWith("DELAY",StringComparison.Ordinal);}
        private static string Bucket(DateTime d,string mode){return mode=="month"?d.ToString("yyyy-MM"):d.ToString("yyyy-MM-dd");}
        private static string S(OracleDataReader r,string n){return Convert.ToString(r[n]).Trim();}
        private static string Filter(string s){s=(s??"").Trim().ToUpperInvariant();return s.Length==0||s=="ALL"?null:s;}
        private static void Validate(FlightAnalyticsRequest r,out DateTime from,out DateTime to){if(r==null||!DateTime.TryParseExact(r.FromDate,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out from)||!DateTime.TryParseExact(r.ToDate,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out to))throw new ArgumentException("Ngày lọc không hợp lệ.");if(from>to)throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");}
        private static OracleConnection Connection(){string value=ConfigurationManager.AppSettings["ConnectDB"];if(String.IsNullOrWhiteSpace(value))throw new ConfigurationErrorsException("Thiếu cấu hình AppSettings ConnectDB.");return new OracleConnection(value);}
    }
}
