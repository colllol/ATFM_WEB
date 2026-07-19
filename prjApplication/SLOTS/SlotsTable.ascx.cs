using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Oracle.ManagedDataAccess.Client;

namespace prjApplication.SLOTS
{
    public partial class SlotsTable : UserControl
    {
        private static readonly string[] AllowedTables = { "T_KHH", "T_SLOT_AERO" };
        private static readonly Regex SafeIdentifier = new Regex("^[A-Z][A-Z0-9_$#]*$", RegexOptions.Compiled);

        public string TableName { get; set; }
        public string DisplayTitle { get; set; }
        public string Description { get; set; }

        private int CurrentPage
        {
            get { return ViewState["SlotsCurrentPage"] == null ? 1 : (int)ViewState["SlotsCurrentPage"]; }
            set { ViewState["SlotsCurrentPage"] = Math.Max(1, value); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            litTitle.Text = HttpUtility.HtmlEncode(DisplayTitle);
            litDescription.Text = HttpUtility.HtmlEncode(Description);
            if (!IsPostBack) BindData();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindData();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            txtSearch.Text = string.Empty;
            txtFilterDate.Text = string.Empty;
            txtAirport.Text = string.Empty;
            BindData();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            BindData();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindData();
        }

        private void BindData()
        {
            pnlError.Visible = false;
            litError.Text = string.Empty;
            string tableName = GetAllowedTable();
            lblSourceBadge.Text = tableName;

            try
            {
                SlotsDataProvider provider = new SlotsDataProvider();
                DataTable metadata = provider.ExecuteQuery(
                    "SELECT COLUMN_NAME, DATA_TYPE FROM USER_TAB_COLUMNS WHERE TABLE_NAME = :P_TABLE ORDER BY COLUMN_ID",
                    new OracleParameter("P_TABLE", OracleDbType.Varchar2) { Value = tableName });

                if (metadata.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy metadata hoặc tài khoản hiện tại chưa có quyền đọc bảng " + tableName + ".");

                List<SlotColumn> columns = metadata.AsEnumerable()
                    .Select(row => new SlotColumn(row["COLUMN_NAME"].ToString(), row["DATA_TYPE"].ToString()))
                    .Where(column => SafeIdentifier.IsMatch(column.Name))
                    .ToList();

                if (columns.Count == 0) throw new InvalidOperationException("Bảng không có cột hợp lệ để hiển thị.");

                string keyword = (txtSearch.Text ?? string.Empty).Trim();
                string airport = (txtAirport.Text ?? string.Empty).Trim().ToUpperInvariant();
                DateTime? filterDate = ParseFilterDate(txtFilterDate.Text);
                string whereClause = BuildWhereClause(columns, keyword, tableName, filterDate, airport);
                int pageSize = GetPageSize();
                int totalRecords = Convert.ToInt32(provider.ExecuteScalar(
                    "SELECT COUNT(1) FROM " + tableName + " q " + whereClause,
                    CreateFilterParameters(keyword, filterDate, airport)) ?? 0, CultureInfo.InvariantCulture);
                int totalPages = Math.Max(1, (int)Math.Ceiling(totalRecords / (double)pageSize));
                if (CurrentPage > totalPages) CurrentPage = totalPages;

                int rowStart = ((CurrentPage - 1) * pageSize) + 1;
                int rowEnd = CurrentPage * pageSize;
                string columnList = string.Join(",", columns.Select(column => "\"" + column.Name + "\""));
                string sql = "SELECT " + columnList + " FROM (" +
                             "SELECT q.*, ROW_NUMBER() OVER (ORDER BY ROWID) ATFM_SLOT_ROW_NO FROM " + tableName + " q " + whereClause +
                             ") WHERE ATFM_SLOT_ROW_NO BETWEEN :P_ROW_START AND :P_ROW_END";

                List<OracleParameter> parameters = CreateFilterParameters(keyword, filterDate, airport).ToList();
                parameters.Add(new OracleParameter("P_ROW_START", OracleDbType.Int32) { Value = rowStart });
                parameters.Add(new OracleParameter("P_ROW_END", OracleDbType.Int32) { Value = rowEnd });
                DataTable data = provider.ExecuteQuery(sql, parameters.ToArray());

                litTable.Text = RenderTable(columns, data, rowStart);
                lblPaging.Text = string.Format("Trang {0}/{1} · {2:N0} dòng", CurrentPage, totalPages, totalRecords);
                btnPrevious.Enabled = CurrentPage > 1;
                btnNext.Enabled = CurrentPage < totalPages;
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = HttpUtility.HtmlEncode(GetFriendlyError(ex));
                litTable.Text = "<div class=\"slots-empty\">Chưa thể tải dữ liệu từ " + HttpUtility.HtmlEncode(tableName) + ".</div>";
                lblPaging.Text = "Không có dữ liệu";
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
            }
        }

        private string GetAllowedTable()
        {
            string selected = (TableName ?? string.Empty).ToUpperInvariant();
            if (!AllowedTables.Contains(selected))
                throw new InvalidOperationException("Nguồn dữ liệu SLOTS không hợp lệ.");
            return selected;
        }

        private int GetPageSize()
        {
            int pageSize;
            return int.TryParse(ddlPageSize.SelectedValue, out pageSize) && pageSize > 0 ? Math.Min(pageSize, 200) : 50;
        }

        private static string BuildWhereClause(IEnumerable<SlotColumn> columns, string keyword, string tableName,
            DateTime? filterDate, string airport)
        {
            List<SlotColumn> availableColumns = columns.ToList();
            List<string> groups = new List<string>();
            string[] searchableTypes = { "CHAR", "VARCHAR2", "NVARCHAR2", "NCHAR", "NUMBER", "FLOAT", "DATE", "TIMESTAMP" };
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                List<string> searchConditions = availableColumns
                    .Where(column => searchableTypes.Any(type => column.DataType.StartsWith(type, StringComparison.OrdinalIgnoreCase)))
                    .Select(column => "UPPER(TO_CHAR(q.\"" + column.Name + "\")) LIKE :P_SEARCH")
                    .ToList();
                if (searchConditions.Count > 0) groups.Add("(" + string.Join(" OR ", searchConditions) + ")");
            }

            string dateColumn = tableName == "T_KHH" ? "Date" : "FLIGHT_DATE";
            if (filterDate.HasValue && availableColumns.Any(column => column.Name == dateColumn))
                groups.Add("q.\"" + dateColumn + "\" >= :P_FILTER_DATE AND q.\"" + dateColumn + "\" < :P_FILTER_DATE_TO");

            if (!string.IsNullOrWhiteSpace(airport))
            {
                string[] airportColumns = tableName == "T_KHH"
                    ? new[] { "From", "To" }
                    : new[] { "FROM_AIRP", "TO_AIRP" };
                List<string> airportConditions = airportColumns
                    .Where(name => availableColumns.Any(column => column.Name == name))
                    .Select(name => "UPPER(TO_CHAR(q.\"" + name + "\")) LIKE :P_AIRPORT")
                    .ToList();
                if (airportConditions.Count > 0) groups.Add("(" + string.Join(" OR ", airportConditions) + ")");
            }

            return groups.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", groups);
        }

        private static OracleParameter[] CreateFilterParameters(string keyword, DateTime? filterDate, string airport)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            if (!string.IsNullOrWhiteSpace(keyword))
                parameters.Add(new OracleParameter("P_SEARCH", OracleDbType.Varchar2) { Value = "%" + keyword.ToUpperInvariant() + "%" });
            if (filterDate.HasValue)
            {
                parameters.Add(new OracleParameter("P_FILTER_DATE", OracleDbType.Date) { Value = filterDate.Value.Date });
                parameters.Add(new OracleParameter("P_FILTER_DATE_TO", OracleDbType.Date) { Value = filterDate.Value.Date.AddDays(1) });
            }
            if (!string.IsNullOrWhiteSpace(airport))
                parameters.Add(new OracleParameter("P_AIRPORT", OracleDbType.Varchar2) { Value = "%" + airport + "%" });
            return parameters.ToArray();
        }

        private static DateTime? ParseFilterDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            DateTime parsed;
            string[] formats = { "yyyy-MM-dd", "dd/MM/yyyy", "dd-MM-yyyy" };
            if (DateTime.TryParseExact(value.Trim(), formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out parsed)) return parsed.Date;
            throw new InvalidOperationException("Ngày bay không đúng định dạng.");
        }

        private static string RenderTable(IList<SlotColumn> columns, DataTable data, int rowStart)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table class=\"table table-bordered slots-data-table\"><thead><tr><th>STT</th>");
            foreach (SlotColumn column in columns) html.Append("<th>").Append(HttpUtility.HtmlEncode(column.Name)).Append("</th>");
            html.Append("</tr></thead><tbody>");

            if (data.Rows.Count == 0)
            {
                html.Append("<tr><td colspan=\"").Append(columns.Count + 1).Append("\"><div class=\"slots-empty\">Không có dữ liệu phù hợp.</div></td></tr>");
            }
            else
            {
                for (int rowIndex = 0; rowIndex < data.Rows.Count; rowIndex++)
                {
                    html.Append("<tr><td>").Append(rowStart + rowIndex).Append("</td>");
                    foreach (SlotColumn column in columns)
                    {
                        string value = FormatValue(data.Rows[rowIndex][column.Name]);
                        html.Append("<td title=\"").Append(HttpUtility.HtmlAttributeEncode(value)).Append("\">")
                            .Append(HttpUtility.HtmlEncode(value)).Append("</td>");
                    }
                    html.Append("</tr>");
                }
            }
            html.Append("</tbody></table>");
            return html.ToString();
        }

        private static string FormatValue(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;
            if (value is DateTime) return ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            if (value is byte[]) return "[BINARY " + ((byte[])value).Length + " bytes]";
            return Convert.ToString(value, CultureInfo.InvariantCulture);
        }

        private static string GetFriendlyError(Exception exception)
        {
            string message = exception.Message ?? "Không xác định được lỗi kết nối dữ liệu.";
            if (message.IndexOf("ORA-12170", StringComparison.OrdinalIgnoreCase) >= 0)
                return "Không thể kết nối máy chủ Oracle (ORA-12170). Hãy kiểm tra mạng nội bộ/VPN rồi tải lại trang.";
            return message;
        }

        private sealed class SlotColumn
        {
            public SlotColumn(string name, string dataType) { Name = name; DataType = dataType; }
            public string Name { get; private set; }
            public string DataType { get; private set; }
        }
    }
}
