using System;
using System.Web.UI;
using prjBusinessLogic;
using System.IO;
using System.Data;
using System.Linq;


namespace prjApplication.Report.OverFlights
{
    public partial class ThongKe12 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _content = null;
        public string _dateFrom = null;
        public string _dateTo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FromDate"] != null && Request["FromDate"].ToString() != "" && Request["FromDate"].ToString() != String.Empty && Request["ToDate"] != null && Request["ToDate"].ToString() != "" && Request["ToDate"].ToString() != String.Empty)
            {
                _dateFrom = Request["FromDate"].ToString();
                _dateTo = Request["ToDate"].ToString();
                if (!IsPostBack)
                {
                    //WriteInfoTime();
                    LoadData();
                    lbldatekhoang.Text = "FromDate " + _dateFrom.Trim() + " ToDate " + _dateTo.Trim();
                    lblcontent.Text = LoadData();
                }
            }
        }

        //public void WriteInfoTime()
        //{
        //    _datekhoang = "Từ " + _dateFrom.Trim() + " đến " + _dateTo.Trim();
        //}
        protected string LoadData()
        {

            if (UltilFunc.checkDate(_dateFrom, _dateTo))
            {
                if (UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy") > UltilFunc.ToDate(_dateTo.Trim(), "dd/MM/yyyy"))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert ('Ngày bắt đầu ko được lớn hơn ngày kết thúc!')", true);
                    return "";
                }
                else
                {
                    ReportStatisticDAL _rpDAL = new ReportStatisticDAL();
                    DataTable dt = _rpDAL.ThongKe12(_dateFrom.Trim(), _dateTo.Trim());
                    return GenDataTable(dt);
                }
            }
            return "";
        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 1;
            string _html = "";
            DataRow _mrow = null;
            if (_dt != null)
            {
                string _a = "";
                string _b = "";

                double _sl = 0;
                for (int j = 0; j < _dt.Rows.Count; j++)
                {
                    _mrow = _dt.Rows[j];
                    _a = _mrow["FLIGHTDATE"].ToString();
                    if (_a != _b)
                    {
                        if (j == 0)
                        {
                            _html += "<tr>";
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + i + "</td>";
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["FLIGHTDATE"].ToString() + "</td>";
                            _b = _mrow["FLIGHTDATE"].ToString();
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["FPL_VIA"].ToString() + "</td>";
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["AMOUNT"].ToString() + "</td>";
                            _sl = Convert.ToDouble(_mrow["AMOUNT"].ToString());
                            _html += "</tr>";
                        }
                        else
                        {
                            _html += "<tr>";
                            _html += "<td class=\"tg -031e\" colspan=\"3\" style=\"text-align:center; \"><b>Sum Of Type" + " " + " " + _b.ToString() + "</b></td>";
                            _html += "<td class=\"tg -031e\"><b>" + _sl + "</b></td>";
                            _html += "</tr>";
                            _sl = 0;
                            _html += "<tr>";
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + (i + 1) + "</td>";
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["FLIGHTDATE"].ToString() + "</td>";
                            _b = _mrow["FLIGHTDATE"].ToString();
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["FPL_VIA"].ToString() + "</td>";
                            _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["AMOUNT"].ToString() + "</td>";
                            _sl += Convert.ToDouble(_mrow["AMOUNT"].ToString());
                            _html += "</tr>";
                            i++;
                        }
                    }
                    else
                    {
                        _html += "<tr>";
                        _html += "<td class=\"tg-yw4l\"></td>";
                        _html += "<td class=\"tg-yw4l\"></td>";
                        _b = _mrow["FLIGHTDATE"].ToString();
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["FPL_VIA"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center; \">" + _mrow["AMOUNT"].ToString() + "</td>";
                        _sl +=
                        Convert.ToDouble(_mrow["AMOUNT"].ToString());
                        _html += "</tr>";

                        if (j == _dt.Rows.Count - 1)
                        {
                            _html += "<tr>";
                            _html += "<td class=\"tg -031e\" colspan=\"3\" style=\"text-align:center; \"><b>Sum Of Type" + " " + " " + _b.ToString() + "</b></td>";
                            _html += "<td class=\"tg -031e\"><b>" + _sl + "</b></td>";
                            _html += "</tr>";
                        }
                    }
                }
            }
            return _html;
        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "ThongKe12" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            this.CreateExcel(html, "ThongKe12" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        #endregion
    }
}