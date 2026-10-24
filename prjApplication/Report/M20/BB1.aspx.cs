using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using TuesPechkin;
using System.Data;
using System.IO;

namespace prjApplication.Report.M20
{
    public partial class BB1 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        public string _datetime = null;
        public string _content = null;
        public string _content1 = null;
        public string _dateFrom = null;
        public string _dateTo = null;

        public string _total = null;

        ReportDHBDAL _rpDAL = new ReportDHBDAL();
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
                    DataTable dt = _rpDAL.DHB20_Get_MONTH_FLIGHTDATE(_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            int _sum = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    string _slFirHN = _rpDAL.DHB20_Fn_THSLB_FIRHN(r["MONTH"].ToString(), _dateFrom.Trim(), _dateTo.Trim());
                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\">" + r["MONTH"].ToString() + "</td>";
                    kq += "<td class=\"tg-031e\">"+ _slFirHN + "</td>";
                    kq += "<td class=\"tg-031e\">0</td>";
                    kq += "<td class=\"tg-031e\">0</td>";
                    kq += "<td class=\"tg-031e\">0</td>";
                    kq += "<td class=\"tg-031e\">0</td>";
                    kq += "<td class=\"tg-031e\"></td>";                    
                    kq += "</tr>";

                    _sum += Int32.Parse(_slFirHN);

                    i++;

                }
                _total = _sum.ToString();
            }
            return kq;
        }
        public void WriteInfoTime()
        {
            _date = DateTime.Now.Day.ToString();
            _month = DateTime.Now.Month.ToString();
            _year = DateTime.Now.Year.ToString();
            _datetime = DateTime.Now.ToString();
            _datekhoang = "Từ " + _dateFrom.Trim() + " đến " + _dateTo.Trim();
        }
        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "BB1_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "BB1_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

      
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}