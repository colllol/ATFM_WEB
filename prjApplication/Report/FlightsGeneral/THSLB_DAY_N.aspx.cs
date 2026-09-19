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
    public partial class THSLB_DAY_N : System.Web.UI.Page
    {
        public string _dateFrom = null;
        public string _dateTo = null;
        public string _content = null;
        public string _TongNgay = null;
        public string _SumFirHn = null;
        public string _SumFirHcm = null;
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
                    lbldateFrom.Text = "FromDate " + _dateFrom.Trim();
                    lbldateTo.Text = "ToDate" + _dateTo.Trim();
                    lblcontent.Text = LoadData();
                    LoadData();

                }
            }
        }
        #region funciton
        protected string LoadData()
        {

            if (UltilFunc.checkDate(_dateFrom, _dateTo))
            {
                if (UltilFunc.ToDate(_dateFrom.Trim(), "dd/MM/yyyy") > UltilFunc.ToDate(_dateTo.Trim(), "dd/MM/yyyy"))
                {
                    this.AlertMessage("Ngày bắt đầu ko được lớn hơn ngày kết thúc!");
                    return "";
                }
                else
                {
                    return GenDataTable();
                }
            }
            return "";
        }
        #endregion
        #region MyRegion
        private string GenDataTable()
        {

            clsResuftAPI _api = new clsResuftAPI();
            DataTable _dt = null;
            _dt = _rpStaticDAL.THSLB_Get_HHKVN(_dateFrom,_dateTo);
           // var ax = _rpStaticDAL.SumTest ("HVN","01/03/2017");
            string _html = null;
            DataRow _mrow = null;
            string _slpurDate = null;
            string _sumPurQt = null;
            string _slpurOper = null;
            string _slQuocnoi = null;
            string _slQuocte = null;
            string _slTong = null;
            string _slFIR = null;

            //double _slSumAll = 0;    
            double _slTongNgay = 0;
            double _slSumFir = 0;
            if (_dt != null)
            {

                if (_dt.Rows.Count > 0)
                {
                    
                    _slpurDate = _rpStaticDAL.THSLB_PUR_QT(_dateFrom,_dateTo);
                    _sumPurQt = _rpStaticDAL.THSLB_SUM_PUR_QT(_dateFrom,_dateTo);

                    _html += "<tr>";
                    _html += "<td class=\"tg-baqh\" rowspan=\""+ (_dt.Rows.Count +1) + "\" style=\"text-align:center\">"+ _dateFrom + "</td>";
                    _html += "<td class=\"tg-yw4l\" colspan=\"5\" style=\"text-align:center\">QUỐC TẾ</td>";
                    _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slpurDate + "</td>";
                    _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _sumPurQt + "</td>";
                    _html += "</tr>";

                    _slTongNgay = Convert.ToInt32(_sumPurQt);

                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];
                        _slQuocnoi = _rpStaticDAL.SUM_QN_BY_HHK(_mrow["OPER_ID"].ToString(), _dateFrom,_dateTo);
                         _slQuocte = _rpStaticDAL.SUM_QT_BY_HHK(_mrow["OPER_ID"].ToString(), _dateFrom,_dateTo);

                        _slTong = (Convert.ToInt32(_slQuocnoi) + Convert.ToInt32(_slQuocte)).ToString();


                        lblSumFirHn.Text = _SumFirHn = _rpStaticDAL.SumFirHn(_dateFrom,_dateTo);
                        lblSumFirHcm.Text = _SumFirHcm = _rpStaticDAL.SumFirHcm(_dateFrom, _dateTo);

                        _slFIR = (Convert.ToInt32(_SumFirHn) + Convert.ToInt32(_SumFirHcm)).ToString();

                        _slSumFir += Convert.ToDouble(_slFIR);

                        _slTongNgay += Convert.ToDouble(_slTong);                        

                        _slpurOper = _rpStaticDAL.SUM_PUR_QN_BY_OPERDATE(_mrow["OPER_ID"].ToString(), _dateFrom,_dateTo);
                        _html += "<tr>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _mrow["OPER_ID"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slQuocnoi + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slQuocte + "</td>";
                        _html += "<td class=\"tg-yw4l\"></td>";
                        _html += "<td class=\"tg-yw4l\"></td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slpurOper + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slTong + "</td>";                        
                        _html += "</tr>";
                        
                    }
                    _TongNgay = _slTongNgay.ToString();
                    _SumFir = _slFIR.ToString();
                    _SumAll = (Convert.ToInt32(_slTongNgay) + Convert.ToInt32(_slFIR)).ToString();

                    lblTongNgay.Text = _TongNgay;
                    lblSumFir.Text = _SumFir;
                    lblSumAll.Text = _SumAll;
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
            this.CreatePDF(html, "THSLB_DAY_N_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            this.CreateExcel(html, "THSLB_DAY_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        #endregion
    }
}