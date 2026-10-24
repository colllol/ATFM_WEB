using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using prjComponents;
using prjInfo;
using prjBusinessLogic;
using prjBusinessLogic.DAL;
using HPCServerDataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace prjApplication.Common
{
    public partial class ChartReportLine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();                
            }
        }

        public void LoadData()
        {
            

            List<Aero> taero = new AeroDAL().GetListPageExport(" AE_CODE like 'VV%'");
            ddlAirport.DataSource = taero;
            ddlAirport.DataTextField = "AE_CODE";
            ddlAirport.DataValueField = "AE_CODE";
            ddlAirport.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //string lisOper = "";
            //string listAir = "";
            //foreach (ListItem item in ddlOper.Items)
            //{
            //    if(item.Selected)
            //    lisOper += $"{item.Value},";
            //}
            //lisOper = lisOper.Substring(0, lisOper.Length - 1);
            //foreach (ListItem item in ddlAirport.Items)
            //{
            //    if (item.Selected)
            //        listAir += $"{item.Value},";
            //}
            //listAir = listAir.Substring(0, listAir.Length - 1);

            //DataTable dt = getDatavalue(UltilFunc.ToDate(txtPERMDATE.Value, "dd/MM/yyyy"), txtFromHours.Value, txtToHours.Value, lisOper, listAir);
            
            //lblBindData.Text = GenDataTable();
        }
        public string drawChart_ByDate(string _data, string _option, string _chart, string _div, string _title)
        {
            string _value = "";
            string _date = "";
            string _areo = "VVTS";
            if (Request["fdate"] != null)
            {
                _date = Request["fdate"].ToString();
                _date = _date.Replace('/', '-');
            }
            if (_date == "")
                return "";
            if (Request["fairport"] != null)
            {
                _areo = Request["fairport"].ToString();
                
            }
            
            DataTable _dt;
            try
            {
                ReportDHBDAL _obj = new ReportDHBDAL();
                _dt = _obj.Char01(_date, _areo);
                _value += "var " + _data + " = new google.visualization.arrayToDataTable([";
                _value += " ['Day', 'LD', 'O/F'],";
                /*
                ['1', 1000, 400, 200],
                _value += _data + ".addColumn('string', 'Flow');";
                _value += _data + ".addColumn('number', 'CEMS1A');";
                _value += _data + ".addColumn('number', 'CEMS1B');";
                _value += _data + ".addColumn('number', 'CEMS2A');";
                _value += _data + ".addColumn('number', 'CEMS2B');";
                _value += _data + ".addRows(  ";*/


                string _tmp = "";
                foreach (DataRow row in _dt.Rows)
                {
                    string[] _tmp1 = row["h1"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp2 = row["h2"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp3 = row["h3"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp4 = row["h4"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp5 = row["h5"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp6 = row["h6"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp7 = row["h7"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp8 = row["h8"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp9 = row["h9"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp10 = row["h10"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp11 = row["h11"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp12 = row["h12"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp13 = row["h13"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp14 = row["h14"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp15 = row["h15"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp16 = row["h16"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp17 = row["h17"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp18 = row["h18"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp19 = row["h19"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp20 = row["h20"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp21 = row["h21"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp22 = row["h22"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp23 = row["h23"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] _tmp24 = row["h24"].ToString().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);



                    _tmp += "['1'," + _tmp1[0].ToString() + "," + _tmp1[1].ToString() + ","  + "],";
                    _tmp += "['2'," + _tmp2[0].ToString() + "," + _tmp2[1].ToString() + ","  + "],";
                    _tmp += "['3'," + _tmp3[0].ToString() + "," + _tmp3[1].ToString() + ","  + "],";
                    _tmp += "['4'," + _tmp4[0].ToString() + "," + _tmp4[1].ToString() + ","  + "],";
                    _tmp += "['5'," + _tmp5[0].ToString() + "," + _tmp5[1].ToString() + ","  + "],";
                    _tmp += "['6'," + _tmp6[0].ToString() + "," + _tmp6[1].ToString() + ","  + "],";
                    _tmp += "['7'," + _tmp7[0].ToString() + "," + _tmp7[1].ToString() + ","  + "],";
                    _tmp += "['8'," + _tmp8[0].ToString() + "," + _tmp8[1].ToString() + ","  + "],";
                    _tmp += "['9'," + _tmp9[0].ToString() + "," + _tmp9[1].ToString() + ","  + "],";
                    _tmp += "['10'," + _tmp10[0].ToString() + "," + _tmp10[1].ToString() + ","  + "],";
                    _tmp += "['11'," + _tmp11[0].ToString() + "," + _tmp11[1].ToString() + ","  + "],";
                    _tmp += "['12'," + _tmp12[0].ToString() + "," + _tmp12[1].ToString() + ","  + "],";
                    _tmp += "['13'," + _tmp13[0].ToString() + "," + _tmp13[1].ToString() + ","  + "],";
                    _tmp += "['14'," + _tmp14[0].ToString() + "," + _tmp14[1].ToString() + ","  + "],";
                    _tmp += "['15'," + _tmp15[0].ToString() + "," + _tmp15[1].ToString() + ","  + "],";
                    _tmp += "['16'," + _tmp16[0].ToString() + "," + _tmp16[1].ToString() + ","  + "],";
                    _tmp += "['17'," + _tmp17[0].ToString() + "," + _tmp17[1].ToString() + ","  + "],";
                    _tmp += "['18'," + _tmp18[0].ToString() + "," + _tmp18[1].ToString() + ","  + "],";
                    _tmp += "['19'," + _tmp19[0].ToString() + "," + _tmp19[1].ToString() + ","  + "],";
                    _tmp += "['20'," + _tmp20[0].ToString() + "," + _tmp20[1].ToString() + ","  + "],";
                    _tmp += "['21'," + _tmp21[0].ToString() + "," + _tmp21[1].ToString() + ","  + "],";
                    _tmp += "['22'," + _tmp22[0].ToString() + "," + _tmp22[1].ToString() + ","  + "],";
                    _tmp += "['23'," + _tmp23[0].ToString() + "," + _tmp23[1].ToString() + ","  + "],";
                    _tmp += "['24'," + _tmp24[0].ToString() + "," + _tmp24[1].ToString() + ","  + "],";


                }
                _value += _tmp + "]);";
                _value += "var " + _option + " = {'title': '" + _title + "',hAxis:{title: 'Hour'},vAxis:{title: 'Total CallSign'}, curveType: 'function'};";
                _value += "var " + _chart + " = new google.visualization.LineChart(document.getElementById('" + _div + "'))";
                _value += ".draw(" + _data + ", " + _option + ");";
            }
            catch
            {

            }
            return _value;
        }
     
        private DataTable getDatavalue(DateTime dDate, string fromHour, string toHour, string listOper, string listAirport)
        {
            return new clsResuftAPI().GetTableApiExtension("REPORT_STATISTIC_PKG", "SoLieuKhiTuong", new { P_FLIGHTDATE = dDate, P_FROMHOUR = fromHour, P_TOHOUR = toHour, P_LISTAIRPORT = listAirport, P_LISTOPER = listOper });
        }
    }
}