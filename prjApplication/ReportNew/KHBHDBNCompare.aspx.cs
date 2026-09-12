using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Script.Services;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class KHBHDBNCompare : ReportPageBase
    {
        private const string Sql = @"
WITH c AS (SELECT ROW_NUMBER() OVER(PARTITION BY UPPER(TRIM(FLIGHTNBR)),UPPER(TRIM(FROM_AIRP)),UPPER(TRIM(TO_AIRP)) ORDER BY ETD,ETA,ROWID) rn, f.* FROM T_FINISHED_FLIGHTS f WHERE FLIGHTDATE>=:d1 AND FLIGHTDATE<:d1+1 AND ISACCEPTED=1 AND UPPER(TRIM(NVL(PERMNBR,'X'))) NOT LIKE 'NOPERM%'), p AS (SELECT ROW_NUMBER() OVER(PARTITION BY UPPER(TRIM(FLIGHTNBR)),UPPER(TRIM(FROM_AIRP)),UPPER(TRIM(TO_AIRP)) ORDER BY ETD,ETA,ROWID) rn, f.* FROM T_FINISHED_FLIGHTS f WHERE FLIGHTDATE>=:d2 AND FLIGHTDATE<:d2+1 AND ISACCEPTED=1 AND UPPER(TRIM(NVL(PERMNBR,'X'))) NOT LIKE 'NOPERM%'), x AS (SELECT c.FLIGHTNBR c_call,c.FROM_AIRP c_from,c.TO_AIRP c_to,c.ETD c_etd,c.ETA c_eta,c.CRAFT_ID c_craft,c.STATUS c_status,c.PERMNBR c_perm,p.FLIGHTNBR p_call,p.FROM_AIRP p_from,p.TO_AIRP p_to,p.ETD p_etd,p.ETA p_eta,p.CRAFT_ID p_craft,p.STATUS p_status,p.PERMNBR p_perm,CASE WHEN c.FLIGHTNBR IS NULL THEN 'MISSING' WHEN p.FLIGHTNBR IS NULL THEN 'NEW' WHEN c.ETD=p.ETD AND c.ETA=p.ETA AND c.FROM_AIRP=p.FROM_AIRP AND c.TO_AIRP=p.TO_AIRP AND NVL(c.CRAFT_ID,'X')=NVL(p.CRAFT_ID,'X') AND NVL(c.STATUS,'X')=NVL(p.STATUS,'X') THEN 'NORMAL' ELSE 'CHANGED' END category FROM c FULL OUTER JOIN p ON UPPER(TRIM(c.FLIGHTNBR))=UPPER(TRIM(p.FLIGHTNBR)) AND UPPER(TRIM(c.FROM_AIRP))=UPPER(TRIM(p.FROM_AIRP)) AND UPPER(TRIM(c.TO_AIRP))=UPPER(TRIM(p.TO_AIRP)) AND c.rn=p.rn), d AS (SELECT x.*,CASE WHEN c_call IS NULL OR p_call IS NULL THEN NULL WHEN COUNT(*) OVER(PARTITION BY NVL(c_call,p_call),NVL(c_from,p_from),NVL(c_to,p_to))>1 THEN 'Trùng số hiệu/chặng' WHEN category='CHANGED' THEN 'Khác giờ, route, loại tàu bay hoặc trạng thái' END reason FROM x) SELECT category,c_call,c_from,c_to,c_etd,c_eta,c_craft,c_perm,p_call,p_from,p_to,p_etd,p_eta,p_craft,p_perm,reason FROM d WHERE (:cat IS NULL OR category=:cat) ORDER BY category,c_call";

        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static object GetData(string currentDate, string compareDate, string category)
        {
            DateTime d1 = Parse(currentDate), d2 = d1.AddDays(-7); var list = new List<Dictionary<string, object>>();
            using (var cn = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString)) using (var cmd = new OracleCommand(Sql.Replace("T_FINISHED_FLIGHTS", "T_DAY_FLIGHTS"), cn))
            { cmd.BindByName=true; cmd.Parameters.Add("d1",OracleDbType.Date).Value=d1; cmd.Parameters.Add("d2",OracleDbType.Date).Value=d2; cmd.Parameters.Add("cat",OracleDbType.Varchar2).Value=String.IsNullOrWhiteSpace(category)?(object)DBNull.Value:category; cn.Open(); using(var rd=cmd.ExecuteReader()){while(rd.Read()){var o=new Dictionary<string,object>(); for(int i=0;i<rd.FieldCount;i++) o[rd.GetName(i)]=rd.IsDBNull(i)?null:rd.GetValue(i); list.Add(o);}} }
            return new { Code="00", ListValue=list };
        }
        private static DateTime Parse(string value) { DateTime d; if(!DateTime.TryParseExact(value,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out d)) throw new ArgumentException("Ngày không hợp lệ"); return d.Date; }
    }
}
