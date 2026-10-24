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
    public partial class RpDHB_SL_CatHa_Mien : System.Web.UI.Page
    {
        public string _tong01 = null; public string _tong02 = null; public string _tong03 = null; public string _tong04 = null; public string _tong05 = null; public string _tong06 = null; public string _tong07 = null; public string _tong08 = null; public string _tong09 = null; public string _tong10 = null; public string _tong11 = null; public string _tong12 = null; public string _tong13 = null; public string _tong14 = null; public string _tong15 = null; public string _tong16 = null; public string _tong17 = null; public string _tong18 = null; public string _tong19 = null; public string _tong20 = null;
        ReportDHBDAL _rpDAL = new ReportDHBDAL();

        public DataTable _dtnew = null;

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
            if (ddlMien.Value == "1")
            {
                _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
                _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"8\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
                _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
                _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
                _htmlHeader += "</td>";
                _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
                _htmlHeader += "</td>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"8\" style =\"width:48%;\">";
                _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
                _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
                _htmlHeader += "---------------------------<br/>";
                _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
                _htmlHeader += "</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"17\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SẢN LƯỢNG CẤT HẠ CÁNH TẠI KHU VỰC MIỀN BẮC</span>";
                _htmlHeader += "</td></tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"8\" style=\"text-align:right;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
                _htmlHeader += "<td class=\"tg-oqgr\"></td>";
                _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"8\" style=\"text-align:left;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td style = \"font-weight:bold;text-align:center;\" class=\"fontRp14\" colspan=\"17\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"17\"></td></tr></table></div>";
            }                
            else if (ddlMien.Value == "2")
            {
                _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
                _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"8\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
                _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
                _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
                _htmlHeader += "</td>";
                _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
                _htmlHeader += "</td>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"8\" style =\"width:48%;\">";
                _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
                _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
                _htmlHeader += "---------------------------<br/>";
                _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
                _htmlHeader += "</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"17\" style = \"text-align:center;\"><span style = \"font-weight: bold;text-align:center;\"> BÁO CÁO SẢN LƯỢNG CẤT HẠ CÁNH TẠI KHU VỰC MIỀN TRUNG</span>";
                _htmlHeader += "</td></tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"8\" style=\"text-align:right;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
                _htmlHeader += "<td class=\"tg-oqgr\"></td>";
                _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"8\" style=\"text-align:left;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td style = \"font-weight:bold;text-align:center;\" class=\"fontRp14\" colspan=\"17\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"17\"></td></tr></table></div>";
            }               
            else
            {
                _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
                _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"10\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
                _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
                _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
                _htmlHeader += "</td>";
                _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
                _htmlHeader += "</td>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"10\" style =\"width:48%;\">";
                _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
                _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
                _htmlHeader += "---------------------------<br/>";
                _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
                _htmlHeader += "</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"21\" style = \"text-align:center;\"><span style = \"font-weight: bold;text-align:center;\"> BÁO CÁO SẢN LƯỢNG CẤT HẠ CÁNH TẠI KHU VỰC MIỀN NAM</span>";
                _htmlHeader += "</td></tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"10\" style=\"text-align:right;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
                _htmlHeader += "<td class=\"tg-oqgr\"></td>";
                _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"10\" style=\"text-align:left;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td style = \"font-weight:bold;text-align:center;\" class=\"fontRp14\" colspan=\"21\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"21\"></td></tr></table></div>";
            }
            
            return _htmlHeader;
        }

        public string BuildContent()
        {
            DataTable dt = _rpDAL.DHB19_Get_FLIGHTDATE(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            if(_dtnew==null)
            {
                _dtnew = _rpDAL.DHB_GET_SLB_HHKVN_DI_DEN_MIEN(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            }
           
            string _htmlContent = "";
            if (ddlMien.Value == "1")
            {
                _htmlContent += "<div id = \"content\">";
                _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                _htmlContent += "<tr>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" rowspan=\"2\">Ngày</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Nội Bài</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Điện Biên</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Cát Bi</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Vinh</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Đồng Hới</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Thọ xuân</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Vân Đồn</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Tổng</th>";
                _htmlContent += "</tr>";
                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "</tr>";

                _htmlContent += GenDataTableMB_New(dt);

                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>CỘNG</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong01 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong02 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong03 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong04 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong05 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong06 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong07 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong08 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong09 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong10 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong11 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong12 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong15 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong16 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong13 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong14 + "</B></td>";
                _htmlContent += "</tr>";
                _htmlContent += "</table>";
                _htmlContent += "</div>";
            }
            else if(ddlMien.Value=="2")
            {
                _htmlContent += "<div id = \"content\">";
                _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                _htmlContent += "<tr>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" rowspan=\"2\">Ngày</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Đà Nẵng</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Phú Bài</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Pleiku</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Phù Cát</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Chu Lai</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Tuy Hòa</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Cam Ranh</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Tổng</th>";
                _htmlContent += "</tr>";
                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "</tr>";

                _htmlContent += GenDataTableMT_New(dt);

                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>CỘNG</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong01 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong02 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong03 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong04 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong05 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong06 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong07 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong08 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong09 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong10 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong11 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong12 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong15 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong16 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong13 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong14 + "</B></td>";
                _htmlContent += "</tr>";
                _htmlContent += "</table>";
                _htmlContent += "</div>";
            }
            else
            {
                _htmlContent += "<div id = \"content\">";
                _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
                _htmlContent += "<tr>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" rowspan=\"2\">Ngày</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVTS</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVPQ</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVCM</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVCS</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVBM</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVDL</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVRG</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVCT</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">VVVT</th>";
                _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Tổng</th>";
                _htmlContent += "</tr>";
                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đi</td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\">Đến</td>";
                _htmlContent += "</tr>";

                _htmlContent += GenDataTableMN_New(dt);

                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>CỘNG</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong01 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong02 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong03 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong04 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong05 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong06 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong07 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong08 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong09 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong10 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong11 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong12 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong15 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong16 + "</B></td>";               

                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong17 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong18 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong19 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong20 + "</B></td>";

                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong13 + "</B></td>";
                _htmlContent += "<td class=\"tg-c3ow fontRp14\"><B>" + _tong14 + "</B></td>";

                _htmlContent += "</tr>";
                _htmlContent += "</table>";
                _htmlContent += "</div>";
            }
            return _htmlContent;
        }


        private string GenDataTableMB_New(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0"; string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0"; string _sldicb = "0"; string _sldencb = "0"; string _sldivinh = "0";
            string _sldenvinh = "0"; string _sldidh = "0"; string _sldendh = "0"; string _slditx = "0"; string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";
            string _sldi = "0"; string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
                    _sldinb = GetValue(r["FLIGHTDATE"].ToString(), "VVNB", "DI");
                    _sldennb = GetValue(r["FLIGHTDATE"].ToString(), "VVNB", "DEN");

                    _sldidb = GetValue(r["FLIGHTDATE"].ToString(), "VVDB", "DI");
                    _sldendb = GetValue(r["FLIGHTDATE"].ToString(), "VVDB", "DEN");

                    _sldicb = GetValue(r["FLIGHTDATE"].ToString(), "VVCI", "DI");
                    _sldencb = GetValue(r["FLIGHTDATE"].ToString(), "VVCI", "DEN");

                    _sldivinh = GetValue(r["FLIGHTDATE"].ToString(), "VVVH", "DI");
                    _sldenvinh = GetValue(r["FLIGHTDATE"].ToString(), "VVVH", "DEN");

                    _sldidh = GetValue(r["FLIGHTDATE"].ToString(), "VVDH", "DI");
                    _sldendh = GetValue(r["FLIGHTDATE"].ToString(), "VVDH", "DEN");

                    _slditx = GetValue(r["FLIGHTDATE"].ToString(), "VVTX", "DI");
                    _sldentx = GetValue(r["FLIGHTDATE"].ToString(), "VVTX", "DEN");

                    _sldivd = GetValue(r["FLIGHTDATE"].ToString(), "VVVD", "DI");
                    _sldenvd = GetValue(r["FLIGHTDATE"].ToString(), "VVVD", "DEN");

                    _sldi = (Int32.Parse(_sldinb) + Int32.Parse(_sldidb) + Int32.Parse(_sldicb) + Int32.Parse(_sldivinh) + Int32.Parse(_sldidh) + Int32.Parse(_slditx) + Int32.Parse(_sldivd)).ToString();

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx) + Int32.Parse(_sldenvd)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _date + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldinb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldennb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldi + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slden + "</td>";
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

        public string GetValue(string date, string from, string type)
        {
            string _return = "0";
            try
            {
                if (_dtnew.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtnew.Rows.Count; i++)
                    {
                        if ((date.Trim() == _dtnew.Rows[i]["FLIGHTDATE"].ToString().Trim()) && (from.Trim() == _dtnew.Rows[i]["FROM_AIR"].ToString().Trim()) && (_dtnew.Rows[i][type].ToString() != "0"))
                        {
                            _return = _dtnew.Rows[i][type].ToString();
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

        private string GenDataTableMT_New(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0"; string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0"; string _sldicb = "0"; string _sldencb = "0"; string _sldivinh = "0";
            string _sldenvinh = "0"; string _sldidh = "0"; string _sldendh = "0"; string _slditx = "0"; string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";
            string _sldi = "0"; string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
                    _sldinb = GetValue(r["FLIGHTDATE"].ToString(), "VVDN", "DI");
                    _sldennb = GetValue(r["FLIGHTDATE"].ToString(), "VVDN", "DEN");

                    _sldidb = GetValue(r["FLIGHTDATE"].ToString(), "VVPB", "DI");
                    _sldendb = GetValue(r["FLIGHTDATE"].ToString(), "VVPB", "DEN");

                    _sldicb = GetValue(r["FLIGHTDATE"].ToString(), "VVPK", "DI");
                    _sldencb = GetValue(r["FLIGHTDATE"].ToString(), "VVPK", "DEN");

                    _sldivinh = GetValue(r["FLIGHTDATE"].ToString(), "VVPC", "DI");
                    _sldenvinh = GetValue(r["FLIGHTDATE"].ToString(), "VVPC", "DEN");

                    _sldidh = GetValue(r["FLIGHTDATE"].ToString(), "VVCA", "DI");
                    _sldendh = GetValue(r["FLIGHTDATE"].ToString(), "VVCA", "DEN");

                    _slditx = GetValue(r["FLIGHTDATE"].ToString(), "VVTH", "DI");
                    _sldentx = GetValue(r["FLIGHTDATE"].ToString(), "VVTH", "DEN");

                    _sldivd = GetValue(r["FLIGHTDATE"].ToString(), "VVCR", "DI");
                    _sldenvd = GetValue(r["FLIGHTDATE"].ToString(), "VVCR", "DEN");

                    _sldi = (Int32.Parse(_sldinb) + Int32.Parse(_sldidb) + Int32.Parse(_sldicb) + Int32.Parse(_sldivinh) + Int32.Parse(_sldidh) + Int32.Parse(_slditx) + Int32.Parse(_sldivd)).ToString();

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx) + Int32.Parse(_sldenvd)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _date + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldinb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldennb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldi + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slden + "</td>";
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

        private string GenDataTableMN_New(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0"; string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0"; string _sldicb = "0"; string _sldencb = "0"; string _sldivinh = "0";
            string _sldenvinh = "0"; string _sldidh = "0"; string _sldendh = "0"; string _slditx = "0"; string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";

            string _sldivvct = "0"; string _sldenvvct = "0";
            string _sldivvvt = "0"; string _sldenvvvt = "0";


            string _sldi = "0"; string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0; int _sum17 = 0; int _sum18 = 0; int _sum19 = 0; int _sum20 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
                    _sldinb = GetValue(r["FLIGHTDATE"].ToString(), "VVTS", "DI");
                    _sldennb = GetValue(r["FLIGHTDATE"].ToString(), "VVTS", "DEN");

                    _sldidb = GetValue(r["FLIGHTDATE"].ToString(), "VVPQ", "DI");
                    _sldendb = GetValue(r["FLIGHTDATE"].ToString(), "VVPQ", "DEN");

                    _sldicb = GetValue(r["FLIGHTDATE"].ToString(), "VVCM", "DI");
                    _sldencb = GetValue(r["FLIGHTDATE"].ToString(), "VVCM", "DEN");

                    _sldivinh = GetValue(r["FLIGHTDATE"].ToString(), "VVCS", "DI");
                    _sldenvinh = GetValue(r["FLIGHTDATE"].ToString(), "VVCS", "DEN");

                    _sldidh = GetValue(r["FLIGHTDATE"].ToString(), "VVBM", "DI");
                    _sldendh = GetValue(r["FLIGHTDATE"].ToString(), "VVBM", "DEN");

                    _slditx = GetValue(r["FLIGHTDATE"].ToString(), "VVDL", "DI");
                    _sldentx = GetValue(r["FLIGHTDATE"].ToString(), "VVDL", "DEN");

                    _sldivd = GetValue(r["FLIGHTDATE"].ToString(), "VVRG", "DI");
                    _sldenvd = GetValue(r["FLIGHTDATE"].ToString(), "VVRG", "DEN");

                    _sldivvct = GetValue(r["FLIGHTDATE"].ToString(), "VVCT", "DI");
                    _sldenvvct = GetValue(r["FLIGHTDATE"].ToString(), "VVCT", "DEN");

                    _sldivvvt = GetValue(r["FLIGHTDATE"].ToString(), "VVVT", "DI");
                    _sldenvvvt = GetValue(r["FLIGHTDATE"].ToString(), "VVVT", "DEN");


                    _sldi = (Int32.Parse(_sldinb) + Int32.Parse(_sldidb) + Int32.Parse(_sldicb) + Int32.Parse(_sldivinh) + Int32.Parse(_sldidh) + Int32.Parse(_slditx) + Int32.Parse(_sldivd) + Int32.Parse(_sldivvct) + Int32.Parse(_sldivvvt)).ToString();

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx) + Int32.Parse(_sldenvd) + Int32.Parse(_sldenvvct) + Int32.Parse(_sldenvvvt)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _date + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldinb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldennb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvd + "</td>";

                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivvct + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvvct + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivvvt + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvvvt + "</td>";

                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldi + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slden + "</td>";
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

                    _sum17 += Int32.Parse(_sldivvct);
                    _sum18 += Int32.Parse(_sldenvvct);
                    _sum19 += Int32.Parse(_sldivvvt);
                    _sum20 += Int32.Parse(_sldenvvvt);

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

                _tong17 = _sum17.ToString();
                _tong18 = _sum18.ToString();
                _tong19 = _sum19.ToString();
                _tong20 = _sum20.ToString();
            }
            return kq;
        }





        private string GenDataTableMB(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0"; string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0"; string _sldicb = "0"; string _sldencb = "0"; string _sldivinh = "0";
            string _sldenvinh = "0"; string _sldidh = "0"; string _sldendh = "0"; string _slditx = "0"; string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";
            string _sldi = "0"; string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
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

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx) + Int32.Parse(_sldenvd)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _date + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldinb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldennb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldi + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slden + "</td>";
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

        private string GenDataTableMT(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0"; string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0"; string _sldicb = "0"; string _sldencb = "0"; string _sldivinh = "0";
            string _sldenvinh = "0"; string _sldidh = "0"; string _sldendh = "0"; string _slditx = "0"; string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";
            string _sldi = "0"; string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
                    _sldinb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVDN", _date);
                    _sldennb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVDN", _date);

                    _sldidb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVPB", _date);
                    _sldendb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVPB", _date);

                    _sldicb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVPK", _date);
                    _sldencb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVPK", _date);

                    _sldivinh = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVPC", _date);
                    _sldenvinh = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVPC", _date);

                    _sldidh = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVCL", _date);
                    _sldendh = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVCL", _date);

                    _slditx = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVTH", _date);
                    _sldentx = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVTH", _date);

                    _sldivd = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVCR", _date);
                    _sldenvd = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVCR", _date);

                    _sldi = (Int32.Parse(_sldinb) + Int32.Parse(_sldidb) + Int32.Parse(_sldicb) + Int32.Parse(_sldivinh) + Int32.Parse(_sldidh) + Int32.Parse(_slditx) + Int32.Parse(_sldivd)).ToString();

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx) + Int32.Parse(_sldenvd)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _date + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldinb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldennb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldenvd + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldi + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _slden + "</td>";
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

        private string GenDataTableMN(DataTable _dt)
        {
            int i = 0;
            string kq = "";
            string _date = "";
            string _sldinb = "0"; string _sldennb = "0"; string _sldidb = "0"; string _sldendb = "0"; string _sldicb = "0"; string _sldencb = "0"; string _sldivinh = "0";
            string _sldenvinh = "0"; string _sldidh = "0"; string _sldendh = "0"; string _slditx = "0"; string _sldentx = "0"; string _sldivd = "0"; string _sldenvd = "0";

            string _sldivvct = "0"; string _sldenvvct = "0";
            string _sldivvvt = "0"; string _sldenvvvt = "0";


            string _sldi = "0"; string _slden = "0";
            int _sum01 = 0; int _sum02 = 0; int _sum03 = 0; int _sum04 = 0; int _sum05 = 0; int _sum06 = 0; int _sum07 = 0; int _sum08 = 0; int _sum09 = 0;
            int _sum10 = 0; int _sum11 = 0; int _sum12 = 0; int _sum13 = 0; int _sum14 = 0; int _sum15 = 0; int _sum16 = 0; int _sum17 = 0; int _sum18 = 0; int _sum19 = 0; int _sum20 = 0;
            if (_dt != null)
            {
                foreach (DataRow r in _dt.Rows)
                {
                    _date = UltilFunc.ToDate(r["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
                    _sldinb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVTS", _date);
                    _sldennb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVTS", _date);

                    _sldidb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVPQ", _date);
                    _sldendb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVPQ", _date);

                    _sldicb = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVCM", _date);
                    _sldencb = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVCM", _date);

                    _sldivinh = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVCS", _date);
                    _sldenvinh = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVCS", _date);

                    _sldidh = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVBM", _date);
                    _sldendh = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVBM", _date);

                    _slditx = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVDL", _date);
                    _sldentx = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVDL", _date);

                    _sldivd = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVRG", _date);
                    _sldenvd = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVRG", _date);

                    _sldivvct = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVCT", _date);
                    _sldenvvct = _rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVCT", _date);

                    _sldivvvt = _rpDAL.DHB19_THSLB_SUM_CBDI_BYSB("VVVT", _date);
                    _sldenvvvt =_rpDAL.DHB19_THSLB_SUM_CBDEN_BYSB("VVVT", _date);


                    _sldi = (Int32.Parse(_sldinb) + Int32.Parse(_sldidb) + Int32.Parse(_sldicb) + Int32.Parse(_sldivinh) + Int32.Parse(_sldidh) + Int32.Parse(_slditx) + Int32.Parse(_sldivd)+ Int32.Parse(_sldivvct) + Int32.Parse(_sldivvvt)).ToString();

                    _slden = (Int32.Parse(_sldennb) + Int32.Parse(_sldendb) + Int32.Parse(_sldencb) + Int32.Parse(_sldenvinh) + Int32.Parse(_sldendh) + Int32.Parse(_sldentx) + Int32.Parse(_sldenvd) + Int32.Parse(_sldenvvct) + Int32.Parse(_sldenvvvt)).ToString();

                    kq += "<tr>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _date + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldinb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldennb + "</td>";
                    kq += "<td class=\"tg-c3ow fontRp14\">" + _sldidb + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldendb + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldicb + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldencb + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldivinh + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldenvinh + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldidh + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldendh + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _slditx + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldentx + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldivd + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldenvd + "</td>";

                    kq += "<td class=\"tg-c3ow\">" + _sldivvct + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldenvvct + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldivvvt + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _sldenvvvt + "</td>";

                    kq += "<td class=\"tg-c3ow\">" + _sldi + "</td>";
                    kq += "<td class=\"tg-c3ow\">" + _slden + "</td>";
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

                    _sum17 += Int32.Parse(_sldivvct);
                    _sum18 += Int32.Parse(_sldenvvct);
                    _sum19 += Int32.Parse(_sldivvvt);
                    _sum20 += Int32.Parse(_sldenvvvt);

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

                _tong17 = _sum17.ToString();
                _tong18 = _sum18.ToString();
                _tong19 = _sum19.ToString();
                _tong20 = _sum20.ToString();
            }
            return kq;
        }


        public string BuildFooter()
        {
            string _htmlFooter = "";

            if ((ddlMien.Value == "1")|| (ddlMien.Value == "2"))
            {
                _htmlFooter += "<div id = \"footer\">";
                _htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"17\" style=\"height:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"5\">NGƯỜI LẬP BIỂU</td>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\">TRUNG TÂM HĐB &amp; ĐP LKL</td>";
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
            }            
            else
            {
                _htmlFooter += "<div id = \"footer\">";
                _htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"21\" style=\"height:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\">NGƯỜI LẬP BIỂU</td>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\">TRUNG TÂM HĐB &amp; ĐP LKL</td>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\">GIÁM ĐỐC</td></tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-0dc2\" colspan=\"7\">";
                _htmlFooter += "<br><br><br><br><br><br><br>";
                _htmlFooter += "</td>";
                _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\" rowspan=\"4\"></td>";
                _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\" rowspan=\"4\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\" style = \"font-size: 14px;line-height:4px;\"><span style = \"font-weight: bold;font-style: italic;font-size: 14px;\" > Nơi nhận :</span></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\" style = \"font-size: 14px;line-height:4px;\">-Như kính gửi</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-oqgr\" colspan=\"7\" style = \"font-size: 14px;line-height:4px;\">-Lưu VT, ĐPL</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-2ph3\" colspan=\"21\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-sq11\" colspan=\"7\" rowspan=\"3\"></td>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\" rowspan=\"3\"></td>";
                _htmlFooter += "<td class=\"tg-k94d\" colspan=\"7\" rowspan=\"3\"></td>";
                _htmlFooter += "</tr><tr></tr><tr></tr></table></div>";
            }
            
            return _htmlFooter;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
            this.CreateExcel(html, "THSLB_SL_CATHA_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_SL_CATHA_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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