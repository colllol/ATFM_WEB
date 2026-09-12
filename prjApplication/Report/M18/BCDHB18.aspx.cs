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

namespace prjApplication.Report.M18
{
    public partial class BCDHB18 : System.Web.UI.Page
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

        public string _slqnmb = null;
        public string _slqtmb = null;

        public string _slqnmt = null;
        public string _slqtmt = null;

        public string _slqnmn = null;
        public string _slqtmn = null;

        public string _sum1 = null;
        public string _sum2 = null;
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
                    _slqnmb = _rpDAL.DHB18_THSLB_SUM_QN_MB(_dateFrom.Trim(), _dateTo.Trim());
                    _slqtmb = _rpDAL.DHB18_THSLB_SUM_QT_MB(_dateFrom.Trim(), _dateTo.Trim());
                    _slqnmt = _rpDAL.DHB18_THSLB_SUM_QN_MT(_dateFrom.Trim(), _dateTo.Trim());
                    _slqtmt = _rpDAL.DHB18_THSLB_SUM_QT_MT(_dateFrom.Trim(), _dateTo.Trim());
                    _slqnmn = _rpDAL.DHB18_THSLB_SUM_QN_MN(_dateFrom.Trim(), _dateTo.Trim());
                    _slqtmn = _rpDAL.DHB18_THSLB_SUM_QT_MN(_dateFrom.Trim(), _dateTo.Trim());

                    _sum1 = (Int32.Parse(_slqnmb) + Int32.Parse(_slqnmt) + Int32.Parse(_slqnmn)).ToString();
                    _sum2 = (Int32.Parse(_slqtmb) + Int32.Parse(_slqtmt) + Int32.Parse(_slqtmn)).ToString();

                }
            }

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
            this.CreatePDF(html, "REPORT_18_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_18_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }


        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}