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

namespace prjApplication.Report.M24
{
    public partial class BB7 : System.Web.UI.Page
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
        public string _operid = null;
        public string _slbQN1 = null;public string _slbQT1 = null;public string _slbQTV1 = null;
        public string _slbQN2 = null; public string _slbQT2 = null; public string _slbQTV2 = null;

        public string _slbQNMT1 = null; public string _slbQTMT1 = null; public string _slbQTVMT1 = null;
        public string _slbQNMT2 = null; public string _slbQTMT2 = null; public string _slbQTVMT2 = null;

        public string _slbQNMN1 = null; public string _slbQTMN1 = null; public string _slbQTVMN1 = null;
        public string _slbQNMN2 = null; public string _slbQTMN2 = null; public string _slbQTVMN2 = null;

        public string _stongQNMB1 = null; public string _stongQTMB1 = null; public string _stongQTVMB1 = null;
        public string _stongQNMT2 = null; public string _stongQTMT2 = null; public string _stongQTVMT2 = null;
        public string _stongQNMN3 = null; public string _stongQTMN3 = null; public string _stongQTVMN3 = null;
        public string _stongQN123 = null; public string _stongQT123 = null; public string _stongQTV123 = null;

        public string _total = null;

        ReportDHBDAL _rpDAL = new ReportDHBDAL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["FromDate"] != null && Request["FromDate"].ToString() != "" && Request["FromDate"].ToString() != String.Empty && Request["ToDate"] != null && Request["ToDate"].ToString() != "" && Request["ToDate"].ToString() != String.Empty)
            {
                _dateFrom = Request["FromDate"].ToString();
                _dateTo = Request["ToDate"].ToString();
                _operid = Request["Oper"].ToString();

                int _QNMB1 = 0; int _QTMB1 = 0; int _QTVMB1 = 0;
                int _QNMT2 = 0; int _QTMT2 = 0; int _QTVMT2 = 0;
                int _QNMN3 = 0; int _QTMN3 = 0; int _QTVMN3 = 0;
                if (!IsPostBack)
                {
                    WriteInfoTime();
                    //Mien bac
                    _slbQN1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QN(_operid, "VVCI", "", _dateFrom, _dateTo);
                    _slbQT1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QT(_operid, "VVCI", "", _dateFrom, _dateTo);
                    _slbQTV1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QTV(_operid, "VVCI", "", _dateFrom, _dateTo);
                    _slbQN2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QN(_operid, "VVNB", "", _dateFrom, _dateTo);
                    _slbQT2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QT(_operid, "VVNB", "", _dateFrom, _dateTo);
                    _slbQTV2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QTV(_operid, "VVNB", "", _dateFrom, _dateTo);

                    _QNMB1 = Int32.Parse(_slbQN1)+ Int32.Parse(_slbQN2);
                    _QTMB1 = Int32.Parse(_slbQT1) + Int32.Parse(_slbQT2);
                    _QTVMB1 = Int32.Parse(_slbQTV1) + Int32.Parse(_slbQTV2);

                    //Mien trung
                    _slbQNMT1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QN(_operid, "VVCA", "", _dateFrom, _dateTo);
                    _slbQTMT1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QT(_operid, "VVCA", "", _dateFrom, _dateTo);
                    _slbQTVMT1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QTV(_operid, "VVCA", "", _dateFrom, _dateTo);
                    _slbQNMT2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QN(_operid, "VVDN", "", _dateFrom, _dateTo);
                    _slbQTMT2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QT(_operid, "VVDN", "", _dateFrom, _dateTo);
                    _slbQTVMT2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QTV(_operid, "VVDN", "", _dateFrom, _dateTo);

                    _QNMT2 = Int32.Parse(_slbQNMT1) + Int32.Parse(_slbQNMT2);
                    _QTMT2 = Int32.Parse(_slbQTMT1) + Int32.Parse(_slbQTMT2);
                    _QTVMT2 = Int32.Parse(_slbQTVMT1) + Int32.Parse(_slbQTVMT2);

                    //Mien nam
                    _slbQNMN1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QN(_operid, "VVBM", "", _dateFrom, _dateTo);
                    _slbQTMN1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QT(_operid, "VVBM", "", _dateFrom, _dateTo);
                    _slbQTVMN1 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QTV(_operid, "VVBM", "", _dateFrom, _dateTo);
                    _slbQNMN2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QN(_operid, "VVTS", "", _dateFrom, _dateTo);
                    _slbQTMN2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QT(_operid, "VVTS", "", _dateFrom, _dateTo);
                    _slbQTVMN2 = _rpDAL.DHB24_Fn_THSLB_BY_OPER_QTV(_operid, "VVTS", "", _dateFrom, _dateTo);

                    _QNMN3 = Int32.Parse(_slbQNMN1) + Int32.Parse(_slbQNMN2);
                    _QTMN3 = Int32.Parse(_slbQTMN1) + Int32.Parse(_slbQTMN2);
                    _QTVMN3 = Int32.Parse(_slbQTVMN1) + Int32.Parse(_slbQTVMN2);

                }
                _stongQNMB1 = _QNMB1.ToString(); _stongQTMB1 = _QTMB1.ToString(); _stongQTVMB1 = _QTVMB1.ToString();
                _stongQNMT2 = _QNMT2.ToString(); _stongQTMT2 = _QTMT2.ToString(); _stongQTVMT2 = _QTVMT2.ToString();
                _stongQNMN3 = _QNMN3.ToString(); _stongQTMN3 = _QTMN3.ToString(); _stongQTVMN3 = _QTVMN3.ToString();

                _stongQN123 = (_QNMB1 + _QNMT2 + _QNMN3).ToString();
                _stongQT123 = (_QTMB1 + _QTMT2 + _QTMN3).ToString();
                _stongQTV123 = (_QTVMB1 + _QTVMT2 + _QTVMN3).ToString();
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

        #region export

        protected void btnPDF_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
            this.CreatePDF(html, "BB_7_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            string html = sw.ToString();
           this.CreateExcel(html, "BB_7_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}