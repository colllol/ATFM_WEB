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
    public partial class RpDHB_SL_CatHa_TT : System.Web.UI.Page
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
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"4\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO THEO KHUNG GIỜ BAY THEO SL THỰC TẾ</span>";
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
            ReportStatisticDAL _objCC = new ReportStatisticDAL();
            //DataTable _dt = _objCC.THSLB_GET_FINISH_SLB(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue,ddlOPER.SelectedValue, txtFromTime.Value, txtToTime.Value, ddlTime.Value);

            DataTable _dt = _objCC.THSLB_GET_FINISH_KHUNGGIO(txtFromDate.Value, txtToDate.Value, txtFromTime.Value, txtToTime.Value);


            DataRow _mrow = null;
            string _htmlContent = "";
            string _TotalCat = "0";
            string _TotalHa = "0";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:25%\">Khung giờ bay</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:25%\">Cất cánh</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:25%\">Hạ cánh</th>";
            _htmlContent += "<th class=\"tg-7btt\" style=\"width:25%\">Tổng</th>";
            _htmlContent += "</tr>";

            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];

                        if(ddlTime.Value=="ALL")
                        {
                            _TotalCat = _objCC.THSLB_GET_FINISH_CAT_ATD(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue, ddlOPER.SelectedValue, _mrow["sf"].ToString(), _mrow["st"].ToString());
                            _TotalHa = _objCC.THSLB_GET_FINISH_HA_ATA(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue, ddlOPER.SelectedValue, _mrow["sf"].ToString(), _mrow["st"].ToString());
                        }
                        else if (ddlTime.Value == "ATD")
                        {
                            _TotalCat = _objCC.THSLB_GET_FINISH_CAT_ATD(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue, ddlOPER.SelectedValue, _mrow["sf"].ToString(), _mrow["st"].ToString());
                            _TotalHa = "0";// _objCC.THSLB_GET_FINISH_HA_ATD(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue, ddlOPER.SelectedValue, _mrow["sf"].ToString(), _mrow["st"].ToString());
                        }
                        else
                        {
                            _TotalCat = "0";// _objCC.THSLB_GET_FINISH_CAT_ATA(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue, ddlOPER.SelectedValue, _mrow["sf"].ToString(), _mrow["st"].ToString());
                            _TotalHa = _objCC.THSLB_GET_FINISH_HA_ATA(txtFromDate.Value, txtToDate.Value, ddlAERO.SelectedValue, ddlOPER.SelectedValue, _mrow["sf"].ToString(), _mrow["st"].ToString());
                        }


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-c3ow\">" + _mrow["sf"].ToString() +" - " + _mrow["st"].ToString()+ "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">" + _TotalCat.ToString() + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">" + _TotalHa.ToString() + "</td>";
                        _htmlContent += "<td class=\"tg-88nc\">" + (Convert.ToInt32(_TotalCat.ToString()) + Convert.ToInt32(_TotalHa.ToString())).ToString() + "</td>";
                        _htmlContent += "</tr>";
                    }
                }
            }

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
            this.CreateExcel(html, "THSLB_KHUNGGIO_TT_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_KHUNGGIO_TT_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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