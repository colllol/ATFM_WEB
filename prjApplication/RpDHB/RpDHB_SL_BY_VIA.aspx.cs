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
    public partial class RpDHB_SL_BY_VIA : System.Web.UI.Page
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
               
                ltrHeader.Text = BuildHeader();
                ltrContent.Text = BuildContent_New();
                ltrFooter.Text = BuildFooter();
            }
        }
        
        public string BuildHeader()
        {
            _datekhoang = " Tháng " + txtFromDate.Value.Trim();

            string _htmlHeader = "";
            
            _htmlHeader = "<div id = \"headerID\" style = \"text-align: center;\">";
            _htmlHeader += "<table class=\"tgheard\" style=\"width: 100%\">";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:48%;\" colspan=\"3\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"3\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"7\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SỐ LIỆU BAY THEO ĐƯỜNG HÀNG KHÔNG</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-c7ws\" colspan=\"7\" style=\"text-align:left;\">Kỳ báo cáo : " + _datekhoang + "</td>";
            _htmlHeader += "</tr>";

            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-c7ws\" colspan=\"7\" style=\"text-align:left;\">Ngày báo cáo : Ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString() + " </td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"7\"></td></tr></table></div>";


            return _htmlHeader;
        }


        public string BuildContent_New()
        {
            string _dateFrom = txtFromDate.Value.ToString().Trim();
            string _time = txtFromDate.Value.ToString().Trim().Replace("-","/");

            string _thangtruoc = "";            
            DataTable _dtRoute = null;
            int days1 = 0; int days2 = 0;
            DataRow _mrow = null;
            if (!String.IsNullOrEmpty(_dateFrom))
            {
                _dtRoute = _DAL.GET_ALL_ROUTE_NAME();
                _thangtruoc = UltilFunc.ReturnMonthBefore(_dateFrom);
                _dtnew = _DAL.DHB_GET_SLB_BY_VIA_MONTH(_dateFrom.ToString().Trim(), _thangtruoc.ToString().Trim());
                days1 = UltilFunc.ReturnDayInMonth(_dateFrom);
                days2 = UltilFunc.ReturnDayInMonth(_thangtruoc);
            }

            string _tongthang = "0"; string _tongthangtruoc = "0";
            string _tongngay1 = "0"; string _tongngay2 = "0";string _phantram = "0%";


            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tgrpbyvia\" style=\"width:100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-hgcj fontRp14\" rowspan=\"2\">STT</th>";
            _htmlContent += "<th class=\"tg-hgcj fontRp14\" rowspan=\"2\">Tên đường hàng không</th>";
            _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Thực hiện tháng " + _dateFrom + " ("+ days1 + "/ngày)</th>";
            _htmlContent += "<th class=\"tg-hgcj fontRp14\" colspan=\"2\">Thực hiện tháng trước " + _thangtruoc + " ("+ days2 + "/ngày)</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" rowspan=\"2\">So sánh % / Ngày</th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-hgcj fontRp14\">Thực hiện tháng  " + _dateFrom + " </td>";
            _htmlContent += "<td class=\"tg-hgcj fontRp14\">Số chuyến / Ngày</td>";
            _htmlContent += "<td class=\"tg-hgcj fontRp14\">Thực hiện tháng " + _thangtruoc + " </td>";
            _htmlContent += "<td class=\"tg-amwm fontRp14\">Số chuyến / Ngày</td>";
            _htmlContent += "</tr>";

            if (_dtRoute != null)
            {
                if (_dtRoute.Rows.Count > 0)
                {
                    for (int j = 0; j < _dtRoute.Rows.Count; j++)
                    {
                        _mrow = _dtRoute.Rows[j];

                        _tongthang = GetValue(_mrow["ROUTE_NAME"].ToString(), "SCTHANG");
                        _tongthangtruoc = GetValue(_mrow["ROUTE_NAME"].ToString(), "SCTHANGTRUOC");
                        if ((_tongthang != "0") && (_tongthangtruoc != "0"))
                        {

                            _tongngay1 = UltilFunc.ReturnTotalInDay(_tongthang, days1);
                            _tongngay2 = UltilFunc.ReturnTotalInDay(_tongthangtruoc, days2);

                            _phantram = UltilFunc.ReturnPhanTramInDay(_tongngay2, _tongngay1);

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-s6z2 fontRp14\">" + (j + 1) + "</td>";
                            _htmlContent += "<td class=\"tg-s6z2 fontRp14\"><b>" + _mrow["ROUTE_NAME"].ToString() + "</b></td>";
                            _htmlContent += "<td class=\"tg-s6z2 fontRp14\">" + _tongthang + "</td>";
                            _htmlContent += "<td class=\"tg-s6z2 fontRp14\">" + _tongngay1 + "</td>";
                            _htmlContent += "<td class=\"tg-s6z2 fontRp14\">" + _tongthangtruoc + "</td>";
                            _htmlContent += "<td class=\"tg-baqh fontRp14\">" + _tongngay2 + "</td>";
                            _htmlContent += "<td class=\"tg-amwm fontRp14\">" + _phantram + "</td>";
                            _htmlContent += "</tr>";
                        }
                        
                    }
                }
            }


            
            _htmlContent += "</table>";
            
            return _htmlContent;
        }

        public string GetValue(string via, string type)
        {
            string _return = "0";
            try
            {
                if (_dtnew != null)
                {
                    if (_dtnew.Rows.Count > 0)
                    {
                        for (int i = 0; i < _dtnew.Rows.Count; i++)
                        {
                            if ((via.Trim() == _dtnew.Rows[i]["ROUTE"].ToString().Trim()) && (_dtnew.Rows[i][type].ToString() != "0"))
                            {
                                _return = _dtnew.Rows[i][type].ToString();
                                break;
                            }

                        }
                    }
                    else
                        _return = "0";
                }
            }
            catch (Exception ex)
            {
                _return = "0";
                throw ex;

            }
            return _return;
        }

        public string BuildFooter()
        {
            string _htmlFooter = "";
            //_htmlFooter += "<div id = \"footer\">";
            //_htmlFooter += "<table class=\"tgfooter\" style=\"width:100%;border:none;border-color:white;\">";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"7\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"7\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<th class=\"tg-mmgy\" colspan=\"3\">ĐẠI DIỆN CÔNG TY QLB MIỀN BẮC<br/>";
            //_htmlFooter += "</th>";
            //_htmlFooter += "<th class=\"tg-qjg1\"></th>";
            //_htmlFooter += "<th class=\"tg-npou\" colspan=\"3\">ĐẠI DIỆN TRUNG TÂM QLLKL</th>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-npou\" colspan=\"2\">TRƯỞNG TT KS ĐƯỜNG DÀI</td>";
            //_htmlFooter += "<td class=\"tg-npou\">GIÁM ĐỐC</td>";
            //_htmlFooter += "<td class=\"tg-qjg1\"></td>";
            //_htmlFooter += "<td class=\"tg-npou\" colspan=\"2\">TRƯỞNG TT HĐB-ĐPLKL</td>";
            //_htmlFooter += "<td class=\"tg-npou\">GIÁM ĐỐC</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"7\"></td>";           
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"7\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-qjg1\" colspan=\"7\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\"><span style = \"font-weight: bold;font-style: italic;\" > Nơi nhận :</span></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"7\">-Như kính gửi</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-oqgr\" colspan=\"7\">-Lưu VT, ĐPL</td>";
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