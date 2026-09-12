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
    public partial class RpDHB_OF_DOTXUAT : System.Web.UI.Page
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
            
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"3\"><span style = \"font-weight: bold\"> BÁO CÁO CÁC CHUYẾN BAY QUÁ CẢNH ĐỘT XUẤT</span><br/><span style = \"font-weight: bold;font-size:13px;\">Tháng " + txtFromDate.Value.ToString().Replace("-"," năm ") + "</span>";
            
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
            if(!String.IsNullOrEmpty(_fromDate))
                 _dt = _DAL.DHB_GET_SLB_HHKQT_OF(_fromDate);
            DataRow _mrow = null;

            

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\"><b>STT</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Ngày bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Số hiệu</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Loại tàu bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Đăng ký</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>From</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>To</b></th>";

            _htmlContent += "<th class=\"tg-baqh\"><b>Đường bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Giờ vào</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Giờ ra</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Số phép</b></th>";
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

                            string _remark = _mrow["FPL_VIA"].ToString();
                            string _fpl = "";
                            string _fpl2 = "";
                            string _result = "";
                            if (_remark.Length > 15)
                            {
                                _result = _remark.Substring(0, 15);
                                _fpl = _remark.Remove(0,15);
                                if (_fpl.Length > 15)
                                {
                                    _result += "<br/><br/>" + _fpl.Substring(0, 15) + "<br/><br/>";
                                    _fpl2 = _fpl.Remove(0, 15);
                                    if (_fpl2.Length > 15)
                                    {
                                        _result +=  _fpl2.Substring(0, 15) + "<br/><br/>" + _fpl2.Remove(0, 15);                                        
                                    }
                                }
                                
                            }
                            else
                                _result = _mrow["FPL_VIA"].ToString();

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-baqh\">" + (j+1) + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + Convert.ToDateTime(_mrow["FLIGHTDATE"]).ToString("dd-MM-yyyy") + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["FLIGHTNBR"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["REAL_CRAFT_TYPE"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["REGISTRATION"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["FROM_AIRP"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["TO_AIRP"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;width:15%\">" + _result + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\"></td>";
                            _htmlContent += "<td class=\"tg-baqh\"></td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["PERMNBR"].ToString() + "</td>";
                            
                            _htmlContent += "</tr>";

                        }
                    }
                }

            }
                                  
            
            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }

        public string BuildContent_NewFix(string _monnth)
        {
            string _fromDate = txtFromDate.Value.ToString().Trim();

            DataTable _dt = null;
            if (!String.IsNullOrEmpty(_fromDate))
                _dt = _DAL.DHB_GET_SLB_HHKQT_OF(_fromDate);
            DataRow _mrow = null;

            int _total = 0;
            switch (_monnth)
            {
                case "01/2019":
                    _total = 996;
                    break;
                case "02/2019":

                    break;
                case "03/2019":

                    break;
                case "04/2019":

                    break;
                case "05/2019":

                    break;
                case "06/2019":

                    break;
                case "07/2019":

                    break;
                case "08/2019":

                    break;
                case "09/2019":

                    break;
                case "10/2019":

                    break;
                case "11/2019":

                    break;
                case "12/2019":

                    break;
                default:

                    break;
            }

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-baqh\"><b>STT</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Ngày bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Số hiệu</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Loại tàu bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Đăng ký</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>From</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>To</b></th>";

            _htmlContent += "<th class=\"tg-baqh\"><b>Đường bay</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Giờ vào</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Giờ ra</b></th>";
            _htmlContent += "<th class=\"tg-baqh\"><b>Số phép</b></th>";
            _htmlContent += "</tr>";

            if (!String.IsNullOrEmpty(_fromDate))
            {
                if (_dt != null)
                {

                    if (_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < _total; j++)
                        {
                            _mrow = _dt.Rows[j];

                            string _remark = _mrow["FPL_VIA"].ToString();
                            string _fpl = "";
                            string _fpl2 = "";
                            string _result = "";
                            if (_remark.Length > 15)
                            {
                                _result = _remark.Substring(0, 15);
                                _fpl = _remark.Remove(0, 15);
                                if (_fpl.Length > 15)
                                {
                                    _result += "<br/><br/>" + _fpl.Substring(0, 15) + "<br/><br/>";
                                    _fpl2 = _fpl.Remove(0, 15);
                                    if (_fpl2.Length > 15)
                                    {
                                        _result += _fpl2.Substring(0, 15) + "<br/><br/>" + _fpl2.Remove(0, 15);
                                    }
                                }

                            }
                            else
                                _result = _mrow["FPL_VIA"].ToString();

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-baqh\">" + (j + 1) + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + Convert.ToDateTime(_mrow["FLIGHTDATE"]).ToString("dd-MM-yyyy") + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["FLIGHTNBR"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["REAL_CRAFT_TYPE"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["REGISTRATION"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["FROM_AIRP"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["TO_AIRP"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\" style=\"text-align:left;width:15%\">" + _result + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\"></td>";
                            _htmlContent += "<td class=\"tg-baqh\"></td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["PERMNBR"].ToString() + "</td>";

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