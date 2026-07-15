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
    public partial class BDC_SL_Bay_Hang_QN : System.Web.UI.Page
    {
        ReportDHBDAL _DAL = new ReportDHBDAL();
        public DataTable _dtnew = null;

        ReportStatisticDAL _DAL2 = new ReportStatisticDAL();
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
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg1\" style =\"width:48%;\" colspan=\"9\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/><span style = \"font-weight:100;\">Số........../ BC - QLLKL</span><br></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"9\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style: italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            if (ddlMien.Value == "1")
            {
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span class=\"fontRp14\" style = \"font-weight: bold\"> BẢNG ĐỐI CHIẾU SỐ LIỆU CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC</span><br/><span class=\"fontRp14\" style = \"font-weight: bold;\">CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MIỀN BẮC</span></td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span class=\"fontRp14\" style = \"font-weight: bold\">Từ ngày : " + txtFromDate.Value.ToString() + "   " + "  Đến ngày:" + txtToDate.Value.ToString() + " </span>";
            }
            else if (ddlMien.Value == "2")
            {
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span style = \"font-weight: bold\"> BẢNG ĐỐI CHIẾU SỐ LIỆU CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC</span><br/><span class=\"fontRp14\" style = \"font-weight: bold;\">CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MIỀN TRUNG</span></td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span class=\"fontRp14\" style = \"font-weight: bold\">Từ ngày : " + txtFromDate.Value.ToString() + "   " + "  Đến ngày:" + txtToDate.Value.ToString() + " </span>";
            }
            else if (ddlMien.Value == "3")
            {
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span class=\"fontRp14\" style = \"font-weight: bold\"> BẢNG ĐỐI CHIẾU SỐ LIỆU CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC</span><br/><span class=\"fontRp14\" style = \"font-weight: bold;\">CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MIỀN NAM</span></td>";
                _htmlHeader += "</tr>";
                _htmlHeader += "<tr>";
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span class=\"fontRp14\" style = \"font-weight: bold\">Từ ngày : " + txtFromDate.Value.ToString() + "   " + "  Đến ngày:" + txtToDate.Value.ToString() + " </span>";
            }
            else
                _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"><span class=\"fontRp14\" style = \"font-weight: bold\"> BẢNG ĐỐI CHIẾU SỐ LIỆU CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC</span><br/><span class=\"fontRp14\" style = \"font-weight: bold;\">Từ ngày : " + txtFromDate.Value.ToString() + "   " + " Đến ngày:" + txtToDate.Value.ToString() + "</span></td>";
               

            _htmlHeader += "</td></tr>";
            
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"fontRp14\" colspan=\"19\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"19\"></td></tr></table></div>";
            return _htmlHeader;
        }

     
        public string BuildContent_New()
        {
            string _fromDate = txtFromDate.Value.ToString().Trim();
            string _toDate = txtFromDate.Value.ToString().Trim();

            DataTable _dt = null;
            if (ddlMien.Value == "1")
            {
                _dt = _DAL.DHB_GET_SB_BYMIEN("MB");
                //_dtnew = _DAL.DHB_GET_SLB_HHKVN("MB", _fromDate);
                _dtnew = _DAL2.DHB_GET_SLB_HHKVN_KTG("MB", _fromDate, _toDate);
            }
            else if (ddlMien.Value == "2")
            {
                _dt = _DAL.DHB_GET_SB_BYMIEN("MT");
                //_dtnew = _DAL.DHB_GET_SLB_HHKVN("MT", _fromDate);
                _dtnew = _DAL2.DHB_GET_SLB_HHKVN_KTG("MT", _fromDate, _toDate);
            }
            else if (ddlMien.Value == "3")
            {
                _dt = _DAL.DHB_GET_SB_BYMIEN("MN");
                //_dtnew = _DAL.DHB_GET_SLB_HHKVN("MN", _fromDate);
                _dtnew = _DAL2.DHB_GET_SLB_HHKVN_KTG("MN", _fromDate, _toDate);
            }
            else
            {
                _dt = _DAL.DHB_GET_SB_BYMIEN("");
                //_dtnew = _DAL.DHB_GET_SLB_HHKVN("", _fromDate);
                _dtnew = _DAL2.DHB_GET_SLB_HHKVN_KTG("", _fromDate, _toDate);
            }
           
            DataRow _mrow = null;

            int _total1 = 0; int _total12 = 0; int _total13 = 0;
            int _total2 = 0; int _total22 = 0; int _total23 = 0;
            int _total3 = 0; int _total32 = 0; int _total33 = 0;
            int _total4 = 0; int _total42 = 0; int _total43 = 0;
            int _total5 = 0; int _total52 = 0; int _total53 = 0;

            int _sumqn1 = 0; int _sumqt1 = 0; int _sumqtv1 = 0;

            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-hgcj fontRp14\" rowspan=\"2\">SÂN BAY</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" colspan=\"3\">HVN</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" colspan=\"3\">VJC</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" colspan=\"3\">PIC</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" colspan=\"3\">VFC</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" colspan=\"3\"> BAV</th>";
            _htmlContent += "<th class=\"tg-amwm fontRp14\" colspan=\"3\">SUM ALL</th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QN</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QT</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QT về</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QN</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QT</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QT về</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QN</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QT</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QT về</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QN</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QT</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QT về</td>";

            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QN</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QT</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QT về</td>";

            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QN</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QN đi QT</td>";
            _htmlContent += "<td class=\"tg-baqh fontRp14\">QT về</td>";
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

                            string _qn1 = "0"; string _qt1 = "0"; string _qtv1 = "0"; 
                            string _qn2 = "0"; string _qt2 = "0"; string _qtv2 = "0"; 
                            string _qn3 = "0"; string _qt3 = "0"; string _qtv3 = "0"; 
                            string _qn4 = "0"; string _qt4 = "0"; string _qtv4 = "0"; 
                            string _qn5 = "0"; string _qt5 = "0"; string _qtv5 = "0"; 

                            int _sumqn = 0; int _sumqt = 0; int _sumqtv = 0;
                            

                            _qn1 = GetValue(_mrow["AE_CODE"].ToString(),"HVN", "QNQN");
                            _qt1 = GetValue(_mrow["AE_CODE"].ToString(), "HVN", "QNQT");
                            _qtv1 = GetValue(_mrow["AE_CODE"].ToString(), "HVN", "QTVE");
                            _total1 += Int32.Parse(_qn1);
                            _total12 += Int32.Parse(_qt1);
                            _total13 += Int32.Parse(_qtv1);

                            _qn2 = GetValue(_mrow["AE_CODE"].ToString(), "VJC", "QNQN");
                            _qt2 = GetValue(_mrow["AE_CODE"].ToString(), "VJC", "QNQT");
                            _qtv2 = GetValue(_mrow["AE_CODE"].ToString(), "VJC", "QTVE");

                            _total2 += Int32.Parse(_qn2);
                            _total22 += Int32.Parse(_qt2);
                            _total23 += Int32.Parse(_qtv2);


                            _qn3 = GetValue(_mrow["AE_CODE"].ToString(), "PIC", "QNQN"); 
                            _qt3 = GetValue(_mrow["AE_CODE"].ToString(), "PIC", "QNQT");
                            _qtv3 = GetValue(_mrow["AE_CODE"].ToString(), "PIC", "QTVE");

                            _total3 += Int32.Parse(_qn3);
                            _total32 += Int32.Parse(_qt3);
                            _total33 += Int32.Parse(_qtv3);

                            _qn4 = GetValue(_mrow["AE_CODE"].ToString(), "VFC", "QNQN");
                            _qt4 = GetValue(_mrow["AE_CODE"].ToString(), "VFC", "QNQT");
                            _qtv4 = GetValue(_mrow["AE_CODE"].ToString(), "VFC", "QTVE");

                            _total4 += Int32.Parse(_qn4);
                            _total42 += Int32.Parse(_qt4);
                            _total43 += Int32.Parse(_qtv4);

                            _qn5 = GetValue(_mrow["AE_CODE"].ToString(), "BAV", "QNQN");
                            _qt5 = GetValue(_mrow["AE_CODE"].ToString(), "BAV", "QNQT");
                            _qtv5 = GetValue(_mrow["AE_CODE"].ToString(), "BAV", "QTVE");

                            _total5 += Int32.Parse(_qn5);
                            _total52 += Int32.Parse(_qt5);
                            _total53 += Int32.Parse(_qtv5);



                            _sumqn = Int32.Parse(_qn1) + Int32.Parse(_qn2) + Int32.Parse(_qn3) + Int32.Parse(_qn4) + Int32.Parse(_qn5);
                            _sumqt = Int32.Parse(_qt1) + Int32.Parse(_qt2) + Int32.Parse(_qt3) + Int32.Parse(_qt4) + Int32.Parse(_qt5);
                            _sumqtv = Int32.Parse(_qtv1) + Int32.Parse(_qtv2) + Int32.Parse(_qtv3) + Int32.Parse(_qtv4) + Int32.Parse(_qtv5);

                            _sumqn1 += _sumqn;
                            _sumqt1 += _sumqt;
                            _sumqtv1 += _sumqtv;

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["AE_CODE"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qtv1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qtv2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qtv3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qtv4 + "</td>";

                            _htmlContent += "<td class=\"tg-baqh\">" + _qn5 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt5 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qtv5 + "</td>";

                            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqn + "</b></td>";
                            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqt + "</b></td>";
                            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqtv + "</b></td>";
                            _htmlContent += "</tr>";

                        }
                    }
                }

            }
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\"><b>TỔNG</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total1 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total12 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total13 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total2 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total22 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total23 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total3 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total32 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total33 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total4 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total42 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total43 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total5 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total52 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _total53 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqn1 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqt1 + "</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqtv1 + "</b></td>";
            _htmlContent += "</tr>";

            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }


        public string GetValue(string aero,string oper,string type)
        {
            string _return = "0";
            try
            {
                if (_dtnew.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtnew.Rows.Count; i++)
                    {
                        if((aero.Trim() == _dtnew.Rows[i]["AERO"].ToString().Trim())&& (oper.Trim() == _dtnew.Rows[i]["OPER"].ToString().Trim())&&(_dtnew.Rows[i][type].ToString()!="0"))
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



        public string BuildFooter()
        {
            string _htmlFooter = "";

            if (ddlMien.Value == "1")
            {
                _htmlFooter += "<div id = \"footer\">";
                _htmlFooter += "<table class=\"tgfooter\" style=\"width:100%;border:none;border-color:white;\">";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td colspan=\"19\" style=\"height:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<th class=\"tg-mmgy\" colspan=\"2\" style=\"font-size:14px;\"> ĐẠI DIỆN CÔNG TY QLB MIỀN BẮC<br/>";
                _htmlFooter += "</th>";
                _htmlFooter += "<th class=\"tg-qjg1\" colspan=\"15\"></th>";
                _htmlFooter += "<th class=\"tg-npou\" colspan=\"2\" style=\"font-size:14px;\">ĐẠI DIỆN TRUNG TÂM QLLKL</th>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">TRƯỞNG TT KS ĐƯỜNG DÀI</td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">GIÁM ĐỐC</td>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"15\"></td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">TRƯỞNG TT HĐB-ĐPLKL</td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">GIÁM ĐỐC</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";

                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\"><span style =\"font-weight: bold;font-style:italic;\" > Nơi nhận :</span></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Như kính gửi</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Lưu VT, ĐPL</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "</table>";
                _htmlFooter += "</div>";
            }
            else if (ddlMien.Value == "2")
            {
                _htmlFooter += "<div id = \"footer\">";
                _htmlFooter += "<table class=\"tgfooter\" style=\"width:100%;border:none;border-color:white;\">";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td colspan=\"19\" style=\"height:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<th class=\"tg-mmgy\" colspan=\"2\" style=\"font-size:14px;\"> ĐẠI DIỆN CÔNG TY QLB MIỀN TRUNG<br/>";
                _htmlFooter += "</th>";
                _htmlFooter += "<th class=\"tg-qjg1\" colspan=\"15\"></th>";
                _htmlFooter += "<th class=\"tg-npou\" colspan=\"2\" style=\"font-size:14px;\">ĐẠI DIỆN TRUNG TÂM QLLKL</th>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">TRƯỞNG TT KS TCTS</td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">GIÁM ĐỐC</td>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"15\"></td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">TRƯỞNG TT HĐB-ĐPLKL</td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">GIÁM ĐỐC</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";

                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\"><span style =\"font-weight: bold;font-style:italic;\" > Nơi nhận :</span></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Như kính gửi</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Lưu VT, ĐPL</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "</table>";
                _htmlFooter += "</div>";
            }
            else if (ddlMien.Value == "3")
            {
                _htmlFooter += "<div id = \"footer\">";
                _htmlFooter += "<table class=\"tgfooter\" style=\"width:100%;border:none;border-color:white;\">";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td colspan=\"19\" style=\"height:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<th class=\"tg-mmgy\" colspan=\"2\" style=\"font-size:14px;\"> ĐẠI DIỆN CÔNG TY QLB MIỀN NAM<br/>";
                _htmlFooter += "</th>";
                _htmlFooter += "<th class=\"tg-qjg1\" colspan=\"15\"></th>";
                _htmlFooter += "<th class=\"tg-npou\" colspan=\"2\" style=\"font-size:14px;\">ĐẠI DIỆN TRUNG TÂM QLLKL</th>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">TRƯỞNG PHÒNG KHÔNG LƯU</td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">GIÁM ĐỐC</td>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"15\"></td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">TRƯỞNG TT HĐB-ĐPLKL</td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\">GIÁM ĐỐC</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";

                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\"><span style =\"font-weight: bold;font-style:italic;\" > Nơi nhận :</span></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Như kính gửi</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Lưu VT, ĐPL</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "</table>";
                _htmlFooter += "</div>";
            }
            else
            {
                _htmlFooter += "<div id = \"footer\">";
                _htmlFooter += "<table class=\"tgfooter\" style=\"width:100%;border:none;border-color:white;\">";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td colspan=\"19\" style=\"height:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<th class=\"tg-mmgy\" colspan=\"2\" style=\"font-size:14px;\"> TRUNG TÂM HĐB & ĐPLKL<br/>";
                _htmlFooter += "</th>";
                _htmlFooter += "<th class=\"tg-qjg1\" colspan=\"15\"></th>";
                _htmlFooter += "<th class=\"tg-npou\" colspan=\"2\" style=\"font-size:14px;\">GIÁM ĐỐC</th>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\"></td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"15\"></td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\"></td>";
                _htmlFooter += "<td class=\"tg-npou\" style=\"font-size:14px;\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";

                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "<td class=\"tg-qjg1\"></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\"><span style =\"font-weight: bold;font-style:italic;\" > Nơi nhận :</span></td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Như kính gửi</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "<tr>";
                _htmlFooter += "<td class=\"tg-qjg1\" colspan=\"19\">-Lưu VT, ĐPL</td>";
                _htmlFooter += "</tr>";
                _htmlFooter += "</table>";
                _htmlFooter += "</div>";
            }





            
            return _htmlFooter;
        }

        public static DataTable GetTable()
        {

            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("SANBAY", typeof(string));
            table.Columns.Add("HANG01", typeof(string));
            table.Columns.Add("HANG02", typeof(string));
            table.Columns.Add("HANG03", typeof(string));
            table.Columns.Add("HANG04", typeof(string));


            table.Rows.Add(1, "VVCI", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(2, "VVDB", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(3, "VVDH", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(4, "VVNB", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(5, "VVTX", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(6, "VVVH", "HVN", "VJC", "PIC", "VFC");
            
            return table;
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
            this.CreateExcel(html, "BAN_DOICHIEU_BCSL_HTN_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "BAN_DOICHIEU_BCSL_HTN_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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