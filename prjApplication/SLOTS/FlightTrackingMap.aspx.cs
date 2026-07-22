using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Services;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.SLOTS
{
    public partial class FlightTrackingMap : prjApplication.ReportNew.ReportPageBase
    {
        private sealed class TrackRow
        {
            public string FlightId { get; set; }
            public string Callsign { get; set; }
            public DateTime UpdatedAt { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Heading { get; set; }
        }

        private sealed class FlightMeta
        {
            public readonly HashSet<string> Operators = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public readonly HashSet<string> PermTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        [WebMethod]
        public static object GetFlights()
        {
            DateTime today = DateTime.Today;
            List<TrackRow> tracks = LoadTodayTracks(today);
            Dictionary<string, FlightMeta> metadata = LoadFlightMetadata(today, tracks.Select(x => x.Callsign));

            var flights = tracks.Select(track =>
            {
                FlightMeta meta;
                metadata.TryGetValue(track.Callsign, out meta);
                string oper = meta == null || meta.Operators.Count == 0
                    ? String.Empty
                    : String.Join(" / ", meta.Operators.OrderBy(x => x));
                string permType = meta == null || meta.PermTypes.Count == 0
                    ? String.Empty
                    : (meta.PermTypes.Count == 1 ? meta.PermTypes.First().ToUpperInvariant() : "MULTI");
                return new
                {
                    flightIdCurrent = track.FlightId,
                    callsign = track.Callsign,
                    operId = oper,
                    permType = permType,
                    latitude = track.Latitude,
                    longitude = track.Longitude,
                    heading = track.Heading,
                    updatedAt = track.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                };
            }).ToList();

            return new
            {
                day = today.ToString("dd/MM/yyyy"),
                serverTime = DateTime.Now.ToString("HH:mm:ss"),
                refreshSeconds = 5,
                flights = flights
            };
        }

        [WebMethod]
        public static object GetFirGeoJson()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/vietnam-fir-VVHM-VVHN.geojson");
            string baseMapPath = HostingEnvironment.MapPath("~/App_Data/southeast-asia-basemap.geojson");
            if (String.IsNullOrWhiteSpace(path) || !File.Exists(path))
                throw new FileNotFoundException("Không tìm thấy dữ liệu biên FIR trong App_Data.");
            if (String.IsNullOrWhiteSpace(baseMapPath) || !File.Exists(baseMapPath))
                throw new FileNotFoundException("Không tìm thấy dữ liệu bản đồ nền offline trong App_Data.");
            return new
            {
                geoJson = File.ReadAllText(path),
                baseGeoJson = File.ReadAllText(baseMapPath),
                source = "Natural Earth 1:110m + vietnam-fir-VVHM-VVHN.geojson"
            };
        }

        private static List<TrackRow> LoadTodayTracks(DateTime today)
        {
            const string sql = @"
                SELECT DISTINCT ON (normalized_callsign)
                       flight_id_current,
                       normalized_callsign,
                       updated_at_utc::timestamptz,
                       last_lat,
                       last_lon,
                       COALESCE(last_track_deg, 0)
                FROM (
                    SELECT flight_id_current,
                           UPPER(TRIM(SPLIT_PART(COALESCE(flight_id_current, ''), '-', 1))) normalized_callsign,
                           updated_at_utc,
                           last_lat,
                           last_lon,
                           last_track_deg
                    FROM public.tracks
                    WHERE flight_id_current IS NOT NULL
                      AND updated_at_utc IS NOT NULL
                      AND last_lat IS NOT NULL
                      AND last_lon IS NOT NULL
                      AND (updated_at_utc::timestamptz AT TIME ZONE 'Asia/Ho_Chi_Minh') >= @fromDate
                      AND (updated_at_utc::timestamptz AT TIME ZONE 'Asia/Ho_Chi_Minh') < @toDate
                ) source
                WHERE normalized_callsign <> ''
                ORDER BY normalized_callsign, updated_at_utc::timestamptz DESC";

            var rows = new List<TrackRow>();
            string connectionString = ConfigurationManager.ConnectionStrings["TracksPostgres"].ConnectionString;
            using (var connection = new NpgsqlConnection(connectionString))
            using (var command = new NpgsqlCommand(sql, connection))
            {
                command.CommandTimeout = 30;
                command.Parameters.AddWithValue("fromDate", today);
                command.Parameters.AddWithValue("toDate", today.AddDays(1));
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        rows.Add(new TrackRow
                        {
                            FlightId = Convert.ToString(reader[0]),
                            Callsign = Convert.ToString(reader[1]),
                            UpdatedAt = Convert.ToDateTime(reader[2], CultureInfo.InvariantCulture),
                            Latitude = Convert.ToDouble(reader[3], CultureInfo.InvariantCulture),
                            Longitude = Convert.ToDouble(reader[4], CultureInfo.InvariantCulture),
                            Heading = reader.IsDBNull(5) ? 0 : Convert.ToDouble(reader[5], CultureInfo.InvariantCulture)
                        });
                    }
                }
            }
            return rows;
        }

        private static Dictionary<string, FlightMeta> LoadFlightMetadata(DateTime today, IEnumerable<string> values)
        {
            string[] callsigns = values.Where(x => !String.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var result = new Dictionary<string, FlightMeta>(StringComparer.OrdinalIgnoreCase);
            if (callsigns.Length == 0) return result;

            using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings["SlotsOracle"].ConnectionString))
            {
                connection.Open();
                for (int offset = 0; offset < callsigns.Length; offset += 800)
                {
                    string[] block = callsigns.Skip(offset).Take(800).ToArray();
                    var parameterNames = new List<string>();
                    using (var command = connection.CreateCommand())
                    {
                        command.BindByName = true;
                        command.CommandTimeout = 60;
                        command.Parameters.Add("flightDate", OracleDbType.Date).Value = today;
                        for (int index = 0; index < block.Length; index++)
                        {
                            string name = "call" + index;
                            parameterNames.Add(":" + name);
                            command.Parameters.Add(name, OracleDbType.Varchar2).Value = block[index];
                        }
                        command.CommandText = @"
                            SELECT UPPER(TRIM(FLIGHTNBR)) FLIGHTNBR,
                                   UPPER(TRIM(OPER_ID)) OPER_ID,
                                   UPPER(TRIM(PERMTYPE)) PERMTYPE
                            FROM T_DAY_FLIGHTS_GOINGON
                            WHERE TRUNC(FLIGHTDATE) = :flightDate
                              AND UPPER(TRIM(FLIGHTNBR)) IN (" + String.Join(",", parameterNames) + ")";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string callsign = Convert.ToString(reader["FLIGHTNBR"]);
                                FlightMeta meta;
                                if (!result.TryGetValue(callsign, out meta))
                                {
                                    meta = new FlightMeta();
                                    result[callsign] = meta;
                                }
                                string oper = Convert.ToString(reader["OPER_ID"]).Trim();
                                string permType = Convert.ToString(reader["PERMTYPE"]).Trim();
                                if (oper.Length > 0) meta.Operators.Add(oper);
                                if (permType.Length > 0) meta.PermTypes.Add(permType);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
