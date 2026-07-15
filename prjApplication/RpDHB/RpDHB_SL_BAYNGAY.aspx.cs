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
using System.Configuration;

namespace prjApplication.RpDHB
{
    public partial class RpDHB_SL_BAYNGAY : System.Web.UI.Page
    {
        ReportStatisticDAL _DAL = new ReportStatisticDAL();
        DataTable _dt1 = null;
        DataTable _dt2 = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltrHeader.Text = BuildHeader();
                ltrContent.Text = BuildContent();
                ltrFooter.Text = BuildFooter();
            }
        }
        public string BuildHeader()
        {
            string _htmlHeader = "";

            _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
            _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"3\">TRUNG TÂM QLLKL<br/>";
            _htmlHeader += "<span class=\"fontRp13\">TRUNG TÂM HĐB&ĐPLKL<br/>";
            _htmlHeader += "<br></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"4\" style =\"width:48%;\">";           
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"8\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;font-size:24px;\"> TỔNG HỢP SỐ LIỆU BAY</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"3\" style=\"text-align:right;font-weight:bold;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
            _htmlHeader += "<td class=\"tg-oqgr\"></td>";
            _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"4\" style=\"text-align:left;font-weight:bold;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td colspan=\"8\"></td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"8\"></td></tr></table></div>";


            return _htmlHeader;
        }
        public string BuildContent()
        {
            string _dateThang = "";
            _dt1 = _DAL.THSLB_GET_FINISH_BAY_QN(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            _dt2 = _DAL.THSLB_GET_FINISH_BAY_QT(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            string _totalQT = _DAL.THSLB_GET_SLB_HQT(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            if (!String.IsNullOrEmpty(txtFromDate.Value.Trim()))
            {
                DateTime _dtime = UltilFunc.ToDate(txtFromDate.Value.Trim().Replace("-", "/"), "dd/MM/yyyy");
                _dateThang = String.Format("{0:MM-yyyy}", _dtime);
            }

            string _slhvnqn = "0"; string _slhvnqt = "0";
            _slhvnqn = GetValue("HVN", _dt1);
            _slhvnqt= GetValue("HVN", _dt2);
            int _sltotal1 = Int32.Parse(_slhvnqn) + Int32.Parse(_slhvnqt);

           string _slbavqn = "0"; string _slbavqt = "0";
            _slbavqn = GetValue("BAV", _dt1);
            _slbavqt = GetValue("BAV", _dt2);
            int _sltotal2 = Int32.Parse(_slbavqn) + Int32.Parse(_slbavqt);

            string _slqn1 = "0"; string _slqt1 = "0";
            _slqn1 = GetValue("PIC", _dt1);
            _slqt1 = GetValue("PIC", _dt2);
            int _sltotal3 = Int32.Parse(_slqn1) + Int32.Parse(_slqt1);

            string _slqn2 = "0"; string _slqt2 = "0";
            _slqn2 = GetValue("VFC", _dt1);
            _slqt2 = GetValue("VFC", _dt2);
            int _sltotal4 = Int32.Parse(_slqn2) + Int32.Parse(_slqt2);


            string _slqn3 = "0"; string _slqt3 = "0";
            _slqn3 = GetValue("VJC", _dt1);
            _slqt3 = GetValue("VJC", _dt2);
            int _sltotal5 = Int32.Parse(_slqn3) + Int32.Parse(_slqt3);

            string _slqn4 = "0"; string _slqt4 = "0";
            _slqn4 = GetValue("VNC", _dt1);
            _slqt4 = GetValue("VNC", _dt2);
            int _sltotal6 = Int32.Parse(_slqn4) + Int32.Parse(_slqt4);

            string _slqn5 = "0"; string _slqt5 = "0";
            _slqn5 = GetValue("VSA", _dt1);
            _slqt5 = GetValue("VSA", _dt2);
            int _sltotal7 = Int32.Parse(_slqn5) + Int32.Parse(_slqt5);

            string _slqn6 = "0"; string _slqt6 = "0";
            _slqn6 = GetValue("SFC", _dt1);
            _slqt6 = GetValue("SFC", _dt2);
            int _sltotal8 = Int32.Parse(_slqn6) + Int32.Parse(_slqt6);


            string _slqn7 = "0"; string _slqt7 = "0";
            _slqn7 = GetValue("HAI", _dt1);
            _slqt7 = GetValue("HAI", _dt2);
            int _sltotal9 = Int32.Parse(_slqn7) + Int32.Parse(_slqt7);

            int _total = _sltotal1+ _sltotal2+ _sltotal3+ _sltotal4+ _sltotal5+ _sltotal6+ _sltotal7+ _sltotal8+ _sltotal9 + Int32.Parse(_totalQT);

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tgbn\" style=\"width: 100%\">";

            _htmlContent += "<colgroup>";
            _htmlContent += "<col style = 'width:138px'>";
            _htmlContent += "<col style = 'width:179px'>";
            _htmlContent += "<col style = 'width:98px'>";
            _htmlContent += "<col style = 'width:101px'>";
            _htmlContent += "<col style = 'width:106px'>";
            _htmlContent += "<col style = 'width:92px'>";
            _htmlContent += "<col style = 'width:141px'>";
            _htmlContent += "<col style = 'width: 132px'>";
            _htmlContent += "</colgroup>";

            _htmlContent += "<thead>";
            _htmlContent += "<tr>";
            _htmlContent += "<th class='tg-9ydz'>THÁNG</th>";
            _htmlContent += "<th class='tg-9ydz'>HÃNG HÀNG KHÔNG</th>";
            _htmlContent += "<th class='tg-9ydz'>QN</th>";
            _htmlContent += "<th class='tg-9ydz'>QT</th>";
            //_htmlContent += "<th class='tg-9ydz'>FIR HAN</th>";
            //_htmlContent += "<th class='tg-9ydz'>FIR HCM</th>";
            _htmlContent += "<th class='tg-9ydz'>GHI CHÚ</th>";
            _htmlContent += "<th class='tg-9ydz'>TỔNG</th>";
            _htmlContent += "</tr>";
            _htmlContent += "</thead>";

            _htmlContent += "<tbody>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _dateThang + "</td>";
            _htmlContent += "<td class='tg-de2y' colspan='3'>QUỐC TẾ</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y'style='text-align:center'><b>" + _totalQT + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y' rowspan='9'></td>";
            _htmlContent += "<td class='tg-de2y'>BAV</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slbavqn + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slbavqt + "</td>";
            //_htmlContent += "<td class='tg-de2y' colspan='2' rowspan='9'></td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y'style='text-align:center'><b>" + _sltotal2 + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>HVN</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slhvnqn + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slhvnqt + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal1 + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>PIC</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn1 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt1 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal3+ "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>VFC</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn2 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt2 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal4 + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>VJC</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn3 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt3 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal5+ "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>VNC</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn4 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt4 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal6+ "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>VSA</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn5 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt5 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal7+ "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>SFC</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn6 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt6 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal8+"</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-de2y'>HAI</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqn7 + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'>" + _slqt7 + "</td>";
            _htmlContent += "<td class='tg-de2y'></td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _sltotal9+ "</b></td>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-sn4r' colspan='5'>Tổng số các chuyến bay trong tháng : "+ _dateThang + "</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _total + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class='tg-sn4r' colspan='5'>Tổng cộng :</td>";
            _htmlContent += "<td class='tg-de2y' style='text-align:center'><b>" + _total + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "</tbody>";
            _htmlContent += "</table>";            
            _htmlContent += "</div>";
            return _htmlContent;
        }
        public string BuildFooter()
        {
            string _htmlFooter = "";
            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tgft\" style=\"width: 100%\">";
            _htmlFooter += "<colgroup>";
            _htmlFooter += "<col style='width:193px'>";
            _htmlFooter += "<col style='width:181px'>";
            _htmlFooter += "<col style='width:178px'>";
            _htmlFooter += "<col style='width:143px'>";
            _htmlFooter += "<col style='width:144px'>";
            _htmlFooter += "<col style='width:130px'>";
            _htmlFooter += "<col style='width:119px'>";
            _htmlFooter += "<col style='width:130px'>";
            _htmlFooter += "</colgroup>";
            _htmlFooter += "<thead>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<th class='tg-zv4m' colspan='8></th>";
            _htmlFooter += "</tr>";
            _htmlFooter += "</thead>";
            _htmlFooter += "<tbody>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class='tg-zv4m' colspan='4'>GHI CHÚ :</td>";
            _htmlFooter += "<td class='tg-zv4m' rowspan='4'></td>";
            _htmlFooter += "<td class='tg-zv4m'>NGÀY</td>";
            _htmlFooter += "<td class='tg-zv4m'>THÁNG</td>";
            _htmlFooter += "<td class='tg-zv4m'>NĂM</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class='tg-zv4m' colspan='4'></td>";
            _htmlFooter += "<td class='tg-zv4m' colspan='3' rowspan='2'></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class='tg-zv4m' colspan='4'></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class='tg-zv4m' colspan='4'>NGƯỜI LẬP BÁO CÁO</td>";
            _htmlFooter += "<td class='tg-8jgo' colspan='3'>TRƯỞNG TT HĐB &amp; ĐPLKL</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "</tbody>";            
            _htmlFooter += "</table></div>";
            return _htmlFooter;
        }
        public string GetValue(string oper, DataTable dt)
        {
            string _return = "0";
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (oper.Trim() == dt.Rows[i]["OPER_ID"].ToString().Trim())
                        {
                            _return = dt.Rows[i]["SC"].ToString();
                            break;
                        }

                    }
                }
                else
                    _return = "0";
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            DateTime _dtime = UltilFunc.ToDate(txtFromDate.Value.Trim().Replace("-", "/"), "dd/MM/yyyy");
            string _dateThang = String.Format("{0:MM/yyyy}", _dtime);

            ltrHeader.Text = "";
            ltrContent.Text = "";
            ltrFooter.Text = "";
            ltrHeader.Text = BuildHeader();            
            ltrContent.Text = BuildContent();
            ltrFooter.Text = BuildFooter();

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string html = GetContent();
            this.CreateExcel(html, "THSLB_BCSL_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_BCSL_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        public string GetContent()
        {
            string _html = null;
            StringWriter sw = new StringWriter();
            HtmlTextWriter h = new HtmlTextWriter(sw);
            exportid.RenderControl(h);           
            _html = sw.GetStringBuilder().ToString();
            return _html;
        }
        public string GetAllContent()
        {
            string _html = null;
            StringWriter sw = new StringWriter();
            HtmlTextWriter h = new HtmlTextWriter(sw);
            exportid.RenderControl(h);
            _html = sw.GetStringBuilder().ToString();
            return _html;
        }
    }
}