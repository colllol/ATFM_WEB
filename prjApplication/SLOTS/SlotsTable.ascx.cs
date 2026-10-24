using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace prjApplication.SLOTS
{
    public partial class SlotsTable : UserControl
    {
        private static readonly string[] AllowedTables = { "T_KHH", "T_SLOT_AERO" };

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

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e) { CurrentPage = 1; BindData(); }
        protected void btnSearch_Click(object sender, EventArgs e) { CurrentPage = 1; BindData(); }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            txtSearch.Text = string.Empty;
            txtFilterDate.Text = string.Empty;
            txtAirport.Text = string.Empty;
            BindData();
        }

        protected void btnPrevious_Click(object sender, EventArgs e) { CurrentPage--; BindData(); }
        protected void btnNext_Click(object sender, EventArgs e) { CurrentPage++; BindData(); }

        private void BindData()
        {
            pnlError.Visible = false;
            litError.Text = string.Empty;
            string tableName = GetAllowedTable();
            string source = tableName == "T_KHH" ? "KHH" : "SLOT_AERO";
            lblSourceBadge.Text = tableName;

            try
            {
                ValidateFilterDate(txtFilterDate.Text);
                SlotsTableApiResponse data = new SlotsApiClient().GetTableData(
                    source,
                    (txtSearch.Text ?? string.Empty).Trim(),
                    (txtFilterDate.Text ?? string.Empty).Trim(),
                    (txtAirport.Text ?? string.Empty).Trim().ToUpperInvariant(),
                    CurrentPage,
                    GetPageSize());

                CurrentPage = Math.Max(1, data.PageIndex);
                IList<SlotsTableApiColumn> columns = data.Columns ?? new List<SlotsTableApiColumn>();
                IList<Dictionary<string, string>> rows = data.Rows ?? new List<Dictionary<string, string>>();
                int rowStart = ((CurrentPage - 1) * data.PageSize) + 1;
                litTable.Text = RenderTable(columns, rows, rowStart);
                lblPaging.Text = string.Format("Trang {0}/{1} · {2:N0} dòng", CurrentPage,
                    Math.Max(1, data.TotalPages), data.TotalRecords);
                btnPrevious.Enabled = CurrentPage > 1;
                btnNext.Enabled = CurrentPage < data.TotalPages;
            }
            catch (Exception ex)
            {
                pnlError.Visible = true;
                litError.Text = HttpUtility.HtmlEncode(ex.Message);
                litTable.Text = "<div class=\"slots-empty\">Chưa thể tải dữ liệu từ API cho " +
                    HttpUtility.HtmlEncode(tableName) + ".</div>";
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
            return int.TryParse(ddlPageSize.SelectedValue, out pageSize) && pageSize > 0
                ? Math.Min(pageSize, 8000) : 100;
        }

        private static void ValidateFilterDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;
            DateTime parsed;
            if (!DateTime.TryParseExact(value.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out parsed))
                throw new InvalidOperationException("Ngày bay không đúng định dạng.");
        }

        private static string RenderTable(IList<SlotsTableApiColumn> columns,
            IList<Dictionary<string, string>> rows, int rowStart)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<table class=\"table table-bordered slots-data-table\"><thead><tr><th>STT</th>");
            foreach (SlotsTableApiColumn column in columns)
                html.Append("<th>").Append(HttpUtility.HtmlEncode(column.Name)).Append("</th>");
            html.Append("</tr></thead><tbody>");

            if (rows.Count == 0)
            {
                html.Append("<tr><td colspan=\"").Append(columns.Count + 1)
                    .Append("\"><div class=\"slots-empty\">Không có dữ liệu phù hợp.</div></td></tr>");
            }
            else
            {
                for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                {
                    html.Append("<tr><td>").Append(rowStart + rowIndex).Append("</td>");
                    foreach (SlotsTableApiColumn column in columns)
                    {
                        string value;
                        if (!rows[rowIndex].TryGetValue(column.Name, out value)) value = string.Empty;
                        html.Append("<td title=\"").Append(HttpUtility.HtmlAttributeEncode(value)).Append("\">")
                            .Append(HttpUtility.HtmlEncode(value)).Append("</td>");
                    }
                    html.Append("</tr>");
                }
            }
            return html.Append("</tbody></table>").ToString();
        }
    }
}
