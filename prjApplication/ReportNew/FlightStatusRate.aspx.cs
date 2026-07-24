using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class FlightStatusRate : ReportPageBase
    {
        [WebMethod]
        public static object GetData(string fromDate, string toDate, string oper, string airport, bool currentDay)
        {
            DateTime from;
            DateTime to;
            if (!DateTime.TryParseExact(fromDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out from) ||
                !DateTime.TryParseExact(toDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out to))
                throw new ArgumentException("Ngày lọc không hợp lệ.");

            if (from > to)
                throw new ArgumentException("Khoảng ngày phải hợp lệ và không vượt quá ngày hiện tại.");

            string table = currentDay ? "T_DAY_FLIGHTS_GOINGON" : "T_FINISHED_FLIGHTS";
            string fromClause;
            string plannedTime;
            string auxiliaryCtes = String.Empty;
            if (currentDay)
            {
                fromClause = table + " f";
                plannedTime = BuildEobtFallbackSql("f.EOBTDATE", "f.EOBT", "NULL");
            }
            else
            {
                // T_FINISHED_FLIGHTS không lưu EOBT. Tìm lại FPL theo REGISTRATION + FLIGHTDATE,
                // sau đó xác thực thêm FLIGHT_ID/CALLSIGN/hành trình để không lấy nhầm chặng bay
                // khi cùng một tàu bay thực hiện nhiều chuyến trong ngày.
                auxiliaryCtes = BuildHistoricalFplCtes();
                fromClause = table + @" f
                    LEFT JOIN historical_fpl day_fpl
                      ON day_fpl.finished_rowid=ROWIDTOCHAR(f.ROWID)
                     AND day_fpl.match_order=1";
                plannedTime = BuildEobtFallbackSql("day_fpl.EOBTDATE", "day_fpl.EOBT", "NULL");
            }
            string sql = BuildSql(auxiliaryCtes, fromClause, plannedTime, currentDay);
            var flights = new List<object>();
            int finished = 0, cancel = 0, delay = 0, wait = 0;

            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
                command.Parameters.Add("oper", OracleDbType.Varchar2).Value = String.IsNullOrWhiteSpace(oper) || oper == "ALL" ? (object)DBNull.Value : oper.Trim().ToUpperInvariant();
                command.Parameters.Add("airport", OracleDbType.Varchar2).Value = String.IsNullOrWhiteSpace(airport) || airport == "ALL" ? (object)DBNull.Value : airport.Trim().ToUpperInvariant();
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string state = Convert.ToString(reader["FLIGHT_STATE"]);
                        // Với kỳ quá khứ, danh sách hủy phải lấy duy nhất từ bảng chuyên biệt bên dưới.
                        if (!currentDay && state == "CANCEL")
                            continue;
                        if (state == "FINISHED") finished++;
                        else if (state == "CANCEL") cancel++;
                        else if (state.StartsWith("DELAY", StringComparison.Ordinal)) delay++;
                        else wait++;
                        flights.Add(new {
                            callsign = Convert.ToString(reader["FLIGHTNBR"]),
                            oper = Convert.ToString(reader["OPER_ID"]),
                            registration = Convert.ToString(reader["REGISTRATION"]),
                            permType = Convert.ToString(reader["PERMTYPE"]),
                            fromAirp = Convert.ToString(reader["FROM_AIRP"]),
                            toAirp = Convert.ToString(reader["TO_AIRP"]),
                            atdDay = Convert.ToString(reader["ATDDAY"]),
                            ataDay = Convert.ToString(reader["ATADAY"]),
                            eobtDay = Convert.ToString(reader["EOBTDAY"]),
                            status = state
                        });
                    }
                }
            }
            if (!currentDay)
            {
                const string cancelSql = @"SELECT FLIGHTNBR, OPER_ID, REGISTRATION, PERMTYPE, FROM_AIRP, TO_AIRP,
                                                  NULLIF(TRIM(ATD),'') ATDDAY, NULLIF(TRIM(ATA),'') ATADAY,
                                                  NULLIF(TRIM(ETD),'') EOBTDAY
                                             FROM T_DAY_FLIGHTS_CANCEL
                                            WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL
                                              AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate
                                              AND (:oper IS NULL OR UPPER(TRIM(OPER_ID))=:oper)
                                              AND (:airport IS NULL OR UPPER(TRIM(FROM_AIRP))=:airport OR UPPER(TRIM(TO_AIRP))=:airport)";
                using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
                using (var command = new OracleCommand(cancelSql, connection))
                {
                    command.BindByName = true;
                    command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
                    command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
                    command.Parameters.Add("oper", OracleDbType.Varchar2).Value = String.IsNullOrWhiteSpace(oper) || oper == "ALL" ? (object)DBNull.Value : oper.Trim().ToUpperInvariant();
                    command.Parameters.Add("airport", OracleDbType.Varchar2).Value = String.IsNullOrWhiteSpace(airport) || airport == "ALL" ? (object)DBNull.Value : airport.Trim().ToUpperInvariant();
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cancel++;
                            flights.Add(new {
                                callsign = Convert.ToString(reader["FLIGHTNBR"]), oper = Convert.ToString(reader["OPER_ID"]),
                                registration = Convert.ToString(reader["REGISTRATION"]), permType = Convert.ToString(reader["PERMTYPE"]),
                                fromAirp = Convert.ToString(reader["FROM_AIRP"]), toAirp = Convert.ToString(reader["TO_AIRP"]),
                                atdDay = Convert.ToString(reader["ATDDAY"]), ataDay = Convert.ToString(reader["ATADAY"]),
                                eobtDay = Convert.ToString(reader["EOBTDAY"]), status = "CANCEL"
                            });
                        }
                    }
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
            var values = new List<string>();
            const string sql = @"SELECT OPER_ID FROM (
                                   SELECT UPPER(TRIM(OPER_ID)) OPER_ID
                                     FROM T_DAY_FLIGHTS_GOINGON
                                    WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL
                                      AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate
                                   UNION
                                   SELECT UPPER(TRIM(OPER_ID)) OPER_ID
                                   FROM T_FINISHED_FLIGHTS
                                    WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL
                                      AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate
                                   UNION
                                   SELECT UPPER(TRIM(OPER_ID)) OPER_ID
                                     FROM T_DAY_FLIGHTS_CANCEL
                                    WHERE PERMTYPE='LD' AND OPER_ID IS NOT NULL
                                      AND FLIGHTDATE>=:fromDate AND FLIGHTDATE<:toDate
                                   UNION
                                   SELECT CASE WHEN UPPER(TRIM(""OPER""))='VNA' THEN 'HVN'
                                               ELSE UPPER(TRIM(""OPER"")) END OPER_ID
                                     FROM T_KHH
                                    WHERE ""OPER"" IS NOT NULL
                                 ) ORDER BY OPER_ID";
            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(sql, connection))
            {
                command.BindByName = true;
                command.Parameters.Add("fromDate", OracleDbType.Date).Value = from;
                command.Parameters.Add("toDate", OracleDbType.Date).Value = to.AddDays(1);
                connection.Open();
                using (var reader = command.ExecuteReader()) while (reader.Read()) values.Add(reader.GetString(0));
            }
            return values;
        }

        private static string BuildFlightTimestampSql(string valueExpression)
        {
            string value = "TRIM(" + valueExpression + ")";
            return @"CASE
                       WHEN REGEXP_LIKE(" + value + @", '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                         CASE
                           WHEN SUBSTR(" + value + @",1,2)=TO_CHAR(FLIGHTDATE,'DD') THEN TRUNC(FLIGHTDATE)
                           WHEN SUBSTR(" + value + @",1,2)=TO_CHAR(FLIGHTDATE+1,'DD') THEN TRUNC(FLIGHTDATE)+1
                           WHEN SUBSTR(" + value + @",1,2)=TO_CHAR(FLIGHTDATE-1,'DD') THEN TRUNC(FLIGHTDATE)-1
                         END
                         + TO_NUMBER(SUBSTR(" + value + @",3,2))/24
                         + TO_NUMBER(SUBSTR(" + value + @",5,2))/1440
                       WHEN REGEXP_LIKE(" + value + @", '^([01][0-9]|2[0-3])[0-5][0-9]$') THEN
                         TRUNC(FLIGHTDATE)
                         + TO_NUMBER(SUBSTR(" + value + @",1,2))/24
                         + TO_NUMBER(SUBSTR(" + value + @",3,2))/1440
                     END";
        }

        private static string BuildEobtFallbackSql(string eobtDateExpression, string eobtExpression, string fallbackExpression)
        {
            return @"CASE
                       WHEN REGEXP_LIKE(TRIM(" + eobtDateExpression + @"), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$')
                         THEN TRIM(" + eobtDateExpression + @")
                       WHEN REGEXP_LIKE(TRIM(" + eobtExpression + @"), '^((0[1-9]|[12][0-9]|3[01]))?([01][0-9]|2[0-3])[0-5][0-9]$')
                         THEN TRIM(" + eobtExpression + @")
                       ELSE " + fallbackExpression + @"
                     END";
        }

        private static string BuildHistoricalFplCtes()
        {
            return @"historical_fpl_source AS (
                     SELECT 1 source_priority, FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION,
                            FROM_AIRP, TO_AIRP, EOBTDATE, EOBT
                       FROM T_DAY_FLIGHTS_GOINGON
                      WHERE FLIGHTDATE>=TRUNC(:fromDate) AND FLIGHTDATE<TRUNC(:toDate)
                     UNION ALL
                     SELECT 2 source_priority, FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION,
                            FROM_AIRP, TO_AIRP, EOBTDATE, EOBT
                       FROM T_DAY_FLIGHTS_GOINGON032024
                      WHERE FLIGHTDATE>=TRUNC(:fromDate) AND FLIGHTDATE<TRUNC(:toDate)
                     UNION ALL
                     SELECT 3 source_priority, FLIGHT_ID, FLIGHTDATE, FLIGHTNBR, REGISTRATION,
                            FROM_AIRP, TO_AIRP, EOBTDATE, EOBT
                       FROM T_T_DAY_FLIGHTS_GOINGON
                      WHERE FLIGHTDATE>=TRUNC(:fromDate) AND FLIGHTDATE<TRUNC(:toDate)
                   ), historical_fpl_candidates AS (
                     SELECT ROWIDTOCHAR(f_match.ROWID) finished_rowid,
                            d.EOBTDATE,
                            d.EOBT,
                            d.source_priority,
                            d.FLIGHT_ID source_flight_id,
                            CASE
                              WHEN UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f_match.FLIGHTNBR))
                               AND NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f_match.FROM_AIRP)),'#')
                               AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f_match.TO_AIRP)),'#') THEN 0
                              WHEN UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f_match.FLIGHTNBR)) THEN 1
                              WHEN NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f_match.FROM_AIRP)),'#')
                               AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f_match.TO_AIRP)),'#') THEN 2
                              ELSE 3
                            END match_rank
                       FROM T_FINISHED_FLIGHTS f_match
                       JOIN historical_fpl_source d
                         ON d.FLIGHTDATE>=TRUNC(f_match.FLIGHTDATE)
                        AND d.FLIGHTDATE<TRUNC(f_match.FLIGHTDATE)+1
                        AND UPPER(TRIM(d.REGISTRATION))=UPPER(TRIM(f_match.REGISTRATION))
                      WHERE f_match.PERMTYPE='LD'
                        AND f_match.OPER_ID IS NOT NULL
                        AND f_match.REGISTRATION IS NOT NULL
                        AND f_match.FLIGHTDATE>=:fromDate AND f_match.FLIGHTDATE<:toDate
                        AND (:oper IS NULL OR UPPER(TRIM(f_match.OPER_ID))=:oper)
                        AND (:airport IS NULL OR UPPER(TRIM(f_match.FROM_AIRP))=:airport OR UPPER(TRIM(f_match.TO_AIRP))=:airport)
                        AND COALESCE(TRIM(d.EOBTDATE),TRIM(d.EOBT)) IS NOT NULL
                        AND (
                             d.FLIGHT_ID=f_match.FLIGHT_ID
                             OR UPPER(TRIM(d.FLIGHTNBR))=UPPER(TRIM(f_match.FLIGHTNBR))
                             OR (
                                  NVL(UPPER(TRIM(d.FROM_AIRP)),'#')=NVL(UPPER(TRIM(f_match.FROM_AIRP)),'#')
                                  AND NVL(UPPER(TRIM(d.TO_AIRP)),'#')=NVL(UPPER(TRIM(f_match.TO_AIRP)),'#')
                                )
                            )
                   ), historical_fpl AS (
                     SELECT c.*,
                            ROW_NUMBER() OVER (
                              PARTITION BY c.finished_rowid
                              ORDER BY c.match_rank,
                                       CASE WHEN REGEXP_LIKE(TRIM(c.EOBTDATE), '^(0[1-9]|[12][0-9]|3[01])([01][0-9]|2[0-3])[0-5][0-9]$') THEN 0 ELSE 1 END,
                                       c.source_priority,
                                       c.source_flight_id
                            ) match_order
                       FROM historical_fpl_candidates c
                   ), ";
        }

        internal static string BuildHistoricalStatusSql()
        {
            string fromClause = @"T_FINISHED_FLIGHTS f
                    LEFT JOIN historical_fpl day_fpl
                      ON day_fpl.finished_rowid=ROWIDTOCHAR(f.ROWID)
                     AND day_fpl.match_order=1";
            string plannedTime = BuildEobtFallbackSql("day_fpl.EOBTDATE", "day_fpl.EOBT", "NULL");
            return BuildSql(BuildHistoricalFplCtes(), fromClause, plannedTime, false);
        }

        private static string BuildSql(string auxiliaryCtes, string fromClause, string plannedTime, bool currentDay)
        {
            string plannedTimestamp = BuildFlightTimestampSql("planned_raw");
            string actualDepartureTimestamp = BuildFlightTimestampSql("actual_departure_raw");
            string isCurrentDay = currentDay ? "1" : "0";

            return @"WITH " + auxiliaryCtes + @"source_rows AS (
                    SELECT f.*,
                           " + plannedTime + @" planned_raw,
                           NULLIF(TRIM(f.ATD), '') actual_departure_raw,
                           CASE WHEN (NULLIF(TRIM(f.FROM_AIRP), '') IS NULL OR UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%')
                                      AND (NULLIF(TRIM(f.TO_AIRP), '') IS NULL OR UPPER(TRIM(f.TO_AIRP)) LIKE 'VV%')
                                THEN 1 ELSE 0 END domestic,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL THEN 1 ELSE 0 END has_from,
                           CASE WHEN NULLIF(TRIM(f.TO_AIRP), '') IS NOT NULL THEN 1 ELSE 0 END has_to,
                           CASE WHEN NULLIF(TRIM(f.ATD), '') IS NOT NULL THEN 1 ELSE 0 END has_atd,
                           CASE WHEN NULLIF(TRIM(f.ATA), '') IS NOT NULL THEN 1 ELSE 0 END has_ata,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) LIKE 'VV%' THEN 1 ELSE 0 END departing_vietnam,
                           CASE WHEN NULLIF(TRIM(f.FROM_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.FROM_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_from,
                           CASE WHEN NULLIF(TRIM(f.TO_AIRP), '') IS NOT NULL AND UPPER(TRIM(f.TO_AIRP)) NOT LIKE 'VV%' THEN 1 ELSE 0 END foreign_to
                      FROM " + fromClause + @"
                     WHERE f.PERMTYPE='LD' AND f.OPER_ID IS NOT NULL
                       AND f.FLIGHTDATE>=:fromDate AND f.FLIGHTDATE<:toDate
                       AND (:oper IS NULL OR UPPER(TRIM(f.OPER_ID))=:oper)
                       AND (:airport IS NULL OR UPPER(TRIM(f.FROM_AIRP))=:airport OR UPPER(TRIM(f.TO_AIRP))=:airport)
                  ), parsed_rows AS (
                    SELECT s.*,
                           " + plannedTimestamp + @" planned_timestamp,
                           " + actualDepartureTimestamp + @" actual_departure_timestamp
                      FROM source_rows s
                  ), metric_rows AS (
                    SELECT p.*,
                           CASE
                             WHEN planned_timestamp IS NOT NULL
                              AND actual_departure_timestamp IS NOT NULL
                              AND actual_departure_timestamp>=planned_timestamp-(6/24)
                              AND actual_departure_timestamp<=planned_timestamp+1
                             THEN ROUND((actual_departure_timestamp-planned_timestamp)*1440)
                           END delay_minutes
                      FROM parsed_rows p
                  )
                SELECT FLIGHTDATE, FLIGHTNBR, OPER_ID, REGISTRATION, PERMTYPE, FROM_AIRP, TO_AIRP,
                       NULLIF(TRIM(ATD), '') ATDDAY,
                       NULLIF(TRIM(ATA), '') ATADAY,
                       planned_raw EOBTDAY,
                       CASE
                         WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=60 THEN 'DELAY_60_PLUS'
                         WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=30 THEN 'DELAY_30_59'
                         WHEN departing_vietnam=1 AND has_to=1 AND has_atd=1 AND delay_minutes>=15 THEN 'DELAY_15_29'
                         WHEN (domestic=1 AND has_from=1 AND has_to=1 AND has_atd=1 AND has_ata=1)
                           OR (domestic=0 AND (
                                (foreign_from=1 AND has_to=1 AND has_ata=1)
                                OR (foreign_to=1 AND has_from=1 AND has_atd=1)
                              )) THEN 'FINISHED'
                         WHEN domestic=0 AND (
                                (foreign_to=1 AND (has_from=0 OR has_atd=0))
                                OR (foreign_from=1 AND (has_to=0 OR has_ata=0))
                              ) THEN 'CANCEL'
                         WHEN " + isCurrentDay + @"=1 AND domestic=1 AND departing_vietnam=1
                              AND has_to=1 AND planned_timestamp IS NOT NULL AND has_atd=0 THEN 'WAIT'
                         WHEN domestic=1 AND (has_from=0 OR has_to=0 OR has_atd=0 OR has_ata=0) THEN 'CANCEL'
                         ELSE 'CANCEL'
                       END FLIGHT_STATE
                  FROM metric_rows
                 ORDER BY FLIGHTDATE, FLIGHTNBR";
        }
    }
}
