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

namespace prjApplication.Report.M10
{
    public partial class BCDHB10 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _datetime = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        public string _content = null;

        public string _dateFrom = null;
        public string _dateTo = null;
        public string _operid = null;
        public string _sum = null;
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
                    
                    DataTable dt = _rpDAL.BCDHB10_Get_Oper_FplVia(_operid,_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _slchuyen = null;
            int _sumtong = 0;
            
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _slchuyen = _rpDAL.BCDHB10_Get_ThSlb_byOper(r["OPER_ID"].ToString(), r["fpl_via"].ToString(), _dateFrom, _dateTo);
                    if (_slchuyen == "-1")
                        _slchuyen = "0";
                   
                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\">" + (i + 1) + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["oper_id"].ToString() + "</td>";
                    kq += "<td class=\"tg-yw4l\">" + r["fpl_via"].ToString() + "</td>";
                    kq += "<td>"+ _slchuyen + "</td>";
                    kq += "<td>"+ _slchuyen + "</td>";
                    kq += "</tr>";

                    _sumtong += Convert.ToInt32(_slchuyen);
                    i++;
                }
                _sum = _sumtong.ToString();
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
            this.CreatePDF(html, "REPORT_10_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_10_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

    
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}