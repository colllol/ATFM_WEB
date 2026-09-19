using System;
using System.Web.UI;
using prjBusinessLogic;
using System.IO;
using System.Data;
using System.Linq;


namespace prjApplication.Report.VietNameseFlights
{
    public partial class BTNuoc_NoSum : System.Web.UI.Page
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
                    //WriteInfoTime();
                    LoadData();
                    lbldatekhoang.Text = "FromDate " + _dateFrom.Trim() + " ToDate " + _dateTo.Trim();
                    lblcontent.Text = LoadData();
                }
            }
        }

        //public void WriteInfoTime()
        //{
        //    _datekhoang = "Từ " + _dateFrom.Trim() + " đến " + _dateTo.Trim();
        //}
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
                    DataTable dt = _rpDAL.BTNuoc_NoSum(_dateFrom.Trim(), _dateTo.Trim());
                    return GenDataTable(dt);
                }
            }
            return "";
        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            var a = "";
            var b = "";
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    b = r["oper_id"].ToString();

                    kq += "<tr>";
                    kq += $"<td>{(a!=b?i.ToString():"")}</td>";
                    kq += "<td>" + r["oper_id"].ToString() + "</td>";
                    kq += "<td>" + r["FLIGHTDATE"].ToString() + "</td>";
                    kq += "<td>" + r["FLIGHTNBR"].ToString() + "</td>";
                    kq += "<td>" + r["REGISTRATION"].ToString() + "</td>";
                    kq += "<td>" + r["REAL_CRAFT_TYPE"].ToString() + "</td>";
                    kq += "<td>" + r["FROM_AIRP"].ToString() + "</td>";
                    kq += "<td>" + r["TO_AIRP"].ToString() + "</td>";
                    kq += "<td>" + r["FPL_VIA"].ToString() + "</td>";
                    kq += "<td>" + r["ETD"].ToString() + "</td>";
                    kq += "<td>" + r["ATA"].ToString() + "</td>";
                    kq += "<td>" + r["PURPOSE"].ToString() + "</td>";
                    kq += "<td>" + r["AMOUNT"].ToString() + "</td>";
                    kq += "</tr>";
                    if (a != b) i++;
                    a = b;
                }
            }
            return kq;
        }
        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "BTNuoc_NoSum" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            this.CreateExcel(html, "BTNuoc_NoSum" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        #endregion
    }
}