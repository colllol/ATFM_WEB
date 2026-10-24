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
    public partial class ChartReport : System.Web.UI.Page
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
            UserDAL _objDAL = new UserDAL();

            string _level="99";
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

                if (_level == "99")
                    litLevel.Text = "&nbsp;chưa có";
                else
                    litLevel.Text = _objDAL.GetLevel(_areo);
            }
            
            DataTable _dt;
            try
            {
                ReportDHBDAL _obj = new ReportDHBDAL();
                _dt = _obj.Char01(_date, _areo);
                grdList.DataSource = _dt;
                grdList.DataBind();

                _value += "var " + _data + " = new google.visualization.arrayToDataTable([";
                _value += " ['Day','Total'],";
              
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




                    _tmp += "['1',"  + (Convert.ToInt32(_tmp1[0].ToString()) + Convert.ToInt32(_tmp1[1].ToString())) + "],";
                    _tmp += "['2',"  + (Convert.ToInt32(_tmp2[0].ToString()) + Convert.ToInt32(_tmp2[1].ToString())) + "],";
                    _tmp += "['3',"  + (Convert.ToInt32(_tmp3[0].ToString()) + Convert.ToInt32(_tmp3[1].ToString())) + "],";
                    _tmp += "['4',"  + (Convert.ToInt32(_tmp4[0].ToString()) + Convert.ToInt32(_tmp4[1].ToString())) + "],";
                    _tmp += "['5',"  + (Convert.ToInt32(_tmp5[0].ToString()) + Convert.ToInt32(_tmp5[1].ToString())) + "],";
                    _tmp += "['6',"  + (Convert.ToInt32(_tmp6[0].ToString()) + Convert.ToInt32(_tmp6[1].ToString())) + "],";
                    _tmp += "['7'," + (Convert.ToInt32(_tmp7[0].ToString()) + Convert.ToInt32(_tmp7[1].ToString())) + "],";
                    _tmp += "['8',"  + (Convert.ToInt32(_tmp8[0].ToString()) + Convert.ToInt32(_tmp8[1].ToString())) + "],";
                    _tmp += "['9',"  + (Convert.ToInt32(_tmp9[0].ToString()) + Convert.ToInt32(_tmp9[1].ToString())) + "],";
                    _tmp += "['10',"   + (Convert.ToInt32(_tmp10[0].ToString()) + Convert.ToInt32(_tmp10[1].ToString())) + "],";
                    _tmp += "['11'," + (Convert.ToInt32(_tmp11[0].ToString()) + Convert.ToInt32(_tmp11[1].ToString())) + "],";
                    _tmp += "['12',"   + (Convert.ToInt32(_tmp12[0].ToString()) + Convert.ToInt32(_tmp12[1].ToString())) + "],";
                    _tmp += "['13',"   + (Convert.ToInt32(_tmp13[0].ToString()) + Convert.ToInt32(_tmp13[1].ToString())) + "],";
                    _tmp += "['14',"   + (Convert.ToInt32(_tmp14[0].ToString()) + Convert.ToInt32(_tmp14[1].ToString())) + "],";
                    _tmp += "['15',"   + (Convert.ToInt32(_tmp15[0].ToString()) + Convert.ToInt32(_tmp15[1].ToString())) + "],";
                    _tmp += "['16',"   + (Convert.ToInt32(_tmp16[0].ToString()) + Convert.ToInt32(_tmp16[1].ToString())) + "],";
                    _tmp += "['17',"  + (Convert.ToInt32(_tmp17[0].ToString()) + Convert.ToInt32(_tmp17[1].ToString())) + "],";
                    _tmp += "['18',"   + (Convert.ToInt32(_tmp18[0].ToString()) + Convert.ToInt32(_tmp18[1].ToString())) + "],";
                    _tmp += "['19',"   + (Convert.ToInt32(_tmp19[0].ToString()) + Convert.ToInt32(_tmp19[1].ToString())) + "],";
                    _tmp += "['20',"  + (Convert.ToInt32(_tmp20[0].ToString()) + Convert.ToInt32(_tmp20[1].ToString())) + "],";
                    _tmp += "['21',"  + (Convert.ToInt32(_tmp21[0].ToString()) + Convert.ToInt32(_tmp21[1].ToString())) + "],";
                    _tmp += "['22',"   + (Convert.ToInt32(_tmp22[0].ToString()) + Convert.ToInt32(_tmp22[1].ToString())) + "],";
                    _tmp += "['23'," + (Convert.ToInt32(_tmp23[0].ToString()) + Convert.ToInt32(_tmp23[1].ToString())) + "],";
                    _tmp += "['24',"  + (Convert.ToInt32(_tmp24[0].ToString()) + Convert.ToInt32(_tmp24[1].ToString())) + "],";


                }
                _value += _tmp + "]);";
                _value += "var " + _option + " = {'title': '" + _title + "',hAxis:{title: 'Hour'},vAxis:{title: 'Total CallSign'}, curveType: 'function'};";
                _value += "var " + _chart + " = new google.visualization.ColumnChart(document.getElementById('" + _div + "'))";
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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = new GridView();
            System.Data.DataTable t = new prjBusinessLogic.ReportDHBDAL().REPORTS_GET_ALL();
            ax.DataSource = t;
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "FlyplanType_" + DateTime.Now.ToFileTime() + ".xls");
        }
    }
}