using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.SLOTS
{
    public sealed class SlotComparisonRow
    {
        public long Id { get; set; }
        public string ResultType { get; set; }
        public DateTime FlightDate { get; set; }
        public string Oper { get; set; }
        public string Callsign { get; set; }
        public string FromAirp { get; set; }
        public string ToAirp { get; set; }
        public string Etd { get; set; }
        public string Remark { get; set; }
        public string SourceRef { get; set; }
    }

    public sealed class SlotComparisonService
    {
        private static readonly string[] ResultTypes = { "KQ1", "KQ2", "KQ3", "KQ4" };
        private readonly SlotsDataProvider provider;

        public SlotComparisonService()
        {
            provider = new SlotsDataProvider();
        }

        public SlotComparisonService(string connectionString)
        {
            provider = new SlotsDataProvider(connectionString);
        }

        public DateTime GetDefaultDate()
        {
            DataTable data = CallPackage("GET_DEFAULT_DATE", new { });
            object value = data.Rows.Count > 0 ? data.Rows[0]["COMPARE_DATE"] : null;
            return value == null || value == DBNull.Value ? DateTime.Today : Convert.ToDateTime(value, CultureInfo.InvariantCulture);
        }

        public IList<string> GetOperators()
        {
            DataTable table = CallPackage("GET_OPERATORS", new { });
            return table.AsEnumerable().Select(row => row["OPER"].ToString()).ToList();
        }

        public IDictionary<string, int> GetSummary(DateTime date, string oper)
        {
            Dictionary<string, int> summary = ResultTypes.ToDictionary(type => type, type => 0);
            DataTable table = CallPackage("GET_SUMMARY", new
            {
                P_COMPARE_DATE = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                P_OPER = IsAllOperators(oper) ? null : NormalizeText(oper)
            });
            foreach (DataRow row in table.Rows) summary[row["RESULT_TYPE"].ToString()] = Convert.ToInt32(row["TOTAL"], CultureInfo.InvariantCulture);
            return summary;
        }

        public int CompareAndSave(DateTime date, string oper, string resultType, string userName)
        {
            resultType = ValidateResultType(resultType);
            string normalizedOper = IsAllOperators(oper) ? "ALL" : NormalizeText(oper);
            SourceData sources = LoadSources(date.Date, normalizedOper);
            Dictionary<string, SourceFlight> slot = ToDictionary(sources.Slot);
            Dictionary<string, SourceFlight> permission = ToDictionary(sources.Permission);
            Dictionary<string, SourceFlight> airlinePlan = ToDictionary(sources.AirlinePlan);
            List<SlotComparisonRow> results = new List<SlotComparisonRow>();

            if (resultType == "KQ1")
            {
                results.AddRange(slot.Where(item => !permission.ContainsKey(item.Key))
                    .Select(item => CreateResult(date, resultType, item.Value, "Có SLOT nhưng không có PHÉP BAY")));
            }
            else if (resultType == "KQ2")
            {
                results.AddRange(permission.Where(item => !slot.ContainsKey(item.Key))
                    .Select(item => CreateResult(date, resultType, item.Value, "Có PHÉP BAY nhưng không có SLOT")));
            }
            else if (resultType == "KQ3")
            {
                results.AddRange(airlinePlan.Where(item => !(slot.ContainsKey(item.Key) && permission.ContainsKey(item.Key)))
                    .Select(item => CreateResult(date, resultType, item.Value, "KHB Hãng: Không có cả trong SLOT và Phép bay")));
            }
            else
            {
                results.AddRange(slot.Where(item => permission.ContainsKey(item.Key) && !airlinePlan.ContainsKey(item.Key))
                    .Select(item => CreateResult(date, resultType, item.Value, "Có cả PHÉP và SLOT nhưng không có KHB Hãng")));
            }

            results = results.OrderBy(row => row.Oper).ThenBy(row => row.Callsign)
                .ThenBy(row => row.FromAirp).ThenBy(row => row.ToAirp).ToList();
            SaveResults(date.Date, normalizedOper, resultType, userName, results);
            return results.Count;
        }

        public IList<SlotComparisonRow> GetResults(DateTime date, string oper, string resultType, int pageIndex, int pageSize, out int totalRecords)
        {
            resultType = ValidateResultType(resultType);
            pageIndex = Math.Max(1, pageIndex);
            pageSize = Math.Max(1, Math.Min(500000, pageSize));
            DataTable data = CallPackage("GET_RESULTS", new
            {
                P_COMPARE_DATE = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                P_RESULT_TYPE = resultType,
                P_OPER = IsAllOperators(oper) ? null : NormalizeText(oper),
                P_PAGE_SIZE = pageSize,
                P_PAGE_INDEX = pageIndex - 1
            });
            totalRecords = data.Rows.Count > 0 ? Convert.ToInt32(data.Rows[0]["SUMRECORD"], CultureInfo.InvariantCulture) : 0;
            return MapResults(data);
        }

        public IList<SlotComparisonRow> GetAllResults(DateTime date, string oper, string resultType)
        {
            int total;
            return GetResults(date, oper, resultType, 1, 500000, out total);
        }

        private SourceData LoadSources(DateTime date, string oper)
        {
            Dictionary<string, OperatorInfo> operatorMap = LoadOperatorMap();
            Dictionary<string, string> airportMap = LoadAirportMap();
            HashSet<string> allowedOperators = new HashSet<string>(GetOperators(), StringComparer.OrdinalIgnoreCase);
            string compareDate = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            DataTable khh = CallPackage("GET_SOURCE_KHH", new { P_COMPARE_DATE = compareDate });
            DataTable slot = CallPackage("GET_SOURCE_SLOT", new { P_COMPARE_DATE = compareDate });
            DataTable permission = CallPackage("GET_SOURCE_PERM", new { P_COMPARE_DATE = compareDate });
            return new SourceData
            {
                AirlinePlan = NormalizeRows(khh, operatorMap, airportMap, allowedOperators, oper, "KHH"),
                Slot = NormalizeRows(slot, operatorMap, airportMap, allowedOperators, oper, "SLOT"),
                Permission = NormalizeRows(permission, operatorMap, airportMap, allowedOperators, oper, "PERM")
            };
        }

        private Dictionary<string, OperatorInfo> LoadOperatorMap()
        {
            DataTable data = CallPackage("GET_OPERATOR_MAP", new { });
            Dictionary<string, OperatorInfo> map = new Dictionary<string, OperatorInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in data.Rows)
            {
                OperatorInfo info = new OperatorInfo { Icao = NormalizeText(row["OPER_ICAO"]), Iata = NormalizeText(row["OPER_IATA"]) };
                if (!string.IsNullOrEmpty(info.Icao)) map[info.Icao] = info;
                if (!string.IsNullOrEmpty(info.Iata) && !map.ContainsKey(info.Iata)) map[info.Iata] = info;
            }
            if (map.ContainsKey("HVN")) map["VNA"] = map["HVN"];
            return map;
        }

        private Dictionary<string, string> LoadAirportMap()
        {
            DataTable data = CallPackage("GET_AIRPORT_MAP", new { });
            Dictionary<string, string> map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow row in data.Rows)
            {
                string icao = NormalizeText(row["AE_CODE"]);
                string iata = NormalizeText(row["AE_IATA"]);
                if (!string.IsNullOrEmpty(icao)) map[icao] = icao;
                if (!string.IsNullOrEmpty(iata) && !map.ContainsKey(iata)) map[iata] = icao;
            }
            return map;
        }

        private static DataTable CallPackage(string storeName, object parameters)
        {
            DataTable data = new global::clsResuftAPI().GetTableApiExtension(
                "SLOT_COMPARE_PKG", storeName, parameters);
            if (data == null)
                throw new InvalidOperationException("Không lấy được dữ liệu " + storeName + " từ API.");
            return data;
        }

        private static List<SourceFlight> NormalizeRows(DataTable data, Dictionary<string, OperatorInfo> operators,
            Dictionary<string, string> airports, HashSet<string> allowedOperators, string selectedOper, string source)
        {
            List<SourceFlight> flights = new List<SourceFlight>();
            foreach (DataRow row in data.Rows)
            {
                string rawCallsign = NormalizeText(row["CALLSIGN"]);
                OperatorInfo info = FindOperator(NormalizeText(row["OPER_HINT"]), rawCallsign, operators);
                if (info == null || !allowedOperators.Contains(info.Icao) ||
                    (!IsAllOperators(selectedOper) && !info.Icao.Equals(selectedOper, StringComparison.OrdinalIgnoreCase))) continue;
                string callsign = NormalizeCallsign(rawCallsign, info);
                string from = NormalizeAirport(row["FROM_AIRP"], airports);
                string to = NormalizeAirport(row["TO_AIRP"], airports);
                if (string.IsNullOrEmpty(callsign) || string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to)) continue;
                flights.Add(new SourceFlight
                {
                    Oper = info.Icao,
                    Callsign = callsign,
                    FromAirp = from,
                    ToAirp = to,
                    Etd = NormalizeTime(row["ETD"]),
                    SourceRef = source + ":" + Convert.ToString(row["ID"], CultureInfo.InvariantCulture)
                });
            }
            return flights.GroupBy(flight => flight.Key, StringComparer.OrdinalIgnoreCase).Select(group => group.First()).ToList();
        }

        private static OperatorInfo FindOperator(string hint, string callsign, Dictionary<string, OperatorInfo> operators)
        {
            OperatorInfo info;
            if (!string.IsNullOrEmpty(hint) && operators.TryGetValue(hint, out info)) return info;
            foreach (KeyValuePair<string, OperatorInfo> item in operators.OrderByDescending(item => item.Key.Length))
                if (!string.IsNullOrEmpty(callsign) && callsign.StartsWith(item.Key, StringComparison.OrdinalIgnoreCase)) return item.Value;
            return null;
        }

        private static string NormalizeCallsign(string callsign, OperatorInfo info)
        {
            callsign = Regex.Replace(NormalizeText(callsign), "[^A-Z0-9]", string.Empty);
            string[] prefixes = info.Icao == "HVN"
                ? new[] { info.Icao, info.Iata, "VNA" }
                : new[] { info.Icao, info.Iata };
            string prefix = prefixes.Where(value => !string.IsNullOrEmpty(value) && callsign.StartsWith(value, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(value => value.Length).FirstOrDefault();
            string suffix = prefix == null ? callsign : callsign.Substring(prefix.Length);
            return string.IsNullOrWhiteSpace(suffix) ? string.Empty : info.Icao + suffix;
        }

        private static string NormalizeAirport(object value, Dictionary<string, string> airports)
        {
            string code = NormalizeText(value);
            string normalized;
            return airports.TryGetValue(code, out normalized) ? normalized : code;
        }

        private static string NormalizeTime(object value)
        {
            string time = NormalizeText(value).Replace(":", string.Empty);
            if (string.IsNullOrEmpty(time)) return string.Empty;
            return time.Length > 4 ? time.Substring(0, 4) : time.PadLeft(4, '0');
        }

        private void SaveResults(DateTime date, string oper, string resultType, string userName, IList<SlotComparisonRow> rows)
        {
            using (OracleConnection connection = new OracleConnection(provider.ConnectionString))
            {
                connection.Open();
                using (OracleTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.BindByName = true;
                            command.CommandText = "MERGE INTO T_SLOT_COMPARE_RUN r USING (SELECT :P_DATE COMPARE_DATE FROM DUAL) s " +
                                "ON (r.COMPARE_DATE = s.COMPARE_DATE) WHEN MATCHED THEN UPDATE SET r.UPDATED_AT = SYSDATE, r.CREATED_BY = :P_USER " +
                                "WHEN NOT MATCHED THEN INSERT (ID, COMPARE_DATE, CREATED_AT, UPDATED_AT, CREATED_BY) " +
                                "VALUES (SEQ_SLOT_COMPARE_RUN.NEXTVAL, :P_DATE, SYSDATE, SYSDATE, :P_USER)";
                            command.Parameters.Add("P_DATE", OracleDbType.Date).Value = date;
                            command.Parameters.Add("P_USER", OracleDbType.Varchar2).Value = string.IsNullOrWhiteSpace(userName) ? "WEB" : userName;
                            command.ExecuteNonQuery();
                        }
                        long runId;
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.CommandText = "SELECT ID FROM T_SLOT_COMPARE_RUN WHERE COMPARE_DATE = :P_DATE";
                            command.Parameters.Add("P_DATE", OracleDbType.Date).Value = date;
                            runId = Convert.ToInt64(command.ExecuteScalar(), CultureInfo.InvariantCulture);
                        }
                        using (OracleCommand command = connection.CreateCommand())
                        {
                            command.Transaction = transaction;
                            command.BindByName = true;
                            command.CommandText = "DELETE FROM T_SLOT_COMPARE_RESULT WHERE RUN_ID = :P_RUN AND RESULT_TYPE = :P_TYPE" +
                                (IsAllOperators(oper) ? string.Empty : " AND OPER = :P_OPER");
                            command.Parameters.Add("P_RUN", OracleDbType.Int64).Value = runId;
                            command.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = resultType;
                            if (!IsAllOperators(oper)) command.Parameters.Add("P_OPER", OracleDbType.Varchar2).Value = oper;
                            command.ExecuteNonQuery();
                        }
                        if (rows.Count > 0) InsertResults(connection, transaction, runId, rows);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void InsertResults(OracleConnection connection, OracleTransaction transaction, long runId, IList<SlotComparisonRow> rows)
        {
            using (OracleCommand command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.BindByName = true;
                command.ArrayBindCount = rows.Count;
                command.CommandText = "INSERT INTO T_SLOT_COMPARE_RESULT " +
                    "(ID, RUN_ID, RESULT_TYPE, FLIGHT_DATE, OPER, CALLSIGN, FROM_AIRP, TO_AIRP, ETD, REMARK, SOURCE_REF, CREATED_AT) " +
                    "VALUES (SEQ_SLOT_COMPARE_RESULT.NEXTVAL, :P_RUN, :P_TYPE, :P_DATE, :P_OPER, :P_CALLSIGN, :P_FROM, :P_TO, :P_ETD, :P_REMARK, :P_SOURCE, SYSDATE)";
                command.Parameters.Add("P_RUN", OracleDbType.Int64).Value = rows.Select(row => runId).ToArray();
                command.Parameters.Add("P_TYPE", OracleDbType.Varchar2).Value = rows.Select(row => row.ResultType).ToArray();
                command.Parameters.Add("P_DATE", OracleDbType.Date).Value = rows.Select(row => row.FlightDate).ToArray();
                command.Parameters.Add("P_OPER", OracleDbType.Varchar2).Value = rows.Select(row => row.Oper).ToArray();
                command.Parameters.Add("P_CALLSIGN", OracleDbType.Varchar2).Value = rows.Select(row => row.Callsign).ToArray();
                command.Parameters.Add("P_FROM", OracleDbType.Varchar2).Value = rows.Select(row => row.FromAirp).ToArray();
                command.Parameters.Add("P_TO", OracleDbType.Varchar2).Value = rows.Select(row => row.ToAirp).ToArray();
                command.Parameters.Add("P_ETD", OracleDbType.Varchar2).Value = rows.Select(row => row.Etd).ToArray();
                command.Parameters.Add("P_REMARK", OracleDbType.Varchar2).Value = rows.Select(row => row.Remark).ToArray();
                command.Parameters.Add("P_SOURCE", OracleDbType.Varchar2).Value = rows.Select(row => row.SourceRef).ToArray();
                command.ExecuteNonQuery();
            }
        }

        private static SlotComparisonRow CreateResult(DateTime date, string type, SourceFlight source, string remark)
        {
            return new SlotComparisonRow
            {
                ResultType = type,
                FlightDate = date,
                Oper = source.Oper,
                Callsign = source.Callsign,
                FromAirp = source.FromAirp,
                ToAirp = source.ToAirp,
                Etd = source.Etd,
                Remark = remark,
                SourceRef = source.SourceRef
            };
        }

        private static Dictionary<string, SourceFlight> ToDictionary(IEnumerable<SourceFlight> source)
        {
            return source.GroupBy(item => item.Key, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
        }

        private static IList<SlotComparisonRow> MapResults(DataTable data)
        {
            return data.AsEnumerable().Select(row => new SlotComparisonRow
            {
                Id = Convert.ToInt64(row["ID"], CultureInfo.InvariantCulture),
                ResultType = row["RESULT_TYPE"].ToString(),
                FlightDate = Convert.ToDateTime(row["FLIGHT_DATE"], CultureInfo.InvariantCulture),
                Oper = row["OPER"].ToString(),
                Callsign = row["CALLSIGN"].ToString(),
                FromAirp = row["FROM_AIRP"].ToString(),
                ToAirp = row["TO_AIRP"].ToString(),
                Etd = row["ETD"].ToString(),
                Remark = row["REMARK"].ToString(),
                SourceRef = row["SOURCE_REF"].ToString()
            }).ToList();
        }

        private static OracleParameter[] CloneParameters(IEnumerable<OracleParameter> parameters)
        {
            return parameters.Select(parameter => new OracleParameter(parameter.ParameterName, parameter.OracleDbType) { Value = parameter.Value }).ToArray();
        }

        private static bool IsAllOperators(string oper)
        {
            return string.IsNullOrWhiteSpace(oper) || oper.Equals("ALL", StringComparison.OrdinalIgnoreCase);
        }

        private static string ValidateResultType(string resultType)
        {
            resultType = NormalizeText(resultType);
            if (!ResultTypes.Contains(resultType)) throw new ArgumentException("Loại kết quả không hợp lệ.", "resultType");
            return resultType;
        }

        private static string NormalizeText(object value)
        {
            return Convert.ToString(value, CultureInfo.InvariantCulture).Trim().ToUpperInvariant();
        }

        private sealed class SourceData
        {
            public List<SourceFlight> Slot { get; set; }
            public List<SourceFlight> Permission { get; set; }
            public List<SourceFlight> AirlinePlan { get; set; }
        }

        private sealed class SourceFlight
        {
            public string Oper { get; set; }
            public string Callsign { get; set; }
            public string FromAirp { get; set; }
            public string ToAirp { get; set; }
            public string Etd { get; set; }
            public string SourceRef { get; set; }
            public string Key { get { return Callsign + "|" + FromAirp + "|" + ToAirp; } }
        }

        private sealed class OperatorInfo
        {
            public string Icao { get; set; }
            public string Iata { get; set; }
        }
    }
}
