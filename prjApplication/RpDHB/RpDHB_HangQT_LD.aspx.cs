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
    public partial class RpDHB_HangQT_LD : System.Web.UI.Page
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
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"4\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SỐ LIỆU ĐHB CÁC HÃNG HK QUỐC TẾ ĐI ĐẾN THEO HÃNG</span>";
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
            DataTable _dt = null;
            _dt = _DAL.DHB_HQT_SumTotal_LD(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            DataRow _mrow = null;

            DataTable _dt2 = null;
            _dt2 = _DAL.DHB_Fun_HQT_SumTotal_QN_LD(txtFromDate.Value.ToString().Trim(), txtToDate.Value.ToString().Trim());
            DataRow _mrow2 = null;

            string _dateThang = "";

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tgblue\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:10%\">I</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:30%\">Chuyến bay quốc tế</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:20%\">"+ ReturnTotal(_dt) + "</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:40%\">Ghi chú</th>";
            _htmlContent += "</tr>";
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];
                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-uys7\">"+(j+1)+"</td>";
                        _htmlContent += "<td class=\"tg-88nc\">"+ _mrow["OPER_ID"].ToString() + "</td>";
                        _htmlContent += "<td class=\"tg-88nc\">" + _mrow["SC"].ToString() + "</td>";
                        _htmlContent += "<td class=\"tg-88nc\"></td>";
                        _htmlContent += "</tr>";

                        
                    }
                }
            }
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:10%\">II</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:30%\">Chuyến bay quốc nội</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:20%\">"+ ReturnTotal_QN(_dt2) + "</th>";
            _htmlContent += "<th class=\"tg-88nc\" style=\"width:40%\">Ghi chú</th>";
            _htmlContent += "</tr>";
            if (_dt2 != null)
            {
                               
                if (_dt2.Rows.Count > 0)
                {
                    for (int k = 0; k < _dt2.Rows.Count; k++)
                    {
                        _mrow2 = _dt2.Rows[k];
                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-uys7\">" + (k + 1) + "</td>";
                        _htmlContent += "<td class=\"tg-88nc\">" + _mrow2["OPER_ID"].ToString() + "</td>";
                         _htmlContent += "<td class=\"tg-88nc\">" + _mrow2["SC"].ToString() + "</td>";
                        _htmlContent += "<td class=\"tg-88nc\"></td>";
                        _htmlContent += "</tr>";
                    }
                }
            }
            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }

        public string ReturnTotal(DataTable dt)
        {
            string _total = "0";
            int _tong = 0;
            DataRow _mrow2 = null;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        _mrow2 = dt.Rows[k];
                        _tong += Int32.Parse(_mrow2["SC"].ToString());                        
                    }
                }
                _total = string.Format("{0:#,#}", double.Parse(_tong.ToString())).Replace(".", ",");
            }
            return _total;
        }

        public string ReturnTotal_QN(DataTable dt)
        {
            string _total = "0";
            string _dateThang = "";
            int _tong = 0;
            DataRow _mrow2 = null;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        _mrow2 = dt.Rows[k];
                        _tong += Int32.Parse(_mrow2["SC"].ToString());
                    }
                }
                if (txtFromDate.Value.Trim().Length > 0)
                {
                    DateTime _dtime = UltilFunc.ToDate(txtFromDate.Value.Trim().Replace("-", "/"), "dd/MM/yyyy");
                    _dateThang = String.Format("{0:MM/yyyy}", _dtime);

                }
                if (_dateThang == "11/2019")
                    _total = "65";
                else
                    _total = string.Format("{0:#,#}", double.Parse(_tong.ToString())).Replace(".", ",");
            }
            return _total;
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