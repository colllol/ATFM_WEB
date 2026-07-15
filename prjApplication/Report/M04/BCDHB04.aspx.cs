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
using System.Data;


namespace prjApplication.Report.M04
{
    public partial class BCDHB04 : System.Web.UI.Page
    {
        public string _dateFrom = null;
        public string _dateTo = null;

        public string _datekhoang = null;
        public string _datetime = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        public string _content = null;

        public string _ThangNay = null;
        public string _ThangTruoc = null;
        public string _SoNgay1 = null;
        public string _SoNgay2 = null;
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
                    _content = GenDataTable();
                }
            }

        }
        private string GenDataTable()
        {
            ReportDHBDAL _rpDAL = new ReportDHBDAL();
            DataTable _dt = null;
            _dt = _rpDAL.DHB04_Get_DuongBay("");
            string _html = null;
            DataRow _mrow = null;

            string _dateThangNay = ""; string _dateThangTruoc = ""; string _valThang = ""; string _valThangTruoc = "";
            string _dayinMonth = "0";
            string _dayinMonthTruoc = "0";
            string _TongNgay1 = "0";
            string _TongNgay2 = "0";
            DateTime _dtime = UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy");
            _dateThangTruoc = UltilFunc.ReturnMonthBefore(_dtime);
            _dateThangNay = String.Format("{0:MM/yyyy}", _dtime);
            _ThangNay = _dateThangNay.ToString();
            _ThangTruoc = _dateThangTruoc.ToString();
            _dayinMonth = DateTime.DaysInMonth(_dtime.Year, _dtime.Month).ToString();

            DateTime _dtimeTruoc = Convert.ToDateTime(_dateThangTruoc);
            _dayinMonthTruoc = DateTime.DaysInMonth(_dtimeTruoc.Year, _dtimeTruoc.Month).ToString();
            _SoNgay1 = _dayinMonth.ToString();
            _SoNgay2 = _dayinMonthTruoc.ToString();


            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];

                        _valThang = _rpDAL.DHB04_TH_DuongBay_Sum_Thang(_mrow["ROUTE_NAME"].ToString(), _dateThangNay, _dateFrom, _dateTo);
                        _valThangTruoc = _rpDAL.DHB04_TH_DuongBay_Sum_ThangTruoc(_mrow["ROUTE_NAME"].ToString(), _dateThangTruoc);

                        _TongNgay1 = ReturnTotalsADate(_valThang, _dayinMonth);
                        _TongNgay2 = ReturnTotalsADate(_valThangTruoc, _dayinMonthTruoc);

                        _html += "<tr>";
                        _html += "<td class=\"tg-s6z2\">" + (j + 1) + "</td>";
                        _html += "<td class=\"tg-s6z2\">" + _mrow["ROUTE_NAME"].ToString() + "</td>";
                        _html += "<td class=\"tg-s6z2\">" + _valThang + "</td>";
                        _html += "<td class=\"tg-s6z2\">" + _TongNgay1 + "</td>";
                        _html += "<td class=\"tg-s6z2\">" + _valThangTruoc + "</td>";
                        _html += "<td class=\"tg-s6z2\">" + _TongNgay2 + "</td>";
                        _html += "<td class=\"tg-s6z2\">" + ReturnPhanTramADate(_TongNgay1, _TongNgay2) + "</td>";
                        _html += "</tr>";
                    }
                }

            }

            return _html;
        }
        public string ReturnTotalsADate(string _total, string _dayMonth)
        {
            string _return = "0";

            if (UltilFunc.IsNumeric(_total))
            {
                _return = (Convert.ToInt32(_total) / Convert.ToInt32(_dayMonth)).ToString();
            }
            return _return;

        }

        public string ReturnPhanTramADate(string _totalThangNay, string _totalThangTruoc)
        {
            string _return = "0";
            int _tong1 = 0; int _tong2 = 0;
            double _average = 0;
            if (UltilFunc.IsNumeric(_totalThangNay))
            {
                _tong1 = Convert.ToInt32(_totalThangNay) * 100;
                _tong2 = Convert.ToInt32(_totalThangTruoc);
                if (_tong2 > 0)
                    _average = Convert.ToDouble((_tong1 / _tong2).ToString());
                _return = Math.Round(_average, 2).ToString();
            }
            return _return + "%";

        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "REPORT_04_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_04_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }



        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}