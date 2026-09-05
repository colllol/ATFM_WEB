using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.FinishFlights
{
    public partial class ListFinishedFlights : PageCoreAdmin
    {
        private sealed class BravoComparisonRow
        {
            public string airline { get; set; }
            public string fromAirp { get; set; }
            public string toAirp { get; set; }
            public string flightDate { get; set; }
            public string callsign { get; set; }
            public int oracleCount { get; set; }
            public string bravoCount { get; set; }
        }

        [WebMethod]
        public static object GetBravoComparison(string fromDate, string toDate, string comparisonType, string region)
        {
            DateTime from = ParseComparisonDate(fromDate);
            DateTime to = ParseComparisonDate(toDate);
            if (to < from) throw new ArgumentException("Khoảng ngày không hợp lệ.");
            string routeCondition;
            switch ((comparisonType ?? "").Trim().ToUpperInvariant())
            {
                case "QN_QT": routeCondition = "UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' AND UPPER(TRIM(f.TO_AIRP)) NOT LIKE 'VV%'"; break;
                case "QT_QN": routeCondition = "UPPER(TRIM(f.FROM_AIRP)) NOT LIKE 'VV%' AND UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%'"; break;
                case "QT_QT": routeCondition = "UPPER(TRIM(f.FROM_AIRP)) NOT LIKE 'VV%' AND UPPER(TRIM(f.TO_AIRP)) NOT LIKE 'VV%'"; break;
                default: routeCondition = "UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' AND UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%'"; break;
            }
            string selectedRegion = (region ?? "").Trim().ToUpperInvariant();
            string sql = @"SELECT UPPER(TRIM(f.OPER_ID)) AIRLINE, UPPER(TRIM(f.FROM_AIRP)) FROM_AIRP,
                                        UPPER(TRIM(f.TO_AIRP)) TO_AIRP, TRUNC(f.FLIGHTDATE) FLIGHT_DATE,
                                        UPPER(TRIM(f.FLIGHTNBR)) CALLSIGN, COUNT(*) ORACLE_COUNT
                                   FROM T_FINISHED_FLIGHTS f
                                   LEFT JOIN M_AERO af ON UPPER(TRIM(af.AE_CODE)) = UPPER(TRIM(f.FROM_AIRP))
                                   LEFT JOIN M_AERO at ON UPPER(TRIM(at.AE_CODE)) = UPPER(TRIM(f.TO_AIRP))
                                  WHERE f.FLIGHTDATE >= :fromDate AND f.FLIGHTDATE < :toDate
                                    AND f.ISACCEPTED = 1
                                    AND f.FLIGHTNBR IS NOT NULL AND f.FROM_AIRP IS NOT NULL AND f.TO_AIRP IS NOT NULL
                                    AND f.OPER_ID IS NOT NULL
                                    AND UPPER(TRIM(f.PERMNBR)) <> 'NOPERM'
                                    AND ( :region IS NULL OR UPPER(TRIM(af.MIEN)) = :region OR UPPER(TRIM(at.MIEN)) = :region )
                                    AND (" + routeCondition + @")
                                  GROUP BY UPPER(TRIM(f.OPER_ID)), UPPER(TRIM(f.FROM_AIRP)), UPPER(TRIM(f.TO_AIRP)), TRUNC(f.FLIGHTDATE), UPPER(TRIM(f.FLIGHTNBR))
                                  ORDER BY TRUNC(f.FLIGHTDATE), UPPER(TRIM(f.OPER_ID)), UPPER(TRIM(f.FLIGHTNBR))";
            List<BravoComparisonRow> rows = new List<BravoComparisonRow>();
            using (OracleConnection connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (OracleCommand command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from.Date;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.Date.AddDays(1);
                command.Parameters.Add("region", OracleDbType.Varchar2).Value = String.IsNullOrWhiteSpace(selectedRegion) || selectedRegion == "ALL" ? (object)DBNull.Value : selectedRegion;
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read()) rows.Add(new BravoComparisonRow
                    {
                        airline = Text(reader["AIRLINE"]), fromAirp = Text(reader["FROM_AIRP"]), toAirp = Text(reader["TO_AIRP"]),
                        flightDate = Convert.ToDateTime(reader["FLIGHT_DATE"], CultureInfo.InvariantCulture).ToString("dd-MM-yyyy"),
                        callsign = Text(reader["CALLSIGN"]), oracleCount = Convert.ToInt32(reader["ORACLE_COUNT"], CultureInfo.InvariantCulture), bravoCount = String.Empty
                    });
                }
            }
            return rows;
        }

        private static DateTime ParseComparisonDate(string value)
        {
            DateTime result;
            if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result)) throw new ArgumentException("Ngày đối chiếu không hợp lệ.");
            return result.Date;
        }

        private static string Text(object value) { return value == null || value == DBNull.Value ? String.Empty : Convert.ToString(value, CultureInfo.InvariantCulture).Trim(); }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                               
            }

        }

        protected void btnExportFin_Click(object sender, EventArgs e)
        {
            Make_Finished();
        }
        public bool Make_Finished()
        {
            var ax = new clsResuftAPI().GetPostValueApiExtension("MAKE_FINISHED", "make_finished_flights_news", new { p_string ="" }).ToString();
            return ax == "1" ? false : true;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string html = GetContent();
            this.CreateExcel(html, "THSLB_BCSL_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        
        public string GetContent()
        {
            string _html = null;
            StringWriter sw = new StringWriter();
            HtmlTextWriter h = new HtmlTextWriter(sw);
            exportid.RenderControl(h);
            _html = sw.GetStringBuilder().ToString();
            return _html;
        }
       
    }


}
