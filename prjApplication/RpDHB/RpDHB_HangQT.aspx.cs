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
    public partial class RpDHB_HangQT : System.Web.UI.Page
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
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:50%;\" colspan=\"2\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/>Số........../ BC - QLLKL<br></th>";
            _htmlHeader += "</td>";
            
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"2\" style =\"width:50%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style:italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"4\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SỐ LIỆU ĐHB CÁC HÃNG HK QUỐC TẾ ĐI ĐẾN</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"2\" style=\"text-align:right;font-weight:bold;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";            
            _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"2\" style=\"text-align:left;font-weight:bold;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
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
            string _qtmb = "0"; string _qtmt = "0"; string _qtmn = "0"; int _total1 = 0;
            _qtmb = _DAL.DHB_Fun_SUM_CBQT_BY_MIEN("MB",txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _qtmt = _DAL.DHB_Fun_SUM_CBQT_BY_MIEN("MT", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _qtmn = _DAL.DHB_Fun_SUM_CBQT_BY_MIEN("MN", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _total1 = Int32.Parse(_qtmb)+ Int32.Parse(_qtmt)+ Int32.Parse(_qtmn);

            string _qnmb = "0"; string _qnmt = "0"; string _qnmn = "0"; int _total2 = 0;
            _qnmb = _DAL.DHB_Fun_SUM_CBQN_BY_MIEN("MB", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _qnmt = _DAL.DHB_Fun_SUM_CBQN_BY_MIEN("MT", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _qnmn = _DAL.DHB_Fun_SUM_CBQN_BY_MIEN("MN", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _total2 = Int32.Parse(_qnmb) + Int32.Parse(_qnmt) + Int32.Parse(_qnmn);


            string _qnmb1 = "0"; string _qnmt1 = "0"; string _qnmn1 = "0"; int _total3 = 0;

            _qnmb1 = _DAL.DHB_Fun_SUM_CBQN_BY_MIEN_HA("MB", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _qnmt1 = _DAL.DHB_Fun_SUM_CBQN_BY_MIEN_HA("MT", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _qnmn1 = _DAL.DHB_Fun_SUM_CBQN_BY_MIEN_HA("MN", txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());

            _total3 = Int32.Parse(_qnmb1) + Int32.Parse(_qnmt1) + Int32.Parse(_qnmn1);

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tgblue\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:10%\">I</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:30%;text-align:left;\">Chuyến bay quốc tế</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:20%\">Số chuyến</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:40%\">Ghi chú</th>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">1</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Bắc</td>";
            _htmlContent += "<td class=\"tg-88nc\">"+ string.Format("{0:#,#}",double.Parse(_qtmb.ToString())).Replace(".", ",") + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">2</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Trung</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qtmt.ToString())).Replace(".", ",") + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">3</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Nam</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qtmn.ToString())).Replace(".", ",")  + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<th style=\"width:10%;background-color: white;\"></th>";
            _htmlContent += "<th style=\"width:30%;background-color: white;text-align:left;\"><b>Cộng</b></th>";
            _htmlContent += "<th style=\"width:20%;background-color: white;\"><b>"+ string.Format("{0:#,#}", double.Parse(_total1.ToString())).Replace(".", ",")  + "</b></th>";
            _htmlContent += "<th style=\"width:40%;background-color: white;\"></th>";
            _htmlContent += "</tr>";




            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:10%\">II</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:30%;text-align:left;\">Chuyến bay quốc nội cất</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:20%\">Số chuyến</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:40%\">Ghi chú</th>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">1</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Bắc</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qnmb.ToString())).Replace(".", ",") + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">2</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Trung</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qnmt.ToString())).Replace(".", ",") + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">3</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\"> Miền Nam</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qnmn.ToString())).Replace(".", ",") + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<th style=\"width:10%;background-color: white;\"></th>";
            _htmlContent += "<th style=\"width:30%;background-color: white;text-align:left;\"><b>Cộng</b></th>";
            _htmlContent += "<th style=\"width:20%;background-color: white;\"><b>" + string.Format("{0:#,#}", double.Parse(_total2.ToString())).Replace(".", ",") + "</b></th>";
            _htmlContent += "<th style=\"width:40%;background-color: white;\"></th>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:10%\">III</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:30%;text-align:left;\">Chuyến bay quốc nội hạ</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:20%\">Số chuyến</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:40%\">Ghi chú</th>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">1</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Bắc</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qnmb1.ToString())).Replace(".", ",")  + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">2</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\">Miền Trung</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qnmt1.ToString())).Replace(".", ",")  + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\">3</td>";
            _htmlContent += "<td class=\"tg-88nc\" style=\"text-align:left;\"> Miền Nam</td>";
            _htmlContent += "<td class=\"tg-88nc\">" + string.Format("{0:#,#}", double.Parse(_qnmn1.ToString())).Replace(".", ",") + "</td>";
            _htmlContent += "<td class=\"tg-88nc\"></td>";
            _htmlContent += "</tr>";

            _htmlContent += "<tr>";
            _htmlContent += "<th style=\"width:10%;background-color: white;\"></th>";
            _htmlContent += "<th style=\"width:30%;background-color: white;text-align:left;\"><b>Cộng</b></th>";
            _htmlContent += "<th style=\"width:20%;background-color: white;\"><b>" + string.Format("{0:#,#}", double.Parse(_total3.ToString())).Replace(".", ",")  + "</b></th>";
            _htmlContent += "<th style=\"width:40%;background-color: white;\"></th>";
            _htmlContent += "</tr>";

            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }
        public string BuildFooter()
        {
            string _htmlFooter = "";
            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tg\" style=\"width:100%;border:none;border-color:white;\">";
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
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"1\"><span style = \"font-weight:bold;font-style:italic;\" > Nơi nhận :</span></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"1\">-Như kính gửi</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-oqgr\" colspan=\"1\">-Lưu VT, ĐPL</td>";
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
            this.CreateExcel(html, "THSLB_BCSL_HQT_LD_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_BCSL_HQT_LD_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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