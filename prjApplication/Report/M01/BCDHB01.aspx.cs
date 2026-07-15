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
using System.Globalization;

namespace prjApplication.Report.M01
{
    public partial class BCDHB01 : System.Web.UI.Page
    {
        public string _dateFrom = null;
        public string _dateTo = null;

        public string _dateky = null;        
        public string _datetime = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;

        public string _content = null;
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
            _dateky = "Từ " + _dateFrom.Trim() + " đến " + _dateTo.Trim();
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

            string _dateTuanTruoc = "";
            string _valTuanTruocLD = ""; string _valTuanTruocOF = ""; string _valTuanNayLD = ""; string _valTuanNayOF = "";
            string _valTongTuan1 = ""; string _valTongTuan2 = "";
            DateTime _dt = UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy");
            _dateTuanTruoc = _dt.AddDays(-7).ToString("dd'/'MM'/'yyyy");

            _valTuanTruocLD = _rpDAL.BCDHB01_TH_Tuan_TypeLD(_dateTuanTruoc, _dateFrom);

            _valTuanNayLD = _rpDAL.BCDHB01_TH_Tuan_TypeLD(_dateFrom, _dateTo);

            _valTuanTruocOF = _rpDAL.BCDHB01_TH_Tuan_TypeOF(_dateTuanTruoc, _dateFrom);

            _valTuanNayOF = _rpDAL.BCDHB01_TH_Tuan_TypeOF(_dateFrom, _dateTo);

            _valTongTuan1 = _rpDAL.GetDaThucHien_RP_Total(_valTuanTruocLD, _valTuanTruocOF);

            _valTongTuan2 = _rpDAL.GetDaThucHien_RP_Total(_valTuanNayLD, _valTuanNayOF);

            string _html = null;
            _html += "<tr>";
            _html += "<td class=\"tg-031e\">1. Bay đi đến<br>";
            _html += "</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTuanTruocLD + "</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTuanNayLD + "</td>";
            _html += "<td class=\"tg-s6z2\"></td>";
            _html += "</tr>";
            _html += "<tr>";
            _html += "<td class=\"tg-031e\">2. Bay quá cảnh<br>";
            _html += "</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTuanTruocOF + "</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTuanNayOF + "</td>";
            _html += "<td class=\"tg-s6z2\"></td>";
            _html += "</tr>";
            _html += "<tr>";
            _html += "<td class=\"tg -031e\">Tổng(1+2)<br>";
            _html += "</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTongTuan1 + "</td>";
            _html += "<td class=\"tg-s6z2\">" + _valTongTuan2 + "</td>";
            _html += "<td class=\"tg-s6z2\"></td>";
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
            this.CreatePDF(html, "REPORT_01_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
           
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
           this.CreateExcel(html, "REPORT_01_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

     
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }


}