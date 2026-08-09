using System;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.Permission
{
    public sealed class SearchPermissionAdvExport : IHttpHandler
    {
        private static readonly string[] ExportColumns =
        {
            "SOURCE_TYPE", "PERM_ID", "PERMNBR", "AUTHOR", "PTYPE", "FTYPE",
            "PERM_NUMBER", "VERSION_NAME", "PERMISSION_DATE", "OPER",
            "DETAIL_ID", "FLIGHT_PK", "CALLSIGN", "REGISTRATION", "FROM_AIRP",
            "TO_AIRP", "ETD", "ETA", "DAYS_FLIGHT", "BEGIN_DATE", "END_DATE",
            "CRAFT", "PURPOSE", "MTOW", "VIA", "REMARK", "STATUS_VALUE",
            "LAST_USER", "LAST_MODIFY"
        };

        private static readonly string[] ExportHeaders =
        {
            "LOẠI PHÉP", "PERM ID", "SỐ PHÉP", "AUTHOR", "P TYPE", "F TYPE",
            "NUMBER", "VERSION", "NGÀY CẤP PHÉP", "OPER",
            "DETAIL ID", "FLIGHT PK", "CALLSIGN", "REGISTRATION", "FROM",
            "TO", "ETD", "ETA", "DAY/DATE", "BEGIN DATE", "END DATE",
            "CRAFT", "PURPOSE", "MTOW", "VIA", "REMARK", "STATUS",
            "LAST USER", "LAST MODIFY"
        };

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string legacyPermissionDate = context.Request.QueryString["permissionDate"];
                DateTime fromPermissionDate = ParsePermissionDate(
                    context.Request.QueryString["fromPermissionDate"] ?? legacyPermissionDate);
                DateTime toPermissionDate = ParsePermissionDate(
                    context.Request.QueryString["toPermissionDate"] ?? legacyPermissionDate);
                if (fromPermissionDate > toPermissionDate)
                    throw new ArgumentException("Từ ngày cấp phép không được lớn hơn Đến ngày cấp phép.");
                string fromTimeText = (context.Request.QueryString["fromTime"] ?? String.Empty).Trim();
                string toTimeText = (context.Request.QueryString["toTime"] ?? String.Empty).Trim();
                bool useTimeFilter = fromTimeText.Length > 0 || toTimeText.Length > 0;
                TimeSpan fromTime = fromTimeText.Length > 0
                    ? ParseSearchTime(fromTimeText, "Từ giờ")
                    : TimeSpan.Zero;
                TimeSpan toTime = toTimeText.Length > 0
                    ? ParseSearchTime(toTimeText, "Đến giờ")
                    : new TimeSpan(23, 59, 0);
                if (useTimeFilter && fromTime > toTime)
                    throw new ArgumentException("Từ giờ không được lớn hơn Đến giờ.");

                string fromAirp = Normalize(context.Request.QueryString["fromAirp"]);
                string toAirp = Normalize(context.Request.QueryString["toAirp"]);
                string via = Normalize(context.Request.QueryString["via"]);
                string permNbr = Normalize(context.Request.QueryString["permNbr"]);
                string author = Normalize(context.Request.QueryString["author"]);
                string pType = Normalize(context.Request.QueryString["pType"]);
                string fType = Normalize(context.Request.QueryString["fType"]);
                string number = Normalize(context.Request.QueryString["number"]);
                string version = Normalize(context.Request.QueryString["version"]);
                string masterDate = Normalize(context.Request.QueryString["masterDate"]);
                string oper = Normalize(context.Request.QueryString["oper"]);
                bool useDetailFilter = useTimeFilter || fromAirp.Length > 0 ||
                    toAirp.Length > 0 || via.Length > 0;

                string setting = ConfigurationManager.ConnectionStrings["SlotsOracle"] == null
                    ? String.Empty
                    : ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString;
                if (String.IsNullOrWhiteSpace(setting))
                    throw new ConfigurationErrorsException("Chưa cấu hình connection string SlotsOracle.");

                using (var connection = new OracleConnection(setting))
                using (var command = new OracleCommand(BuildExportSql(), connection))
                {
                    command.BindByName = true;
                    command.CommandTimeout = 180;
                    command.Parameters.Add("selectedDate", OracleDbType.Date).Value = fromPermissionDate.Date;
                    command.Parameters.Add("nextDate", OracleDbType.Date).Value = toPermissionDate.Date.AddDays(1);
                    command.Parameters.Add("useDetailFilter", OracleDbType.Int32).Value = useDetailFilter ? 1 : 0;
                    command.Parameters.Add("useTimeFilter", OracleDbType.Int32).Value = useTimeFilter ? 1 : 0;
                    command.Parameters.Add("fromHhmm", OracleDbType.Varchar2).Value =
                        fromTime.ToString(@"hhmm", CultureInfo.InvariantCulture);
                    command.Parameters.Add("toHhmm", OracleDbType.Varchar2).Value =
                        toTime.ToString(@"hhmm", CultureInfo.InvariantCulture);
                    command.Parameters.Add("fromAirp", OracleDbType.Varchar2).Value = ToOracleValue(fromAirp);
                    command.Parameters.Add("toAirp", OracleDbType.Varchar2).Value = ToOracleValue(toAirp);
                    command.Parameters.Add("via", OracleDbType.Varchar2).Value = ToOracleValue(via);
                    command.Parameters.Add("permNbr", OracleDbType.Varchar2).Value = ToOracleValue(permNbr);
                    command.Parameters.Add("author", OracleDbType.Varchar2).Value = ToOracleValue(author);
                    command.Parameters.Add("pType", OracleDbType.Varchar2).Value = ToOracleValue(pType);
                    command.Parameters.Add("fType", OracleDbType.Varchar2).Value = ToOracleValue(fType);
                    command.Parameters.Add("numberFilter", OracleDbType.Varchar2).Value = ToOracleValue(number);
                    command.Parameters.Add("versionFilter", OracleDbType.Varchar2).Value = ToOracleValue(version);
                    command.Parameters.Add("masterDate", OracleDbType.Varchar2).Value = ToOracleValue(masterDate);
                    command.Parameters.Add("oper", OracleDbType.Varchar2).Value = ToOracleValue(oper);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        context.Response.Clear();
                        context.Response.BufferOutput = false;
                        context.Response.ContentType = "application/vnd.ms-excel";
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.Charset = "utf-8";
                        context.Response.AddHeader(
                            "Content-Disposition",
                            "attachment; filename=SearchPermissionAdv_" +
                            fromPermissionDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + "_" +
                            toPermissionDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture) + ".xls");
                        context.Response.BinaryWrite(Encoding.UTF8.GetPreamble());
                        context.Response.Write(BuildExcelHeader(fromPermissionDate, toPermissionDate,
                            useTimeFilter, fromTime, toTime, fromAirp, toAirp, via));

                        while (reader.Read())
                        {
                            context.Response.Write("<tr>");
                            for (int index = 0; index < ExportColumns.Length; index++)
                            {
                                object value = reader[ExportColumns[index]];
                                context.Response.Write("<td>");
                                context.Response.Write(HttpUtility.HtmlEncode(
                                    value == DBNull.Value
                                        ? String.Empty
                                        : Convert.ToString(value, CultureInfo.InvariantCulture)));
                                context.Response.Write("</td>");
                            }
                            context.Response.Write("</tr>");
                        }
                    }
                }

                context.Response.Write("</tbody></table></body></html>");
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.Write("Không thể Export Excel: " + ex.GetBaseException().Message);
                System.Diagnostics.Trace.TraceError("SearchPermissionAdvExport failed: {0}", ex);
            }
        }

        private static string BuildExcelHeader(
            DateTime fromPermissionDate,
            DateTime toPermissionDate,
            bool useTimeFilter,
            TimeSpan fromTime,
            TimeSpan toTime,
            string fromAirp,
            string toAirp,
            string via)
        {
            var criteria = new StringBuilder();
            criteria.Append("Ngày cấp phép: ")
                .Append(fromPermissionDate.ToString("dd-MM-yyyy"))
                .Append(" - ")
                .Append(toPermissionDate.ToString("dd-MM-yyyy"));
            if (useTimeFilter)
            {
                criteria.Append(" | ETD/ETA: ")
                    .Append(fromTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture))
                    .Append(" - ")
                    .Append(toTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture));
            }
            if (fromAirp.Length > 0) criteria.Append(" | FROM: ").Append(fromAirp);
            if (toAirp.Length > 0) criteria.Append(" | TO: ").Append(toAirp);
            if (via.Length > 0) criteria.Append(" | VIA: ").Append(via);

            var html = new StringBuilder();
            html.Append("<html><head><meta charset=\"utf-8\" />")
                .Append("<style>table{border-collapse:collapse;font-family:Arial;font-size:10pt}")
                .Append("th,td{border:1px solid #8ba9bd;padding:5px;mso-number-format:\"\\@\"}")
                .Append("th{background:#337fb7;color:#fff;font-weight:bold}</style></head><body>")
                .Append("<h2>SEARCH PERMISSION ADV</h2><p>")
                .Append(HttpUtility.HtmlEncode(criteria.ToString()))
                .Append("</p><table><thead><tr>");
            for (int index = 0; index < ExportHeaders.Length; index++)
                html.Append("<th>").Append(HttpUtility.HtmlEncode(ExportHeaders[index])).Append("</th>");
            html.Append("</tr></thead><tbody>");
            return html.ToString();
        }

        private static string BuildExportSql()
        {
            return @"
                SELECT *
                FROM
                (
                    SELECT 'SC' SOURCE_TYPE, m.PERM_ID,
                           NVL(TRIM(m.PERMNBR_ID), TRIM(m.PERMNBR)) PERMNBR,
                           NVL(TRIM(a.AUTHOR_NAME), TRIM(m.AUTHOR_ID)) AUTHOR,
                           TRIM(m.PERMTYPE) PTYPE, NVL(TRIM(m.FLIGHTTYPE), 'SC') FTYPE,
                           TRIM(m.PERMNBR) PERM_NUMBER, NVL(TRIM(m.VERSION), '-') VERSION_NAME,
                           TO_CHAR(m.PERMDATE, 'DD-MM-YYYY') PERMISSION_DATE, TRIM(m.OPER_ID) OPER,
                           d.ID DETAIL_ID, d.FLIGHT_PK, d.FLIGHTNBR CALLSIGN, d.REGISTRATION,
                           d.FROM_AIRP, d.TO_AIRP, d.ETD, d.ETA,
                           REPLACE(NVL(d.DAY1, '0') || NVL(d.DAY2, '0') || NVL(d.DAY3, '0') ||
                               NVL(d.DAY4, '0') || NVL(d.DAY5, '0') || NVL(d.DAY6, '0') ||
                               NVL(d.DAY7, '0'), '0', '.') DAYS_FLIGHT,
                           TO_CHAR(d.BEGINDATE, 'DD-MM-YYYY') BEGIN_DATE,
                           TO_CHAR(d.ENDDATE, 'DD-MM-YYYY') END_DATE,
                           NVL(TRIM(c.MA), TO_CHAR(d.CRAFT_ID)) CRAFT,
                           d.PURPOSE_ID PURPOSE, d.MTOW, d.VIA, d.REMARK,
                           NVL(TRIM(d.STATUS), '-') STATUS_VALUE,
                           NVL(TRIM(d.LASTUSER), '-') LAST_USER,
                           NVL(TO_CHAR(d.LASTMODIFY, 'DD-MM-YYYY HH24:MI:SS'), '-') LAST_MODIFY
                    FROM T_PERMMASTER_SC m
                    LEFT JOIN M_FPAUTHOR a ON a.AUTHOR_CODE = m.AUTHOR_ID
                    LEFT JOIN T_PERMDETAIL_SC d ON d.PERM_ID = m.PERM_ID
                    LEFT JOIN M_CRAFT_TYPE c ON c.CRAFT_ID = d.CRAFT_ID
                    WHERE m.PERMDATE >= :selectedDate AND m.PERMDATE < :nextDate
                      AND (:useDetailFilter = 0 OR
                          (d.ID IS NOT NULL
                           AND (:useTimeFilter = 0
                                OR LPAD(TRIM(d.ETD), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                                OR LPAD(TRIM(d.ETA), 4, '0') BETWEEN :fromHhmm AND :toHhmm)
                           AND (:fromAirp IS NULL OR UPPER(TRIM(d.FROM_AIRP)) LIKE '%' || :fromAirp || '%')
                           AND (:toAirp IS NULL OR UPPER(TRIM(d.TO_AIRP)) LIKE '%' || :toAirp || '%')
                           AND (:via IS NULL OR UPPER(TRIM(d.VIA)) LIKE '%' || :via || '%')))

                    UNION ALL

                    SELECT 'NO' SOURCE_TYPE, m.PERM_ID,
                           NVL(TRIM(m.PERMNBR_ID), TRIM(m.PERMNBR)) PERMNBR,
                           NVL(TRIM(a.AUTHOR_NAME), TRIM(m.AUTHOR_ID)) AUTHOR,
                           TRIM(m.PERMTYPE) PTYPE, NVL(TRIM(m.FLIGHTTYPE), 'NO') FTYPE,
                           TRIM(m.PERMNBR) PERM_NUMBER, NVL(TRIM(m.VERSION), '-') VERSION_NAME,
                           TO_CHAR(m.PERMDATE, 'DD-MM-YYYY') PERMISSION_DATE, TRIM(m.OPER_ID) OPER,
                           d.ID DETAIL_ID, d.FLIGHT_PK, d.FLIGHTNBR CALLSIGN, d.REGISTRATION,
                           d.FROM_AIRP, d.TO_AIRP, d.ETD, d.ETA,
                           NVL(TRIM(d.DAYSFLIGHT), '-') DAYS_FLIGHT,
                           NVL(TO_CHAR(d.MAX_DATE, 'DD-MM-YYYY'), '-') BEGIN_DATE,
                           NVL(TO_CHAR(d.MAX_DATE, 'DD-MM-YYYY'), '-') END_DATE,
                           NVL(TRIM(c.MA), TO_CHAR(d.CRAFT_ID)) CRAFT,
                           d.PURPOSE_ID PURPOSE, d.MTOW, d.VIA, d.REMARK,
                           NVL(TRIM(d.STATUS), '-') STATUS_VALUE,
                           NVL(TRIM(d.LASTUSER), '-') LAST_USER,
                           NVL(TO_CHAR(d.LASTMODIFY, 'DD-MM-YYYY HH24:MI:SS'), '-') LAST_MODIFY
                    FROM T_PERMMASTER_NO m
                    LEFT JOIN M_FPAUTHOR a ON a.AUTHOR_CODE = m.AUTHOR_ID
                    LEFT JOIN T_PERMDETAIL_NO d ON d.PERM_ID = m.PERM_ID
                    LEFT JOIN M_CRAFT_TYPE c ON c.CRAFT_ID = d.CRAFT_ID
                    WHERE m.PERMDATE >= :selectedDate AND m.PERMDATE < :nextDate
                      AND (:useDetailFilter = 0 OR
                          (d.ID IS NOT NULL
                           AND (:useTimeFilter = 0
                                OR LPAD(TRIM(d.ETD), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                                OR LPAD(TRIM(d.ETA), 4, '0') BETWEEN :fromHhmm AND :toHhmm)
                           AND (:fromAirp IS NULL OR UPPER(TRIM(d.FROM_AIRP)) LIKE '%' || :fromAirp || '%')
                           AND (:toAirp IS NULL OR UPPER(TRIM(d.TO_AIRP)) LIKE '%' || :toAirp || '%')
                           AND (:via IS NULL OR UPPER(TRIM(d.VIA)) LIKE '%' || :via || '%')))
                ) export_data
                WHERE (:permNbr IS NULL OR UPPER(PERMNBR) LIKE '%' || :permNbr || '%')
                  AND (:author IS NULL OR UPPER(AUTHOR) LIKE '%' || :author || '%')
                  AND (:pType IS NULL OR UPPER(PTYPE) LIKE '%' || :pType || '%')
                  AND (:fType IS NULL OR UPPER(FTYPE) LIKE '%' || :fType || '%')
                  AND (:numberFilter IS NULL OR UPPER(PERM_NUMBER) LIKE '%' || :numberFilter || '%')
                  AND (:versionFilter IS NULL OR UPPER(VERSION_NAME) LIKE '%' || :versionFilter || '%')
                  AND (:masterDate IS NULL OR UPPER(PERMISSION_DATE) LIKE '%' || :masterDate || '%')
                  AND (:oper IS NULL OR UPPER(OPER) LIKE '%' || :oper || '%')
                ORDER BY PERMNBR, SOURCE_TYPE, DETAIL_ID";
        }

        private static DateTime ParsePermissionDate(string value)
        {
            DateTime result;
            if (!DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out result))
                throw new ArgumentException("Ngày cấp phép không hợp lệ.");
            return result.Date;
        }

        private static TimeSpan ParseSearchTime(string value, string fieldName)
        {
            string normalized = (value ?? String.Empty).Trim();
            if (normalized.Length == 4 && normalized.IndexOf(':') < 0)
                normalized = normalized.Insert(2, ":");
            TimeSpan result;
            if (!TimeSpan.TryParseExact(normalized, @"hh\:mm", CultureInfo.InvariantCulture,
                out result) || result < TimeSpan.Zero || result >= TimeSpan.FromDays(1))
                throw new ArgumentException(fieldName + " không hợp lệ. Định dạng yêu cầu HH:mm.");
            return result;
        }

        private static string Normalize(string value)
        {
            return (value ?? String.Empty).Trim().ToUpperInvariant();
        }

        private static object ToOracleValue(string value)
        {
            return String.IsNullOrWhiteSpace(value) ? (object)DBNull.Value : value;
        }
    }
}
