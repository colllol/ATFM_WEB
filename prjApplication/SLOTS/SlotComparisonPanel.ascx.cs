using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjApplication.SLOTS
{
    public partial class SlotComparisonPanel : UserControl
    {
        private const int PageSize = 100;
        private readonly SlotComparisonService service = new SlotComparisonService();
        private readonly SlotsApiClient apiClient = new SlotsApiClient();

        public string Mode { get; set; }

        private string CurrentResultType
        {
            get { return Convert.ToString(ViewState["SlotResultType"] ?? "KQ1", CultureInfo.InvariantCulture); }
            set { ViewState["SlotResultType"] = value; }
        }

        private int CurrentPage
        {
            get { return ViewState["SlotResultPage"] == null ? 1 : (int)ViewState["SlotResultPage"]; }
            set { ViewState["SlotResultPage"] = Math.Max(1, value); }
        }

        private bool IsComparisonMode { get { return string.Equals(Mode, "Compare", StringComparison.OrdinalIgnoreCase); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            ConfigureMode();
            if (!IsPostBack)
            {
                if (IsComparisonMode)
                {
                    txtCompareDate.Text = service.GetDefaultDate().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    BindOperators(service.GetOperators());
                }
                else
                {
                    SlotComparisonBootstrapApiResponse bootstrap = apiClient.GetComparisonBootstrap();
                    txtCompareDate.Text = bootstrap.DefaultDate;
                    BindOperators(bootstrap.Operators ?? new List<string>());
                }
                BindResult();
            }
        }

        protected void Kq_Command(object sender, CommandEventArgs e)
        {
            ClearMessages();
            try
            {
                CurrentResultType = Convert.ToString(e.CommandArgument, CultureInfo.InvariantCulture);
                CurrentPage = 1;
                if (IsComparisonMode)
                {
                    int count = service.CompareAndSave(GetSelectedDate(), ddlOper.SelectedValue, CurrentResultType, GetCurrentUser());
                    pnlSuccess.Visible = true;
                    litSuccess.Text = string.Format("Đã hoàn thành {0}: lưu {1:N0} kết quả cho ngày {2:dd/MM/yyyy}.", CurrentResultType, count, GetSelectedDate());
                }
                BindResult();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            BindResultSafely();
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            BindResultSafely();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            BindResultSafely();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ClearMessages();
            try
            {
                DateTime date = GetSelectedDate();
                IList<SlotComparisonRow> rows = IsComparisonMode
                    ? service.GetAllResults(date, ddlOper.SelectedValue, CurrentResultType)
                    : GetAllApiResults(date).Select(ToServiceRow).ToList();
                string fileName = string.Format("{0}_{1}_{2}.xls", CurrentResultType, date.ToString("ddMMyy", CultureInfo.InvariantCulture), ddlOper.SelectedValue);
                string html = BuildExcelHtml(rows, date, ddlOper.SelectedValue, CurrentResultType);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = "utf-8";
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.Write("\uFEFF" + html);
                Response.End();
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void ConfigureMode()
        {
            btnFilter.Visible = !IsComparisonMode;
            btnExport.Visible = !IsComparisonMode;
            litPageTitle.Text = IsComparisonMode ? "Đối chiếu SLOT – Phép bay – Kế hoạch hãng" : "Kết quả đối chiếu";
            litPageDescription.Text = IsComparisonMode
                ? "Thực hiện KQ1–KQ4 từ T_SLOT_AERO, T_DAY_FLIGHTS và T_KHH; kết quả được lưu theo ngày và OPER."
                : "Tra cứu kết quả đã lưu và xuất Excel theo biểu mẫu BM.QLL-ĐCKSPK.";
        }

        private void BindOperators(IEnumerable<string> operators)
        {
            ddlOper.Items.Clear();
            ddlOper.Items.Add(new ListItem("Tất cả hãng", "ALL"));
            foreach (string oper in operators) ddlOper.Items.Add(new ListItem(oper, oper));
        }

        private void BindResultSafely()
        {
            ClearMessages();
            try { BindResult(); }
            catch (Exception ex) { ShowError(ex); }
        }

        private void BindResult()
        {
            DateTime date = GetSelectedDate();
            if (!IsComparisonMode)
            {
                BindApiResult(date);
                return;
            }
            int totalRecords;
            IList<SlotComparisonRow> rows = service.GetResults(date, ddlOper.SelectedValue, CurrentResultType, CurrentPage, PageSize, out totalRecords);
            int totalPages = Math.Max(1, (int)Math.Ceiling(totalRecords / (double)PageSize));
            if (CurrentPage > totalPages)
            {
                CurrentPage = totalPages;
                rows = service.GetResults(date, ddlOper.SelectedValue, CurrentResultType, CurrentPage, PageSize, out totalRecords);
            }
            litResultTitle.Text = HttpUtility.HtmlEncode(CurrentResultType + " – " + GetResultName(CurrentResultType));
            litResultDescription.Text = string.Format("Ngày {0:dd/MM/yyyy} · OPER: {1}", date, ddlOper.SelectedValue);
            lblResultTotal.Text = string.Format("{0:N0} kết quả", totalRecords);
            lblPaging.Text = string.Format("Trang {0}/{1}", CurrentPage, totalPages);
            btnPrevious.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < totalPages;
            litResultTable.Text = RenderTable(rows, ((CurrentPage - 1) * PageSize) + 1);
            ApplyActiveButton();
            BindSummary(date);
        }

        private void BindApiResult(DateTime date)
        {
            SlotComparisonApiResponse result = apiClient.GetComparisonResults(
                date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), ddlOper.SelectedValue,
                CurrentResultType, CurrentPage, PageSize);
            CurrentPage = Math.Max(1, result.PageIndex);
            litResultTitle.Text = HttpUtility.HtmlEncode(CurrentResultType + " - " + GetResultName(CurrentResultType));
            litResultDescription.Text = string.Format("Ngày {0:dd/MM/yyyy} · OPER: {1}", date, ddlOper.SelectedValue);
            lblResultTotal.Text = string.Format("{0:N0} kết quả", result.TotalRecords);
            lblPaging.Text = string.Format("Trang {0}/{1}", CurrentPage, Math.Max(1, result.TotalPages));
            btnPrevious.Enabled = CurrentPage > 1;
            btnNext.Enabled = CurrentPage < result.TotalPages;
            litResultTable.Text = RenderApiTable(result.Rows ?? new List<SlotComparisonApiRow>(),
                ((CurrentPage - 1) * PageSize) + 1);
            ApplyActiveButton();
            ApplyApiSummary(result.Summary);
        }

        private void ApplyApiSummary(IDictionary<string, int> summary)
        {
            summary = summary ?? new Dictionary<string, int>();
            btnKQ1.Text = string.Format(CultureInfo.InvariantCulture, "KQ1 ({0:N0})", GetCount(summary, "KQ1"));
            btnKQ2.Text = string.Format(CultureInfo.InvariantCulture, "KQ2 ({0:N0})", GetCount(summary, "KQ2"));
            btnKQ3.Text = string.Format(CultureInfo.InvariantCulture, "KQ3 ({0:N0})", GetCount(summary, "KQ3"));
            btnKQ4.Text = string.Format(CultureInfo.InvariantCulture, "KQ4 ({0:N0})", GetCount(summary, "KQ4"));
        }

        private static int GetCount(IDictionary<string, int> summary, string key)
        {
            int count;
            return summary.TryGetValue(key, out count) ? count : 0;
        }

        private IList<SlotComparisonApiRow> GetAllApiResults(DateTime date)
        {
            var rows = new List<SlotComparisonApiRow>();
            int page = 1;
            SlotComparisonApiResponse result;
            do
            {
                result = apiClient.GetComparisonResults(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    ddlOper.SelectedValue, CurrentResultType, page, 8000);
                if (result.Rows != null) rows.AddRange(result.Rows);
                page++;
            } while (page <= result.TotalPages);
            return rows;
        }

        private static SlotComparisonRow ToServiceRow(SlotComparisonApiRow row)
        {
            DateTime flightDate;
            DateTime.TryParseExact(row.FlightDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out flightDate);
            return new SlotComparisonRow
            {
                FlightDate = flightDate,
                Oper = row.Oper,
                Callsign = row.Callsign,
                FromAirp = row.FromAirp,
                ToAirp = row.ToAirp,
                Etd = row.Etd,
                Remark = row.Remark
            };
        }

        private static string RenderApiTable(IList<SlotComparisonApiRow> rows, int startNumber)
        {
            if (rows.Count == 0) return "<div class=\"slot-result-empty\">Chưa có kết quả đối chiếu phù hợp.</div>";
            StringBuilder html = new StringBuilder("<table class=\"slot-result-table\"><thead><tr><th>No.</th><th>Flightdate</th><th>OPER</th><th>CALLSIGN</th><th>FROM</th><th>TO</th><th>ETD</th><th>Remark</th></tr></thead><tbody>");
            for (int index = 0; index < rows.Count; index++)
            {
                SlotComparisonApiRow row = rows[index];
                DateTime flightDate;
                string dateText = DateTime.TryParseExact(row.FlightDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out flightDate) ? flightDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : row.FlightDate;
                html.Append("<tr><td>").Append(startNumber + index).Append("</td><td>").Append(Encode(dateText))
                    .Append("</td><td>").Append(Encode(row.Oper)).Append("</td><td>").Append(Encode(row.Callsign))
                    .Append("</td><td>").Append(Encode(row.FromAirp)).Append("</td><td>").Append(Encode(row.ToAirp))
                    .Append("</td><td>").Append(Encode(row.Etd)).Append("</td><td>").Append(Encode(row.Remark)).Append("</td></tr>");
            }
            return html.Append("</tbody></table>").ToString();
        }

        private void BindSummary(DateTime date)
        {
            IDictionary<string, int> summary = service.GetSummary(date, ddlOper.SelectedValue);
            btnKQ1.Text = string.Format(CultureInfo.InvariantCulture, "KQ1 ({0:N0})", summary["KQ1"]);
            btnKQ2.Text = string.Format(CultureInfo.InvariantCulture, "KQ2 ({0:N0})", summary["KQ2"]);
            btnKQ3.Text = string.Format(CultureInfo.InvariantCulture, "KQ3 ({0:N0})", summary["KQ3"]);
            btnKQ4.Text = string.Format(CultureInfo.InvariantCulture, "KQ4 ({0:N0})", summary["KQ4"]);
        }

        private void ApplyActiveButton()
        {
            LinkButton[] buttons = { btnKQ1, btnKQ2, btnKQ3, btnKQ4 };
            foreach (LinkButton button in buttons)
            {
                string type = button.CommandArgument;
                button.CssClass = "slot-kq-btn slot-kq-btn--" + type.Substring(2) +
                    (type == CurrentResultType ? " is-active" : string.Empty);
            }
        }

        private DateTime GetSelectedDate()
        {
            DateTime date;
            if (!DateTime.TryParseExact(txtCompareDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                throw new InvalidOperationException("Ngày đối chiếu không hợp lệ.");
            return date.Date;
        }

        private static string RenderTable(IList<SlotComparisonRow> rows, int startNumber)
        {
            if (rows.Count == 0) return "<div class=\"slot-result-empty\">Chưa có kết quả đối chiếu phù hợp.</div>";
            StringBuilder html = new StringBuilder("<table class=\"slot-result-table\"><thead><tr><th>No.</th><th>Flightdate</th><th>OPER</th><th>CALLSIGN</th><th>FROM</th><th>TO</th><th>ETD</th><th>Remark</th></tr></thead><tbody>");
            for (int index = 0; index < rows.Count; index++)
            {
                SlotComparisonRow row = rows[index];
                html.Append("<tr><td>").Append(startNumber + index).Append("</td><td>").Append(row.FlightDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture))
                    .Append("</td><td>").Append(Encode(row.Oper)).Append("</td><td>").Append(Encode(row.Callsign))
                    .Append("</td><td>").Append(Encode(row.FromAirp)).Append("</td><td>").Append(Encode(row.ToAirp))
                    .Append("</td><td>").Append(Encode(row.Etd)).Append("</td><td>").Append(Encode(row.Remark)).Append("</td></tr>");
            }
            return html.Append("</tbody></table>").ToString();
        }

        private static string BuildExcelHtml(IList<SlotComparisonRow> rows, DateTime date, string oper, string resultType)
        {
            StringBuilder html = new StringBuilder();
            html.Append("<html><head><meta charset='utf-8'><style>td,th{border:1px solid #333;padding:5px}table{border-collapse:collapse}.text{mso-number-format:'\\@'}</style></head><body><table>")
                .Append("<tr><th colspan='8'>").Append(Encode(GetResultName(resultType).ToUpperInvariant())).Append("</th></tr>")
                .Append("<tr><td>NGÀY</td><td colspan='7'>").Append(date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)).Append("</td></tr>")
                .Append("<tr><td>OPER</td><td colspan='7'>").Append(Encode(oper)).Append("</td></tr>")
                .Append("<tr><th>No.</th><th>Flightdate</th><th>OPER</th><th>CALLSIGN</th><th>FROM</th><th>TO</th><th>ETD</th><th>Remark</th></tr>");
            for (int index = 0; index < rows.Count; index++)
            {
                SlotComparisonRow row = rows[index];
                html.Append("<tr><td>").Append(index + 1).Append("</td><td>").Append(row.FlightDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture))
                    .Append("</td><td>").Append(Encode(row.Oper)).Append("</td><td class='text'>").Append(Encode(row.Callsign))
                    .Append("</td><td>").Append(Encode(row.FromAirp)).Append("</td><td>").Append(Encode(row.ToAirp))
                    .Append("</td><td class='text'>").Append(Encode(row.Etd)).Append("</td><td>").Append(Encode(row.Remark)).Append("</td></tr>");
            }
            return html.Append("</table></body></html>").ToString();
        }

        private static string GetResultName(string type)
        {
            if (type == "KQ1") return "Danh sách chuyến bay có SLOT nhưng không có Phép bay";
            if (type == "KQ2") return "Danh sách chuyến bay có Phép bay nhưng không có SLOT";
            if (type == "KQ3") return "Kế hoạch hãng không có đồng thời SLOT và Phép bay";
            return "Có SLOT và Phép bay nhưng không có Kế hoạch hãng";
        }

        private string GetCurrentUser()
        {
            if (Context != null && Context.User != null && Context.User.Identity != null && !string.IsNullOrWhiteSpace(Context.User.Identity.Name))
                return Context.User.Identity.Name;
            object sessionUser = Session == null ? null : Session["UserName"];
            return sessionUser == null ? "WEB" : Convert.ToString(sessionUser, CultureInfo.InvariantCulture);
        }

        private void ClearMessages()
        {
            pnlError.Visible = false;
            pnlSuccess.Visible = false;
            litError.Text = string.Empty;
            litSuccess.Text = string.Empty;
        }

        private void ShowError(Exception exception)
        {
            pnlError.Visible = true;
            litError.Text = Encode(exception.Message);
        }

        private static string Encode(object value) { return HttpUtility.HtmlEncode(Convert.ToString(value, CultureInfo.InvariantCulture)); }
    }
}
