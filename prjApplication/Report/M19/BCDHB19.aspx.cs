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

namespace prjApplication.Report.M19
{
    public partial class BCDHB19 : System.Web.UI.Page
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
        public string _tong01 = null; public string _tong02 = null; public string _tong03 = null; public string _tong04 = null; public string _tong05 = null; public string _tong06 = null; public string _tong07 = null; public string _tong08 = null; public string _tong09 = null; public string _tong10 = null; public string _tong11 = null; public string _tong12 = null; public string _tong13 = null; public string _tong14 = null; public string _tong15 = null; public string _tong16 = null;
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
                    DataTable dt = _rpDAL.DHB19_Get_FLIGHTDATE(_dateFrom.Trim(), _dateTo.Trim());
                    _content = GenDataTable(dt);
                }
            }

        }
        private string GenDataTable(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0";string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0";string _sldicb = "0";string _sldencb = "0";string _sldivinh = "0";
            string _sldenvinh = "0";string _sldidh = "0";string _sldendh = "0";string _slditx = "0";string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";
            string _sldi = "0";string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd/MM/yyyy");
                    _sldinb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVNB", _date);
                    _sldennb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVNB", _date);

                    _sldidb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVDB", _date);
                    _sldendb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVDB", _date);

                    _sldicb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVCI", _date);
                    _sldencb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVCI", _date);

                    _sldivinh = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVVH", _date);
                    _sldenvinh = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVVH", _date);

                    _sldidh = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVDH", _date);
                    _sldendh = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVDH", _date);

                    _slditx = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVTX", _date);
                    _sldentx = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVTX", _date);

                    _sldivd = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVVD", _date);
                    _sldenvd = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVVD", _date);

                    _sldi = (Int32.Parse(_sldinb) + Int32.Parse(_sldidb) + Int32.Parse(_sldicb) + Int32.Parse(_sldivinh) + Int32.Parse(_sldidh) + Int32.Parse(_slditx) + Int32.Parse(_sldivd)).ToString();

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx)+Int32.Parse(_sldenvd)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-s6z2\">" + _date + "</td>";
                    kq += "<td class=\"tg-031e\">"+ _sldinb + "</td>";
                    kq += "<td class=\"tg-031e\">"+ _sldennb + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-031e\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-031e\">" + _sldenvd + "</td>";
                    kq += "<td class=\"tg-031e\">"+ _sldi + "</td>";
                    kq += "<td class=\"tg-031e\">"+ _slden + "</td>";                   
                    kq += "</tr>";

                    _sum01 += Int32.Parse(_sldinb);
                    _sum02 += Int32.Parse(_sldennb);
                    _sum03 += Int32.Parse(_sldidb);
                    _sum04 += Int32.Parse(_sldendb);
                    _sum05 += Int32.Parse(_sldicb);
                    _sum06 += Int32.Parse(_sldencb);
                    _sum07 += Int32.Parse(_sldivinh);
                    _sum08 += Int32.Parse(_sldenvinh);
                    _sum09 += Int32.Parse(_sldidh);
                    _sum10 += Int32.Parse(_sldendh);
                    _sum11 += Int32.Parse(_slditx);
                    _sum12 += Int32.Parse(_sldentx);

                    _sum15 += Int32.Parse(_sldivd);
                    _sum16 += Int32.Parse(_sldenvd);

                    _sum13 += Int32.Parse(_sldi);
                    _sum14 += Int32.Parse(_slden);
                    i++;

                }

                _tong01 = _sum01.ToString();
                _tong02 = _sum02.ToString();
                _tong03 = _sum03.ToString();
                _tong04 = _sum04.ToString();
                _tong05 = _sum05.ToString();
                _tong06 = _sum06.ToString();
                _tong07 = _sum07.ToString();
                _tong08 = _sum08.ToString();
                _tong09 = _sum09.ToString();
                _tong10 = _sum10.ToString();
                _tong11 = _sum11.ToString();
                _tong12 = _sum12.ToString();

                _tong13 = _sum13.ToString();
                _tong14 = _sum14.ToString();

                _tong15 = _sum15.ToString();
                _tong16 = _sum16.ToString();
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
            this.CreatePDF(html, "REPORT_19_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/StyleReports.css"), System.Drawing.Printing.PaperKind.A4);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            divContent.RenderControl(htw);
            
            string html = sw.GetStringBuilder().ToString();            
            this.CreateExcel(html, "REPORT_19_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleReports.css"));
        }

     
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion
    }
}