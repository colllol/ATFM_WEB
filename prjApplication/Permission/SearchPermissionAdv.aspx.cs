using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.Permission
{
    public partial class SearchPermissionAdv : PageBaseCallBack
    {
        private sealed class PermissionSummary
        {
            public string SourceType { get; set; }
            public long PermId { get; set; }
            public string PermNbr { get; set; }
            public string Author { get; set; }
            public string PType { get; set; }
            public string FType { get; set; }
            public string Number { get; set; }
            public string Version { get; set; }
            public string PermissionDate { get; set; }
            public string Oper { get; set; }
        }

        private sealed class PermissionFlightInfo
        {
            public long Id { get; set; }
            public long FlightPk { get; set; }
            public string Callsign { get; set; }
            public string Registration { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string Eta { get; set; }
            public string DaysFlight { get; set; }
            public string BeginDate { get; set; }
            public string EndDate { get; set; }
            public string Craft { get; set; }
            public string Purpose { get; set; }
            public string Mtow { get; set; }
            public string Via { get; set; }
            public string Remark { get; set; }
            public string Status { get; set; }
            public string LastUser { get; set; }
            public string LastModify { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static object SearchByPermissionDate(
            string permissionDate,
            string fromTime,
            string toTime)
        {
            try
            {
                DateTime selectedDate = ParsePermissionDate(permissionDate);
                TimeSpan selectedFromTime = ParseSearchTime(fromTime, "Từ giờ");
                TimeSpan selectedToTime = ParseSearchTime(toTime, "Đến giờ");
                if (selectedFromTime > selectedToTime)
                    throw new ArgumentException("Từ giờ không được lớn hơn Đến giờ.");

                string selectedFromHhmm = selectedFromTime.ToString(@"hhmm", CultureInfo.InvariantCulture);
                string selectedToHhmm = selectedToTime.ToString(@"hhmm", CultureInfo.InvariantCulture);
                var items = new List<PermissionSummary>();

                const string sql = @"
                    SELECT SOURCE_TYPE, PERM_ID, PERMNBR, AUTHOR, PTYPE, FTYPE,
                           PERM_NUMBER, VERSION_NAME, PERMISSION_DATE, OPER
                    FROM
                    (
                        SELECT 'SC' SOURCE_TYPE,
                               m.PERM_ID,
                               NVL(TRIM(m.PERMNBR_ID), TRIM(m.PERMNBR)) PERMNBR,
                               NVL(TRIM(a.AUTHOR_NAME), TRIM(m.AUTHOR_ID)) AUTHOR,
                               TRIM(m.PERMTYPE) PTYPE,
                               NVL(TRIM(m.FLIGHTTYPE), 'SC') FTYPE,
                               TRIM(m.PERMNBR) PERM_NUMBER,
                               NVL(TRIM(m.VERSION), '-') VERSION_NAME,
                               TO_CHAR(m.PERMDATE, 'DD-MM-YYYY') PERMISSION_DATE,
                               TRIM(m.OPER_ID) OPER
                        FROM T_PERMMASTER_SC m
                        LEFT JOIN M_FPAUTHOR a ON a.AUTHOR_CODE = m.AUTHOR_ID
                        WHERE m.PERMDATE >= :selectedDate
                          AND m.PERMDATE < :nextDate
                          AND EXISTS
                          (
                              SELECT 1
                              FROM T_PERMDETAIL_SC d
                              WHERE d.PERM_ID = m.PERM_ID
                                AND
                                (
                                    LPAD(TRIM(d.ETD), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                                    OR LPAD(TRIM(d.ETA), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                                )
                          )

                        UNION ALL

                        SELECT 'NO' SOURCE_TYPE,
                               m.PERM_ID,
                               NVL(TRIM(m.PERMNBR_ID), TRIM(m.PERMNBR)) PERMNBR,
                               NVL(TRIM(a.AUTHOR_NAME), TRIM(m.AUTHOR_ID)) AUTHOR,
                               TRIM(m.PERMTYPE) PTYPE,
                               NVL(TRIM(m.FLIGHTTYPE), 'NO') FTYPE,
                               TRIM(m.PERMNBR) PERM_NUMBER,
                               NVL(TRIM(m.VERSION), '-') VERSION_NAME,
                               TO_CHAR(m.PERMDATE, 'DD-MM-YYYY') PERMISSION_DATE,
                               TRIM(m.OPER_ID) OPER
                        FROM T_PERMMASTER_NO m
                        LEFT JOIN M_FPAUTHOR a ON a.AUTHOR_CODE = m.AUTHOR_ID
                        WHERE m.PERMDATE >= :selectedDate
                          AND m.PERMDATE < :nextDate
                          AND EXISTS
                          (
                              SELECT 1
                              FROM T_PERMDETAIL_NO d
                              WHERE d.PERM_ID = m.PERM_ID
                                AND
                                (
                                    LPAD(TRIM(d.ETD), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                                    OR LPAD(TRIM(d.ETA), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                                )
                          )
                    )
                    ORDER BY PERMNBR, SOURCE_TYPE";

                using (var connection = CreateConnection())
                using (var command = new OracleCommand(sql, connection))
                {
                    command.BindByName = true;
                    command.CommandTimeout = 60;
                    command.Parameters.Add("selectedDate", OracleDbType.Date).Value = selectedDate.Date;
                    command.Parameters.Add("nextDate", OracleDbType.Date).Value = selectedDate.Date.AddDays(1);
                    command.Parameters.Add("fromHhmm", OracleDbType.Varchar2).Value = selectedFromHhmm;
                    command.Parameters.Add("toHhmm", OracleDbType.Varchar2).Value = selectedToHhmm;
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new PermissionSummary
                            {
                                SourceType = ReadString(reader, "SOURCE_TYPE"),
                                PermId = ReadInt64(reader, "PERM_ID"),
                                PermNbr = ReadString(reader, "PERMNBR"),
                                Author = ReadString(reader, "AUTHOR"),
                                PType = ReadString(reader, "PTYPE"),
                                FType = ReadString(reader, "FTYPE"),
                                Number = ReadString(reader, "PERM_NUMBER"),
                                Version = ReadString(reader, "VERSION_NAME"),
                                PermissionDate = ReadString(reader, "PERMISSION_DATE"),
                                Oper = ReadString(reader, "OPER")
                            });
                        }
                    }
                }

                return new
                {
                    Code = "00",
                    Message = "Success",
                    PermissionDate = selectedDate.ToString("dd-MM-yyyy"),
                    FromTime = selectedFromTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture),
                    ToTime = selectedToTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture),
                    Total = items.Count,
                    Items = items
                };
            }
            catch (Exception ex)
            {
                return ErrorResult("SearchByPermissionDate", ex);
            }
        }

        [WebMethod]
        public static object GetPermissionDetail(
            string sourceType,
            long permId,
            string fromTime,
            string toTime)
        {
            try
            {
                string normalizedType = (sourceType ?? String.Empty).Trim().ToUpperInvariant();
                if (normalizedType != "SC" && normalizedType != "NO")
                    throw new ArgumentException("Loại phép phải là SC hoặc NO.");
                if (permId <= 0)
                    throw new ArgumentException("PERM_ID không hợp lệ.");

                TimeSpan selectedFromTime = ParseSearchTime(fromTime, "Từ giờ");
                TimeSpan selectedToTime = ParseSearchTime(toTime, "Đến giờ");
                if (selectedFromTime > selectedToTime)
                    throw new ArgumentException("Từ giờ không được lớn hơn Đến giờ.");

                string selectedFromHhmm = selectedFromTime.ToString(@"hhmm", CultureInfo.InvariantCulture);
                string selectedToHhmm = selectedToTime.ToString(@"hhmm", CultureInfo.InvariantCulture);

                List<PermissionFlightInfo> flights;

                using (var connection = CreateConnection())
                {
                    connection.Open();
                    flights = LoadFlights(
                        connection,
                        normalizedType,
                        permId,
                        selectedFromHhmm,
                        selectedToHhmm);
                }

                return new
                {
                    Code = "00",
                    Message = "Success",
                    SourceType = normalizedType,
                    PermId = permId,
                    Total = flights.Count,
                    Flights = flights
                };
            }
            catch (Exception ex)
            {
                return ErrorResult("GetPermissionDetail", ex);
            }
        }

        private static List<PermissionFlightInfo> LoadFlights(
            OracleConnection connection,
            string sourceType,
            long permId,
            string fromHhmm,
            string toHhmm)
        {
            string sql = sourceType == "SC" ? @"
                SELECT d.ID, d.FLIGHT_PK, d.FLIGHTNBR, d.REGISTRATION,
                       d.FROM_AIRP, d.TO_AIRP, d.ETD, d.ETA,
                       REPLACE(
                           NVL(d.DAY1, '0') || NVL(d.DAY2, '0') || NVL(d.DAY3, '0') ||
                           NVL(d.DAY4, '0') || NVL(d.DAY5, '0') || NVL(d.DAY6, '0') || NVL(d.DAY7, '0'),
                           '0', '.'
                       ) DAYS_FLIGHT,
                       TO_CHAR(d.BEGINDATE, 'DD-MM-YYYY') BEGIN_DATE,
                       TO_CHAR(d.ENDDATE, 'DD-MM-YYYY') END_DATE,
                       NVL(TRIM(c.MA), TO_CHAR(d.CRAFT_ID)) CRAFT_NAME,
                       d.PURPOSE_ID, d.MTOW, d.VIA, d.REMARK,
                       NVL(TRIM(d.STATUS), '-') STATUS_VALUE,
                       NVL(TRIM(d.LASTUSER), '-') LAST_USER,
                       NVL(TO_CHAR(d.LASTMODIFY, 'DD-MM-YYYY HH24:MI:SS'), '-') LAST_MODIFY
                FROM T_PERMDETAIL_SC d
                LEFT JOIN M_CRAFT_TYPE c ON c.CRAFT_ID = d.CRAFT_ID
                WHERE d.PERM_ID = :permId
                  AND
                  (
                      LPAD(TRIM(d.ETD), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                      OR LPAD(TRIM(d.ETA), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                  )
                ORDER BY d.ID" : @"
                SELECT d.ID, d.FLIGHT_PK, d.FLIGHTNBR, d.REGISTRATION,
                       d.FROM_AIRP, d.TO_AIRP, d.ETD, d.ETA,
                       NVL(TRIM(d.DAYSFLIGHT), '-') DAYS_FLIGHT,
                       NVL(TO_CHAR(d.MAX_DATE, 'DD-MM-YYYY'), '-') BEGIN_DATE,
                       NVL(TO_CHAR(d.MAX_DATE, 'DD-MM-YYYY'), '-') END_DATE,
                       NVL(TRIM(c.MA), TO_CHAR(d.CRAFT_ID)) CRAFT_NAME,
                       d.PURPOSE_ID, d.MTOW, d.VIA, d.REMARK,
                       NVL(TRIM(d.STATUS), '-') STATUS_VALUE,
                       NVL(TRIM(d.LASTUSER), '-') LAST_USER,
                       NVL(TO_CHAR(d.LASTMODIFY, 'DD-MM-YYYY HH24:MI:SS'), '-') LAST_MODIFY
                FROM T_PERMDETAIL_NO d
                LEFT JOIN M_CRAFT_TYPE c ON c.CRAFT_ID = d.CRAFT_ID
                WHERE d.PERM_ID = :permId
                  AND
                  (
                      LPAD(TRIM(d.ETD), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                      OR LPAD(TRIM(d.ETA), 4, '0') BETWEEN :fromHhmm AND :toHhmm
                  )
                ORDER BY d.ID";

            var flights = new List<PermissionFlightInfo>();
            using (var command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                command.CommandTimeout = 60;
                command.Parameters.Add("permId", OracleDbType.Int64).Value = permId;
                command.Parameters.Add("fromHhmm", OracleDbType.Varchar2).Value = fromHhmm;
                command.Parameters.Add("toHhmm", OracleDbType.Varchar2).Value = toHhmm;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        flights.Add(new PermissionFlightInfo
                        {
                            Id = ReadInt64(reader, "ID"),
                            FlightPk = ReadInt64(reader, "FLIGHT_PK"),
                            Callsign = ReadString(reader, "FLIGHTNBR"),
                            Registration = ReadString(reader, "REGISTRATION"),
                            FromAirp = ReadString(reader, "FROM_AIRP"),
                            ToAirp = ReadString(reader, "TO_AIRP"),
                            Etd = ReadString(reader, "ETD"),
                            Eta = ReadString(reader, "ETA"),
                            DaysFlight = ReadString(reader, "DAYS_FLIGHT"),
                            BeginDate = ReadString(reader, "BEGIN_DATE"),
                            EndDate = ReadString(reader, "END_DATE"),
                            Craft = ReadString(reader, "CRAFT_NAME"),
                            Purpose = ReadString(reader, "PURPOSE_ID"),
                            Mtow = ReadString(reader, "MTOW"),
                            Via = ReadString(reader, "VIA"),
                            Remark = ReadString(reader, "REMARK"),
                            Status = ReadString(reader, "STATUS_VALUE"),
                            LastUser = ReadString(reader, "LAST_USER"),
                            LastModify = ReadString(reader, "LAST_MODIFY")
                        });
                    }
                }
            }

            return flights;
        }

        private static DateTime ParsePermissionDate(string value)
        {
            DateTime result;
            if (!DateTime.TryParseExact(
                value,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result))
            {
                throw new ArgumentException("Ngày cấp phép không hợp lệ.");
            }
            return result.Date;
        }

        private static TimeSpan ParseSearchTime(string value, string fieldName)
        {
            string normalized = (value ?? String.Empty).Trim();
            if (normalized.Length == 4 && normalized.IndexOf(':') < 0)
                normalized = normalized.Insert(2, ":");

            TimeSpan result;
            if (!TimeSpan.TryParseExact(
                    normalized,
                    @"hh\:mm",
                    CultureInfo.InvariantCulture,
                    out result) ||
                result < TimeSpan.Zero ||
                result >= TimeSpan.FromDays(1))
            {
                throw new ArgumentException(fieldName + " không hợp lệ. Định dạng yêu cầu HH:mm.");
            }

            return result;
        }

        private static OracleConnection CreateConnection()
        {
            var setting = ConfigurationManager.ConnectionStrings["SlotsOracle"];
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

        private static object ErrorResult(string action, Exception ex)
        {
            Exception rootError = ex.GetBaseException();
            System.Diagnostics.Trace.TraceError(
                "SearchPermissionAdv.{0} failed: {1}",
                action,
                ex);

            return new
            {
                Code = "99",
                Message = action + ": " + rootError.Message
            };
        }
    }
}
