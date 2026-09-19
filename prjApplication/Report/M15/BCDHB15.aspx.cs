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


namespace prjApplication.Report.M15
{
    public partial class BCDHB15 : System.Web.UI.Page
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

        public string _operid = null;

        public string _totalhcm = null;

        public string _sctotal = "0";
        public string _sctotalqn = "0";
        public string _sctotalqt = "0";


        public string _totalSc = null;
        public string _totalNo = null;
        ReportDHBDAL _rpDAL = new ReportDHBDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FromDate"] != null && Request["FromDate"].ToString() != "" && Request["FromDate"].ToString() != String.Empty && Request["ToDate"] != null && Request["ToDate"].ToString() != "" && Request["ToDate"].ToString() != String.Empty)
            {
                _dateFrom = Request["FromDate"].ToString();
                _dateTo = Request["ToDate"].ToString();
                _operid = Request["Oper"].ToString();
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
                    DataTable dt = _rpDAL.BCDHB15_GET_FROMAIR_BY_HHK(_operid,_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _scqn = "0";
            string _scqt = "0";

           

            int _scqnint = 0;
            int _scqtint = 0;

            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _scqn = _rpDAL.BCDHB15_GET_SLBQN_BY_HHK_SB(_operid, r["FROM_AIRP"].ToString().Trim(), _dateFrom.Trim(), _dateTo.Trim());
                    _scqt = _rpDAL.BCDHB15_GET_SLBQT_BY_HHK_SB(_operid, r["FROM_AIRP"].ToString().Trim(), _dateFrom.Trim(), _dateTo.Trim());

                    _scqnint += Int32.Parse(_scqn);
                    _scqtint += Int32.Parse(_scqt);

                    _sctotal = (Int32.Parse(_scqn) + Int32.Parse(_scqt)).ToString();

                    kq += "<tr>";                    
                    kq += "<td class=\"tg-s6z2\">" + r["FROM_AIRP"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">"+ _scqn + "</td>";
                    kq += "<td class=\"tg-yw4l\">"+ _scqt + "</td>";
                    kq += "<td class=\"tg-yw4l\">"+ _sctotal + "</td>";                    
                    kq += "</tr>";


                    i++;

                }
                _sctotalqn = _scqnint.ToString();
                _sctotalqt = _scqtint.ToString();

                _sctotal = (_scqnint + _scqtint).ToString();

            }
            return kq;
        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_15_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
           this.CreateExcel(html, "REPORT_15_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

       

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}