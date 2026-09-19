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
    public partial class RpDHB_BC_SoLieu : System.Web.UI.Page
    {
        ReportDHBDAL _DAL = new ReportDHBDAL();
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
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"2\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"2\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style:italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"5\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SỐ LIỆU BAY</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"2\" style=\"text-align:right;font-weight:bold;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
            _htmlHeader += "<td class=\"tg-oqgr\"></td>";
            _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"2\" style=\"text-align:left;font-weight:bold;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td style = \"font-weight:bold;text-align:center;\" class=\"fontRp14\" colspan=\"5\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"5\"></td></tr></table></div>";


            return _htmlHeader;
        }
        public string BuildContent()
        {
            DataTable _dt = null;
            string _totalLD = "0"; string _totalOF = "0"; int _totalLDOF = 0;
            _totalLD = _DAL.DHB_SumTotalLD(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _totalLDOF = Int32.Parse(_totalLD) + Int32.Parse(_totalOF);
            string _totalTLHN = "0"; string _totalTLHCM = "0"; string _totalDXHN = "0"; string _totalDXHCM = "0";

            _dt = _DAL.DHB_Fun_SLB_SumTotal_OF(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            DataRow _mrow = null;

            double _tlHCM = 0; double _tlDXHCM = 0;

            if (_dt != null)
            {

                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];
                        _totalTLHN = _mrow["TotalTLFirHN"].ToString();
                        _totalTLHCM = _mrow["TotalTLFirHCM"].ToString();
                        _totalDXHN = _mrow["TotalDXFirHN"].ToString();
                        _totalDXHCM = _mrow["TotalDXFirHCM"].ToString();

                    }                   
                    
                }
            }


           


            double _ts1HnHcm = Double.Parse(_totalTLHN) + Double.Parse(_totalDXHN);
            double _ts2HnHcm = _tlHCM + Double.Parse(_totalDXHCM);

            double _all = _ts1HnHcm + _ts2HnHcm;

            double _allTotal = _all + Double.Parse(_totalLD);

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
            _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
            _htmlContent += "<td class=\"tg-amwm\">"+ _totalLD + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
            _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">"+ _all + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
            _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
            _htmlContent += "<td class=\"tg-amwm\">"+ _totalTLHN + "</td>";
            _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">"+ _ts1HnHcm + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
            _htmlContent += "</td>";
            _htmlContent += "<td class=\"tg-amwm\">"+ _totalDXHN + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
            _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
            _htmlContent += "<td class=\"tg-amwm\">"+ _tlHCM + "</td>";
            _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">"+ _ts2HnHcm + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
            _htmlContent += "<td class=\"tg-amwm\">"+ _tlDXHCM + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
            _htmlContent += "<td class=\"tg-amwm\">"+ _allTotal + "</td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }

        public string BuildContentFix(string _month)
        {
            
            string _htmlContent = "";


            switch (_month)
            {
                case "01/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "02/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 12317 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 37138 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22015 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22835 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 820 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 13756 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 14303 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 547 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 49455 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "03/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 37776 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 38133 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22074 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 38133 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 789 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 14795 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15270 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 475 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 75909 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "04/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 41283 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 38193 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 21850 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22369 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 519 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 15387 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15824 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 437 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 79476 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "05/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 42476 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 38653 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22319 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22737 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 418 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 15468 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15916 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 448 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 81129 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "06/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 43525 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 37601 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 21668 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22106 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 438 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 15051 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15494 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 444 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 81126 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "07/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 45605 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 39426 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 23048 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 23580 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 532 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 15381 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15846 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 465 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 85031 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "08/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 43600 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 39638 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 23330 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 23763 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 433 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 15368 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15875 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 507 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 83238 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "09/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 40324 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 37543 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22069 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22422 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 353 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 14545 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15121 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 576 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 77867 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "10/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 42546 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 38291 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22458 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22860 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 402 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 14860 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15431 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 571 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 80837 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "11/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 42839 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 36915 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22004 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 22421 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 417 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 13888 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 14494 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 606 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 79754 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "12/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 46110 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 38821 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 22714 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 23216 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 502 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 14997 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 15605 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 608 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 84931 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;                
                default:
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tgSLB\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-hgcj\" colspan=\"4\">HÌNH THỨC CHUYẾN BAY</th>";
                    _htmlContent += "<th class=\"tg-amwm\">TỔNG SỐ</th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay đi đến</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Bay quá cảnh</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"5\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HAN</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất<br/>";
                    _htmlContent += "</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\" rowspan=\"2\">FIR HCM</td>";
                    _htmlContent += "<td class=\"tg-0lax\">Thường lệ</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-amwm\" rowspan=\"2\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-0lax\">Đột xuất</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-1wig\" colspan=\"4\">Cộng</td>";
                    _htmlContent += "<td class=\"tg-amwm\">" + 0 + "</td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-dgz8\" colspan=\"5\"></td>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
            }           
            return _htmlContent;
        }



        public string BuildFooter()
        {
            string _htmlFooter = "";
            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"2\">NGƯỜI LẬP BIỂU</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"2\">TRƯỞNG TT ĐHB &amp; ĐP LKL</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"1\">GIÁM ĐỐC</td></tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-0dc2\" colspan=\"2\">";
            _htmlFooter += "<br><br><br><br><br><br><br>";
            _htmlFooter += "</td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"2\" rowspan=\"4\"></td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"1\" rowspan=\"4\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"2\"><span style = \"font-weight: bold\" > Nơi nhận :</span></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"2\">-Như kính gửi</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-oqgr\" colspan=\"2\">-Lưu VT, ĐPL</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-2ph3\" colspan=\"5\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-sq11\" colspan=\"2\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"2\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"1\" rowspan=\"3\"></td>";
            _htmlFooter += "</tr><tr></tr><tr></tr></table></div>";
            return _htmlFooter;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            DateTime _dtime = UltilFunc.ToDate(txtFromDate.Value.Trim().Replace("-","/"), "dd/MM/yyyy");           
            string _dateThang = String.Format("{0:MM/yyyy}", _dtime);

            ltrHeader.Text = "";
            ltrContent.Text = "";
            ltrFooter.Text = "";
            ltrHeader.Text = BuildHeader();
            //if(UltilFunc.ReturnUseFix()==1)
            //{
            //    ltrContent.Text = BuildContentFix(_dateThang);
            //}
            //else
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
            //contentid.RenderControl(h);
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