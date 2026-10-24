using System;
using System.Web.UI;
using prjBusinessLogic;
using System.IO;
using System.Data;
using System.Linq;

namespace prjApplication.Report.FlightsStatistic
{
    public partial class TKSLCBAYYDD_5 : System.Web.UI.Page
    {

        public string _content = null;
        public string _contentQt = null;
        public string _dateFrom = null;
        public string _dateTo = null;

        ReportStatisticDAL _rpDAL = new ReportStatisticDAL();
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
                    lbldatekhoang.Text = "FromDate" + _dateFrom.Trim() + "ToDate" + _dateTo.Trim();
                    lblcontent.Text = LoadData();
                    lblcontentQt.Text = LoadDataQt();


                }
            }
        }
        
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
                    return GenDataTable();
                }
            }
            return "";
        }
        protected string LoadDataQt()
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
                    return GenDataTableQt();
                }
            }
            return "";
        }
        private string GenDataTable()
        {
            DataTable _dt = null;
            _dt = _rpDAL.THSLB_Get_HHKVN(_dateFrom,_dateTo);
            string _html = null;
            DataRow _mrow = null;
            string _slQuocnoi = null;
            string _slQuocte = null;
            string _slTong = null;
            double _slTongNgay = 0;
            double _SumQuocNoi = 0;
            double _SumQuocte = 0;
            if (_dt != null)
            {

                if (_dt.Rows.Count > 0)
                {                                      
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];
                        _slQuocnoi = _rpDAL.SUM_QN_BY_HHK(_mrow["OPER_ID"].ToString(), _dateFrom,_dateTo);
                        _slQuocte = _rpDAL.SUM_QT_BY_HHK(_mrow["OPER_ID"].ToString(), _dateFrom,_dateTo);

                        _slTong = (Convert.ToInt32(_slQuocnoi) + Convert.ToInt32(_slQuocte)).ToString();

                        _slTongNgay += Convert.ToDouble(_slTong);
                        _SumQuocNoi = _SumQuocNoi + Convert.ToDouble(_slQuocnoi);
                        _SumQuocte = _SumQuocte + Convert.ToDouble(_slQuocte);

                        _html += "<tr>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + (j+1).ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _mrow["OPER_ID"].ToString() + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slQuocnoi + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slQuocte + "</td>";
                        _html += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _slTong + "</td>";
                        _html += "</tr>";
                    }
                    lblSumQuocNoi.Text = _SumQuocNoi.ToString();
                    lblSumQuocTe.Text = _SumQuocte.ToString();
                    lblSumAmount.Text = _slTongNgay.ToString();
                }
            }            
            return _html;
        }

        private string GenDataTableQt()
        {
            DataTable _dt1 = null;
            _dt1 = _rpDAL.THSLB_GET_HHKQT(_dateFrom,_dateTo);
            string _htmlQt = null;

            double _SumAmountQt = 0;
            DataRow _mrowQt = null;

            if (_dt1 != null)
            {
                if (_dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt1.Rows.Count; i++)
                    {
                        _mrowQt = _dt1.Rows[i];

                        _SumAmountQt += Convert.ToInt32(_mrowQt["AMOUNT"].ToString());

                        _htmlQt += "<tr>";
                        _htmlQt += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + (i + 1).ToString() + "</td>";
                        _htmlQt += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _mrowQt["OPER_ID"].ToString() + "</td>";
                        _htmlQt += "<td></td>";
                        _htmlQt += "<td></td>";
                        _htmlQt += "<td class=\"tg-yw4l\" style=\"text-align:center\">" + _mrowQt["AMOUNT"].ToString() + "</td>";
                        _htmlQt += "</tr>";
                    }
                    lblSumAmountQt.Text = _SumAmountQt.ToString();
                }
            }
            return _htmlQt;
        }

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            this.CreatePDF(html, "TKSLCBAYYDD_5_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string html = this.RenderToHTML(divContent);
            this.CreateExcel(html, "TKSLCBAYYDD_5_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }
        #endregion
    }
}