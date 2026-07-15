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
    public partial class RpDHB_SL_BieuDo_KhungGio : System.Web.UI.Page
    {
        public int _gioihan = 10;

        ReportStatisticDAL _DAL = new ReportStatisticDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdownList();  
            }
        }

        public string BuildContent()
        {
            
            string _html = "";

            _html += "<script>";

            _html += "Highcharts.chart('container', {";
            _html += "chart: { type: 'column'},";
            _html += "title: { text: 'BIỂU ĐỒ THỐNG KÊ ĐÁNH GIÁ NĂNG LỰC KHAI THÁC VÙNG TRỜI'},";
            _html += "xAxis: { categories: ['0000 - 0059', '0100 - 0159', '0200 - 0259', '0300 - 0359', '0400 - 0459', '0500 - 0559', '0600 - 0659', '0700 - 0759', '0800 - 0859', '0900 - 0959', '1000 - 1059', '1100 - 1159', '1200 - 1259', '1300 - 1359', '1400 - 1459', '1500 - 1559', '1600 - 1659', '1700 - 1759', '1800 - 1859', '1900 - 1959', '2000 - 2059', '2100 - 2159', '2200 - 2259', '2300 - 2359']},";
            _html += "yAxis: { min: 0,title: { text: ''},stackLabels: { enabled: true,style: { fontWeight: 'bold',color: (Highcharts.defaultOptions.title.style && Highcharts.defaultOptions.title.style.color) || 'grey'} } },";
            _html += "legend: { align: 'right',x: -30,verticalAlign: 'top',y: 25,floating: true,backgroundColor: Highcharts.defaultOptions.legend.backgroundColor || 'white',borderColor: '#CCC',borderWidth: 1,shadow: false},";
            _html += "tooltip: { headerFormat: '<b>{point.x}</b><br/>',pointFormat: '{series.name}: {point.y}<br/>Tổng chuyến: {point.stackTotal}'},";
            _html += "plotOptions: { column: { stacking: 'normal',dataLabels: { enabled: true} } },";
            _html += "series: [{ name: 'Số chuyến vượt giới hạn', data: ["+ BuildColour() + "] , color: Highcharts.getOptions().colors[3]},{ name: 'Số chuyến',data: [" + BuildTongChuyen() + "] , color: Highcharts.getOptions().colors[2]}";
            _html += " , { type: 'spline',name: 'Giới hạn của sân',data: ["+ _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + ", " + _gioihan + "],marker: { lineWidth: 2,lineColor: Highcharts.getOptions().colors[3],fillColor: 'white'} }]});";

            _html += "</script>";
                        
            return _html;
        }
        public string BuildTongChuyen()
        {
            ReportStatisticDAL _objCC = new ReportStatisticDAL();
            
            DataTable _dt = _objCC.THSLB_GET_FINISH_KHUNGGIO(txtFromDate.Value, txtFromDate.Value,"0000","2359");


            DataRow _mrow = null;
            string _htmlContent = null;
            string _TotalCat = "0";
            string _TotalHa = "0";
            int _Tong = 0;
            
            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];                       
                        _TotalCat = _objCC.THSLB_GET_FINISH_CAT_ATD(txtFromDate.Value, txtFromDate.Value, ddlAERO.SelectedItem.Text, "", _mrow["sf"].ToString(), _mrow["st"].ToString());
                        _TotalHa = _objCC.THSLB_GET_FINISH_HA_ATA(txtFromDate.Value, txtFromDate.Value, ddlAERO.SelectedItem.Text, "", _mrow["sf"].ToString(), _mrow["st"].ToString());

                        _Tong = Convert.ToInt32(_TotalCat.ToString()) + Convert.ToInt32(_TotalHa.ToString());
                        if (_htmlContent == null)
                        {
                            if(_Tong<= _gioihan)
                                _htmlContent = _Tong.ToString();
                            else
                                _htmlContent = _gioihan.ToString();
                        }                            
                        else
                        {
                            if (_Tong <= _gioihan)
                                _htmlContent += "," + _Tong.ToString();
                            else
                                _htmlContent += "," + _gioihan.ToString();
                        }                           

                    }
                }
            }
            return _htmlContent;
        }

        public string BuildColour()
        {
            ReportStatisticDAL _objCC = new ReportStatisticDAL();

            DataTable _dt = _objCC.THSLB_GET_FINISH_KHUNGGIO(txtFromDate.Value, txtFromDate.Value, "0000", "2359");


            DataRow _mrow = null;
            string _htmlMau = null;           
            string _TotalCat = "0";
            string _TotalHa = "0";
            int _Tong = 0;

            if (_dt != null)
            {
                if (_dt.Rows.Count > 0)
                {
                    for (int j = 0; j < _dt.Rows.Count; j++)
                    {
                        _mrow = _dt.Rows[j];
                        _TotalCat = _objCC.THSLB_GET_FINISH_CAT_ATD(txtFromDate.Value, txtFromDate.Value, ddlAERO.SelectedItem.Text, "", _mrow["sf"].ToString(), _mrow["st"].ToString());
                        _TotalHa = _objCC.THSLB_GET_FINISH_HA_ATA(txtFromDate.Value, txtFromDate.Value, ddlAERO.SelectedItem.Text, "", _mrow["sf"].ToString(), _mrow["st"].ToString());

                        _Tong = Convert.ToInt32(_TotalCat.ToString()) + Convert.ToInt32(_TotalHa.ToString());
                        if (_htmlMau == null)
                        {
                            if(_Tong<= _gioihan)
                                _htmlMau = "0";
                            else
                                _htmlMau = (_Tong - _gioihan).ToString();
                        }                           
                        else
                        {
                            if (_Tong <= _gioihan)
                                _htmlMau += "," + "0";
                            else
                                _htmlMau += "," + (_Tong - _gioihan).ToString();
                        }                          

                    }
                }
            }
            return _htmlMau;
        }


        protected void LoadDropdownList()
        {
            this.FillDropdownList<Aero>(ddlAERO, new AeroDAL().GetListAeroVV(),"AE_CODE", "ID", "--");            
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            
            ltrContent.Text = "";
            Aero obj = new AeroDAL().GetAeroById(ddlAERO.SelectedValue);
            if(obj!=null)
            {
                _gioihan = CommonLib.IsNumeric(obj.TN) ? Convert.ToInt32(obj.TN) : 0;
            } 
            ltrContent.Text = BuildContent();
            
        }
                       
    }
}