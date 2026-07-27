using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.ReportNew
{
    public partial class AnomalyWarning : ReportPageBase
    {
        private sealed class DelayAlert
        {
            public int Level { get; set; }
            public string FlightNumber { get; set; }
            public string Operator { get; set; }
            public string Registration { get; set; }
            public string PermitType { get; set; }
            public string FromAirport { get; set; }
            public string ToAirport { get; set; }
            public string Etd { get; set; }
            public string Atd { get; set; }
            public long DelayMinutes { get; set; }
        }

        [WebMethod]
        public static object GetDelayAlerts()
        {
            const string sql = @"
                SELECT TRUNC(SYSDATE) REPORT_DAY,
                       f.FLIGHTDATE,
                       f.FLIGHTNBR,
                       f.OPER_ID,
                       f.REGISTRATION,
                       f.PERMTYPE,
                       f.FROM_AIRP,
                       f.TO_AIRP,
                       TRIM(f.ETD) ETD,
                       TRIM(f.ATD) ATD
                  FROM T_DAY_FLIGHTS_GOINGON f
                 WHERE f.FLIGHTDATE >= TRUNC(SYSDATE)
                   AND f.FLIGHTDATE < TRUNC(SYSDATE) + 1
                   AND f.ETD IS NOT NULL
                   AND f.ATD IS NOT NULL
                 ORDER BY f.FLIGHTDATE, f.FLIGHTNBR";

            DateTime reportDay = DateTime.Today;
            var alerts = new List<DelayAlert>();
            using (var connection = new OracleConnection(
                ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            using (var command = new OracleCommand(sql, connection))
            {
                command.CommandTimeout = 120;
                connection.Open();
                using (OracleDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["REPORT_DAY"] != DBNull.Value)
                            reportDay = Convert.ToDateTime(reader["REPORT_DAY"]).Date;

                        DateTime flightDate;
                        DateTime etdTimestamp;
                        DateTime atdTimestamp;
                        string etd = NormalizeFlightTime(reader["ETD"], 4);
                        string atd = NormalizeFlightTime(reader["ATD"], 6);
                        if (!TryGetFlightDate(reader["FLIGHTDATE"], out flightDate) ||
                            !TryParseEtd(etd, flightDate, out etdTimestamp) ||
                            !TryParseAtd(atd, flightDate, out atdTimestamp))
                            continue;

                        long delayMinutes = Convert.ToInt64(
                            (atdTimestamp - etdTimestamp).TotalMinutes);
                        int level = GetDelayAlertLevel(delayMinutes);
                        if (level == 0)
                            continue;

                        alerts.Add(new DelayAlert {
                            Level = level,
                            FlightNumber = Convert.ToString(reader["FLIGHTNBR"]).Trim(),
                            Operator = Convert.ToString(reader["OPER_ID"]).Trim(),
                            Registration = Convert.ToString(reader["REGISTRATION"]).Trim(),
                            PermitType = Convert.ToString(reader["PERMTYPE"]).Trim(),
                            FromAirport = Convert.ToString(reader["FROM_AIRP"]).Trim(),
                            ToAirport = Convert.ToString(reader["TO_AIRP"]).Trim(),
                            Etd = etd,
                            Atd = atd,
                            DelayMinutes = delayMinutes
                        });
                    }
                }
            }

            List<DelayAlert> sorted = alerts
                .OrderByDescending(item => item.Level)
                .ThenByDescending(item => item.DelayMinutes)
                .ThenBy(item => item.FlightNumber)
                .ToList();

            return new {
                reportDay = reportDay.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                source = "T_DAY_FLIGHTS_GOINGON",
                total = sorted.Count,
                level1 = sorted.Count(item => item.Level == 1),
                level2 = sorted.Count(item => item.Level == 2),
                level3 = sorted.Count(item => item.Level == 3),
                alerts = sorted.Select((item, index) => new {
                    no = index + 1,
                    level = item.Level,
                    levelName = "Mức " + item.Level,
                    flightNumber = item.FlightNumber,
                    oper = item.Operator,
                    registration = item.Registration,
                    permitType = item.PermitType,
                    fromAirport = item.FromAirport,
                    toAirport = item.ToAirport,
                    etd = item.Etd,
                    atd = item.Atd,
                    delayMinutes = item.DelayMinutes,
                    delayDuration = FormatDelayDuration(item.DelayMinutes)
                }).ToList()
            };
        }

        private static string NormalizeFlightTime(object value, int requiredLength)
        {
            string text = Convert.ToString(value).Trim();
            if (text.Length != requiredLength || text.Any(character => !Char.IsDigit(character)))
                return String.Empty;
            return text;
        }

        private static bool TryGetFlightDate(object value, out DateTime flightDate)
        {
            if (value != null && value != DBNull.Value && value is DateTime)
            {
                flightDate = ((DateTime)value).Date;
                return true;
            }

            string text = Convert.ToString(value).Trim();
            return DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out flightDate)
                || DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.None, out flightDate);
        }

        private static bool TryParseEtd(string value, DateTime flightDate, out DateTime timestamp)
        {
            DateTime parsedTime;
            if (!DateTime.TryParseExact(
                value,
                "HHmm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedTime))
            {
                timestamp = DateTime.MinValue;
                return false;
            }

            timestamp = flightDate.Date.Add(parsedTime.TimeOfDay);
            return true;
        }

        private static bool TryParseAtd(string value, DateTime flightDate, out DateTime timestamp)
        {
            int day;
            int hour;
            int minute;
            timestamp = DateTime.MinValue;
            if (value.Length != 6 ||
                !Int32.TryParse(value.Substring(0, 2), out day) ||
                !Int32.TryParse(value.Substring(2, 2), out hour) ||
                !Int32.TryParse(value.Substring(4, 2), out minute) ||
                hour < 0 || hour > 23 ||
                minute < 0 || minute > 59)
                return false;

            DateTime flightDay = flightDate.Date;
            DateTime atdDay;
            if (flightDay.Day == day)
                atdDay = flightDay;
            else if (flightDay.AddDays(1).Day == day)
                atdDay = flightDay.AddDays(1);
            else if (flightDay.AddDays(-1).Day == day)
                atdDay = flightDay.AddDays(-1);
            else
                return false;

            timestamp = atdDay.AddHours(hour).AddMinutes(minute);
            return true;
        }

        private static int GetDelayAlertLevel(long delayMinutes)
        {
            if (delayMinutes >= 60)
                return 3;
            if (delayMinutes >= 30)
                return 2;
            if (delayMinutes >= 15)
                return 1;
            return 0;
        }

        private static string FormatDelayDuration(long delayMinutes)
        {
            TimeSpan delay = TimeSpan.FromMinutes(delayMinutes);
            int totalHours = Convert.ToInt32(Math.Floor(delay.TotalHours));
            return String.Format(
                CultureInfo.InvariantCulture,
                "{0:00}:{1:00}",
                totalHours,
                delay.Minutes);
        }
    }
}
