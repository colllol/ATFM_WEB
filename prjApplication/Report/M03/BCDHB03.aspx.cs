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

namespace prjApplication.Report.M03
{
    public partial class BCDHB03 : System.Web.UI.Page
    {
        public string _datekhoang = null;
        public string _datetime = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;

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
            _dt = _rpDAL.BCDHB03_Get_HHKVN(_dateFrom, _dateTo);
            string _html = null;
            DataRow _mrow = null;
            string _dateThangTruoc1 = "0"; string _dateThang = "0";

            DateTime _dtime = UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy");
            _dateThangTruoc1 = UltilFunc.ReturnMonthBefore(_dtime);
            _dateThang = String.Format("{0:MM/yyyy}", _dtime);


            string _valThangTruocQN1 = "0"; string _valThangTruocQN2 = "0";
            string _valThangTruocQT1 = "0"; string _valThangTruocQT2 = "0";


            if (_dt != null)
            {

                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];

                        _valThangTruocQN1 = _rpDAL.DHB03_TH_HHKVN_SumQN_Thang(_mrow["OPER_ID"].ToString(), _dateThang, _dateFrom, _dateTo);
                        _valThangTruocQN2 = _rpDAL.DHB03_TH_HHKVN_SumQN_ThangTruoc(_mrow["OPER_ID"].ToString(), _dateThangTruoc1);
                        _valThangTruocQT1 = _rpDAL.DHB03_TH_HHKVN_SumQT_Thang(_mrow["OPER_ID"].ToString(), _dateThang, _dateFrom, _dateTo);
                        _valThangTruocQT2 = _rpDAL.DHB03_TH_HHKVN_SumQT_ThangTruoc(_mrow["OPER_ID"].ToString(), _dateThangTruoc1);
                        _html += "<tr>";
                        _html += "<td class=\"tg -yw4l\"><b>" + _mrow["OPER_NAME"].ToString() + "</b></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "</tr>";

                        _html += "<tr>";
                        _html += "<td class=\"tg -yw4l\">-Quốc tế</td>";
                        _html += "<td class=\"tg -yw4l\">" + _valThangTruocQT1 + "</td>";
                        _html += "<td class=\"tg -yw4l\">" + _valThangTruocQT2 + "</td>";
                        _html += "<td class=\"tg -yw4l\">0</td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "</tr>";

                        _html += "<tr>";
                        _html += "<td class=\"tg -yw4l\">-Quốc Nội</td>";
                        _html += "<td class=\"tg -yw4l\">" + _valThangTruocQN1 + "</td>";
                        _html += "<td class=\"tg -yw4l\">" + _valThangTruocQN2 + "</td>";
                        _html += "<td class=\"tg -yw4l\">0</td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
                        _html += "<td class=\"tg -yw4l\"></td>";
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
            this.CreatePDF(html, "REPORT_03_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreateExcel(html, "REPORT_03_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }


        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}