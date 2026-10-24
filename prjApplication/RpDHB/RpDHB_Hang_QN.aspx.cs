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
    public partial class RpDHB_Hang_QN : System.Web.UI.Page
    {
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
            _htmlHeader += "<td class=\"tg-5fpg1\" style =\"width:48%;\" colspan=\"5\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/><span style = \"font-weight:100;\">Số........../ BC - QLLKL</span><br></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"6\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"12\"><span style = \"font-weight: bold\"> BÁO CÁO</span>";
            _htmlHeader += "<br><span style =\"font-weight: bold\">SỐ LIỆU ĐHB THEO HÃNG CÁC HÃNG HK QUỐC NỘI</span><br/></td></tr>";           
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"5\" style=\"text-align:right;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
            _htmlHeader += "<td class=\"tg-oqgr\"></td>";
            _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"6\" style=\"text-align:left;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td style = \"font-weight:bold;text-align:center;\" class=\"fontRp14\" colspan=\"12\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"12\"></td></tr></table></div>";
            
            return _htmlHeader;
        }
        public string BuildContent()
        {
            ReportDHBDAL _rpDAL = new ReportDHBDAL();
            DataTable _dt = null;
            _dt = _rpDAL.BCDHB03_Get_HHKVN(txtFromDate.Value.ToString(), txtToDate.Value.ToString());
            DataRow _mrow = null;

            string strPAX = "0"; string strVIP = "0"; string strFER = "0"; string strQRF = "0";string strTotal = "0";
            string strPAX_QT = null; string strVIP_QT = "0"; string strFER_QT = "0"; string strQRF_QT = "0"; string strTotal_QT = "0"; string strTotal_ALL = "0";

            double strPAX_Total = 0; double strVIP_Total = 0; double strFER_Total = 0; double strQRF_Total = 0; double strTotal_Sum = 0;

            double strPAX_Total_QT = 0; double strVIP_Total_QT = 0; double strFER_Total_QT = 0; double strQRF_Total_QT = 0; double strTotal_Sum_QT = 0;

            double strTotalALL = 0;
            string _dateThang = "";
            if (txtFromDate.Value.Trim().Length > 0)
            {
                DateTime _dtime = UltilFunc.ToDate(txtFromDate.Value.Trim().Replace("-", "/"), "dd/MM/yyyy");
                _dateThang = String.Format("{0:MM/yyyy}", _dtime);                
            }

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";           
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-88nc\" rowspan=\"2\">OPER</th>";
            _htmlContent += "<th class=\"tg-88nc\" colspan=\"5\">Domestic</th>";
            _htmlContent += "<th class=\"tg-7btt\" colspan=\"5\">International</th>";
            _htmlContent += "<th class=\"tg-7btt\" rowspan=\"2\"><br>SUM<br>ALL<br><br></th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";            
            _htmlContent += "<td class=\"tg-c3ow\">PAX</td>";
            _htmlContent += "<td class=\"tg-c3ow\">VIP</td>";
            _htmlContent += "<td class=\"tg-c3ow\">FER</td>";
            _htmlContent += "<td class=\"tg-c3ow\">OTHER</td>";
            _htmlContent += "<td class=\"tg-c3ow\"><b>SUM</b></td>";
            _htmlContent += "<td class=\"tg-c3ow\">PAX</td>";
            _htmlContent += "<td class=\"tg-c3ow\">VIP</td>";
            _htmlContent += "<td class=\"tg-c3ow\">FER</td>";
            _htmlContent += "<td class=\"tg-c3ow\">OTHER</td>";
            _htmlContent += "<td class=\"tg-c3ow\"><b>SUM</b></td>";
            _htmlContent += "</tr>";
            
            if (_dt != null)
            {

                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j<_dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];

                         strPAX = _rpDAL.DHB_HQN_SumQN_MUCDICH(_mrow["OPER_ID"].ToString(), "PAX", txtFromDate.Value.ToString(), txtToDate.Value.ToString());
                        strVIP = _rpDAL.DHB_HQN_SumQN_MUCDICH(_mrow["OPER_ID"].ToString(),"VIP", txtFromDate.Value.ToString(), txtToDate.Value.ToString());

                          strFER = _rpDAL.DHB_HQN_SumQN_MUCDICH(_mrow["OPER_ID"].ToString(), "FER", txtFromDate.Value.ToString(), txtToDate.Value.ToString());


                        strQRF = _rpDAL.DHB_HQN_SumQN_MUCDICH(_mrow["OPER_ID"].ToString(),"QRF", txtFromDate.Value.ToString(), txtToDate.Value.ToString());

                        strTotal = (Convert.ToInt32(strPAX) + Convert.ToInt32(strVIP) + Convert.ToInt32(strFER) + Convert.ToInt32(strQRF)).ToString();

                         strPAX_QT = _rpDAL.DHB_HQN_SumQT_MUCDICH(_mrow["OPER_ID"].ToString(), "PAX", txtFromDate.Value.ToString(), txtToDate.Value.ToString());

                        strVIP_QT = _rpDAL.DHB_HQN_SumQT_MUCDICH(_mrow["OPER_ID"].ToString(), "VIP", txtFromDate.Value.ToString(), txtToDate.Value.ToString());
                        strFER_QT = _rpDAL.DHB_HQN_SumQT_MUCDICH(_mrow["OPER_ID"].ToString(), "FER", txtFromDate.Value.ToString(), txtToDate.Value.ToString());

                        strQRF_QT = _rpDAL.DHB_HQN_SumQT_MUCDICH(_mrow["OPER_ID"].ToString(), "QRF", txtFromDate.Value.ToString(), txtToDate.Value.ToString());

                        strTotal_QT = (Convert.ToInt32(strPAX_QT) + Convert.ToInt32(strVIP_QT) + Convert.ToInt32(strFER_QT) + Convert.ToInt32(strQRF_QT)).ToString();

                        strTotal_ALL = (Convert.ToInt32(strTotal) + Convert.ToInt32(strTotal_QT)).ToString();

                        strPAX_Total += Convert.ToDouble(strPAX);
                        strVIP_Total += Convert.ToDouble(strVIP);
                        strFER_Total += Convert.ToDouble(strFER);
                        strQRF_Total += Convert.ToDouble(strQRF);
                        strTotal_Sum += Convert.ToDouble(strTotal);

                        strPAX_Total_QT += Convert.ToDouble(strPAX_QT);
                        strVIP_Total_QT += Convert.ToDouble(strVIP_QT);
                        strFER_Total_QT += Convert.ToDouble(strFER_QT);
                        strQRF_Total_QT += Convert.ToDouble(strQRF_QT);
                        strTotal_Sum_QT += Convert.ToDouble(strTotal_QT);

                        strTotalALL += Convert.ToDouble(strTotal_ALL);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-uys7\">" + _mrow["OPER_ID"].ToString() + "</td>";
                        _htmlContent += "<td class=\"tg-uys7\">"+ strPAX + "</td>";
                        _htmlContent += "<td class=\"tg-uys7\">"+ strVIP + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">"+ strFER + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">"+ strQRF + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\"><b>"+ strTotal + "</b></td>";
                        _htmlContent += "<td class=\"tg-c3ow\">"+ strPAX_QT + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">" + strVIP_QT + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">" + strFER_QT + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\">" + strQRF_QT + "</td>";
                        _htmlContent += "<td class=\"tg-c3ow\"><b>"+ strTotal_QT + "</b></td>";
                        _htmlContent += "<td class=\"tg-c3ow\"><b>"+ strTotal_ALL + "</b></td>";
                        _htmlContent += "</tr>";
                    }
                }
            }
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-uys7\"><b>SUM</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strPAX_Total + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strVIP_Total + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strFER_Total + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strQRF_Total + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strTotal_Sum + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strPAX_Total_QT + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strVIP_Total_QT + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strFER_Total_QT + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strQRF_Total_QT + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strTotal_Sum_QT + "</b></td>";
            _htmlContent += "<td class=\"tg-uys7\"><b>" + strTotalALL + "</b></td>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-dgz8\" colspan=\"12\"></td>";
            _htmlContent += "</tr></table>";           
            _htmlContent +="</div>";
            return _htmlContent;
        }
        public string BuildFooter()
        {
            string _htmlFooter = "";
            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tg\" style=\"width:100%;border:none;border-color:white;\">";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"12\" style=\"height:14px;\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"4\">NGƯỜI LẬP BIỂU</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"4\">TRUNG TÂM HĐB &amp; ĐP LKL</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"4\">GIÁM ĐỐC</td></tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-0dc2\" colspan=\"4\">";
            _htmlFooter += "<br><br><br><br><br><br><br>";
            _htmlFooter += "</td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\" rowspan=\"4\"></td>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\" rowspan=\"4\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\" style = \"font-size:14px;line-height:4px;\"><span style = \"font-weight: bold;font-style:italic;font-size:14px;\" > Nơi nhận :</span></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-dgz8\" colspan=\"4\" style = \"font-size:14px;line-height:4px;\">-Như kính gửi</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-oqgr\" colspan=\"4\" style = \"font-size:14px;line-height:4px;\">-Lưu VT, ĐPL</td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-2ph3\" colspan=\"12\"></td>";
            _htmlFooter += "</tr>";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-sq11\" colspan=\"4\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"4\" rowspan=\"3\"></td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"4\" rowspan=\"3\"></td>";
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
            this.CreateExcel(html, "THSLB_HANG_QN_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_HANG_QN_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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