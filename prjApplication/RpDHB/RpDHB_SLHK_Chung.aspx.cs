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
    public partial class RpDHB_SLHK_Chung : System.Web.UI.Page
    {
        ReportDHBDAL _DAL = new ReportDHBDAL();
        public DataTable _dtnew = null;

        public string _datekhoang = null;
        public string _date = null;
        public string _month = null;
        public string _year = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WriteInfoTime();
                ltrHeader.Text = BuildHeader();
                ltrContent.Text = BuildContent_New();
                ltrFooter.Text = BuildFooter();
            }
        }
        public void WriteInfoTime()
        {
            _date = DateTime.Now.Day.ToString();
            _month = DateTime.Now.Month.ToString();
            _year = DateTime.Now.Year.ToString();            
            _datekhoang = "Từ " + txtFromDate.Value.Trim() + " đến " + txtToDate.Value.Trim();
        }
        public string BuildHeader()
        {
            _datekhoang = "Từ " + txtFromDate.Value.Trim() + " đến " + txtToDate.Value.Trim();

            string _htmlHeader = "";
            
            _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
            _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"5\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"5\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"11\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SỐ LIỆU BAY HÀNG KHÔNG CHUNG</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-c7ws\" colspan=\"3\" style=\"text-align:left;\">Kỳ báo cáo : " + _datekhoang + "</td>";
            _htmlHeader += "</tr>";

            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-c7ws\" colspan=\"3\" style=\"text-align:left;\">Ngày báo cáo : Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString() + " </td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"11\"></td></tr></table></div>";

            return _htmlHeader;
        }


        public string BuildContent_New()
        {
            string _dateFrom = txtFromDate.Value.ToString().Trim();
            string _dateTo = txtToDate.Value.ToString().Trim();

            DataTable _dt = null;
            if (!String.IsNullOrEmpty(_dateFrom) && !String.IsNullOrEmpty(_dateTo))
                _dt = _DAL.BCDHB06_Get_SLB_ByTimes(_dateFrom.Trim(), _dateTo.Trim());            
            DataRow _mrow = null;



            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\"><b>STT</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Nhà khai thác tàu bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Số hiệu chuyến bay (callsign)</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Loại MB (Aircraft type)</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Mục đích khai thác (purpose)</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Từ điểm (from) </b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Đến điểm (to) </b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Ngày khai thác (Date flight)</b></th>";

            _htmlContent += "<th class=\"tg-baqh\"><b>Đường HK (ATS route)</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Giờ CC chính thức (ATD) </b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Giờ HC chính thức (ATA)</b></th>";
           
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\"></th>";
            _htmlContent += "<th class=\"tg-baqh\">(1)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(2)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(3)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(4)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(5)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(6)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(7)</th>";

            _htmlContent += "<th class=\"tg-baqh\">(8)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(9)</th>";
            _htmlContent += "<th class=\"tg-baqh\">(10)</th>";

            _htmlContent += "</tr>";

            
                if (_dt != null)
                {

                    if (_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < _dt.Rows.Count; j++)
                        {
                            _mrow = _dt.Rows[j];

                            string _remark = _mrow["FPL_VIA"].ToString();
                            string _fpl = "";
                            string _fpl2 = "";
                            string _result = "";
                            //if (_remark.Length > 15)
                            //{
                            //    _result = _remark.Substring(0, 15);
                            //    _fpl = _remark.Remove(0, 15);
                            //    if (_fpl.Length > 15)
                            //    {
                            //        _result += "<br/><br/>" + _fpl.Substring(0, 15) + "<br/><br/>";
                            //        _fpl2 = _fpl.Remove(0, 15);
                            //        if (_fpl2.Length > 15)
                            //        {
                            //            _result += _fpl2.Substring(0, 15) + "<br/><br/>" + _fpl2.Remove(0, 15);
                            //        }
                            //    }

                            //}
                            //else
                                _result = _mrow["FPL_VIA"].ToString();

                            _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">" + (j+1) + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _mrow["oper_id"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;\">" + _mrow["FLIGHTNBR"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["CRAFT_TYPE"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["PURPOSE"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["FROM_AIRP"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["TO_AIRP"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + Convert.ToDateTime(_mrow["FLIGHTDATE"]).ToString("dd-MM-yyyy") + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;width:15%\">" + _result + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["ATD"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["ATA"].ToString() + "</td>";                            

                            _htmlContent += "</tr>";

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
            //_htmlFooter += "<table class=\"tgfooter\" style=\"width:100%;border:none;border-color:white;\">";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"11\" ></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"11\" ></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<th class=\"tg-mmgy\" colspan=\"5\">ĐẠI DIỆN CÔNG TY QLB MIỀN BẮC<br/>";
            //_htmlFooter += "</th>";
            //_htmlFooter += "<th class=\"tg-qjg1\"></th>";
            //_htmlFooter += "<th class=\"tg-npou\" colspan=\"5\">ĐẠI DIỆN TRUNG TÂM QLLKL</th>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-npou\" colspan=\"2\">TRƯỞNG TT KS ĐƯỜNG DÀI</td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-npou\" colspan=\"2\">GIÁM ĐỐC</td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-npou\" colspan=\"2\">TRƯỞNG TT HĐB-ĐPLKL</td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-npou\" colspan=\"2\">GIÁM ĐỐC</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"11\" ></td>";            
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"11\" ></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"11\" ></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"11\"><span style = \"font-weight: bold;font-style: italic;\" > Nơi nhận :</span></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"11\">-Như kính gửi</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-oqgr\" colspan=\"11\">-Lưu VT, ĐPL</td>";
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