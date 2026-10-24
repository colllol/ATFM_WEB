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
    public partial class RpDHB_VIA_2FIR_BY_OPER : System.Web.UI.Page
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
            if (ddlMien.Value == "1")
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"3\"><span style = \"font-weight: bold\"> BÁO CÁO CÁC CHUYẾN BAY QUA FIR HÀ NỘI THEO HÃNG</span><br/><span style = \"font-weight: bold;font-size:13px;\">Tháng " + txtFromDate.Value.ToString().Replace("-", " năm ") + "</span>";
            else 
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"3\"><span style = \"font-weight: bold\"> BÁO CÁO CÁC CHUYẾN BAY QUA FIR HỒ CHÍ MINH THEO HÃNG</span><br/><span style = \"font-weight: bold;font-size:13px;\">Tháng " + txtFromDate.Value.ToString().Replace("-", " năm ") + "</span>";
                      

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

            DataTable _dt = null;
            if (!String.IsNullOrEmpty(_fromDate))
            {
                if (ddlMien.Value == "1")
                    _dt = _DAL.DHB_Fun_List_OF_FIR_HN(_fromDate);
                else
                    _dt = _DAL.DHB_Fun_List_OF_FIR_HCM(_fromDate);
            }
                
            DataRow _mrow = null;

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\"><b>STT</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Hãng</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Đường bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Số chuyến</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Tổng số</b></th>";            
            _htmlContent += "</tr>";

            if (!String.IsNullOrEmpty(_fromDate))
            {
                if (_dt != null)
                {

                    if (_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < _dt.Rows.Count; j++)
                        {
                            _mrow = _dt.Rows[j];
                                                      

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-baqh\">" + (j + 1) + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">"+ _mrow["OPER_ID"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["FPL_VIA"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["SC"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["SC"].ToString() + "</td>";                           
                            _htmlContent += "</tr>";

                        }
                    }
                }

            }


            _htmlContent += "</table>";
            _htmlContent += "</div>";
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
            this.CreateExcel(html, "BCSL_HQT_OF_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "BCSL_HQT_OF_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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