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

namespace prjApplication.Report.KhiTuong
{
    public partial class RpKhiTuong : System.Web.UI.Page
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
            List<Oper> t = new OperDAL().GetAllObject();
            ddlOper.DataSource = t;
            ddlOper.DataTextField = "OPER_ICAO";
            ddlOper.DataValueField = "OPER_ICAO";
            ddlOper.DataBind();

            List<Aero> taero = new AeroDAL().GetListAll();
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
            
            lblBindData.Text = GenDataTable();
        }
        private string GenDataTable()
        {

            clsResuftAPI _api = new clsResuftAPI();
            DataTable _dt = null;            
            string _html = null;           
            int i = 0;
            foreach (ListItem item in ddlAirport.Items)
            {
                string _value1 = "0";
                string _value2 = "0";
                string _total = "0";
                string _airport = "";
                string _oper = "";
                if (item.Selected)
                {
                    _airport = item.Value.ToString();
                    _html += "<tr>";
                    _html += "<td class=\"tg-031e\">"+(i+1)+"</td>";
                    _html += "<td class=\"tg-031e\" colspan=\"5\"><b>"+ _airport + "</b></td>";                    
                    _html += "</tr>";                    
                    i++;
                    foreach (ListItem itemOper in ddlOper.Items)
                    {
                        if (itemOper.Selected)
                        {
                            _oper = itemOper.Value.ToString();
                            _value1 = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Fn_Sum_Hang_Airport_Time", new { P_FLIGHTDATE = UltilFunc.ToDate(txtPERMDATE.Value, "dd/MM/yyyy"), P_FROMHOUR = txtFromHours.Value, P_TOHOUR = txtToHours.Value, P_AIRPORT = _airport, P_OPER = _oper }).ToString();

                            _value2 = new clsResuftAPI().GetValueApiExtension("REPORT_STATISTIC_PKG", "Fn_Sum_Hang_Airport_Time2", new { P_FLIGHTDATE = UltilFunc.ToDate(txtPERMDATE.Value, "dd/MM/yyyy"), P_FROMHOUR = txtFromHours.Value, P_TOHOUR = txtToHours.Value, P_AIRPORT = _airport, P_OPER = _oper }).ToString();
                            _total = (Convert.ToDouble(_value1) + Convert.ToDouble(_value2)).ToString();
                            _html += "<tr>";
                            _html += "<td class=\"tg-031e\"></td>";
                            _html += "<td class=\"tg-031e\"></td>";
                            _html += "<td class=\"tg-031e\">" + _oper + "</td>";
                            _html += "<td class=\"tg-031e\">"+ _value1 + "</td>";
                            _html += "<td class=\"tg-031e\">"+ _value2 + "</td>";
                            _html += "<td class=\"tg-031e\">"+ _total + "</td>";
                            _html += "</tr>";
                        }
                    }
                }
            }
            return _html;
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