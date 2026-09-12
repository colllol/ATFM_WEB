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
    public partial class RpDHB_SL_Hang_QN : System.Web.UI.Page
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
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg1\" style =\"width:48%;\" colspan=\"8\">" + ConfigurationManager.AppSettings["CompanyName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "<span class=\"fontRp13\">" + ConfigurationManager.AppSettings["CenterName"].ToString().ToUpper() + "<br/>";
            _htmlHeader += "-----------------------------<br/><span style = \"font-weight:100;\">Số........../ BC - QLLKL<br></span></th>";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" style =\"width:4%;\">";
            _htmlHeader += "</td>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"9\" style =\"width:48%;\">";
            _htmlHeader += "<span class=\"fontRp12\" style =\"font-weight: bold\" >CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</span><br/>";
            _htmlHeader += "<span class=\"fontRp13\" style =\"font-weight: bold\" >Độc lập - Tự do - Hạnh phúc</span><br/>";
            _htmlHeader += "---------------------------<br/>";
            _htmlHeader += "<span class=\"fontRp14\" style = \"font-style:italic;font-weight:100;\" >Hà nội , ngày.." + DateTime.Now.Day.ToString() + "..tháng.." + DateTime.Now.Month.ToString() + "..năm.." + DateTime.Now.Year.ToString() + "..</span><br/><br/>";
            _htmlHeader += "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg fontRp14\" colspan=\"18\" style = \"text-align:center;\"><span style = \"font-weight:bold;text-align:center;\"> BÁO CÁO SỐ LIỆU CÁC HÃNG HK TRONG NƯỚC</span>";
            _htmlHeader += "</td></tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-mqab fontRp14\"  colspan=\"8\" style=\"text-align:right;font-weight:bold;\">Từ ngày :" + txtFromDate.Value.ToString() + " </td>";
            _htmlHeader += "<td class=\"tg-oqgr\"></td>";
            _htmlHeader += "<td class=\"tg-31sd fontRp14\"  colspan=\"9\" style=\"text-align:left;font-weight:bold;\">Đến ngày:" + txtToDate.Value.ToString() + "</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td style = \"text-align:center;\" class=\"fontRp14\" colspan=\"18\">Kính gửi: Ban tài chính - Tổng Công ty Quản lý bay Việt Nam</td>";
            _htmlHeader += "</tr>";
            _htmlHeader += "<tr>";
            _htmlHeader += "<td class=\"tg-5fpg\" colspan=\"18\"></td></tr></table></div>";
            return _htmlHeader;
        }

        public string BuildContent_New()
        {
            string _fromDate = txtFromDate.Value.ToString().Trim();
            string _toDate = txtToDate.Value.ToString().Trim();
            DataTable _dt = null;
            _dt = _DAL.DHB_GET_SB_BYMIEN("");
            _dtnew = _DAL.DHB_GET_SLB_HHKVN_BAY_QN_QT(_fromDate, _toDate);
            DataRow _mrow = null;

            string _qn1 = "0"; string _qt1 = "0"; int _total1 = 0;
            string _qn2 = "0"; string _qt2 = "0"; int _total2 = 0;
            string _qn3 = "0"; string _qt3 = "0"; int _total3 = 0;
            string _qn4 = "0"; string _qt4 = "0"; int _total4 = 0;
            string _qn5 = "0"; string _qt5 = "0"; int _total5 = 0;
            string _qn6 = "0"; string _qt6 = "0"; int _total6 = 0;

            string _qn7 = "0"; string _qt7 = "0"; int _total7 = 0;
            string _qn8 = "0"; string _qt8 = "0"; int _total8 = 0;
            string _qn9 = "0"; string _qt9 = "0"; int _total9 = 0;
            string _qn10 = "0"; string _qt10 = "0"; int _total10 = 0;

            int _sumqn1 = 0; int _sumqt1 = 0;

            int _t1 = 0; int _t2 = 0; int _t3 = 0; int _t4 = 0; int _t5 = 0; int _t6 = 0; int _t7 = 0; int _t8 = 0; int _t9 = 0; int _t10 = 0; int _t11 = 0; int _t12 = 0; int _t13 = 0; int _t14 = 0; int _t15 = 0; int _t16 = 0; int _t17 = 0;int _t18 = 0; int _t19 = 0; int _t20 = 0;
            int _t21 = 0; int _t22 = 0; int _t23 = 0; int _t24 = 0; int _t25 = 0; int _t26 = 0;
            int _t27 = 0; int _t28 = 0; int _t29 = 0; int _t30 = 0; int _t31 = 0; int _t32 = 0;
            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-hgcj\" rowspan=\"2\">SÂN BAY</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">HVN</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VJC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">PIC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VFC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">BAV</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">HAI</th>";

            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VSA</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VNC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">SFC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VAG</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"2\">SUM ALL</th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";

            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            /*BSunng*/
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";

            _htmlContent += "<td class=\"tg-baqh\"><b>QN</b></td>";
            _htmlContent += "<td class=\"tg-baqh\"><b>QT</b></td>";
            _htmlContent += "</tr>";
            if (!String.IsNullOrEmpty(_fromDate) && !String.IsNullOrEmpty(_toDate))
            {
                if (_dt != null)
                {

                    if (_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < _dt.Rows.Count; j++)
                        {
                            _mrow = _dt.Rows[j];

                            _qn1 = GetValue(_mrow["AE_CODE"].ToString(), "HVN", "QN");
                            _qt1 = GetValue(_mrow["AE_CODE"].ToString(), "HVN", "QT");

                            //_qn1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["AE_CODE"].ToString(), "HVN", _fromDate, _toDate);
                            //_qt1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["AE_CODE"].ToString(), "HVN", _fromDate, _toDate);
                            _total1 = Int32.Parse(_qn1) + Int32.Parse(_qt1);

                            _t1 += Int32.Parse(_qn1);
                            _t2 += Int32.Parse(_qt1);
                            _t3 += _total1;

                            _qn2 = GetValue(_mrow["AE_CODE"].ToString(), "VJC", "QN");
                            _qt2 = GetValue(_mrow["AE_CODE"].ToString(), "VJC", "QT");
                            //_qn2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["AE_CODE"].ToString(), "VJC", _fromDate, _toDate);
                            //_qt2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["AE_CODE"].ToString(), "VJC", _fromDate, _toDate);
                            _total2 = Int32.Parse(_qn2) + Int32.Parse(_qt2);

                            _t4 += Int32.Parse(_qn2);
                            _t5 += Int32.Parse(_qt2);
                            _t6 += _total2;

                            _qn3 = GetValue(_mrow["AE_CODE"].ToString(), "PIC", "QN");
                            _qt3 = GetValue(_mrow["AE_CODE"].ToString(), "PIC", "QT");
                            //_qn3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["AE_CODE"].ToString(), "PIC", _fromDate, _toDate);
                            //_qt3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["AE_CODE"].ToString(), "PIC", _fromDate, _toDate);
                            _total3 = Int32.Parse(_qn3) + Int32.Parse(_qt3);

                            _t7 += Int32.Parse(_qn3);
                            _t8 += Int32.Parse(_qt3);
                            _t9 += _total3;

                            _qn4 = GetValue(_mrow["AE_CODE"].ToString(), "VFC", "QN");
                            _qt4 = GetValue(_mrow["AE_CODE"].ToString(), "VFC", "QT");
                            //_qn4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["AE_CODE"].ToString(), "VFC", _fromDate, _toDate);
                            //_qt4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["AE_CODE"].ToString(), "VFC", _fromDate, _toDate);
                            _total4 = Int32.Parse(_qn4) + Int32.Parse(_qt4);


                            _t10 += Int32.Parse(_qn4);
                            _t11 += Int32.Parse(_qt4);
                            _t12 += _total4;

                            _qn5 = GetValue(_mrow["AE_CODE"].ToString(), "BAV", "QN");
                            _qt5 = GetValue(_mrow["AE_CODE"].ToString(), "BAV", "QT");
                            //_qn5 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["AE_CODE"].ToString(), "BAV", _fromDate, _toDate);
                            //_qt5 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["AE_CODE"].ToString(), "BAV", _fromDate, _toDate);
                            _total5 = Int32.Parse(_qn5) + Int32.Parse(_qt5);

                          
                            _t13 += Int32.Parse(_qn5);
                            _t14 += Int32.Parse(_qt5);
                            _t15 += _total5;

                            _qn6 = GetValue(_mrow["AE_CODE"].ToString(), "HAI", "QN");
                            _qt6 = GetValue(_mrow["AE_CODE"].ToString(), "HAI", "QT");
                            _total6 = Int32.Parse(_qn6) + Int32.Parse(_qt6);

                            _t18 += Int32.Parse(_qn6);
                            _t19 += Int32.Parse(_qt6);
                            _t20 += _total6;



                            _qn7 = GetValue(_mrow["AE_CODE"].ToString(), "VSA", "QN");
                            _qt7 = GetValue(_mrow["AE_CODE"].ToString(), "VSA", "QT");
                            _total7 = Int32.Parse(_qn7) + Int32.Parse(_qt7);

                            _t21 += Int32.Parse(_qn7);
                            _t22 += Int32.Parse(_qt7);
                            _t23 += _total7;

                            _qn8 = GetValue(_mrow["AE_CODE"].ToString(), "VNC", "QN");
                            _qt8 = GetValue(_mrow["AE_CODE"].ToString(), "VNC", "QT");
                            _total8 = Int32.Parse(_qn8) + Int32.Parse(_qt8);

                            _t24 += Int32.Parse(_qn8);
                            _t25 += Int32.Parse(_qt8);
                            _t26 += _total8;

                            _qn9 = GetValue(_mrow["AE_CODE"].ToString(), "SFC", "QN");
                            _qt9 = GetValue(_mrow["AE_CODE"].ToString(), "SFC", "QT");
                            _total9 = Int32.Parse(_qn9) + Int32.Parse(_qt9);

                            _t27 += Int32.Parse(_qn9);
                            _t28 += Int32.Parse(_qt9);
                            _t29 += _total9;


                            _qn10 = GetValue(_mrow["AE_CODE"].ToString(), "VAG", "QN");
                            _qt10 = GetValue(_mrow["AE_CODE"].ToString(), "VAG", "QT");
                            _total10 = Int32.Parse(_qn10) + Int32.Parse(_qt10);

                            _t30 += Int32.Parse(_qn10);
                            _t31 += Int32.Parse(_qt10);
                            _t32 += _total10;


                            _sumqn1 = Int32.Parse(_qn1) + Int32.Parse(_qn2) + Int32.Parse(_qn3) + Int32.Parse(_qn4) + Int32.Parse(_qn5)+ Int32.Parse(_qn6) + Int32.Parse(_qn7) + Int32.Parse(_qn8) + Int32.Parse(_qn9) + Int32.Parse(_qn10);
                            _sumqt1 = Int32.Parse(_qt1) + Int32.Parse(_qt2) + Int32.Parse(_qt3) + Int32.Parse(_qt4) + Int32.Parse(_qt5)+ Int32.Parse(_qt6) + Int32.Parse(_qt7) + Int32.Parse(_qt8) + Int32.Parse(_qt9) + Int32.Parse(_qn10);

                            _t16 += _sumqn1;
                            _t17 += _sumqt1;
                           

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _mrow["AE_CODE"].ToString() + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn5 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt5 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total5 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn6 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt6 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total6 + "</td>";

                            _htmlContent += "<td class=\"tg-baqh\">" + _qn7 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt7 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total7 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn8 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt8 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total8 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn9 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt9 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total9 + "</td>";

                            _htmlContent += "<td class=\"tg-baqh\">" + _qn10 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt10 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total10 + "</td>";


                            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqn1 + "</b></td>";
                            _htmlContent += "<td class=\"tg-baqh\"><b>" + _sumqt1 + "</b></td>";
                            _htmlContent += "</tr>";

                        }

                        
                    }

                }

                _htmlContent += "<tr>";
                _htmlContent += "<td class=\"tg-baqh\"><b>Tổng</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t1 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t2 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t3 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t4 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t5 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t6 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t7 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t8 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t9 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t10 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t11 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t12 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t13 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t14 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t15 + "</b></td>";

                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t18 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t19 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t20 + "</b></td>";

                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t21 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t22 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t23 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t24 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t25 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t26 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t27 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t28 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t29 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t32 + "</b></td>";

                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t16 + "</b></td>";
                _htmlContent += "<td class=\"tg-baqh\"><b>" + _t17 + "</b></td>";
                _htmlContent += "</tr>";

            }


            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }

        public string GetValue(string aero, string oper, string type)
        {
            string _return = "0";
            int _total = 0;
            try
            {
                if (_dtnew.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtnew.Rows.Count; i++)
                    {
                        if ((aero.Trim() == _dtnew.Rows[i]["AERO"].ToString().Trim()) && (oper.Trim() == _dtnew.Rows[i]["OPER"].ToString().Trim()) && (_dtnew.Rows[i][type].ToString() != "0"))
                        {
                            _total += Int32.Parse(_dtnew.Rows[i][type].ToString());                           
                        }

                    }
                    _return = _total.ToString();
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


        public string BuildContent()
        {
            string _fromDate = txtFromDate.Value.ToString().Trim();
            string _toDate = txtToDate.Value.ToString().Trim();
            DataTable _dt = null;
            _dt = GetTable();
            DataRow _mrow = null;

            string _qn1 = "0"; string _qt1 = "0"; int _total1 = 0;
            string _qn2 = "0"; string _qt2 = "0"; int _total2 = 0;
            string _qn3 = "0"; string _qt3 = "0"; int _total3 = 0;
            string _qn4 = "0"; string _qt4 = "0"; int _total4 = 0;
            int _sumqn1 = 0; int _sumqt1 = 0;

             string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-hgcj\" rowspan=\"2\">SÂN BAY</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">HVN</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VJC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">PIC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VFC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"2\">SUM ALL</th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "</tr>";
            if (!String.IsNullOrEmpty(_fromDate) && !String.IsNullOrEmpty(_toDate))
            {
                if (_dt != null)
                {

                    if (_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < _dt.Rows.Count; j++)
                        {
                            _mrow = _dt.Rows[j];

                            _qn1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG01"].ToString(), _fromDate, _toDate);
                            _qt1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG01"].ToString(), _fromDate, _toDate);
                            _total1 = Int32.Parse(_qn1) + Int32.Parse(_qt1);

                            _qn2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG02"].ToString(), _fromDate, _toDate);
                            _qt2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG02"].ToString(), _fromDate, _toDate);
                            _total2 = Int32.Parse(_qn2) + Int32.Parse(_qt2);

                            _qn3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG03"].ToString(), _fromDate, _toDate);
                            _qt3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG03"].ToString(), _fromDate, _toDate);
                            _total3 = Int32.Parse(_qn3) + Int32.Parse(_qt3);

                            _qn4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG04"].ToString(), _fromDate, _toDate);
                            _qt4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL(_mrow["SANBAY"].ToString(), _mrow["HANG04"].ToString(), _fromDate, _toDate);
                            _total4 = Int32.Parse(_qn4) + Int32.Parse(_qt4);

                            _sumqn1 = Int32.Parse(_qn1) + Int32.Parse(_qn2) + Int32.Parse(_qn3) + Int32.Parse(_qn4);
                            _sumqt1 = Int32.Parse(_qt1) + Int32.Parse(_qt2) + Int32.Parse(_qt3) + Int32.Parse(_qt4);

                            _htmlContent += "<tr>";
                            _htmlContent += "<td class=\"tg-baqh\">"+_mrow["SANBAY"].ToString()+"</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total2 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total3 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qn4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _qt4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _total4 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _sumqn1 + "</td>";
                            _htmlContent += "<td class=\"tg-baqh\">" + _sumqt1 + "</td>";
                            _htmlContent += "</tr>";

                        }
                    }
                }

            }


            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }
        public string BuildContent_OLD()
        {
            string _fromDate = txtFromDate.Value.ToString().Trim();
            string _toDate = txtToDate.Value.ToString().Trim();


            string _htmlContent = "";
            _htmlContent += "<div id = \"content\">";
            _htmlContent += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlContent += "<tr>";
            _htmlContent += "<th class=\"tg-hgcj\" rowspan=\"2\">SÂN BAY</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">HVN</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VJC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">PIC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"3\">VFC</th>";
            _htmlContent += "<th class=\"tg-amwm\" colspan=\"2\">SUM ALL</th>";
            _htmlContent += "</tr>";
            _htmlContent += "<tr>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "<td class=\"tg-baqh\">SUM</td>";
            _htmlContent += "<td class=\"tg-baqh\">QN</td>";
            _htmlContent += "<td class=\"tg-baqh\">QT</td>";
            _htmlContent += "</tr>";
            if (!String.IsNullOrEmpty(_fromDate) && !String.IsNullOrEmpty(_toDate))
            {
               
                        string _qn1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVBM", "HVN", _fromDate, _toDate);
                        string _qt1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVBM", "HVN", _fromDate, _toDate);
                        int _total1 = Int32.Parse(_qn1) + Int32.Parse(_qt1);

                        string _qn2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVBM", "VJC", _fromDate, _toDate);
                        string _qt2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVBM", "VJC", _fromDate, _toDate);
                        int _total2 = Int32.Parse(_qn2) + Int32.Parse(_qt2);

                        string _qn3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVBM", "PIC", _fromDate, _toDate);
                        string _qt3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVBM", "PIC", _fromDate, _toDate);
                        int _total3 = Int32.Parse(_qn3) + Int32.Parse(_qt3);

                        string _qn4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVBM", "VFC", _fromDate, _toDate);
                        string _qt4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVBM", "VFC", _fromDate, _toDate);
                        int _total4 = Int32.Parse(_qn4) + Int32.Parse(_qt4);

                        int _sumqn1 = Int32.Parse(_qn1) + Int32.Parse(_qn2) + Int32.Parse(_qn3) + Int32.Parse(_qn4);
                        int _sumqt1 = Int32.Parse(_qt1) + Int32.Parse(_qt2) + Int32.Parse(_qt3) + Int32.Parse(_qt4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVBM</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qn1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qt1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _total1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qn2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qt2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _total2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qn3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qt3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _total3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qn4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qt4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _total4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqn1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqt1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnca1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCA", "HVN", _fromDate, _toDate);
                        string _qtca1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCA", "HVN", _fromDate, _toDate);
                        int _totalca1 = Int32.Parse(_qnca1) + Int32.Parse(_qtca1);

                        string _qnca2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCA", "VJC", _fromDate, _toDate);
                        string _qtca2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCA", "VJC", _fromDate, _toDate);
                        int _totalca2 = Int32.Parse(_qnca2) + Int32.Parse(_qtca2);

                        string _qnca3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCA", "PIC", _fromDate, _toDate);
                        string _qtca3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCA", "PIC", _fromDate, _toDate);
                        int _totalca3 = Int32.Parse(_qnca3) + Int32.Parse(_qtca3);

                        string _qnca4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCA", "VFC", _fromDate, _toDate);
                        string _qtca4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCA", "VFC", _fromDate, _toDate);
                        int _totalca4 = Int32.Parse(_qnca4) + Int32.Parse(_qtca4);

                        int _sumqnca1 = Int32.Parse(_qnca1) + Int32.Parse(_qnca2) + Int32.Parse(_qnca3) + Int32.Parse(_qnca4);
                        int _sumqtca1 = Int32.Parse(_qtca1) + Int32.Parse(_qtca2) + Int32.Parse(_qtca3) + Int32.Parse(_qtca4);



                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVCA</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnca1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtca1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalca1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnca2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtca2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalca2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnca3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtca3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalca3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnca4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtca4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalca4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnca1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtca1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnci1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCI", "HVN", _fromDate, _toDate);
                        string _qtci1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCI", "HVN", _fromDate, _toDate);
                        int _totalci1 = Int32.Parse(_qnci1) + Int32.Parse(_qtci1);

                        string _qnci2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCI", "VJC", _fromDate, _toDate);
                        string _qtci2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCI", "VJC", _fromDate, _toDate);
                        int _totalci2 = Int32.Parse(_qnci2) + Int32.Parse(_qtci2);

                        string _qnci3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCI", "PIC", _fromDate, _toDate);
                        string _qtci3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCI", "PIC", _fromDate, _toDate);
                        int _totalci3 = Int32.Parse(_qnci3) + Int32.Parse(_qtci3);

                        string _qnci4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCI", "VFC", _fromDate, _toDate);
                        string _qtci4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCI", "VFC", _fromDate, _toDate);
                        int _totalci4 = Int32.Parse(_qnci4) + Int32.Parse(_qtci4);

                        int _sumqnci1 = Int32.Parse(_qnci1) + Int32.Parse(_qnci2) + Int32.Parse(_qnci3) + Int32.Parse(_qnci4);
                        int _sumqtci1 = Int32.Parse(_qtci1) + Int32.Parse(_qtci2) + Int32.Parse(_qtci3) + Int32.Parse(_qtci4);



                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVCI</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnci1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtci1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalci1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnci2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtci2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalci2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnci3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtci3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalci3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnci4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtci4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalci4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnci1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtci1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qncm1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCM", "HVN", _fromDate, _toDate);
                        string _qtcm1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCM", "HVN", _fromDate, _toDate);
                        int _totalcm1 = Int32.Parse(_qncm1) + Int32.Parse(_qtcm1);

                        string _qncm2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCM", "VJC", _fromDate, _toDate);
                        string _qtcm2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCM", "VJC", _fromDate, _toDate);
                        int _totalcm2 = Int32.Parse(_qncm2) + Int32.Parse(_qtcm2);

                        string _qncm3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCM", "PIC", _fromDate, _toDate);
                        string _qtcm3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCM", "PIC", _fromDate, _toDate);
                        int _totalcm3 = Int32.Parse(_qncm3) + Int32.Parse(_qtcm3);

                        string _qncm4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCM", "VFC", _fromDate, _toDate);
                        string _qtcm4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCM", "VFC", _fromDate, _toDate);
                        int _totalcm4 = Int32.Parse(_qncm4) + Int32.Parse(_qtcm4);

                        int _sumqncm1 = Int32.Parse(_qncm1) + Int32.Parse(_qncm2) + Int32.Parse(_qncm3) + Int32.Parse(_qncm4);
                        int _sumqtcm1 = Int32.Parse(_qtcm1) + Int32.Parse(_qtcm2) + Int32.Parse(_qtcm3) + Int32.Parse(_qtcm4);



                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVCM</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncm1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcm1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcm1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncm2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcm2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcm2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncm3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcm3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcm3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncm4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcm4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcm4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqncm1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtcm1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qncr1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCR", "HVN", _fromDate, _toDate);
                        string _qtcr1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCR", "HVN", _fromDate, _toDate);
                        int _totalcr1 = Int32.Parse(_qncr1) + Int32.Parse(_qtcr1);

                        string _qncr2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCR", "VJC", _fromDate, _toDate);
                        string _qtcr2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCR", "VJC", _fromDate, _toDate);
                        int _totalcr2 = Int32.Parse(_qncr2) + Int32.Parse(_qtcr2);

                        string _qncr3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCR", "PIC", _fromDate, _toDate);
                        string _qtcr3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCR", "PIC", _fromDate, _toDate);
                        int _totalcr3 = Int32.Parse(_qncr3) + Int32.Parse(_qtcr3);

                        string _qncr4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCR", "VFC", _fromDate, _toDate);
                        string _qtcr4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCR", "VFC", _fromDate, _toDate);
                        int _totalcr4 = Int32.Parse(_qncr4) + Int32.Parse(_qtcr4);

                        int _sumqncr1 = Int32.Parse(_qncr1) + Int32.Parse(_qncr2) + Int32.Parse(_qncr3) + Int32.Parse(_qncr4);
                        int _sumqtcr1 = Int32.Parse(_qtcr1) + Int32.Parse(_qtcr2) + Int32.Parse(_qtcr3) + Int32.Parse(_qtcr4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVCR</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncr1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcr1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcr1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncr2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcr2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcr2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncr3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcr3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcr3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncr4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcr4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcr4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqncr1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtcr1 + "</td>";
                        _htmlContent += "</tr>";


                        string _qncs1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCS", "HVN", _fromDate, _toDate);
                        string _qtcs1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCS", "HVN", _fromDate, _toDate);
                        int _totalcs1 = Int32.Parse(_qncs1) + Int32.Parse(_qtcs1);

                        string _qncs2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCS", "VJC", _fromDate, _toDate);
                        string _qtcs2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCS", "VJC", _fromDate, _toDate);
                        int _totalcs2 = Int32.Parse(_qncs2) + Int32.Parse(_qtcs2);

                        string _qncs3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCS", "PIC", _fromDate, _toDate);
                        string _qtcs3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCS", "PIC", _fromDate, _toDate);
                        int _totalcs3 = Int32.Parse(_qncs3) + Int32.Parse(_qtcs3);

                        string _qncs4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCS", "VFC", _fromDate, _toDate);
                        string _qtcs4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCS", "VFC", _fromDate, _toDate);
                        int _totalcs4 = Int32.Parse(_qncs4) + Int32.Parse(_qtcs4);

                        int _sumqncs1 = Int32.Parse(_qncs1) + Int32.Parse(_qncs2) + Int32.Parse(_qncs3) + Int32.Parse(_qncs4);
                        int _sumqtcs1 = Int32.Parse(_qtcs1) + Int32.Parse(_qtcs2) + Int32.Parse(_qtcs3) + Int32.Parse(_qtcs4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVCS</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncs1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcs1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcs1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncs2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcs2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcs2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncs3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcs3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcs3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qncs4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtcs4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalcs4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqncs1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtcs1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnct1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCT", "HVN", _fromDate, _toDate);
                        string _qtct1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCT", "HVN", _fromDate, _toDate);
                        int _totalct1 = Int32.Parse(_qnct1) + Int32.Parse(_qtct1);

                        string _qnct2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCT", "VJC", _fromDate, _toDate);
                        string _qtct2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCT", "VJC", _fromDate, _toDate);
                        int _totalct2 = Int32.Parse(_qnct2) + Int32.Parse(_qtct2);

                        string _qnct3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCT", "PIC", _fromDate, _toDate);
                        string _qtct3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCT", "PIC", _fromDate, _toDate);
                        int _totalct3 = Int32.Parse(_qnct3) + Int32.Parse(_qtct3);

                        string _qnct4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVCT", "VFC", _fromDate, _toDate);
                        string _qtct4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVCT", "VFC", _fromDate, _toDate);
                        int _totalct4 = Int32.Parse(_qnct4) + Int32.Parse(_qtct4);

                        int _sumqnct1 = Int32.Parse(_qnct1) + Int32.Parse(_qnct2) + Int32.Parse(_qnct3) + Int32.Parse(_qnct4);
                        int _sumqtct1 = Int32.Parse(_qtct1) + Int32.Parse(_qtct2) + Int32.Parse(_qtct3) + Int32.Parse(_qtct4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVCT</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnct1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtct1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalct1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnct2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtct2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalct2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnct3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtct3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalct3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnct4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtct4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalct4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnct1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtct1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qndb1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDB", "HVN", _fromDate, _toDate);
                        string _qtdb1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDB", "HVN", _fromDate, _toDate);
                        int _totaldb1 = Int32.Parse(_qndb1) + Int32.Parse(_qtdb1);

                        string _qndb2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDB", "VJC", _fromDate, _toDate);
                        string _qtdb2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDB", "VJC", _fromDate, _toDate);
                        int _totaldb2 = Int32.Parse(_qndb2) + Int32.Parse(_qtdb2);

                        string _qndb3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDB", "PIC", _fromDate, _toDate);
                        string _qtdb3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDB", "PIC", _fromDate, _toDate);
                        int _totaldb3 = Int32.Parse(_qndb3) + Int32.Parse(_qtdb3);

                        string _qndb4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDB", "VFC", _fromDate, _toDate);
                        string _qtdb4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDB", "VFC", _fromDate, _toDate);
                        int _totaldb4 = Int32.Parse(_qndb4) + Int32.Parse(_qtdb4);

                        int _sumqndb1 = Int32.Parse(_qndb1) + Int32.Parse(_qndb2) + Int32.Parse(_qndb3) + Int32.Parse(_qndb4);
                        int _sumqtdb1 = Int32.Parse(_qtdb1) + Int32.Parse(_qtdb2) + Int32.Parse(_qtdb3) + Int32.Parse(_qtdb4);



                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVDB</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqndb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtdb1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qndh1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDH", "HVN", _fromDate, _toDate);
                        string _qtdh1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDH", "HVN", _fromDate, _toDate);
                        int _totaldh1 = Int32.Parse(_qndh1) + Int32.Parse(_qtdh1);

                        string _qndh2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDH", "VJC", _fromDate, _toDate);
                        string _qtdh2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDH", "VJC", _fromDate, _toDate);
                        int _totaldh2 = Int32.Parse(_qndh2) + Int32.Parse(_qtdh2);

                        string _qndh3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDH", "PIC", _fromDate, _toDate);
                        string _qtdh3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDH", "PIC", _fromDate, _toDate);
                        int _totaldh3 = Int32.Parse(_qndh3) + Int32.Parse(_qtdh3);

                        string _qndh4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDH", "VFC", _fromDate, _toDate);
                        string _qtdh4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDH", "VFC", _fromDate, _toDate);
                        int _totaldh4 = Int32.Parse(_qndh4) + Int32.Parse(_qtdh4);

                        int _sumqndh1 = Int32.Parse(_qndh1) + Int32.Parse(_qndh2) + Int32.Parse(_qndh3) + Int32.Parse(_qndh4);
                        int _sumqtdh1 = Int32.Parse(_qtdh1) + Int32.Parse(_qtdh2) + Int32.Parse(_qtdh3) + Int32.Parse(_qtdh4);



                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVDH</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndh1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdh1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldh1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndh2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdh2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldh2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndh3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdh3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldh3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndh4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdh4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldh4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqndh1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtdh1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qndl1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDL", "HVN", _fromDate, _toDate);
                        string _qtdl1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDL", "HVN", _fromDate, _toDate);
                        int _totaldl1 = Int32.Parse(_qndl1) + Int32.Parse(_qtdl1);

                        string _qndl2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDL", "VJC", _fromDate, _toDate);
                        string _qtdl2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDL", "VJC", _fromDate, _toDate);
                        int _totaldl2 = Int32.Parse(_qndl2) + Int32.Parse(_qtdl2);

                        string _qndl3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDL", "PIC", _fromDate, _toDate);
                        string _qtdl3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDL", "PIC", _fromDate, _toDate);
                        int _totaldl3 = Int32.Parse(_qndl3) + Int32.Parse(_qtdl3);

                        string _qndl4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDL", "VFC", _fromDate, _toDate);
                        string _qtdl4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDL", "VFC", _fromDate, _toDate);
                        int _totaldl4 = Int32.Parse(_qndl4) + Int32.Parse(_qtdl4);

                        int _sumqndl1 = Int32.Parse(_qndl1) + Int32.Parse(_qndl2) + Int32.Parse(_qndl3) + Int32.Parse(_qndl4);
                        int _sumqtdl1 = Int32.Parse(_qtdl1) + Int32.Parse(_qtdl2) + Int32.Parse(_qtdl3) + Int32.Parse(_qtdl4);




                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVDL</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndl2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdl2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldl2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndl3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdl3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldl3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndl4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdl4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldl4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqndl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtdl1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qndn1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDN", "HVN", _fromDate, _toDate);
                        string _qtdn1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDN", "HVN", _fromDate, _toDate);
                        int _totaldn1 = Int32.Parse(_qndn1) + Int32.Parse(_qtdn1);

                        string _qndn2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDN", "VJC", _fromDate, _toDate);
                        string _qtdn2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDN", "VJC", _fromDate, _toDate);
                        int _totaldn2 = Int32.Parse(_qndn2) + Int32.Parse(_qtdn2);

                        string _qndn3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDN", "PIC", _fromDate, _toDate);
                        string _qtdn3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDN", "PIC", _fromDate, _toDate);
                        int _totaldn3 = Int32.Parse(_qndn3) + Int32.Parse(_qtdn3);

                        string _qndn4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVDN", "VFC", _fromDate, _toDate);
                        string _qtdn4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVDN", "VFC", _fromDate, _toDate);
                        int _totaldn4 = Int32.Parse(_qndn4) + Int32.Parse(_qtdn4);

                        int _sumqndn1 = Int32.Parse(_qndn1) + Int32.Parse(_qndn2) + Int32.Parse(_qndn3) + Int32.Parse(_qndn4);
                        int _sumqtdn1 = Int32.Parse(_qtdn1) + Int32.Parse(_qtdn2) + Int32.Parse(_qtdn3) + Int32.Parse(_qtdn4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVDN</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndn1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdn1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldn1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndn2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdn2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldn2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndn3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdn3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldn3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qndn4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtdn4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaldn4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqndn1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtdn1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnhl1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVHL", "HVN", _fromDate, _toDate);
                        string _qthl1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVHL", "HVN", _fromDate, _toDate);
                        int _totalhl1 = Int32.Parse(_qnhl1) + Int32.Parse(_qthl1);

                        string _qnhl2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVHL", "VJC", _fromDate, _toDate);
                        string _qthl2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVHL", "VJC", _fromDate, _toDate);
                        int _totalhl2 = Int32.Parse(_qnhl2) + Int32.Parse(_qthl2);

                        string _qnhl3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVHL", "PIC", _fromDate, _toDate);
                        string _qthl3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVHL", "PIC", _fromDate, _toDate);
                        int _totalhl3 = Int32.Parse(_qnhl3) + Int32.Parse(_qthl3);

                        string _qnhl4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVHL", "VFC", _fromDate, _toDate);
                        string _qthl4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVHL", "VFC", _fromDate, _toDate);
                        int _totalhl4 = Int32.Parse(_qnhl4) + Int32.Parse(_qthl4);

                        int _sumqnhl1 = Int32.Parse(_qnhl1) + Int32.Parse(_qnhl2) + Int32.Parse(_qnhl3) + Int32.Parse(_qnhl4);
                        int _sumqthl1 = Int32.Parse(_qthl1) + Int32.Parse(_qthl2) + Int32.Parse(_qthl3) + Int32.Parse(_qthl4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVHL</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnhl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qthl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalhl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnhl2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qthl2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalhl2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnhl3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qthl3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalhl3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnhl4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qthl4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalhl4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnhl1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqthl1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnlc1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVLC", "HVN", _fromDate, _toDate);
                        string _qtlc1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVLC", "HVN", _fromDate, _toDate);
                        int _totallc1 = Int32.Parse(_qnlc1) + Int32.Parse(_qtlc1);

                        string _qnlc2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVLC", "VJC", _fromDate, _toDate);
                        string _qtlc2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVLC", "VJC", _fromDate, _toDate);
                        int _totallc2 = Int32.Parse(_qnlc2) + Int32.Parse(_qtlc2);

                        string _qnlc3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVLC", "PIC", _fromDate, _toDate);
                        string _qtlc3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVLC", "PIC", _fromDate, _toDate);
                        int _totallc3 = Int32.Parse(_qnlc3) + Int32.Parse(_qtlc3);

                        string _qnlc4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVLC", "VFC", _fromDate, _toDate);
                        string _qtlc4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVLC", "VFC", _fromDate, _toDate);
                        int _totallc4 = Int32.Parse(_qnlc4) + Int32.Parse(_qtlc4);

                        int _sumqnlc1 = Int32.Parse(_qnlc1) + Int32.Parse(_qnlc2) + Int32.Parse(_qnlc3) + Int32.Parse(_qnlc4);
                        int _sumqtlc1 = Int32.Parse(_qtlc1) + Int32.Parse(_qtlc2) + Int32.Parse(_qtlc3) + Int32.Parse(_qtlc4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVLC</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnlc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtlc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totallc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnlc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtlc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totallc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnlc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtlc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totallc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnlc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtlc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totallc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnlc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtlc1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnnb1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNB", "HVN", _fromDate, _toDate);
                        string _qtnb1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNB", "HVN", _fromDate, _toDate);
                        int _totalnb1 = Int32.Parse(_qnnb1) + Int32.Parse(_qtnb1);

                        string _qnnb2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNB", "VJC", _fromDate, _toDate);
                        string _qtnb2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNB", "VJC", _fromDate, _toDate);
                        int _totalnb2 = Int32.Parse(_qnnb2) + Int32.Parse(_qtnb2);

                        string _qnnb3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNB", "PIC", _fromDate, _toDate);
                        string _qtnb3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNB", "PIC", _fromDate, _toDate);
                        int _totalnb3 = Int32.Parse(_qnnb3) + Int32.Parse(_qtnb3);

                        string _qnnb4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNB", "VFC", _fromDate, _toDate);
                        string _qtnb4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNB", "VFC", _fromDate, _toDate);
                        int _totalnb4 = Int32.Parse(_qnnb4) + Int32.Parse(_qtnb4);

                        int _sumqnnb1 = Int32.Parse(_qnnb1) + Int32.Parse(_qnnb2) + Int32.Parse(_qnnb3) + Int32.Parse(_qnnb4);
                        int _sumqtnb1 = Int32.Parse(_qtnb1) + Int32.Parse(_qtnb2) + Int32.Parse(_qtnb3) + Int32.Parse(_qtnb4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVNB</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnnb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtnb1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnnc1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNC", "HVN", _fromDate, _toDate);
                        string _qtnc1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNC", "HVN", _fromDate, _toDate);
                        int _totalnc1 = Int32.Parse(_qnnc1) + Int32.Parse(_qtnc1);

                        string _qnnc2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNC", "VJC", _fromDate, _toDate);
                        string _qtnc2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNC", "VJC", _fromDate, _toDate);
                        int _totalnc2 = Int32.Parse(_qnnc2) + Int32.Parse(_qtnc2);

                        string _qnnc3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNC", "PIC", _fromDate, _toDate);
                        string _qtnc3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNC", "PIC", _fromDate, _toDate);
                        int _totalnc3 = Int32.Parse(_qnnc3) + Int32.Parse(_qtnc3);

                        string _qnnc4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVNC", "VFC", _fromDate, _toDate);
                        string _qtnc4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVNC", "VFC", _fromDate, _toDate);
                        int _totalnc4 = Int32.Parse(_qnnc4) + Int32.Parse(_qtnc4);

                        int _sumqnnc1 = Int32.Parse(_qnnc1) + Int32.Parse(_qnnc2) + Int32.Parse(_qnnc3) + Int32.Parse(_qnnc4);
                        int _sumqtnc1 = Int32.Parse(_qtnc1) + Int32.Parse(_qtnc2) + Int32.Parse(_qtnc3) + Int32.Parse(_qtnc4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVNC</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnnc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtnc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalnc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnnc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtnc1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnpb1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPB", "HVN", _fromDate, _toDate);
                        string _qtpb1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPB", "HVN", _fromDate, _toDate);
                        int _totalpb1 = Int32.Parse(_qnpb1) + Int32.Parse(_qtpb1);

                        string _qnpb2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPB", "VJC", _fromDate, _toDate);
                        string _qtpb2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPB", "VJC", _fromDate, _toDate);
                        int _totalpb2 = Int32.Parse(_qnpb2) + Int32.Parse(_qtpb2);

                        string _qnpb3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPB", "PIC", _fromDate, _toDate);
                        string _qtpb3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPB", "PIC", _fromDate, _toDate);
                        int _totalpb3 = Int32.Parse(_qnpb3) + Int32.Parse(_qtpb3);

                        string _qnpb4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPB", "VFC", _fromDate, _toDate);
                        string _qtpb4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPB", "VFC", _fromDate, _toDate);
                        int _totalpb4 = Int32.Parse(_qnpb4) + Int32.Parse(_qtpb4);

                        int _sumqnpb1 = Int32.Parse(_qnpb1) + Int32.Parse(_qnpb2) + Int32.Parse(_qnpb3) + Int32.Parse(_qnpb4);
                        int _sumqtpb1 = Int32.Parse(_qtpb1) + Int32.Parse(_qtpb2) + Int32.Parse(_qtpb3) + Int32.Parse(_qtpb4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVPB</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpb2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpb3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpb4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnpb1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtpb1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnpc1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPC", "HVN", _fromDate, _toDate);
                        string _qtpc1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPC", "HVN", _fromDate, _toDate);
                        int _totalpc1 = Int32.Parse(_qnpc1) + Int32.Parse(_qtpc1);

                        string _qnpc2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPC", "VJC", _fromDate, _toDate);
                        string _qtpc2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPC", "VJC", _fromDate, _toDate);
                        int _totalpc2 = Int32.Parse(_qnpc2) + Int32.Parse(_qtpc2);

                        string _qnpc3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPC", "PIC", _fromDate, _toDate);
                        string _qtpc3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPC", "PIC", _fromDate, _toDate);
                        int _totalpc3 = Int32.Parse(_qnpc3) + Int32.Parse(_qtpc3);

                        string _qnpc4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPC", "VFC", _fromDate, _toDate);
                        string _qtpc4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPC", "VFC", _fromDate, _toDate);
                        int _totalpc4 = Int32.Parse(_qnpc4) + Int32.Parse(_qtpc4);

                        int _sumqnpc1 = Int32.Parse(_qnpc1) + Int32.Parse(_qnpc2) + Int32.Parse(_qnpc3) + Int32.Parse(_qnpc4);
                        int _sumqtpc1 = Int32.Parse(_qtpc1) + Int32.Parse(_qtpc2) + Int32.Parse(_qtpc3) + Int32.Parse(_qtpc4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVPC</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpc2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpc3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpc4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnpc1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtpc1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnpk1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPK", "HVN", _fromDate, _toDate);
                        string _qtpk1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPK", "HVN", _fromDate, _toDate);
                        int _totalpk1 = Int32.Parse(_qnpk1) + Int32.Parse(_qtpk1);

                        string _qnpk2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPK", "VJC", _fromDate, _toDate);
                        string _qtpk2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPK", "VJC", _fromDate, _toDate);
                        int _totalpk2 = Int32.Parse(_qnpk2) + Int32.Parse(_qtpk2);

                        string _qnpk3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPK", "PIC", _fromDate, _toDate);
                        string _qtpk3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPK", "PIC", _fromDate, _toDate);
                        int _totalpk3 = Int32.Parse(_qnpk3) + Int32.Parse(_qtpk3);

                        string _qnpk4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPK", "VFC", _fromDate, _toDate);
                        string _qtpk4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPK", "VFC", _fromDate, _toDate);
                        int _totalpk4 = Int32.Parse(_qnpk4) + Int32.Parse(_qtpk4);

                        int _sumqnpk1 = Int32.Parse(_qnpk1) + Int32.Parse(_qnpk2) + Int32.Parse(_qnpk3) + Int32.Parse(_qnpk4);
                        int _sumqtpk1 = Int32.Parse(_qtpk1) + Int32.Parse(_qtpk2) + Int32.Parse(_qtpk3) + Int32.Parse(_qtpk4);

                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVPK</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpk1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpk1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpk1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpk2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpk2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpk2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpk3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpk3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpk3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpk4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpk4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpk4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnpk1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtpk1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnpq1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPQ", "HVN", _fromDate, _toDate);
                        string _qtpq1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPQ", "HVN", _fromDate, _toDate);
                        int _totalpq1 = Int32.Parse(_qnpq1) + Int32.Parse(_qtpq1);

                        string _qnpq2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPQ", "VJC", _fromDate, _toDate);
                        string _qtpq2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPQ", "VJC", _fromDate, _toDate);
                        int _totalpq2 = Int32.Parse(_qnpq2) + Int32.Parse(_qtpq2);

                        string _qnpq3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPQ", "PIC", _fromDate, _toDate);
                        string _qtpq3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPQ", "PIC", _fromDate, _toDate);
                        int _totalpq3 = Int32.Parse(_qnpq3) + Int32.Parse(_qtpq3);

                        string _qnpq4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVPQ", "VFC", _fromDate, _toDate);
                        string _qtpq4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVPQ", "VFC", _fromDate, _toDate);
                        int _totalpq4 = Int32.Parse(_qnpq4) + Int32.Parse(_qtpq4);

                        int _sumqnpq1 = Int32.Parse(_qnpq1) + Int32.Parse(_qnpq2) + Int32.Parse(_qnpq3) + Int32.Parse(_qnpq4);
                        int _sumqtpq1 = Int32.Parse(_qtpq1) + Int32.Parse(_qtpq2) + Int32.Parse(_qtpq3) + Int32.Parse(_qtpq4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVPQ</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpq1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpq1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpq1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpq2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpq2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpq2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpq3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpq3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpq3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnpq4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtpq4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalpq4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnpq1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtpq1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnrg1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVRG", "HVN", _fromDate, _toDate);
                        string _qtrg1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVRG", "HVN", _fromDate, _toDate);
                        int _totalrg1 = Int32.Parse(_qnrg1) + Int32.Parse(_qtrg1);

                        string _qnrg2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVRG", "VJC", _fromDate, _toDate);
                        string _qtrg2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVRG", "VJC", _fromDate, _toDate);
                        int _totalrg2 = Int32.Parse(_qnrg2) + Int32.Parse(_qtrg2);

                        string _qnrg3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVRG", "PIC", _fromDate, _toDate);
                        string _qtrg3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVRG", "PIC", _fromDate, _toDate);
                        int _totalrg3 = Int32.Parse(_qnrg3) + Int32.Parse(_qtrg3);

                        string _qnrg4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVRG", "VFC", _fromDate, _toDate);
                        string _qtrg4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVRG", "VFC", _fromDate, _toDate);
                        int _totalrg4 = Int32.Parse(_qnrg4) + Int32.Parse(_qtrg4);

                        int _sumqnrg1 = Int32.Parse(_qnrg1) + Int32.Parse(_qnrg2) + Int32.Parse(_qnrg3) + Int32.Parse(_qnrg4);
                        int _sumqtrg1 = Int32.Parse(_qtrg1) + Int32.Parse(_qtrg2) + Int32.Parse(_qtrg3) + Int32.Parse(_qtrg4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVRG</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnrg1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtrg1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalrg1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnrg2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtrg2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalrg2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnrg3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtrg3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalrg3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnrg4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtrg4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalrg4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnrg1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtrg1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qnth1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTH", "HVN", _fromDate, _toDate);
                        string _qtth1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTH", "HVN", _fromDate, _toDate);
                        int _totalth1 = Int32.Parse(_qnth1) + Int32.Parse(_qtth1);

                        string _qnth2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTH", "VJC", _fromDate, _toDate);
                        string _qtth2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTH", "VJC", _fromDate, _toDate);
                        int _totalth2 = Int32.Parse(_qnth2) + Int32.Parse(_qtth2);

                        string _qnth3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTH", "PIC", _fromDate, _toDate);
                        string _qtth3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTH", "PIC", _fromDate, _toDate);
                        int _totalth3 = Int32.Parse(_qnth3) + Int32.Parse(_qtth3);

                        string _qnth4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTH", "VFC", _fromDate, _toDate);
                        string _qtth4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTH", "VFC", _fromDate, _toDate);
                        int _totalth4 = Int32.Parse(_qnth4) + Int32.Parse(_qtth4);

                        int _sumqnth1 = Int32.Parse(_qnth1) + Int32.Parse(_qnth2) + Int32.Parse(_qnth3) + Int32.Parse(_qnth4);
                        int _sumqtth1 = Int32.Parse(_qtth1) + Int32.Parse(_qtth2) + Int32.Parse(_qtth3) + Int32.Parse(_qtth4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVTH</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnth1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtth1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalth1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnth2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtth2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalth2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnth3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtth3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalth3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnth4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtth4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalth4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnth1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtth1 + "</td>";
                        _htmlContent += "</tr>";

                        string _qntx1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTX", "HVN", _fromDate, _toDate);
                        string _qttx1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTX", "HVN", _fromDate, _toDate);
                        int _totaltx1 = Int32.Parse(_qntx1) + Int32.Parse(_qttx1);

                        string _qntx2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTX", "VJC", _fromDate, _toDate);
                        string _qttx2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTX", "VJC", _fromDate, _toDate);
                        int _totaltx2 = Int32.Parse(_qntx2) + Int32.Parse(_qttx2);

                        string _qntx3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTX", "PIC", _fromDate, _toDate);
                        string _qttx3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTX", "PIC", _fromDate, _toDate);
                        int _totaltx3 = Int32.Parse(_qntx3) + Int32.Parse(_qttx3);

                        string _qntx4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVTX", "VFC", _fromDate, _toDate);
                        string _qttx4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVTX", "VFC", _fromDate, _toDate);
                        int _totaltx4 = Int32.Parse(_qntx4) + Int32.Parse(_qttx4);

                        int _sumqntx1 = Int32.Parse(_qntx1) + Int32.Parse(_qntx2) + Int32.Parse(_qntx3) + Int32.Parse(_qntx4);
                        int _sumqttx1 = Int32.Parse(_qttx1) + Int32.Parse(_qttx2) + Int32.Parse(_qttx3) + Int32.Parse(_qttx4);



                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVTX</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qntx1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qttx1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaltx1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qntx2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qttx2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaltx2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qntx3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qttx3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaltx3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qntx4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qttx4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totaltx4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqntx1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqttx1 + "</td>";
                        _htmlContent += "</tr>";


                        string _qnvd1 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVVD", "HVN", _fromDate, _toDate);
                        string _qtvd1 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVVD", "HVN", _fromDate, _toDate);
                        int _totalvd1 = Int32.Parse(_qnvd1) + Int32.Parse(_qtvd1);

                        string _qnvd2 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVVD", "VJC", _fromDate, _toDate);
                        string _qtvd2 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVVD", "VJC", _fromDate, _toDate);
                        int _totalvd2 = Int32.Parse(_qnvd2) + Int32.Parse(_qtvd2);

                        string _qnvd3 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVVD", "PIC", _fromDate, _toDate);
                        string _qtvd3 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVVD", "PIC", _fromDate, _toDate);
                        int _totalvd3 = Int32.Parse(_qnvd3) + Int32.Parse(_qtvd3);

                        string _qnvd4 = _DAL.DHB_Fun_SUM_CBQN_BYSB_DAL("VVVD", "VFC", _fromDate, _toDate);
                        string _qtvd4 = _DAL.DHB_Fun_SUM_CBQT_BYSB_DAL("VVVD", "VFC", _fromDate, _toDate);
                        int _totalvd4 = Int32.Parse(_qnvd4) + Int32.Parse(_qtvd4);

                        int _sumqnvd1 = Int32.Parse(_qnvd1) + Int32.Parse(_qnvd2) + Int32.Parse(_qnvd3) + Int32.Parse(_qnvd4);
                        int _sumqtvd1 = Int32.Parse(_qtvd1) + Int32.Parse(_qtvd2) + Int32.Parse(_qtvd3) + Int32.Parse(_qtvd4);


                        _htmlContent += "<tr>";
                        _htmlContent += "<td class=\"tg-baqh\">VVVD</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnvd1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtvd1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalvd1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnvd2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtvd2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalvd2 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnvd3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtvd3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalvd3 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qnvd4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _qtvd4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _totalvd4 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqnvd1 + "</td>";
                        _htmlContent += "<td class=\"tg-baqh\">" + _sumqtvd1 + "</td>";
                        _htmlContent += "</tr>";
                 
            }
            
            _htmlContent += "</table>";
            _htmlContent += "</div>";
            return _htmlContent;
        }
        public string BuildFooter()
        {
            string _htmlFooter = "";
            //_htmlFooter += "<div id = \"footer\">";
            //_htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-k94d\" colspan=\"18\" style=\"height:14px;\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-k94d\" colspan=\"6\">NGƯỜI LẬP BIỂU</td>";
            //_htmlFooter += "<td class=\"tg-k94d\" colspan=\"6\">TRƯỞNG TT ĐHB &amp; ĐP LKL</td>";
            //_htmlFooter += "<td class=\"tg-k94d\" colspan=\"6\">GIÁM ĐỐC</td></tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-0dc2\" colspan=\"6\">";
            //_htmlFooter += "<br><br><br><br><br><br><br>";
            //_htmlFooter += "</td>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"6\" rowspan=\"4\"></td>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"6\" rowspan=\"4\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"6\"><span style = \"font-weight:bold;font-style: italic;\" > Nơi nhận :</span></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-dgz8\" colspan=\"6\">-Như kính gửi</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-oqgr\" colspan=\"6\">-Lưu VT, ĐPL</td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-2ph3\" colspan=\"18\"></td>";
            //_htmlFooter += "</tr>";
            //_htmlFooter += "<tr>";
            //_htmlFooter += "<td class=\"tg-sq11\" colspan=\"6\" rowspan=\"3\"></td>";
            //_htmlFooter += "<td class=\"tg-k94d\" colspan=\"6\" rowspan=\"3\"></td>";
            //_htmlFooter += "<td class=\"tg-k94d\" colspan=\"6\" rowspan=\"3\"></td>";
            //_htmlFooter += "</tr><tr></tr><tr></tr></table></div>";


            _htmlFooter += "<div id = \"footer\">";
            _htmlFooter += "<table class=\"tg\" style=\"width: 100%\">";
            _htmlFooter += "<tr>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"1\">NGƯỜI LẬP BIỂU</td>";
            _htmlFooter += "<td class=\"tg-k94d\" colspan=\"2\">TRUNG TÂM HĐB &amp; ĐP LKL</td>";
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

        public static DataTable GetTable()
        {
            
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("SANBAY", typeof(string));
            table.Columns.Add("HANG01", typeof(string));
            table.Columns.Add("HANG02", typeof(string));
            table.Columns.Add("HANG03", typeof(string));
            table.Columns.Add("HANG04", typeof(string));
           
            
            table.Rows.Add(1, "VVBM", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(2, "VVCA", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(3, "VVCI", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(4, "VVCM", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(5, "VVCR", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(6, "VVCS", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(7, "VVCT", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(8, "VVDB", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(9, "VVDH", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(10, "VVDL", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(11, "VVDN", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(12, "VVHL", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(13, "VVLC", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(14, "VVNB", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(15, "VVNC", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(16, "VVPB", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(17, "VVPC", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(18, "VVPK", "HVN", "VJC", "PIC", "VFC");
            table.Rows.Add(19, "VVPQ", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(20, "VVRG", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(21, "VVTH", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(22, "VVTX", "HVN", "VJC", "PIC", "VFC");
            //table.Rows.Add(23, "VVVD", "HVN", "VJC", "PIC", "VFC");

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
            this.CreateExcel(html, "THSLB_BCSL_HTN_LD_N_" + DateTime.Now.ToFileTime() + ".xls", Server.MapPath("~/Style/StyleRpDHB.css"));
        }

        protected void btnExportWord_Click(object sender, EventArgs e)
        {
            string html = GetAllContent();
            this.CreateWord(html, "THSLB_BCSL_HTN_LD_N_" + DateTime.Now.ToFileTime() + ".doc", Server.MapPath("~/Style/StyleRpDHB.css"));
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