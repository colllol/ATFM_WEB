using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Services;
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
            public string OperId { get; set; }
            public string PermType { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string Eta { get; set; }
            public bool Skip { get; set; }
        }

        private sealed class FlightCandidate
        {
            public string Callsign { get; set; }
            public string OperId { get; set; }
            public string PermType { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string Eta { get; set; }
        }

        private sealed class TrackApiResponse
        {
            public List<TrackApiItem> flights { get; set; }
        }

        private sealed class TrackApiItem
        {
            public string flightIdCurrent { get; set; }
            public string callsign { get; set; }
            public string updatedAtUtc { get; set; }
            public double latitude { get; set; }
            public double longitude { get; set; }
            public double heading { get; set; }
        }

        [WebMethod]
        public static object GetFlights()
        {
            DateTime today = DateTime.UtcNow.Date;
            List<TrackRow> tracks = LoadTodayTracks(today);
            Dictionary<string, FlightMeta> metadata = LoadFlightMetadata(today, tracks);

            var flights = tracks.Where(track =>
            {
                FlightMeta meta;
                return !metadata.TryGetValue(track.Callsign, out meta) || !meta.Skip;
            }).Select(track =>
            {
                FlightMeta meta;
                metadata.TryGetValue(track.Callsign, out meta);
                if (meta == null) meta = UnknownFlight();
                return new
                {
                    flightIdCurrent = track.FlightId,
                    callsign = track.Callsign,
                    operId = meta.OperId,
                    permType = meta.PermType,
                    fromAirp = meta.FromAirp,
                    toAirp = meta.ToAirp,
                    etd = meta.Etd,
                    eta = meta.Eta,
                    isOther = String.Equals(meta.PermType, "OTHER", StringComparison.OrdinalIgnoreCase),
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
            string baseUrl = RequireAppSetting("FlightTrackingApiBaseUrl").TrimEnd('/');
            string endpoint = baseUrl + "/api/v1/tracks?date=" + today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            int timeoutSeconds;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["FlightTrackingApiTimeoutSeconds"], out timeoutSeconds)
                || timeoutSeconds < 1 || timeoutSeconds > 120)
            {
                timeoutSeconds = 10;
            }

            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "GET";
            request.Accept = "application/json";
            request.Timeout = timeoutSeconds * 1000;
            request.ReadWriteTimeout = timeoutSeconds * 1000;

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception)
            {
                response = exception.Response as HttpWebResponse;
                if (response == null)
                {
                    throw new InvalidOperationException("Không thể kết nối Flight Tracking API tại " + baseUrl + ".", exception);
                }
            }

            using (response)
            using (Stream responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream ?? Stream.Null))
            {
                string content = reader.ReadToEnd();
                if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
                {
                    throw new InvalidOperationException(
                        "Flight Tracking API trả về HTTP " + (int)response.StatusCode + ". " + LimitText(content, 300));
                }

                TrackApiResponse payload = new JavaScriptSerializer().Deserialize<TrackApiResponse>(content);
                if (payload == null || payload.flights == null) return new List<TrackRow>();

                var rows = new List<TrackRow>();
                foreach (TrackApiItem item in payload.flights)
                {
                    DateTime updatedAt;
                    if (item == null
                        || String.IsNullOrWhiteSpace(item.callsign)
                        || !DateTime.TryParseExact(
                            item.updatedAtUtc,
                            "yyyy-MM-dd'T'HH:mm:ss",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out updatedAt))
                    {
                        continue;
                    }

                    rows.Add(new TrackRow
                    {
                        FlightId = item.flightIdCurrent ?? String.Empty,
                        Callsign = item.callsign,
                        UpdatedAt = updatedAt,
                        Latitude = item.latitude,
                        Longitude = item.longitude,
                        Heading = item.heading
                    });
                }
                return rows;
            }
        }

        private static string RequireAppSetting(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrWhiteSpace(value))
                throw new ConfigurationErrorsException("Thiếu appSettings/" + key + " trong Web.config.");
            return value.Trim();
        }

        private static string LimitText(string value, int maximumLength)
        {
            string normalized = (value ?? String.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            return normalized.Length <= maximumLength ? normalized : normalized.Substring(0, maximumLength) + "...";
        }

        private static Dictionary<string, FlightMeta> LoadFlightMetadata(DateTime today, IEnumerable<TrackRow> values)
        {
            TrackRow[] tracks = values.Where(x => !String.IsNullOrWhiteSpace(x.Callsign)).ToArray();
            string[] callsigns = tracks.Select(x => x.Callsign).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var result = new Dictionary<string, FlightMeta>(StringComparer.OrdinalIgnoreCase);
            if (callsigns.Length == 0) return result;
            var candidates = new Dictionary<string, List<FlightCandidate>>(StringComparer.OrdinalIgnoreCase);

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
                                   UPPER(TRIM(PERMTYPE)) PERMTYPE,
                                   TRIM(FROM_AIRP) FROM_AIRP,
                                   TRIM(TO_AIRP) TO_AIRP,
                                   TRIM(ETD) ETD,
                                   TRIM(ETA) ETA
                            FROM T_DAY_FLIGHTS_GOINGON
                            WHERE TRUNC(FLIGHTDATE) = :flightDate
                              AND UPPER(TRIM(FLIGHTNBR)) IN (" + String.Join(",", parameterNames) + ")";
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string callsign = Convert.ToString(reader["FLIGHTNBR"]);
                                List<FlightCandidate> matches;
                                if (!candidates.TryGetValue(callsign, out matches))
                                {
                                    matches = new List<FlightCandidate>();
                                    candidates[callsign] = matches;
                                }
                                matches.Add(new FlightCandidate
                                {
                                    Callsign = callsign,
                                    OperId = Convert.ToString(reader["OPER_ID"]).Trim(),
                                    PermType = Convert.ToString(reader["PERMTYPE"]).Trim(),
                                    FromAirp = Convert.ToString(reader["FROM_AIRP"]).Trim(),
                                    ToAirp = Convert.ToString(reader["TO_AIRP"]).Trim(),
                                    Etd = Convert.ToString(reader["ETD"]).Trim(),
                                    Eta = Convert.ToString(reader["ETA"]).Trim()
                                });
                            }
                        }
                    }
                }
            }
            foreach (TrackRow track in tracks)
            {
                List<FlightCandidate> matches;
                candidates.TryGetValue(track.Callsign, out matches);
                result[track.Callsign] = ResolveFlightMetadata(track, matches ?? new List<FlightCandidate>());
            }
            return result;
        }

        private static FlightMeta ResolveFlightMetadata(TrackRow track, IList<FlightCandidate> candidates)
        {
            if (candidates.Count == 0) return UnknownFlight();
            FlightCandidate selected;
            if (candidates.Count == 1)
            {
                selected = candidates[0];
            }
            else
            {
                List<FlightCandidate> matching = candidates.Where(x => FlightTimeContains(track.UpdatedAt, x.Etd, x.Eta)).ToList();
                if (matching.Count != 1) return new FlightMeta { Skip = true, PermType = "OTHER" };
                selected = matching[0];
            }

            bool hasCompleteRoute = !String.IsNullOrWhiteSpace(selected.FromAirp) && !String.IsNullOrWhiteSpace(selected.ToAirp);
            return new FlightMeta
            {
                OperId = hasCompleteRoute ? selected.OperId : String.Empty,
                PermType = hasCompleteRoute && !String.IsNullOrWhiteSpace(selected.PermType) ? selected.PermType : "OTHER",
                FromAirp = selected.FromAirp,
                ToAirp = selected.ToAirp,
                Etd = selected.Etd,
                Eta = selected.Eta
            };
        }

        private static FlightMeta UnknownFlight()
        {
            return new FlightMeta
            {
                OperId = String.Empty,
                PermType = "OTHER",
                FromAirp = String.Empty,
                ToAirp = String.Empty,
                Etd = String.Empty,
                Eta = String.Empty
            };
        }

        private static bool FlightTimeContains(DateTime updatedAt, string etdValue, string etaValue)
        {
            int? etd = FlightTimeMinutes(etdValue);
            int? eta = FlightTimeMinutes(etaValue);
            if (!etd.HasValue || !eta.HasValue) return false;
            int current = updatedAt.Hour * 60 + updatedAt.Minute;
            return etd.Value <= eta.Value
                ? etd.Value <= current && current <= eta.Value
                : current >= etd.Value || current <= eta.Value;
        }

        private static int? FlightTimeMinutes(string value)
        {
            string text = (value ?? String.Empty).ToUpperInvariant().Replace("UTC", String.Empty).Trim().TrimEnd('+');
            Match match = Regex.Match(text, @"^(\d{1,2}):?(\d{2})(?::\d{2})?$");
            if (!match.Success) return null;
            int hour;
            int minute;
            if (!Int32.TryParse(match.Groups[1].Value, out hour) || !Int32.TryParse(match.Groups[2].Value, out minute)) return null;
            if (hour > 23 || minute > 59) return null;
            return hour * 60 + minute;
        }
    }
}
