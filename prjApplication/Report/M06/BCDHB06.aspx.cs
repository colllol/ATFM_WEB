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

namespace prjApplication.Report.M06
{
    public partial class BCDHB06 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        public string _datetime = null;
        public string _content = null;

        public string _dateFrom = null;
        public string _dateTo = null;

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
                    DataTable dt = _rpDAL.BCDHB06_Get_SLB_ByTimes(_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    
                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\">" + (i + 1) + "</td>";
                    kq += "<td class=\"tg-031e\">" + r["oper_id"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["PERMNBR"].ToString() + "</td>";                    
                    kq += "<td class=\"tg-yw4l\">" + r["MA"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["PURPOSE"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["FROM_AIRP"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["TO_AIRP"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["FLIGHTDATE"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["FPL_VIA"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["ATD"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["ATA"].ToString() + "</td>";                    
                    kq += "</tr>";

                    
                    i++;

                }
               
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
            this.CreatePDF(html, "REPORT_06_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_06_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

     
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}