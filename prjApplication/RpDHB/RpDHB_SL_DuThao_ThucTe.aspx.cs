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
    public partial class RpDHB_SL_DuThao_ThucTe : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownList();
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
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"2\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"4\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> THỐNG KÊ ĐỐI CHIẾU SỐ LIỆU DỰ THẢO / THỰC TẾ</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"2\" style=\"text-align:right;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";

            _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"2\" style=\"text-align:left;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td style = \"font-weight:bold;text-align:center;\" class=\"fontRp14\" colspan=\"4\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"4\"></td></tr></table></div>";



            return _htmlHeader;
        }
        public string BuildContent()
        {
            ReportDHBDAL _rpDAL = new ReportDHBDAL();
            DataTable _dt = _rpDAL.DHB19_Get_FLIGHTDATE(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            ReportStatisticDAL _objCC = new ReportStatisticDAL();

            

            DataRow _mrow = null;
            string _htmlContent = "";
            string _date = "";
            string _phantram = "0";
            string _duthao = "0";
            string _thucte = "0";

            string _total_phantram = "0";
            int _total_duthao = 0;
            int _total_thucte = 0;


            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 80%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:15%\">Ngày</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:30%\">Số liệu dự thảo</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:30%\">Số liệu thực tế</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:25%\">%</th>";
            _htmlContent += "</tr>";

            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];

                        _date = UltilFunc.ToDate(_mrow["FLIGHTDATE"].ToString(), "MM/dd/yyyy").ToString("dd-MM-yyyy");
                        _duthao = _objCC.THSLB_GET_KHB_DUTHAO(_date,ddlAERO.SelectedValue,ddlOPER.SelectedValue);
                        _thucte = _objCC.THSLB_GET_KHB_THUCTE(_date,ddlAERO.SelectedValue,ddlOPER.SelectedValue);
                        _phantram = UltilFunc.ReturnPhanTramInDay(_duthao, _thucte);

                        _total_duthao += Int32.Parse(_duthao);
                        _total_thucte += Int32.Parse(_thucte);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-c3ow\">"+ _date + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">"+ _duthao + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">"+ _thucte + "</td>";
                        _htmlContent += "<td class=\"tg-88nc\">"+ _phantram + "</td>";
                        _htmlContent += "</tr>";


                    }
                }
            }

            _total_phantram = UltilFunc.ReturnPhanTramInDay(_total_duthao.ToString(), _total_thucte.ToString());

            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:15%\">Cộng</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:30%\">"+ _total_duthao + "</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:30%\">"+ _total_thucte + "</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:25%\">"+ _total_phantram + "</th>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-dgz8\" colspan=\"4\"></td>";
            _htmlContent += "</tr></table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }
        public string BuildFooter()
        {
            string _htmlFooter = "";
            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"1\">NGƯỜI LẬP BIỂU</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"2\">TRƯỞNG TT ĐHB &amp; ĐP LKL</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"1\">GIÁM ĐỐC</td></tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-0dc2\" colspan=\"1\">";
            _htmlFooter += "<br><br><br><br><br><br><br>";
            _htmlFooter += "</td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"2\" rowspan=\"4\"></td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"1\" rowspan=\"4\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\"><span style = \"font-weight: bold;font-style: italic;\" > Nơi nhận :</span></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\">-Như kính gửi</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-oqgr\" colspan=\"4\">-Lưu VT, ĐPL</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-2ph3\" colspan=\"4\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-sq11\" colspan=\"1\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"2\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"1\" rowspan=\"3\"></td>";
            _htmlFooter += "</tr><tr></tr><tr></tr></table></div>";
            return _htmlFooter;
        }
        protected void LoadDropdownList()
        {
            this.FillDropdownList<Aero>(ddlAERO, new AeroDAL().GetListAeroVV(), "AE_CODE", "AE_CODE", "--");
            this.FillDropdownList<Oper>(ddlOPER, new OperDAL().GetAllObject(), "OPER_ICAO", "OPER_ICAO", "--");
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
            this.CreateExcel(html, "THSLB_DUTHAO_TT_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_DUTHAO_TT_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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