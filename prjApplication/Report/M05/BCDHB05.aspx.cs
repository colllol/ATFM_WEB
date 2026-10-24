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

namespace prjApplication.Report.M05
{
    public partial class BCDHB05 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        public string _datetime = null;
        public string _content1 = null;
        public string _content2 = null;
        public string _dateFrom = null;
        public string _dateTo = null;

        public string _dateNow = null;

        public string _firHN = null;
        public string _firHCM = null;

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
                    
                    DataTable dt1 = _rpDAL.BCDHB05_GET_OPER_VN(_dateFrom.Trim());
                    _content1 = GenDataTable(dt1,1);
                    DataTable dt2 = _rpDAL.BCDHB05_GET_OPER_QT(_dateFrom.Trim());
                    _content2 = GenDataTable(dt2,2);

                    _firHN = _rpDAL.BCDHB05_GET_SLB_FIRHN(_dateFrom.Trim());
                    _firHCM = _rpDAL.BCDHB05_GET_SLB_FIRHCM(_dateFrom.Trim());
                }
            }
        }

        private string GenDataTable(DataTable _dt,int type)
        {
            int i = 0;
            string kq = "";
            string _slbVN = "0";
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    if(type==1)
                        _slbVN = _rpDAL.BCDHB05_GET_SLB_BY_HHKVN(r["oper_id"].ToString(), _dateFrom.Trim());
                    else
                        _slbVN = _rpDAL.BCDHB05_GET_SLB_BY_HHKQT(r["oper_id"].ToString(), _dateFrom.Trim());
                    if (_slbVN == "-1")
                        _slbVN = "0";
                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\">" + r["OPER_NAME"].ToString() + "</td>";
                    kq += "<td class=\"tg-031e\">" + _slbVN.ToString() + "</td>";                   
                    kq += "</tr>";
                    i++;

                }

            }
            return kq;
        }

        public void WriteInfoTime()
        {
            _date = DateTime.Now.Day.ToString();
            _month = DateTime.Now.Month.ToString();
            _year = DateTime.Now.Year.ToString();
            _datetime = DateTime.Now.ToString();
            _dateNow = _date + "/" + _month + "/" + _year;
            
            _datekhoang = "Từ " + _dateFrom.Trim() + " đến " + _dateTo.Trim();
        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_05_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_05_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

      
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}