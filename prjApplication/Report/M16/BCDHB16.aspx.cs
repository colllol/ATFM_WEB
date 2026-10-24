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

namespace prjApplication.Report.M16
{
    public partial class BCDHB16 : System.Web.UI.Page
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
                    DataTable dt = _rpDAL.DHB16_Get_HHKVN_BAYQN(_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);

                    DataTable dt1 = _rpDAL.DHB16_Get_HHKVN_BAYQT(_dateFrom.Trim(), _dateTo.Trim());
                    _content1 = GenDataTable1(dt1);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string slb = "0";
            int _tinhsc = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    slb = _rpDAL.DHB16_THSLB_SUM_QN(r["oper_id"].ToString(), _dateFrom.Trim(), _dateTo.Trim());

                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\">" + (i + 1) + "</td>";
                    kq += "<td class=\"tg-031e\">" + r["oper_id"].ToString() + "</td>";
                    kq += "<td class=\"tg-031e\">" + slb + "</td>";
                    kq += "<td class=\"tg-031e\"></td>";
                    kq += "</tr>";

                    _tinhsc += Int32.Parse(slb);

                    i++;

                }
                _sum1 = _tinhsc.ToString();
            }
            return kq;
        }

        private string GenDataTable1(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            int _tinhsc1 = 0;
            string slb = "0";
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    slb = _rpDAL.DHB16_THSLB_SUM_QT(r["oper_id"].ToString(), _dateFrom.Trim(), _dateTo.Trim());
                    kq += "<tr>";
                    kq += "<td class=\"tg-031e\">" + (i + 1) + "</td>";
                    kq += "<td class=\"tg-031e\">" + r["oper_id"].ToString() + "</td>";
                    kq += "<td class=\"tg-031e\">" + slb + "</td>";
                    kq += "<td class=\"tg-031e\"></td>";
                    kq += "</tr>";

                    _tinhsc1 += Int32.Parse(slb);

                    i++;

                }
                _sum2 = _tinhsc1.ToString();
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
            this.CreatePDF(html, "REPORT_16_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_16_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

      
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}