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

namespace prjApplication.CalendarFlight
{
    public partial class CalendarSpec : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "CalendarDayFlight";
        public string IDCHA
        {
            get
            {
                try
                {
                    return Convert.ToInt64(Request["ID"]).ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        public string _ObjRender
        {
            get
            {
                DayFlights obj = new DayFlights();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                LoadMultiDropdownList();
                txtFromDate.Value= DateTime.Now.ToString("dd/MM/yyyy");
                txtFLIGHTDATE1.Value = DateTime.Now.ToString("dd/MM/yyyy");
                LoadData();
                lbnCreate.Visible = _Role.R_Add;
                btnRenderKhb.Visible = _Role.R_Pub;

            }
        }

        #region PHAN SEARCH
        protected void linkSearch_Click(object sender, EventArgs e)
        {

            PhanTrang1.PageIndex = 0;
            LoadData();
        }


        public string GetWhereConditionInGrid()
        {
            string where = " 1=1 ";
            if (!String.IsNullOrEmpty(txtPERMNBR1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(PERMNBR) like N'%{0}%'", UltilFunc.SqlFormatText(txtPERMNBR1.Value.Trim()));
            if (!String.IsNullOrEmpty(ddlPERMTYPE1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(PERMTYPE) like '%{0}%'", UltilFunc.SqlFormatText(ddlPERMTYPE1.Value.Trim()));

            if (!String.IsNullOrEmpty(txtFLIGHT_TYPE1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(FLIGHT_TYPE) like N'%{0}%'", UltilFunc.SqlFormatText(txtFLIGHT_TYPE1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtPURPOSE1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(PURPOSE) like N'%{0}%'", UltilFunc.SqlFormatText(txtPURPOSE1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtCRAFT_ID1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(CRAFT_ID) like N'%{0}%'", UltilFunc.SqlFormatText(txtCRAFT_ID1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtMTOW1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(MTOW) like N'%{0}%'", UltilFunc.SqlFormatText(txtMTOW1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtVALIDHOURS1.Value.Trim()))
                where += " AND VALIDHOURS = " + Convert.ToInt32(txtVALIDHOURS1.Value.Trim());
            // if (!String.IsNullOrEmpty(txtDATEOLD1.Value.Trim()))
            //  where += " AND DATE_OLD =" + whereDateHelper(txtDATEOLD1.Value.Trim()) + "";
            if (!String.IsNullOrEmpty(txtFLIGHTDATE1.Value.Trim()))
                where += " AND FLIGHTDATE  =" + whereDateHelper(txtFLIGHTDATE1.Value.Trim()) + "";
            if (!String.IsNullOrEmpty(txtFLIGHTNBR1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(FLIGHTNBR) like N'%{0}%'", UltilFunc.SqlFormatText(txtFLIGHTNBR1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtREGISTRATION1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(REGISTRATION) like N'%{0}%'", UltilFunc.SqlFormatText(txtREGISTRATION1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtFROM_AIRP1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(FROM_AIRP) like N'%{0}%'", UltilFunc.SqlFormatText(txtFROM_AIRP1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtTO_AIRP1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(TO_AIRP) like N'%{0}%'", UltilFunc.SqlFormatText(txtTO_AIRP1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtETD1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(ETD) like N'%{0}%'", UltilFunc.SqlFormatText(txtETD1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtETA1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(ETA) like N'%{0}%'", UltilFunc.SqlFormatText(txtETA1.Value.Trim()));
            //if (!String.IsNullOrEmpty(txtATD1.Value.Trim()))
            // where += " AND " + string.Format(" UPPER(ATD) like N'%{0}%'", UltilFunc.SqlFormatText(txtATD1.Value.Trim()));
            // if (!String.IsNullOrEmpty(txtATA1.Value.Trim()))
            // where += " AND " + string.Format(" UPPER(ATA) like N'%{0}%'", UltilFunc.SqlFormatText(txtATA1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtVIA1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(VIA) like N'%{0}%'", UltilFunc.SqlFormatText(txtVIA1.Value.Trim()));

            // if (!String.IsNullOrEmpty(txtLETTERNBR_PK1.Value.Trim()))
            // where += " AND LETTERNBR_PK  >=" + whereDateHelper(txtLETTERNBR_PK1.Value.Trim()) + "";
            // if (!String.IsNullOrEmpty(txtNBR1.Value.Trim()))
            // where += " AND " + string.Format(" UPPER(NBR) like N'%{0}%'", UltilFunc.SqlFormatText(txtNBR1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtOPER_ID1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(OPER_ID) like N'%{0}%'", UltilFunc.SqlFormatText(txtOPER_ID1.Value.Trim()));
            // if (!String.IsNullOrEmpty(txtPLANSTATUS1.Value.Trim()))
            // where += " AND PLAN_STATUS = " + Convert.ToInt32(txtPLANSTATUS1.Value.Trim());
            if (!String.IsNullOrEmpty(txtREMARK1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(REMARK) like N'%{0}%'", UltilFunc.SqlFormatText(txtREMARK1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtCODE1.Value.Trim()))
                where += " AND " + string.Format(" UPPER(CODE) like N'%{0}%'", UltilFunc.SqlFormatText(txtCODE1.Value.Trim()));
            if (!String.IsNullOrEmpty(txtDOF1.Value.Trim()))
                where += " AND DOF  >=" + whereDateHelper(txtDOF1.Value.Trim()) + "";
            where += $" and a.IsAccess=1 ";
            return HttpUtility.UrlEncode(where.ToUpper());
        }

        private string whereDateHelper(string value)
        {
            return $"TO_DATE('{value}', 'DD-MM-YYYY')";
        }


        protected void btnPDF_Click(object sender, EventArgs e)
        {
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter htw = new HtmlTextWriter(sw);
            //Repeater ax = grdSource;
            //ax.ID = "grdPDF";
            //ax.DataSource = new DayFlightsDAL().GetAllObject(GetWhereConditionInGrid());
            //ax.DataBind();
            //ax.RenderControl(htw);
            //string html = sw.ToString();
            //html = html.Insert(0, "<style>thead {display: table-header-group;}tfoot {display: table-row-group;}tr {page-break-inside: avoid;}</style> ");
            //this.CreatePDF(html, "LISTDAYFLIGHT_" + DateTime.Now.ToFileTime() + ".pdf", Server.MapPath("~/Style/assets/css/bootstrap1.min.css"), System.Drawing.Printing.PaperKind.A4Extra);

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            GridView ax = new GridView();
            ax.DataSource = new DayFlightsDAL().GetPageObject(100000000, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            //ax.DataSource = new PermMasterNoDAL().GetPagePermMasterNoExport(GetWhereConditionInGrid());
            ax.DataBind();
            this.CreateExcel(this.RenderToHTML(ax), "LISTDAYFLIGHT_" + DateTime.Now.ToFileTime() + ".xls");

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
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
                case "btnCreateOnclick":
                    kq = btnCreateOnclick(_arg[0]);
                    break;
                case "GetOneObject":
                    kq = GetOneObject(_arg[0]);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
                case "btnDeleteOnclick":
                    kq = btnDeleteOnclick(_arg[0]);
                    break;
                case "mbtnAddNewFlightDetailOnclick":
                    kq = mbtnAddNewFlightDetailOnclick(_arg[0]);
                    break;
                case "GetOneObjectNoUpdate":
                    kq = GetOneObjectNoUpdate(_arg[0]);
                    break;
                case "mDeleteRowOnclick":
                    kq = mDeleteRowOnclick(_arg[0]);
                    break;
                case "mShowDetail":
                    kq = mShowDetail(_arg[0]);
                    break;
                case "mbtnUpdateFlightDetailOnclick":
                    kq = mbtnUpdateFlightDetailOnclick(_arg[0]);
                    break;
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
            }

            return kq;
        }
        #endregion

        #region function
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new DayFlightsDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        void LoadData()
        {
            List<DayFlights> t = new List<DayFlights>();
            if (chkKhbTrungLap.Checked)
                t = new DayFlightsDAL().GetPageTrungLap(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            else
                t = new DayFlightsDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            if (t.Count > 0)
                PhanTrang1.TotalRecord = t.GetTotalRecord<DayFlights>();
            grdSource.DataSource = t;
            grdSource.DataBind();
        }

        private void LoadMultiDropdownList()
        {
            this.FillDropdownList<CraftType>(ddlCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_CODE", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");
            this.FillDropdownList<CraftType>(mCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(mPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_CODE", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(mFROM_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");
            this.FillDropdownList<Aero>(mTO_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");
            this.FillDropdownList<Oper>(ddlOPER_ID, new OperDAL().GetAllObject(), "OPER_ICAO", "OPER_ICAO");
            this.FillDropdownList<Oper>(mOPER_ID, new OperDAL().GetAllObject(), "OPER_ICAO", "OPER_ICAO");
        }
        private string LoadDataGrid(string[] ThamSo)
        {
            List<DayFlights> t = new DayFlightsDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            PhanTrang1.TotalRecord = t.GetTotalRecord<DayFlights>();
            grdSource.DataSource = t;
            grdSource.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            grdSource.RenderControl(hw);
            return sb.ToString();
        }
        private string GetOneObject(string id)
        {
            DayFlights obj = new DayFlightsDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string mShowDetail(string id)
        {
            return GetOneObject(id);
        }
        private string GetOneObjectNoUpdate(string id)
        {
            DayFlights obj = new DayFlightsDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DayFlights>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            obj.ISACCESS = 1;
            bool kq = new DayFlightsDAL().InsertObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            if (!kq)
                return "Insert error";
            return "Insert sussess";
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DayFlights>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            bool kq = new DayFlightsDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new DayFlightsDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new DayFlightsDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "NOK";
            return "OK";
        }
        private string mbtnAddNewFlightDetailOnclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DayFlights>(ThamSo, dateTimeConverter);
                var kq = new DayFlightsDAL().InsertReturnId(obj);
                string ax = kq.ToString() == "-1" ? "Insert sussess" : "Insert error";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[InsertReturnId] [{ax}]", 0.0);
                return kq.ToString();
            }
            catch (Exception ex) { return "-1"; }
        }

        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var kq = btnUpdateOnclick(thamso);
            return kq == "Update sussess" ? "OK" : "NOK";
        }
        #endregion

        #region control event

        protected void grdSource_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex != -1 && e.Row.RowType != DataControlRowType.Header)
            {
                GridViewRow gvRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
                GridView grd = ((GridView)sender);
                gvRow.CssClass = "detail-row";
                TableCell tCell = new TableCell();
                tCell.CssClass = "text-left";
                tCell.ColumnSpan = ((GridView)sender).Columns.Count - 1;
                string id = grd.DataKeys[e.Row.RowIndex].Value.ToString();

                if (id != "0")
                {
                    tCell.Text = rListHistoryFlightDetails(new DayFlightsDAL().GetHistoryById(id), id);
                    if (tCell.Text == "")
                        return;
                    gvRow.Cells.Add(tCell);
                    Table tbl = e.Row.Parent as Table;
                    tbl.Rows.Add(gvRow);
                }

                e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#FFFFFF'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'");
            }

        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }
        protected void lbDelete_Click(object sender, EventArgs e)
        {
            LinkButton lbbtn = (LinkButton)sender;
            bool kq = new DayFlightsDAL().DeleteObject(lbbtn.CommandArgument);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[DayFlightsDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            this.AlertMessage(ax);
            PhanTrang1_Paging_IndexChange(sender, e);

        }
        #endregion

        protected void btnRenderKhb_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Value != "")
            {
                object ax;
                //var ax = new clsResuftAPI().GetValueApiExtension("PERMISSION2TEXT", "RenderKhb", new { DATE_FLY = UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy") });
                if (ddlSelect.Value == "1")
                {
                    ax = new clsResuftAPI().GetValueApiExtension("PERMISSION2TEXT", "RenderKhb", new { DATE_FLY = UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy") });
                }
                else if (ddlSelect.Value == "1")
                    ax = new clsResuftAPI().GetValueApiExtension("PERMISSION2TEXT", "RenderKhb_oper", new { DATE_FLY = UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy") });
                else if (ddlSelect.Value == "2")
                    ax = new clsResuftAPI().GetValueApiExtension("PERMISSION2TEXT", "RenderKhb_airport", new { DATE_FLY = UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy") });
            }
            else
                this.AlertMessage("Select date!");
        }

        protected void btnRenderKhb_2_Click(object sender, EventArgs e)
        {
            if (txtFromDate.Value != "")
            {
                var ax = new clsResuftAPI().GetValueApiExtension("PERMISSION2TEXT", "RenderKhb_airport", new { DATE_FLY = UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy") });
            }
            else
                this.AlertMessage("Select date!");
        }

        protected void chkKhbTrungLap_CheckedChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}