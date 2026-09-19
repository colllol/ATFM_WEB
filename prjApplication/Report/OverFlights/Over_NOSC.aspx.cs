using System;
using System.Web.UI;
using prjBusinessLogic;
using System.IO;
using System.Data;
using System.Linq;


namespace prjApplication.Report.OverFlights
{
    public partial class Over_NOSC : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _content = null;
        public string _dateFrom = null;
        public string _dateTo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FromDate"] != null && Request["FromDate"].ToString() != "" && Request["FromDate"].ToString() != String.Empty && Request["ToDate"] != null && Request["ToDate"].ToString() != "" && Request["ToDate"].ToString() != String.Empty)
            {
                _dateFrom = Request["FromDate"].ToString();
                _dateTo = Request["ToDate"].ToString();
                if (!IsPostBack)
                {
                    LoadData();
                    lbldatekhoang.Text = "FromDate " + _dateFrom.Trim() + " ToDate " + _dateTo.Trim();
                    lblcontent.Text = LoadData();
                }
            }
        }
        protected string LoadData()
        {

            if (UltilFunc.checkDate(_dateFrom, _dateTo))
            {
                if (UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy") > UltilFunc.ToDate(_dateTo.Trim(), "dd/MM/yyyy"))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert ('Ngày bắt đầu ko được lớn hơn ngày kết thúc!')", true);
                    return "";
                }
                else
                {
                    ReportStatisticDAL _rpDAL = new ReportStatisticDAL();
                    DataTable dt = _rpDAL.Over_NOSC(_dateFrom.Trim(), _dateTo.Trim());
                    return GenDataTable(dt);
                }
            }
            return "";
        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            ReportStatisticDAL _rpStatic = new ReportStatisticDAL();

            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\" style=\"text-align:center\">" + (i + 1) + "</td>";
                    kq += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + UltilFunc.ToDate(r["FLIGHTDATE"].ToString(),"dd/mm/yyyy").ToShortDateString() + "</td>";
                    kq += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + r["FLIGHTNBR"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + r["REAL_CRAFT_TYPE"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + r["FPL_VIA"].ToString() + "</td>";
                    kq += "</tr>";
                    i++;
                }
                lblSumFirHn.Text  = _rpStatic.Over_NOSC_FirHn(_dateFrom, _dateTo);
                lblSumFirHcm.Text  = _rpStatic.Over_NOSC_FirHcm(_dateFrom, _dateTo);
                lblSumFlights.Text = _rpStatic.Over_NOSC_Sum(_dateFrom, _dateTo);
            }
            return kq;
        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "Over_NOSC" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            this.CreateExcel(html, "Over_NOSC" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        #endregion
    }
}