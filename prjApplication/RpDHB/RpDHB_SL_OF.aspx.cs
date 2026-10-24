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
    public partial class RpDHB_SL_OF : System.Web.UI.Page
    {
        ReportDHBDAL _DAL = new ReportDHBDAL();
        public DataTable _dtnew = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltrHeader.Text = BuildHeader();
                ltrContent.Text = BuildContent_New();
                ltrFooter.Text = BuildFooter();
            }
        }
        public string BuildHeader()
        {
            string _htmlHeader = "";
            _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
            _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
            _htmlHeader += "<colgroup>";
            _htmlHeader += "<col style =\"width: 48%\">";
            _htmlHeader += "<col style=\"width: 4%\">";
            _htmlHeader += "<col style = \"width: 48%\">";
            _htmlHeader += "</colgroup>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<th class=\"tg-2ph3\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span style = \"font-weight: bold\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
            _htmlHeader += "<th class=\"tg-oqgr\"></th>";
            _htmlHeader += "<th class=\"tg-2ph3\"></br>";
            _htmlHeader += "<span style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span style = \"font-style: italic\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/></th>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";

            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"3\"><span style = \"font-weight: bold\"> BÁO CÁO SỐ LIỆU BAY QUÁ CẢNH</span><br/><span style = \"font-weight: bold;font-size:13px;\">Tháng " + txtFromDate.Value.ToString().Replace("-", " năm ") + "</span>";

            _htmlHeader += "</td></tr>";

            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-c7ws\" colspan=\"3\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-c7ws\" colspan=\"3\"></td></tr></table></div>";
            return _htmlHeader;
        }


        public string BuildContent_New()
        {
            string _fromDate = txtFromDate.Value.ToString().Trim();

            string _totalTLHN = "0"; string _totalTLHCM = "0"; string _totalDXHN = "0"; string _totalDXHCM = "0";
            string _totalVip = "0"; string _totalQsu = "0";
            DataTable _dt = null;
            if (!String.IsNullOrEmpty(_fromDate))
                _dt = _DAL.DHB_Fun_SLB_OF(_fromDate);
            DataRow _mrow = null;

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
                        _totalVip = _mrow["TotalVIP"].ToString();
                        _totalQsu = _mrow["TotalQSU"].ToString();
                    }
                }
            }

            double _ts1HnHcm = Double.Parse(_totalTLHN) + Double.Parse(_totalDXHN);
            double _ts2HnHcm = Double.Parse(_totalTLHCM) + Double.Parse(_totalDXHCM);

            double _tsHnHcm = Double.Parse(_totalTLHN) + Double.Parse(_totalTLHCM);
            double _tsdxHnHcm = Double.Parse(_totalDXHN) + Double.Parse(_totalDXHCM);
            double _tstldxHnHcm = _tsHnHcm + _tsdxHnHcm;



            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
            _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
            _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
            _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";            
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
            _htmlContent += "<td class=\"tg-baqh\">"+ _totalTLHN + "</td>";
            _htmlContent += "<td class=\"tg-baqh\">"+ _totalTLHCM + "</td>";
            _htmlContent += "<td class=\"tg-baqh\">"+ _tsHnHcm + "</td>";
            _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : "+ _totalVip + " </td>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
            _htmlContent += "<td class=\"tg-baqh\">"+ _totalDXHN + "</td>";
            _htmlContent += "<td class=\"tg-baqh\">"+ _totalDXHCM + "</td>";
            _htmlContent += "<td class=\"tg-baqh\">"+ _tsdxHnHcm + "</td>";
            _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : "+ _totalQsu + "</td>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>"+ _ts1HnHcm + "</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>"+ _ts2HnHcm + "</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>"+ _tstldxHnHcm + "</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
            _htmlContent += "</tr>";

            
            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }

        public string BuildContent_NewFix(string _month)
        {
            

            string _htmlContent = "";

            switch (_month)
            {
                case "01/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 23.737 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 14.874 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 38.611 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 6 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 549 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 447 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 996 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 49 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 24.286 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.321 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 39.607 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "02/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 22.015 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 13.756 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 35.771 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 5 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 820 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 547 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 1.367 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 127 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.835 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 14.030 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 37.138 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "03/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 23.675 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 15.355 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 39030 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 10 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 562 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 442 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 1.004 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 90 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 24.237 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.797 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 40.034 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "04/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 21.850 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 15.387 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 37.237 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 11 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 519 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 437 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 956 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 43 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.369 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.824 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 38.193 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";
                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "05/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 22.319 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 15.468 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 37.787 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 7 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 418 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 448 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 866 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 64 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.737 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.916 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 38.653 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "06/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 21.668 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 15.050 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 36.719 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 03 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 438 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 444 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 882 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 96 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.106 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.494 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 37.601 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "07/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 23.048 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 15.381 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 38.429 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 0 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 532 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 465 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 997 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 49 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 23.580 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.846 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 39.426 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "08/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + "23.330" + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 15.368 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 38.698 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 6 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 433 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 507 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 940 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 61 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 23.763 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.875 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 39.638 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "09/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 22.069 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 14.545 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 36.614 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 8 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 353 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 576 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 929 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 34 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.422 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.121 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 37.453 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "10/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 22.458 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 14.860 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 37.318 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 8 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 402 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 571 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 973 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 49 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.860 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.431 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 38.291 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "11/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 22.004 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 13.888 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 35.892 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 11 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 417 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 606 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 1.023 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 62 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 22.421 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 14.494 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 36.915 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                case "12/2019":
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 22.714 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 14.997 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 37.711 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 02 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 502 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 608 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + "1.110" + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 26 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 23.216 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 15.605 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 38.821 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
                    _htmlContent += "</tr>";


                    _htmlContent += "</table>";
                    _htmlContent += "</div>";
                    break;
                default:
                    _htmlContent += "<div id = \"content\">";
                    _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tính chất chuyến bay</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hà Nội</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Fir Hồ Chí Minh</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 15%;\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\" style=\"width: 40%;\"><b>Ghi chú</b></th>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">THƯỜNG LỆ</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến VIP : " + 0 + " </td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<td class=\"tg-baqh\">ĐỘT XUẤT</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\">" + 0 + "</td>";
                    _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">Số chuyến quân sự : " + 0 + "</td>";
                    _htmlContent += "</tr>";

                    _htmlContent += "<tr>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>Tổng</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 0 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 0 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b>" + 0 + "</b></th>";
                    _htmlContent += "<th class=\"tg-baqh\"><b></b></th>";
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
            //_htmlFooter += "<div id = \"footer\">";
            //_htmlFooter += "<table class=\"tgfooter\" style=\"width: 100%\">";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<th class=\"tg-mmgy\" colspan=\"2\">ĐẠI DIỆN CÔNG TY QLB MIỀN BẮC<br/>";
            //_htmlFooter += "</th>";
            //_htmlFooter += "<th class=\"tg-qjg1\"></th>";
            //_htmlFooter += "<th class=\"tg-npou\" colspan=\"2\">ĐẠI DIỆN TRUNG TÂM QLLKL</th>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-npou\">TRƯỞNG TT KS ĐƯỜNG DÀI</td>";
            //_htmlFooter += "<td class=\"tg-npou\">GIÁM ĐỐC</td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-npou\">TRƯỞNG TT HĐB-ĐPLKL</td>";
            //_htmlFooter += "<td class=\"tg-npou\">GIÁM ĐỐC</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\"><span style = \"font-weight: bold;font-style: italic;\" > Nơi nhận :</span></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\">-Như kính gửi</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-oqgr\" colspan=\"4\">-Lưu VT, ĐPL</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "</table>";
            //_htmlFooter += "</div>";


            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"17\" style=\"height:14px;\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"5\">NGƯỜI LẬP BIỂU</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\">TRƯỞNG TT ĐHB &amp; ĐP LKL</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"5\">GIÁM ĐỐC</td></tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-0dc2\" colspan=\"5\">";
            _htmlFooter += "<br><br><br><br><br><br><br>";
            _htmlFooter += "</td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\" rowspan=\"4\"></td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"5\" rowspan=\"4\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"5\" style = \"font-size: 14px;line-height:4px;\"><span style = \"font-weight: bold;font-style: italic;font-size: 14px;\" > Nơi nhận :</span></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\" style = \"font-size: 14px;line-height:4px;\">-Như kính gửi</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-oqgr\" colspan=\"5\" style = \"font-size: 14px;line-height:4px;\">-Lưu VT, ĐPL</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-2ph3\" colspan=\"17\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-sq11\" colspan=\"5\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"5\" rowspan=\"3\"></td>";
            _htmlFooter += "</tr><tr></tr><tr></tr></table></div>";

            return _htmlFooter;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ltrHeader.Text = "";
            ltrContent.Text = "";
            ltrFooter.Text = "";
            ltrHeader.Text = BuildHeader();            
            ltrContent.Text = BuildContent_New();            
            ltrFooter.Text = BuildFooter();

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string html = GetContent();
            this.CreateExcel(html, "BCSL_OF_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "BCSL_OF_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
        }
        public string GetContent()
        {
            string _html = null;
            StringWriter sw = new StringWriter();
            HtmlTextWriter h = new HtmlTextWriter(sw);
            contentid.RenderControl(h);
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