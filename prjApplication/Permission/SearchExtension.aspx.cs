using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Services;
using System.Web.UI;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.Permission
{
    public partial class SearchExtension : Page
    {
        public sealed class SearchRequest
        {
            public string FlightNbr { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Craft { get; set; }
            public string Via { get; set; }
            public string FlightDate { get; set; }
            public string PermNbr { get; set; }
            public string Oper { get; set; }
            public string FlightType { get; set; }
            public string PermType { get; set; }
            public string Etd { get; set; }
            public string Purpose { get; set; }
            public string Remark { get; set; }
            public int PageSize { get; set; }
            public int PageIndex { get; set; }
        }

        private sealed class SearchItem
        {
            public long RNUM { get; set; }
            public long RECORD_SUM { get; set; }
            public long PERM_ID { get; set; }
            public string PERMNBR_ID { get; set; }
            public string PERMDATE { get; set; }
            public string DAYLY { get; set; }
            public string VALIDDATE { get; set; }
            public string VALIDDATEPER { get; set; }
            public string FLIGHTNBR { get; set; }
            public string FROM_AIRP { get; set; }
            public string TO_AIRP { get; set; }
            public string ETD { get; set; }
            public string PERMTYPE { get; set; }
            public string FTYPE { get; set; }
            public string OPER_ID { get; set; }
            public string PURPOSE_ID { get; set; }
            public string CRAFT { get; set; }
            public string VIA { get; set; }
            public string REMARK { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static object SearchPermissions(SearchRequest request)
        {
            try
            {
                request = request ?? new SearchRequest();
                int pageSize = Math.Max(1, Math.Min(request.PageSize <= 0 ? 500 : request.PageSize, 1000));
                int pageIndex = Math.Max(0, request.PageIndex);
                int firstRow = pageIndex * pageSize + 1;
                int lastRow = firstRow + pageSize - 1;
                DateTime? flightDate = ParseOptionalDate(request.FlightDate);
                var items = new List<SearchItem>();

                const string sql = @"
                    SELECT RNUM, RECORD_SUM, PERM_ID, PERMNBR_ID, PERMDATE,
                           DAYLY, VALIDDATE, VALIDDATEPER, FLIGHTNBR,
                           FROM_AIRP, TO_AIRP, ETD, PERMTYPE, FTYPE,
                           OPER_ID, PURPOSE_ID, CRAFT, VIA, REMARK
                    FROM
                    (
                        SELECT ROW_NUMBER() OVER
                               (
                                   ORDER BY m.PERMDATE DESC, m.PERM_ID DESC, d.ID DESC
                               ) RNUM,
                               COUNT(*) OVER () RECORD_SUM,
                               m.PERM_ID,
                               NVL(TRIM(m.PERMNBR_ID), TRIM(m.PERMNBR)) PERMNBR_ID,
                               TO_CHAR(m.PERMDATE, 'YYYY-MM-DD') PERMDATE,
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY1)), '0') = '0' THEN '.' ELSE '1' END ||
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY2)), '0') = '0' THEN '.' ELSE '2' END ||
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY3)), '0') = '0' THEN '.' ELSE '3' END ||
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY4)), '0') = '0' THEN '.' ELSE '4' END ||
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY5)), '0') = '0' THEN '.' ELSE '5' END ||
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY6)), '0') = '0' THEN '.' ELSE '6' END ||
                               CASE WHEN NVL(TRIM(TO_CHAR(d.DAY7)), '0') = '0' THEN '.' ELSE '7' END DAYLY,
                               NVL(TRIM(TO_CHAR(m.VALIDHOURS)), '') VALIDDATE,
                               TO_CHAR(d.BEGINDATE, 'DD-MM-YYYY') || '->' ||
                                   TO_CHAR(d.ENDDATE, 'DD-MM-YYYY') VALIDDATEPER,
                               TRIM(d.FLIGHTNBR) FLIGHTNBR,
                               TRIM(d.FROM_AIRP) FROM_AIRP,
                               TRIM(d.TO_AIRP) TO_AIRP,
                               TRIM(d.ETD) ETD,
                               TRIM(m.PERMTYPE) PERMTYPE,
                               TRIM(m.FLIGHTTYPE) FTYPE,
                               TRIM(m.OPER_ID) OPER_ID,
                               TRIM(d.PURPOSE_ID) PURPOSE_ID,
                               TRIM(c.MA) CRAFT,
                               TRIM(d.VIA) VIA,
                               TRIM(d.REMARK) REMARK
                        FROM T_PERMMASTER_SC m
                        JOIN T_PERMDETAIL_SC d ON d.PERM_ID = m.PERM_ID
                        LEFT JOIN M_CRAFT_TYPE c ON c.CRAFT_ID = d.CRAFT_ID
                        WHERE (:flightNbr IS NULL OR UPPER(TRIM(d.FLIGHTNBR)) = :flightNbr)
                          AND (:fromAirp IS NULL OR UPPER(TRIM(d.FROM_AIRP)) LIKE '%' || :fromAirp || '%')
                          AND (:toAirp IS NULL OR UPPER(TRIM(d.TO_AIRP)) LIKE '%' || :toAirp || '%')
                          AND (:craft IS NULL OR UPPER(TRIM(c.MA)) LIKE '%' || :craft || '%')
                          AND (:via IS NULL OR UPPER(TRIM(d.VIA)) LIKE '%' || :via || '%')
                          AND
                          (
                              :permNbr IS NULL
                              OR UPPER(TRIM(m.PERMNBR_ID)) LIKE '%' || :permNbr || '%'
                              OR UPPER(TRIM(m.PERMNBR)) LIKE '%' || :permNbr || '%'
                          )
                          AND (:oper IS NULL OR UPPER(TRIM(m.OPER_ID)) LIKE '%' || :oper || '%')
                          AND (:flightType IS NULL OR UPPER(TRIM(m.FLIGHTTYPE)) = :flightType)
                          AND (:permType IS NULL OR UPPER(TRIM(m.PERMTYPE)) = :permType)
                          AND (:etd IS NULL OR UPPER(TRIM(d.ETD)) LIKE '%' || :etd || '%')
                          AND (:purpose IS NULL OR UPPER(TRIM(d.PURPOSE_ID)) = :purpose)
                          AND (:remark IS NULL OR UPPER(TRIM(d.REMARK)) LIKE '%' || :remark || '%')
                          AND
                          (
                              :flightDate IS NULL
                              OR
                              (
                                  TRUNC(:flightDate) BETWEEN TRUNC(d.BEGINDATE)
                                      AND TRUNC(NVL(d.ENDDATE, d.BEGINDATE))
                                  AND
                                  CASE TRUNC(:flightDate) - TRUNC(:flightDate, 'IW') + 1
                                      WHEN 1 THEN NVL(TRIM(TO_CHAR(d.DAY1)), '0')
                                      WHEN 2 THEN NVL(TRIM(TO_CHAR(d.DAY2)), '0')
                                      WHEN 3 THEN NVL(TRIM(TO_CHAR(d.DAY3)), '0')
                                      WHEN 4 THEN NVL(TRIM(TO_CHAR(d.DAY4)), '0')
                                      WHEN 5 THEN NVL(TRIM(TO_CHAR(d.DAY5)), '0')
                                      WHEN 6 THEN NVL(TRIM(TO_CHAR(d.DAY6)), '0')
                                      WHEN 7 THEN NVL(TRIM(TO_CHAR(d.DAY7)), '0')
                                  END <> '0'
                              )
                          )
                    ) filtered
                    WHERE filtered.RNUM BETWEEN :firstRow AND :lastRow
                    ORDER BY filtered.RNUM";

                using (var connection = CreateConnection())
                using (var command = new OracleCommand(sql, connection))
                {
                    command.BindByName = true;
                    command.CommandTimeout = 60;
                    AddString(command, "flightNbr", Normalize(request.FlightNbr));
                    AddString(command, "fromAirp", Normalize(request.FromAirp));
                    AddString(command, "toAirp", Normalize(request.ToAirp));
                    AddString(command, "craft", Normalize(request.Craft));
                    AddString(command, "via", Normalize(request.Via));
                    AddString(command, "permNbr", Normalize(request.PermNbr));
                    AddString(command, "oper", Normalize(request.Oper));
                    AddString(command, "flightType", Normalize(request.FlightType));
                    AddString(command, "permType", Normalize(request.PermType));
                    AddString(command, "etd", Normalize(request.Etd));
                    AddString(command, "purpose", Normalize(request.Purpose));
                    AddString(command, "remark", Normalize(request.Remark));
                    command.Parameters.Add("flightDate", OracleDbType.Date).Value =
                        flightDate.HasValue ? (object)flightDate.Value.Date : DBNull.Value;
                    command.Parameters.Add("firstRow", OracleDbType.Int32).Value = firstRow;
                    command.Parameters.Add("lastRow", OracleDbType.Int32).Value = lastRow;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new SearchItem
                            {
                                RNUM = ReadInt64(reader, "RNUM"),
                                RECORD_SUM = ReadInt64(reader, "RECORD_SUM"),
                                PERM_ID = ReadInt64(reader, "PERM_ID"),
                                PERMNBR_ID = ReadString(reader, "PERMNBR_ID"),
                                PERMDATE = ReadString(reader, "PERMDATE"),
                                DAYLY = ReadString(reader, "DAYLY"),
                                VALIDDATE = ReadString(reader, "VALIDDATE"),
                                VALIDDATEPER = ReadString(reader, "VALIDDATEPER"),
                                FLIGHTNBR = ReadString(reader, "FLIGHTNBR"),
                                FROM_AIRP = ReadString(reader, "FROM_AIRP"),
                                TO_AIRP = ReadString(reader, "TO_AIRP"),
                                ETD = ReadString(reader, "ETD"),
                                PERMTYPE = ReadString(reader, "PERMTYPE"),
                                FTYPE = ReadString(reader, "FTYPE"),
                                OPER_ID = ReadString(reader, "OPER_ID"),
                                PURPOSE_ID = ReadString(reader, "PURPOSE_ID"),
                                CRAFT = ReadString(reader, "CRAFT"),
                                VIA = ReadString(reader, "VIA"),
                                REMARK = ReadString(reader, "REMARK")
                            });
                        }
                    }
                }

                return new
                {
                    Code = "00",
                    Message = "Success",
                    Total = items.Count == 0 ? 0L : items[0].RECORD_SUM,
                    Items = items
                };
            }
            catch (Exception ex)
            {
                Exception rootError = ex.GetBaseException();
                System.Diagnostics.Trace.TraceError("SearchExtension.SearchPermissions failed: {0}", ex);
                return new { Code = "99", Message = rootError.Message, Total = 0, Items = new SearchItem[0] };
            }
        }

        private static DateTime? ParseOptionalDate(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;

            DateTime parsed;
            string[] formats = { "dd-MM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd" };
            if (!DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out parsed))
                throw new ArgumentException("FLIGHT DATE không hợp lệ. Định dạng yêu cầu DD-MM-YYYY.");
            return parsed.Date;
        }

        private static string Normalize(string value)
        {
            return String.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
        }

        private static void AddString(OracleCommand command, string name, string value)
        {
            command.Parameters.Add(name, OracleDbType.Varchar2).Value =
                String.IsNullOrEmpty(value) ? (object)DBNull.Value : value;
        }

        private static OracleConnection CreateConnection()
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["SlotsOracle"];
            if (setting == null || String.IsNullOrWhiteSpace(setting.ConnectionString))
                throw new ConfigurationErrorsException("Chưa cấu hình connection string SlotsOracle.");
            return new OracleConnection(setting.ConnectionString);
        }

        private static long ReadInt64(OracleDataReader reader, string columnName)
        {
            object value = reader[columnName];
            return value == DBNull.Value ? 0L : Convert.ToInt64(value, CultureInfo.InvariantCulture);
        }

        private static string ReadString(OracleDataReader reader, string columnName)
        {
            object value = reader[columnName];
            return value == DBNull.Value ? String.Empty : Convert.ToString(value, CultureInfo.InvariantCulture);
        }
    }
}
