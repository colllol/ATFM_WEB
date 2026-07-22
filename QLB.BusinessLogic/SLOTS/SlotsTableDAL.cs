using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class SlotsTableDAL
    {
        private sealed class SourceDefinition
        {
            public string Name;
            public string Table;
            public string DateColumn;
            public string[] AirportColumns;
        }

        private static readonly IDictionary<string, SourceDefinition> Sources =
            new Dictionary<string, SourceDefinition>(StringComparer.OrdinalIgnoreCase)
            {
                { "KHH", new SourceDefinition { Name = "KHH", Table = "T_KHH", DateColumn = "Date", AirportColumns = new[] { "From", "To" } } },
                { "SLOT_AERO", new SourceDefinition { Name = "SLOT_AERO", Table = "T_SLOT_AERO", DateColumn = "FLIGHT_DATE", AirportColumns = new[] { "FROM_AIRP", "TO_AIRP" } } }
            };

        public SlotsTableResponse GetData(SlotsTableRequest request)
        {
            if (request == null) throw new ArgumentException("Dữ liệu yêu cầu không được để trống.");
            SourceDefinition source;
            if (!Sources.TryGetValue((request.Source ?? String.Empty).Trim(), out source))
                throw new ArgumentException("Nguồn dữ liệu chỉ chấp nhận KHH hoặc SLOT_AERO.");

            DateTime? flightDate = ParseDate(request.FlightDate);
            int pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            int pageSize = request.PageSize <= 0 ? 100 : Math.Min(request.PageSize, 8000);
            string keyword = (request.Keyword ?? String.Empty).Trim();
            string airport = (request.Airport ?? String.Empty).Trim().ToUpperInvariant();

            using (OracleConnection connection = CreateConnection())
            {
                connection.Open();
                List<SlotsTableColumn> columns = LoadColumns(connection, source.Table);
                if (columns.Count == 0) throw new InvalidOperationException("Không tìm thấy metadata của bảng " + source.Table + ".");
                string where = BuildWhere(columns, source, keyword, flightDate, airport);
                int total = ExecuteCount(connection, source.Table, where, keyword, flightDate, airport);
                int totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)pageSize));
                pageIndex = Math.Min(pageIndex, totalPages);
                List<Dictionary<string, string>> rows = LoadRows(connection, source.Table, columns, where,
                    keyword, flightDate, airport, ((pageIndex - 1) * pageSize) + 1, pageIndex * pageSize);
                return new SlotsTableResponse { Source = source.Name, Columns = columns, Rows = rows, PageIndex = pageIndex,
                    PageSize = pageSize, TotalRecords = total, TotalPages = totalPages };
            }
        }

        private static List<SlotsTableColumn> LoadColumns(OracleConnection connection, string table)
        {
            var result = new List<SlotsTableColumn>();
            using (var command = CreateCommand(connection, "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLUMNS WHERE TABLE_NAME=:tableName ORDER BY COLUMN_ID"))
            {
                command.Parameters.Add("tableName", OracleDbType.Varchar2).Value = table;
                using (var reader = command.ExecuteReader()) while (reader.Read()) result.Add(new SlotsTableColumn
                { Name = Convert.ToString(reader[0]), DataType = Convert.ToString(reader[1]) });
            }
            return result;
        }

        private static string BuildWhere(IList<SlotsTableColumn> columns, SourceDefinition source, string keyword, DateTime? date, string airport)
        {
            var groups = new List<string>();
            string[] searchable = { "CHAR", "VARCHAR2", "NVARCHAR2", "NCHAR", "NUMBER", "FLOAT", "DATE", "TIMESTAMP" };
            if (keyword.Length > 0)
            {
                var items = columns.Where(c => searchable.Any(t => c.DataType.StartsWith(t, StringComparison.OrdinalIgnoreCase)))
                    .Select(c => "UPPER(TO_CHAR(q.\"" + c.Name + "\")) LIKE :search").ToList();
                if (items.Count > 0) groups.Add("(" + String.Join(" OR ", items) + ")");
            }
            if (date.HasValue && columns.Any(c => c.Name == source.DateColumn))
                groups.Add("q.\"" + source.DateColumn + "\">=:flightDate AND q.\"" + source.DateColumn + "\"<:flightDateTo");
            if (airport.Length > 0)
            {
                var items = source.AirportColumns.Where(n => columns.Any(c => c.Name == n))
                    .Select(n => "UPPER(TO_CHAR(q.\"" + n + "\")) LIKE :airport").ToList();
                if (items.Count > 0) groups.Add("(" + String.Join(" OR ", items) + ")");
            }
            return groups.Count == 0 ? String.Empty : " WHERE " + String.Join(" AND ", groups);
        }

        private static int ExecuteCount(OracleConnection connection, string table, string where, string keyword, DateTime? date, string airport)
        {
            using (var command = CreateCommand(connection, "SELECT COUNT(1) FROM " + table + " q" + where))
            { AddFilters(command, keyword, date, airport); return Convert.ToInt32(command.ExecuteScalar(), CultureInfo.InvariantCulture); }
        }

        private static List<Dictionary<string, string>> LoadRows(OracleConnection connection, string table,
            IList<SlotsTableColumn> columns, string where, string keyword, DateTime? date, string airport, int start, int end)
        {
            string list = String.Join(",", columns.Select(c => "\"" + c.Name + "\""));
            string sql = "SELECT " + list + " FROM (SELECT q.*, ROW_NUMBER() OVER(ORDER BY ROWID) ATFM_SLOT_ROW_NO FROM " +
                table + " q" + where + ") WHERE ATFM_SLOT_ROW_NO BETWEEN :rowStart AND :rowEnd";
            var result = new List<Dictionary<string, string>>();
            using (var command = CreateCommand(connection, sql))
            {
                AddFilters(command, keyword, date, airport);
                command.Parameters.Add("rowStart", OracleDbType.Int32).Value = start;
                command.Parameters.Add("rowEnd", OracleDbType.Int32).Value = end;
                using (var reader = command.ExecuteReader()) while (reader.Read())
                {
                    var row = new Dictionary<string, string>();
                    foreach (SlotsTableColumn column in columns) row[column.Name] = FormatValue(reader[column.Name]);
                    result.Add(row);
                }
            }
            return result;
        }

        private static void AddFilters(OracleCommand command, string keyword, DateTime? date, string airport)
        {
            if (command.CommandText.Contains(":search")) command.Parameters.Add("search", OracleDbType.Varchar2).Value = "%" + keyword.ToUpperInvariant() + "%";
            if (command.CommandText.Contains(":flightDate"))
            {
                command.Parameters.Add("flightDate", OracleDbType.Date).Value = date.Value.Date;
                command.Parameters.Add("flightDateTo", OracleDbType.Date).Value = date.Value.Date.AddDays(1);
            }
            if (command.CommandText.Contains(":airport")) command.Parameters.Add("airport", OracleDbType.Varchar2).Value = "%" + airport + "%";
        }

        private static string FormatValue(object value)
        {
            if (value == null || value == DBNull.Value) return String.Empty;
            if (value is DateTime) return ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            if (value is byte[]) return "[BINARY " + ((byte[])value).Length + " bytes]";
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private static DateTime? ParseDate(string value)
        {
            if (String.IsNullOrWhiteSpace(value)) return null;
            DateTime date;
            if (!DateTime.TryParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                throw new ArgumentException("Ngày bay phải đúng định dạng yyyy-MM-dd.");
            return date.Date;
        }

        private static OracleCommand CreateCommand(OracleConnection connection, string sql)
        { return new OracleCommand(sql, connection) { BindByName = true, CommandTimeout = 90 }; }

        private static OracleConnection CreateConnection()
        {
            string value = ConfigurationManager.AppSettings["ConnectDB"];
            if (String.IsNullOrWhiteSpace(value)) throw new ConfigurationErrorsException("Thiếu cấu hình AppSettings ConnectDB.");
            return new OracleConnection(value);
        }
    }
}
