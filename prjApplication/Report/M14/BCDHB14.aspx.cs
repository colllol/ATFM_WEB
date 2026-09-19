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
using System.Data;
using TuesPechkin;

namespace prjApplication.Report.M14
{
    public partial class BCDHB14 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _datetime = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        public string _content = null;

        public string _dateFrom = null;
        public string _dateTo = null;
        public string _total = null;

        public string _totalhn = null;
        public string _totalhcm = null;

        public string _totalfhn = null;
        public string _totalfhcm = null;

        public string _totalfhnNo = null;
        public string _totalfhcmNo = null;


        public string _totalSc = null;
        public string _totalNo = null;

        

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

        public void WriteInfoTime()
        {
            _date = DateTime.Now.Day.ToString();
            _month = DateTime.Now.Month.ToString();
            _year = DateTime.Now.Year.ToString();
            _datetime = DateTime.Now.ToString();
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
                    _totalfhn = _rpDAL.SumFirHn(_dateFrom.Trim(), _dateTo.Trim());
                    _totalfhcm = _rpDAL.SumFirHcm(_dateFrom.Trim(), _dateTo.Trim());

                    _totalSc = (Int32.Parse(_totalfhn) + Int32.Parse(_totalfhcm)).ToString();

                    _totalfhnNo = _rpDAL.SumFirHnByNo(_dateFrom.Trim(), _dateTo.Trim());
                    _totalfhcmNo = _rpDAL.SumFirHcmByNo(_dateFrom.Trim(), _dateTo.Trim());

                    _totalNo = (Int32.Parse(_totalfhnNo) + Int32.Parse(_totalfhcmNo)).ToString();

                    _totalhn = (Int32.Parse(_totalfhn) + Int32.Parse(_totalfhnNo)).ToString();

                    _totalhcm = (Int32.Parse(_totalfhcm) + Int32.Parse(_totalfhcmNo)).ToString();

                    _total = (Int32.Parse(_totalSc) + Int32.Parse(_totalNo)).ToString();
                }
            }

        }

        
        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_14_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_14_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

     

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}