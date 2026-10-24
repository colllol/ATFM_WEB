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
using System.Data;

namespace prjApplication.Report.M07
{
    public partial class BCDHB07 : System.Web.UI.Page
    {
        public string _dateFrom = null;
        public string _dateTo = null;

        public string _datekhoang = null;
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
                    ReportDHBDAL _rpDAL = new ReportDHBDAL();
                    DataTable dt = _rpDAL.BCDHB07(_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            string _html = null;
            DataRow _mrow = null;
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        _mrow = _dt.Rows[i];

                        _html += "<tr>";
                        _html += "<td class=\"tg-031e\">" + (i + 1) + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["flightdate"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["flightnbr"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["craft_type"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["registration"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["from_airp"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["to_airp"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["fpl_via"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["flightdate"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["flightdate"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["permnbr"].ToString() + "</td>";
                        _html += "</tr>";

                    }

                }
            }
            return _html;
        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_07_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_07_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }



        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }

}