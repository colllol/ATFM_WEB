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

namespace prjApplication.Report.FlightsGeneral
{
    public partial class THSLB_Month_N : System.Web.UI.Page
    {
        public string _dateFrom = null;
        public string _dateTo = null;
        public string _content = null;
        public string _TongNgay = null;
        public string _SumFirHnMonth = null;
        public string _SumFirHcmMonth = null;
        public string _SumFir = null;
        public string _SumAll = null;

        ReportStatisticDAL _rpStaticDAL = new ReportStatisticDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FromDate"] != null && Request["FromDate"].ToString() != "" && Request["FromDate"].ToString() != String.Empty && Request["ToDate"] != null && Request["ToDate"].ToString() != "" && Request["ToDate"].ToString() != String.Empty)
            {
                _dateFrom = Request["FromDate"].ToString();
                _dateTo = Request["ToDate"].ToString();
                if (!IsPostBack)
                {

                    LoadData();

                }
            }
        }

        #region funciton
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
        #endregion
        #region MyRegion
        private string GenDataTable()
        {

            clsResuftAPI _api = new clsResuftAPI();
            DataTable _dt = null;
            _dt = _rpStaticDAL.THSLB_Get_HHKVN(_dateFrom,_dateTo);
            string _html = null;
            DataRow _mrow = null;

            string _dateThang = "0";
            DateTime _dtime = UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy");
            _dateThang = String.Format("{0:MM/yyyy}", _dtime);

            string _slpurDate = null;
            string _sumPurQt = null;
            string _slpurOper = null;
            string _slQuocnoi = null;
            string _slQuocte = null;
            string _slTong = null;
            string _slFIR = null;
   
            double _slTongNgay = 0;
            double _slSumFir = 0;
            if (_dt != null)
            {

                if (_dt.Rows.Count > 0)
                {

                    _slpurDate = _rpStaticDAL.Thslb_Pur_Qt_Month(_dateThang);
                    _sumPurQt = _rpStaticDAL.Thslb_Sum_Pur_Qt_Month(_dateThang);

                    _html += "<tr>";
                    _html += "<td class=\"tg-baqh\" rowspan=\"" + (_dt.Rows.Count + 1) + "\">" + _dateThang + "</td>";
                    _html += "<td class=\"tg-yw4l\" colspan=\"5\">QUỐC TẾ</td>";
                    _html += "<td class=\"tg-yw4l\">" + _slpurDate + "</td>";
                    _html += "<td class=\"tg-yw4l\">"+ _sumPurQt + "</td>";
                    _html += "</tr>";
                    _slTongNgay = Convert.ToInt32(_sumPurQt);

                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];
                        _slQuocnoi = _rpStaticDAL.Thslb_Sum_Qn_Month(_mrow["OPER_ID"].ToString(), _dateThang);
                        _slQuocte = _rpStaticDAL.Thslb_Sum_Qt_Month(_mrow["OPER_ID"].ToString(), _dateThang);

                        _slTong = (Convert.ToInt32(_slQuocnoi) + Convert.ToInt32(_slQuocte)).ToString();


                        _SumFirHnMonth = _rpStaticDAL.SumFirHnMonth(_dateThang);
                        _SumFirHcmMonth = _rpStaticDAL.SumFirHcmMonth(_dateThang);

                        _slFIR = (Convert.ToInt32(_SumFirHnMonth) + Convert.ToInt32(_SumFirHcmMonth)).ToString();

                        _slSumFir += Convert.ToDouble(_slFIR);

                        _slTongNgay += Convert.ToDouble(_slTong);

                        _slpurOper = _rpStaticDAL.Thslb_Sum_Pur_Vn_Month(_mrow["OPER_ID"].ToString(), _dateThang);
                        _html += "<tr>";
                        _html += "<td class=\"tg-yw4l\">" + _mrow["OPER_ID"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _slQuocnoi + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _slQuocte + "</td>";
                        _html += "<td class=\"tg-yw4l\"></td>";
                        _html += "<td class=\"tg-yw4l\"></td>";
                        _html += "<td class=\"tg-yw4l\">" + _slpurOper + "</td>";
                        _html += "<td class=\"tg-yw4l\">" + _slTong + "</td>";
                        _html += "</tr>";

                    }
                    _TongNgay = _slTongNgay.ToString();
                    _SumFir = _slFIR.ToString();
                    _SumAll = (Convert.ToInt32(_slTongNgay) + Convert.ToInt32(_slFIR)).ToString();
                }
            }
            return _html;
        }
        #endregion
        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "THSLB_Month_N_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            this.CreateExcel(html, "THSLB_Month_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        #endregion
    }
}