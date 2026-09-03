using prjInfo;
using prjBusinessLogic;
using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace prjApplication.PlanMessage
{
    public partial class SendAMHSHistory : PageBaseCallBack
    {
        private const string AliasSession = "SendAMHSHistory";
        private const string FromDateSession = AliasSession + ".FromDate";
        private const string ToDateSession = AliasSession + ".ToDate";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtFromDate.Value = Session[FromDateSession] as string
                    ?? DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                txtToDate.Value = Session[ToDateSession] as string
                    ?? DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                LoadDataGrid();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            CustomPaging1.PageIndex = 0;
            SaveSearchDates();
            LoadDataGrid();
        }

        public override string GetCallbackResult()
        {
            if (string.IsNullOrEmpty(_EventArgument))
                return string.Empty;

            string[] args = _EventArgument.Split(new[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0 || !string.Equals(args[args.Length - 1], "LoadDataGrid", StringComparison.OrdinalIgnoreCase))
                return string.Empty;

            return LoadDataGrid();
        }

        private string LoadDataGrid()
        {
            litStatus.Text = string.Empty;
            DataTable table = null;

            try
            {
                string fromDate = NormalizeDate(txtFromDate.Value, "TỪ NGÀY");
                string toDate = NormalizeDate(txtToDate.Value, "ĐẾN NGÀY");
                SaveSearchDates();

                table = new clsResuftAPI().GetTableApiExtension(
                    "SEND_AMHS_HISTORY_PKG",
                    "GET_PAGE",
                    new
                    {
                        P_FROM_DATE = fromDate,
                        P_TO_DATE = toDate,
                        P_PAGE_SIZE = CustomPaging1.PageSize,
                        P_PAGE_INDEX = CustomPaging1.PageIndex
                    });

                if (table != null && table.Rows.Count > 0 && table.Columns.Contains("SUMRECORD"))
                    CustomPaging1.TotalsRecord = Convert.ToInt32(table.Rows[0]["SUMRECORD"]);
                else
                    CustomPaging1.TotalsRecord = 0;
            }
            catch (Exception ex)
            {
                CustomPaging1.TotalsRecord = 0;
                litStatus.Text = "<span class=\"amhsh-status\">"
                    + HttpUtility.HtmlEncode("Không tải được lịch sử AMHS: " + ex.GetBaseException().Message)
                    + "</span>";
            }

            grdSource.DataSource = table;
            grdSource.DataBind();
            return RenderToHTML(grdSource);
        }

        private void SaveSearchDates()
        {
            Session[FromDateSession] = txtFromDate.Value;
            Session[ToDateSession] = txtToDate.Value;
        }

        private static string NormalizeDate(string value, string fieldName)
        {
            DateTime parsed;
            if (!DateTime.TryParseExact(
                    (value ?? string.Empty).Trim(),
                    new[] { "dd/MM/yyyy", "dd-MM-yyyy" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out parsed))
            {
                throw new ArgumentException(fieldName + " không hợp lệ, định dạng DD/MM/YYYY.");
            }

            return parsed.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

        protected string HtmlText(object value)
        {
            return HttpUtility.HtmlEncode(value == null || value == DBNull.Value ? string.Empty : value.ToString());
        }

        protected string FormatDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            DateTime parsed;
            if (value is DateTime)
                parsed = (DateTime)value;
            else if (!DateTime.TryParseExact(
                value.ToString(),
                new[]
                {
                    "dd/MM/yyyy HH:mm:ss",
                    "dd-MM-yyyy HH:mm:ss",
                    "yyyy-MM-dd HH:mm:ss",
                    "yyyy-MM-ddTHH:mm:ss",
                    "yyyy-MM-ddTHH:mm:ss.fff"
                },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsed))
                return HtmlText(value);

            if (parsed != DateTime.MinValue)
                return parsed.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            return HtmlText(value);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }
    }
}
