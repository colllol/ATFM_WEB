using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Drawing;

namespace prjApplication.CancelFlight
{
    public partial class ListCancelFlights : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "CancelFlight";
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                    Session.SetValueForControlSearch(this, _AliasSession);
                    Session["MySearchCANCELFLIGHTS"] = null;
                    
                }
                catch
                {                    
                    Session.SetValueSearch(_AliasSession, " 1=1");
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                }
                LoadData();
               
            }
        }

        #region PHAN SEARCH
        protected void linkSearch_Click(object sender, EventArgs e)
        {

            string where = GetWhereConditionInGrid();
            Session["MySearchCANCELFLIGHTS"] = DataTableSearch();
            Session.SetValueSearch(_AliasSession, where);
            CustomPaging1.PageIndex = 0;
            CustomPaging1.ValueSearch = where;
            LoadData();
        }

        public DataTable DataTableSearch()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("PERMNBR", typeof(string)));
            dt.Columns.Add(new DataColumn("FLIGHT_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("PERMTYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("FLIGHT_TYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("PURPOSE", typeof(string)));
            dt.Columns.Add(new DataColumn("CRAFT_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("MTOW", typeof(string)));
            dt.Columns.Add(new DataColumn("VALIDHOURS", typeof(string)));
            dt.Columns.Add(new DataColumn("DATE_OLD", typeof(string)));
            dt.Columns.Add(new DataColumn("FLIGHTDATE", typeof(string)));
            dt.Columns.Add(new DataColumn("FLIGHTNBR", typeof(string)));
            dt.Columns.Add(new DataColumn("REGISTRATION", typeof(string)));
            dt.Columns.Add(new DataColumn("FROM_AIRP", typeof(string)));
            dt.Columns.Add(new DataColumn("TO_AIRP", typeof(string)));
            dt.Columns.Add(new DataColumn("ETD", typeof(string)));
            dt.Columns.Add(new DataColumn("ETA", typeof(string)));
            dt.Columns.Add(new DataColumn("ATD", typeof(string)));
            dt.Columns.Add(new DataColumn("ATA", typeof(string)));
            dt.Columns.Add(new DataColumn("VIA", typeof(string)));
            dt.Columns.Add(new DataColumn("LASTUSER", typeof(string)));
            dt.Columns.Add(new DataColumn("LASTMODIFY", typeof(string)));
            dt.Columns.Add(new DataColumn("CRAFT_TYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("REMARK", typeof(string)));
            dt.Columns.Add(new DataColumn("FPL_VIA", typeof(string)));
            
            foreach (GridViewRow row in grdSource.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {


                    TextBox txtColum01 = (TextBox)row.FindControl("txtColum01");
                    TextBox txtColum02 = (TextBox)row.FindControl("txtColum02");
                    TextBox txtColum03 = (TextBox)row.FindControl("txtColum03");
                    TextBox txtColum04 = (TextBox)row.FindControl("txtColum04");
                    TextBox txtColum05 = (TextBox)row.FindControl("txtColum05");
                    TextBox txtColum06 = (TextBox)row.FindControl("txtColum06");
                    TextBox txtColum07 = (TextBox)row.FindControl("txtColum07");
                    TextBox txtColum08 = (TextBox)row.FindControl("txtColum08");
                    TextBox txtColum09 = (TextBox)row.FindControl("txtColum09");
                    TextBox txtColum10 = (TextBox)row.FindControl("txtColum10");
                    TextBox txtColum11 = (TextBox)row.FindControl("txtColum11");
                    TextBox txtColum12 = (TextBox)row.FindControl("txtColum12");
                    TextBox txtColum13 = (TextBox)row.FindControl("txtColum13");
                    TextBox txtColum14 = (TextBox)row.FindControl("txtColum14");
                    TextBox txtColum15 = (TextBox)row.FindControl("txtColum15");
                    TextBox txtColum16 = (TextBox)row.FindControl("txtColum16");
                    TextBox txtColum17 = (TextBox)row.FindControl("txtColum17");
                    TextBox txtColum18 = (TextBox)row.FindControl("txtColum18");
                    TextBox txtColum19 = (TextBox)row.FindControl("txtColum19");
                    TextBox txtColum20 = (TextBox)row.FindControl("txtColum20");
                    TextBox txtColum21 = (TextBox)row.FindControl("txtColum21");
                    TextBox txtColum22 = (TextBox)row.FindControl("txtColum22");
                    TextBox txtColum23 = (TextBox)row.FindControl("txtColum23");
                   

                    dr = dt.NewRow();
                    dr[0] = txtColum01.Text.ToString().Trim();
                    dr[1] = "0";
                    dr[2] = txtColum02.Text.ToString().Trim();
                    dr[3] = txtColum03.Text.ToString().Trim();
                    dr[4] = txtColum04.Text.ToString().Trim();
                    dr[5] = txtColum05.Text.ToString().Trim();
                    dr[6] = txtColum06.Text.ToString().Trim();
                    dr[7] = txtColum07.Text.ToString().Trim();
                    dr[8] = txtColum08.Text.ToString().Trim();
                    dr[9] = txtColum09.Text.ToString().Trim();
                    dr[10] = txtColum10.Text.ToString().Trim();
                    dr[11] = txtColum11.Text.ToString().Trim();
                    dr[12] = txtColum12.Text.ToString().Trim();
                    dr[13] = txtColum13.Text.ToString().Trim();
                    dr[14] = txtColum14.Text.ToString().Trim();
                    dr[15] = txtColum15.Text.ToString().Trim();
                    dr[16] = txtColum16.Text.ToString().Trim();
                    dr[17] = txtColum17.Text.ToString().Trim();
                    dr[18] = txtColum18.Text.ToString().Trim();
                    dr[19] = txtColum19.Text.ToString().Trim();
                    dr[20] = txtColum20.Text.ToString().Trim();
                    dr[21] = txtColum21.Text.ToString().Trim();
                    dr[22] = txtColum22.Text.ToString().Trim();
                    dr[23] = txtColum23.Text.ToString().Trim();
                   
                    dt.Rows.Add(dr);
                }
                break;
            }

            return dt;

        }
        public string GetWhereConditionInGrid()
        {
            string where = " 1=1 ";
            foreach (GridViewRow row in grdSource.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {

                    TextBox txtColum01 = (TextBox)row.FindControl("txtColum01");
                    TextBox txtColum02 = (TextBox)row.FindControl("txtColum02");
                    TextBox txtColum03 = (TextBox)row.FindControl("txtColum03");
                    TextBox txtColum04 = (TextBox)row.FindControl("txtColum04");
                    TextBox txtColum05 = (TextBox)row.FindControl("txtColum05");
                    TextBox txtColum06 = (TextBox)row.FindControl("txtColum06");
                    TextBox txtColum07 = (TextBox)row.FindControl("txtColum07");
                    TextBox txtColum08 = (TextBox)row.FindControl("txtColum08");
                    TextBox txtColum09 = (TextBox)row.FindControl("txtColum09");
                    TextBox txtColum10 = (TextBox)row.FindControl("txtColum10");
                    TextBox txtColum11 = (TextBox)row.FindControl("txtColum11");
                    TextBox txtColum12 = (TextBox)row.FindControl("txtColum12");
                    TextBox txtColum13 = (TextBox)row.FindControl("txtColum13");
                    TextBox txtColum14 = (TextBox)row.FindControl("txtColum14");
                    TextBox txtColum15 = (TextBox)row.FindControl("txtColum15");
                    TextBox txtColum16 = (TextBox)row.FindControl("txtColum16");
                    TextBox txtColum17 = (TextBox)row.FindControl("txtColum17");
                    TextBox txtColum18 = (TextBox)row.FindControl("txtColum18");
                    TextBox txtColum19 = (TextBox)row.FindControl("txtColum19");
                    TextBox txtColum20 = (TextBox)row.FindControl("txtColum20");
                    TextBox txtColum21 = (TextBox)row.FindControl("txtColum21");
                    TextBox txtColum22 = (TextBox)row.FindControl("txtColum22");
                    TextBox txtColum23 = (TextBox)row.FindControl("txtColum23");
                   

                    if (!String.IsNullOrEmpty(txtColum01.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(PERMNBR) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum01.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum02.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(PERMTYPE) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum02.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum03.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(FLIGHT_TYPE) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum03.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum04.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(PURPOSE) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum04.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum05.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(CRAFT_ID) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum05.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum06.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(MTOW) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum06.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum07.Text.Trim()))
                        where += " AND VALIDHOURS = " + Convert.ToInt32(txtColum07.Text.Trim());
                    if (!String.IsNullOrEmpty(txtColum08.Text.Trim()))
                        where += " AND DATE_OLD =" + whereDateHelper(txtColum08.Text.Trim()) + "";
                    if (!String.IsNullOrEmpty(txtColum09.Text.Trim()))
                        where += " AND FLIGHTDATE  >=" + whereDateHelper(txtColum09.Text.Trim()) + "";
                    if (!String.IsNullOrEmpty(txtColum10.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(FLIGHTNBR) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum10.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum11.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(REGISTRATION) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum11.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum12.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(FROM_AIRP) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum12.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum13.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(TO_AIRP) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum13.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum14.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(ETD) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum14.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum15.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(ETA) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum15.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum16.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(ATD) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum16.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum17.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(ATA) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum17.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum18.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(VIA) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum18.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum19.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(LASTUSER) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum19.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum20.Text.Trim()))
                        where += " AND LASTMODIFY  =" + whereDateHelper(txtColum20.Text.Trim()) + "";
                    if (!String.IsNullOrEmpty(txtColum21.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(CRAFT_TYPE) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum21.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum22.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(REMARK) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum22.Text.Trim()));
                    if (!String.IsNullOrEmpty(txtColum23.Text.Trim()))
                        where += " AND " + string.Format(" UPPER(FPL_VIA) like N'%{0}%'", UltilFunc.SqlFormatText(txtColum23.Text.Trim()));
                   
                }
                break;
            }

            return HttpUtility.UrlEncode(where.ToUpper());
        }

        private string whereDateHelper(string value)
        {
            return $"TO_DATE('{value}', 'DD-MM-YYYY')";
        }

        public DataView BindGridData(DataTable _dt)
        {

            try
            {

                DataTable dt = new DataTable();
                DataRow dr;

                dt.Columns.Add(new DataColumn("PERMNBR", typeof(string)));
                dt.Columns.Add(new DataColumn("FLIGHT_ID", typeof(string)));
                dt.Columns.Add(new DataColumn("PERMTYPE", typeof(string)));
                dt.Columns.Add(new DataColumn("FLIGHT_TYPE", typeof(string)));
                dt.Columns.Add(new DataColumn("PURPOSE", typeof(string)));
                dt.Columns.Add(new DataColumn("CRAFT_ID", typeof(string)));
                dt.Columns.Add(new DataColumn("MTOW", typeof(string)));
                dt.Columns.Add(new DataColumn("VALIDHOURS", typeof(string)));
                dt.Columns.Add(new DataColumn("DATE_OLD", typeof(string)));
                dt.Columns.Add(new DataColumn("FLIGHTDATE", typeof(string)));
                dt.Columns.Add(new DataColumn("FLIGHTNBR", typeof(string)));
                dt.Columns.Add(new DataColumn("REGISTRATION", typeof(string)));
                dt.Columns.Add(new DataColumn("FROM_AIRP", typeof(string)));
                dt.Columns.Add(new DataColumn("TO_AIRP", typeof(string)));
                dt.Columns.Add(new DataColumn("ETD", typeof(string)));
                dt.Columns.Add(new DataColumn("ETA", typeof(string)));
                dt.Columns.Add(new DataColumn("ATD", typeof(string)));
                dt.Columns.Add(new DataColumn("ATA", typeof(string)));
                dt.Columns.Add(new DataColumn("VIA", typeof(string)));
                dt.Columns.Add(new DataColumn("LASTUSER", typeof(string)));
                dt.Columns.Add(new DataColumn("LASTMODIFY", typeof(string)));
                dt.Columns.Add(new DataColumn("CRAFT_TYPE", typeof(string)));
                dt.Columns.Add(new DataColumn("REMARK", typeof(string)));
                dt.Columns.Add(new DataColumn("FPL_VIA", typeof(string)));
               

                if (_dt != null)
                {
                    if (_dt.Rows.Count > 0)
                    {
                        if (Session["MySearchCANCELFLIGHTS"] != null)
                        {
                            DataTable dtseach = (DataTable)Session["MySearchCANCELFLIGHTS"];
                            dt.Rows.Add(dtseach.Rows[0]["PERMNBR"].ToString(), "0", dtseach.Rows[0]["PERMTYPE"].ToString(), dtseach.Rows[0]["FLIGHT_TYPE"].ToString(), dtseach.Rows[0]["PURPOSE"].ToString(), dtseach.Rows[0]["CRAFT_ID"].ToString(), dtseach.Rows[0]["MTOW"].ToString(), dtseach.Rows[0]["VALIDHOURS"].ToString(),
                                dtseach.Rows[0]["DATE_OLD"].ToString(), dtseach.Rows[0]["FLIGHTDATE"].ToString(), dtseach.Rows[0]["FLIGHTNBR"].ToString(), dtseach.Rows[0]["REGISTRATION"].ToString(), dtseach.Rows[0]["FROM_AIRP"].ToString(), dtseach.Rows[0]["TO_AIRP"].ToString()
                                , dtseach.Rows[0]["ETD"].ToString(), dtseach.Rows[0]["ETA"].ToString(), dtseach.Rows[0]["ATD"].ToString(), dtseach.Rows[0]["ATA"].ToString(), dtseach.Rows[0]["VIA"].ToString(), dtseach.Rows[0]["LASTUSER"].ToString()
                                , dtseach.Rows[0]["LASTMODIFY"].ToString(), dtseach.Rows[0]["CRAFT_TYPE"].ToString(), dtseach.Rows[0]["REMARK"].ToString(), dtseach.Rows[0]["FPL_VIA"].ToString());
                        }
                        else
                            dt.Rows.Add("", "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                        for (int i = 0; i < _dt.Rows.Count; i++)
                        {
                            dr = dt.NewRow();
                            dr[0] = _dt.Rows[i]["PERMNBR"].ToString();
                            dr[1] = _dt.Rows[i]["FLIGHT_ID"].ToString();
                            dr[2] = _dt.Rows[i]["PERMTYPE"].ToString();
                            dr[3] = _dt.Rows[i]["FLIGHT_TYPE"].ToString();
                            dr[4] = _dt.Rows[i]["PURPOSE"].ToString();
                            dr[5] = _dt.Rows[i]["CRAFT_ID"].ToString();
                            dr[6] = _dt.Rows[i]["MTOW"].ToString();
                            dr[7] = _dt.Rows[i]["VALIDHOURS"].ToString();
                            if (_dt.Rows[i]["DATE_OLD"] != Convert.DBNull)
                            {
                                DateTime _dtold = Convert.ToDateTime(_dt.Rows[i]["DATE_OLD"].ToString());
                                if (_dtold != DateTime.MinValue)
                                    dr[8] = _dtold.ToString("dd/MM/yyyy");
                                else
                                    dr[8] = "";
                            }
                            else
                                dr[8] = "";


                            if (_dt.Rows[i]["FLIGHTDATE"] != Convert.DBNull)
                            {
                                DateTime _dtflight = Convert.ToDateTime(_dt.Rows[i]["FLIGHTDATE"].ToString());
                                if (_dtflight != DateTime.MinValue)
                                    dr[9] = _dtflight.ToString("dd/MM/yyyy");
                                else
                                    dr[9] = "";
                            }
                            else
                                dr[9] = "";

                            dr[10] = _dt.Rows[i]["FLIGHTNBR"].ToString();
                            dr[11] = _dt.Rows[i]["REGISTRATION"].ToString();
                            dr[12] = _dt.Rows[i]["FROM_AIRP"].ToString();
                            dr[13] = _dt.Rows[i]["TO_AIRP"].ToString();
                            dr[14] = _dt.Rows[i]["ETD"].ToString();
                            dr[15] = _dt.Rows[i]["ETA"].ToString();
                            dr[16] = _dt.Rows[i]["ATD"].ToString();
                            dr[17] = _dt.Rows[i]["ATA"].ToString();
                            dr[18] = _dt.Rows[i]["VIA"].ToString();
                            dr[19] = _dt.Rows[i]["LASTUSER"].ToString();

                            if (_dt.Rows[i]["LASTMODIFY"] != Convert.DBNull)
                            {
                                DateTime _dtleter = Convert.ToDateTime(_dt.Rows[i]["LASTMODIFY"].ToString());
                                if (_dtleter != DateTime.MinValue)
                                    dr[20] = _dtleter.ToString("dd/MM/yyyy HH:mm");
                                else
                                    dr[20] = "";
                            }
                            else
                                dr[20] = "";

                            dr[21] = _dt.Rows[i]["CRAFT_TYPE"].ToString();
                            dr[22] = _dt.Rows[i]["REMARK"].ToString();
                            dr[23] = _dt.Rows[i]["FPL_VIA"].ToString();                           
                            dt.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    if (Session["MySearchCANCELFLIGHTS"] != null)
                    {
                        DataTable dtseach = (DataTable)Session["MySearchCANCELFLIGHTS"];
                        dt.Rows.Add(dtseach.Rows[0]["PERMNBR"].ToString(), "0", dtseach.Rows[0]["PERMTYPE"].ToString(), dtseach.Rows[0]["FLIGHT_TYPE"].ToString(), dtseach.Rows[0]["PURPOSE"].ToString(), dtseach.Rows[0]["CRAFT_ID"].ToString(), dtseach.Rows[0]["MTOW"].ToString(), dtseach.Rows[0]["VALIDHOURS"].ToString(),
                            dtseach.Rows[0]["DATE_OLD"].ToString(), dtseach.Rows[0]["FLIGHTDATE"].ToString(), dtseach.Rows[0]["FLIGHTNBR"].ToString(), dtseach.Rows[0]["REGISTRATION"].ToString(), dtseach.Rows[0]["FROM_AIRP"].ToString(), dtseach.Rows[0]["TO_AIRP"].ToString()
                            , dtseach.Rows[0]["ETD"].ToString(), dtseach.Rows[0]["ETA"].ToString(), dtseach.Rows[0]["ATD"].ToString(), dtseach.Rows[0]["ATA"].ToString(), dtseach.Rows[0]["VIA"].ToString(), dtseach.Rows[0]["LASTUSER"].ToString()
                            , dtseach.Rows[0]["LASTMODIFY"].ToString(), dtseach.Rows[0]["CRAFT_TYPE"].ToString(), dtseach.Rows[0]["REMARK"].ToString(), dtseach.Rows[0]["FPL_VIA"].ToString());
                    }
                    else
                        dt.Rows.Add("", "0", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "");
                }

                DataView dv = new DataView(dt);
                return dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region call back
        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { _phanCach }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
               
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
               
                
            }

            return kq;
        }
        #endregion
        #region function
       
        void LoadData()
        {
            List<CancelFlights> t = new CancelFlightsDAL().GetListPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<CancelFlights>();
            DataTable dt = new CancelFlightsDAL().GetTableObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            //Add by Nvthai
            DataView _dv = BindGridData(dt);
            grdSource.DataSource = _dv;
            grdSource.DataBind();
        }

        
        private string LoadDataGrid(string[] ThamSo)
        {
            List<CancelFlights> t = new CancelFlightsDAL().GetListPage(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<CancelFlights>();
            grdSource.DataSource = t;
            grdSource.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            grdSource.RenderControl(hw);
            return sb.ToString();
        }
       
        #endregion

        #region control event

        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1 && e.Row.RowType != DataControlRowType.Header)
            {
                
                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFFF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
            }
            if (e.Row.RowIndex == 0 && e.Row.RowType != DataControlRowType.Header)
            {
                TextBox txtColum01 = (TextBox)e.Row.FindControl("txtColum01");
                txtColum01.Visible = true;
                TextBox txtColum02 = (TextBox)e.Row.FindControl("txtColum02");
                txtColum02.Visible = true;
                TextBox txtColum03 = (TextBox)e.Row.FindControl("txtColum03");
                txtColum03.Visible = true;
                TextBox txtColum04 = (TextBox)e.Row.FindControl("txtColum04");
                txtColum04.Visible = true;
                TextBox txtColum05 = (TextBox)e.Row.FindControl("txtColum05");
                txtColum05.Visible = true;
                TextBox txtColum06 = (TextBox)e.Row.FindControl("txtColum06");
                txtColum06.Visible = true;
                TextBox txtColum07 = (TextBox)e.Row.FindControl("txtColum07");
                txtColum07.Visible = true;
                TextBox txtColum08 = (TextBox)e.Row.FindControl("txtColum08");
                txtColum08.Visible = true;
                TextBox txtColum09 = (TextBox)e.Row.FindControl("txtColum09");
                txtColum09.Visible = true;
                TextBox txtColum10 = (TextBox)e.Row.FindControl("txtColum10");
                txtColum10.Visible = true;
                TextBox txtColum11 = (TextBox)e.Row.FindControl("txtColum11");
                txtColum11.Visible = true;
                TextBox txtColum12 = (TextBox)e.Row.FindControl("txtColum12");
                txtColum12.Visible = true;
                TextBox txtColum13 = (TextBox)e.Row.FindControl("txtColum13");
                txtColum13.Visible = true;
                TextBox txtColum14 = (TextBox)e.Row.FindControl("txtColum14");
                txtColum14.Visible = true;

                TextBox txtColum15 = (TextBox)e.Row.FindControl("txtColum15");
                txtColum15.Visible = true;
                TextBox txtColum16 = (TextBox)e.Row.FindControl("txtColum16");
                txtColum16.Visible = true;
                TextBox txtColum17 = (TextBox)e.Row.FindControl("txtColum17");
                txtColum17.Visible = true;
                TextBox txtColum18 = (TextBox)e.Row.FindControl("txtColum18");
                txtColum18.Visible = true;
                TextBox txtColum19 = (TextBox)e.Row.FindControl("txtColum19");
                txtColum19.Visible = true;
                TextBox txtColum20 = (TextBox)e.Row.FindControl("txtColum20");
                txtColum20.Visible = true;
                TextBox txtColum21 = (TextBox)e.Row.FindControl("txtColum21");
                txtColum21.Visible = true;
                TextBox txtColum22 = (TextBox)e.Row.FindControl("txtColum22");
                txtColum22.Visible = true;
                TextBox txtColum23 = (TextBox)e.Row.FindControl("txtColum23");
                txtColum23.Visible = true;
                

                Button btnSearch = (Button)e.Row.FindControl("linkSearch");
                btnSearch.Visible = true;

               
                HtmlControl htmlcol01 = (HtmlControl)e.Row.FindControl("col01");
                htmlcol01.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol02 = (HtmlControl)e.Row.FindControl("col02");
                htmlcol02.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol03 = (HtmlControl)e.Row.FindControl("col03");
                htmlcol03.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol04 = (HtmlControl)e.Row.FindControl("col04");
                htmlcol04.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol05 = (HtmlControl)e.Row.FindControl("col05");
                htmlcol05.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol06 = (HtmlControl)e.Row.FindControl("col06");
                htmlcol06.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol07 = (HtmlControl)e.Row.FindControl("col07");
                htmlcol07.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol08 = (HtmlControl)e.Row.FindControl("col08");
                htmlcol08.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol09 = (HtmlControl)e.Row.FindControl("col09");
                htmlcol09.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol10 = (HtmlControl)e.Row.FindControl("col10");
                htmlcol10.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol11 = (HtmlControl)e.Row.FindControl("col11");
                htmlcol11.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol12 = (HtmlControl)e.Row.FindControl("col12");
                htmlcol12.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol13 = (HtmlControl)e.Row.FindControl("col13");
                htmlcol13.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol14 = (HtmlControl)e.Row.FindControl("col14");
                htmlcol14.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol15 = (HtmlControl)e.Row.FindControl("col15");
                htmlcol15.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol16 = (HtmlControl)e.Row.FindControl("col16");
                htmlcol16.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol17 = (HtmlControl)e.Row.FindControl("col17");
                htmlcol17.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol18 = (HtmlControl)e.Row.FindControl("col18");
                htmlcol18.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol19 = (HtmlControl)e.Row.FindControl("col19");
                htmlcol19.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol20 = (HtmlControl)e.Row.FindControl("col20");
                htmlcol20.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol21 = (HtmlControl)e.Row.FindControl("col21");
                htmlcol21.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol22 = (HtmlControl)e.Row.FindControl("col22");
                htmlcol22.Attributes.Add("style", "display:none;");

                HtmlControl htmlcol23 = (HtmlControl)e.Row.FindControl("col23");
                htmlcol23.Attributes.Add("style", "display:none;");

                

                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F1F5FA'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#D6E1EA'");


            }
        }

        #endregion

    }
}