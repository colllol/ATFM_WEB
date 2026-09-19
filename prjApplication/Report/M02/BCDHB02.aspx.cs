using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using prjComponents;
using System.IO;
using TuesPechkin;
using System.Data;

namespace prjApplication.Report.M02
{
    public partial class BCDHB02 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _datetime = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;

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
                    WriteInfoTime();
                    LoadData();
                }
            }
        }

        public void WriteInfoTime()
        {
            _date = DateTime.Now.Day.ToString();
            _month = DateTime.Now.Month.ToString();
            _year = DateTime.Now.Year.ToString();
            _datetime = DateTime.Now.ToString("dd/MM/yyyy");

            _datekhoang = "Từ " + _dateFrom.Trim() + " đến " + _dateTo.Trim();
        }

        protected void LoadData()
        {

            if (UltilFunc.checkDate(_dateFrom, _dateTo))
            {
                if (UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy") > UltilFunc.ToDate(_dateTo.Trim(), "dd/MM/yyyy"))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert ('Ngày bắt đầu ko được lớn hơn ngày kết thúc!')", true);
                    return;
                }
                else
                {
                    _content = GenDataTable();
                }
            }

        }

        private string GenDataTable()
        {
            ReportDHBDAL _rpDAL = new ReportDHBDAL();

            string _dateThangNay = ""; string _dateThangTruoc = "";
            string _valThangTruocLD = ""; string _valThangTruocOF = ""; string _valThangNayLD = ""; string _valThangNayOF = "";
            string _valTongThang1 = ""; string _valTongThang2 = "";
            DateTime _dt = UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy");
            _dateThangTruoc = UltilFunc.ReturnMonthBefore(_dt);
            _dateThangNay = String.Format("{0:MM/yyyy}", _dt);
            

            _valThangTruocLD = _rpDAL.BCDHB02_TH_ThangTruoc_TypeLD(_dateThangTruoc);

            _valThangTruocOF = _rpDAL.BCDHB02_TH_ThangTruoc_TypeOF(_dateThangTruoc);

            _valThangNayLD = _rpDAL.BCDHB02_TH_Thang_TypeLD(_dateThangNay, _dateFrom, _dateTo);

            _valThangNayOF = _rpDAL.BCDHB02_TH_Thang_TypeOF(_dateThangNay, _dateFrom, _dateTo);

            _valTongThang1 = _rpDAL.GetDaThucHien_RP_Total(_valThangNayLD, _valThangNayOF);

            _valTongThang2 = _rpDAL.GetDaThucHien_RP_Total(_valThangTruocLD, _valThangTruocOF);

            string _html = null;

            _html += "<tr>";
            _html += "<td class=\"tg-yw4l\">1.Bay đi đến</td>";
            _html += "<td class=\"tg-s6z2\">" + _valThangNayLD + "</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">" + _valThangTruocLD + "</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "</tr>";
            _html += "<tr>";
            _html += "<td class=\"tg -yw4l\">2.Bay quá cảnh</td>";
            _html += "<td class=\"tg-s6z2\">" + _valThangNayOF + "</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">" + _valThangTruocOF + "</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "</tr>";
            _html += "<tr>";
            _html += "<td class=\"tg -yw4l\">3.Tổng cộng</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTongThang1 + "</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTongThang2 + "</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "<td class=\"tg-s6z2\">0</td>";
            _html += "</tr>";
            return _html;
        }

       


        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_02_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
           this.CreateExcel(html, "REPORT_02_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

      
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}