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
using prjComponents;
namespace prjApplication.ScheduleFlights
{
    public partial class ScheduleFlights : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _PhanCachArg = "_____";
        public string _IdSelect = "0";
        private string _AliasSession = "ScheduleFlights";
        private bool isSearch = false;
        public bool isShowDelete
        {
            get
            {
                if (ViewState["count"] == null) return false;
                return (bool)ViewState["count"];
            }
            set
            {
                ViewState["count"] = value;
            }
        }
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
                ScheduleDayFlights obj = new ScheduleDayFlights();

                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                LoadMultiDropdownList();

                btnCreateDayFlight.Visible = _Role.R_Pub;
                btnRenderCustom.Visible = _Role.R_Pub;
                btnCreate.Visible = _Role.R_Add;
            }
        }
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
            bool kq = new ScheduleDayFlightsDAL().RetoryHis(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[ScheduleFlightDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        void LoadData()
        {
            clsDaylyFlightSearch objSearch = GetObjSearch();
            if (isSearch)
                objSearch.PageIndex = 0;
            else
                objSearch.PageIndex = PhanTrang1.PageIndex;
            objSearch.PageSize = PhanTrang1.PageSize;
            isSearch = false;
            DataTable dt = new DataTable();
            if (!lnkDelete.Visible)
                dt = new ScheduleDayFlightsDAL().GetTableWithValueSearch(objSearch);
            else
                dt = new ScheduleDayFlightsDAL().GetTableDelete(objSearch);
            if (dt == null)
            {
                rpSourceBody.DataSource = dt;
                rpSourceBody.DataBind();
                return;
            }
            if (dt != null || dt.Rows.Count < 0) PhanTrang1.TotalRecord = Convert.ToInt32(dt.Rows[0]["TOTALRECORDS"].ToString());
            rpSourceBody.DataSource = dt;
            rpSourceBody.DataBind();
        }
        protected void linkSearch_Click(object sender, EventArgs e)
        {
            isSearch = true;
            LoadData();
            isSearch = true;
        }

        private void LoadMultiDropdownList()
        {
            this.FillDropdownList<CraftType>(ddlCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");

            this.FillDropdownList<FlyPurpose>(ddlPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            List<Aero> _objAero = new List<Aero>();
            _objAero = new AeroDAL().GetListAll();

            this.FillDropdownList<Aero>(ddlTO_AIRP, _objAero, "AE_CODE", "AE_CODE");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, _objAero, "AE_CODE", "AE_CODE");

            //this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");


            this.FillDropdownList<CraftType>(mCRAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(mPURPOSE, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");

            //this.FillDropdownList<Aero>(mFROM_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");
            //this.FillDropdownList<Aero>(mTO_AIRP, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE");
            this.FillDropdownList<Aero>(mFROM_AIRP, _objAero, "AE_CODE", "AE_CODE");
            this.FillDropdownList<Aero>(mTO_AIRP, _objAero, "AE_CODE", "AE_CODE");

            this.FillDropdownList<Oper>(ddlOPER_ID, new OperDAL().GetAllObject(), "OPER_NAME", "OPER_ICAO");
            this.FillDropdownList<Oper>(mOPER_ID, new OperDAL().GetAllObject(), "OPER_NAME", "OPER_ICAO");

            this.FillDropdownList<CraftType>(ddlCRAFT_Search, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID", "--");
            // this.FillDropdownList<Aero>(ddlFROM_AIRP_Search, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE", "--");
            //this.FillDropdownList<Aero>(ddlTO_AIRP_Search, new AeroDAL().GetListAll(), "AE_CODE", "AE_CODE", "--");
            this.FillDropdownList<Aero>(ddlFROM_AIRP_Search, _objAero, "AE_CODE", "AE_CODE", "--");
            this.FillDropdownList<Aero>(ddlTO_AIRP_Search, _objAero, "AE_CODE", "AE_CODE", "--");


            this.FillDropdownList<FlyPurpose>(ddlPURPOSE_Search, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE", "--");
            ddlFLIGHT_TYPE_Search.Items.Add(new ListItem("--", ""));
            ddlFLIGHT_TYPE_Search.Items.Add(new ListItem("SC", "SC"));
            ddlFLIGHT_TYPE_Search.Items.Add(new ListItem("NO", "NO"));
            ddlFLIGHT_TYPE_Search.SelectedIndex = 0;
            ddlPERMTYPE_Search.Items.Add(new ListItem("--", ""));
            ddlPERMTYPE_Search.Items.Add(new ListItem("LD", "LD"));
            ddlPERMTYPE_Search.Items.Add(new ListItem("O/F", "O/F"));
            ddlPERMTYPE_Search.SelectedIndex = 0;
        }
        private string LoadDataGrid(string[] ThamSo)
        {
            LoadData();
            return this.RenderToHTML(rpSourceBody);
        }
        private string GetOneObject(string id)
        {
            ScheduleDayFlights obj = new ScheduleDayFlightsDAL().GetObjectById(id);
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
            ScheduleDayFlights obj = new ScheduleDayFlightsDAL().GetObjectById(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ScheduleDayFlights>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            var kq = new ScheduleDayFlightsDAL().InsertReturnId(obj);
            string ax = kq == 1 ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[ScheduleFlightDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            if (kq == -1)
                return "Insert error";
            return "Insert sussess";
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ScheduleDayFlights>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            bool kq = new ScheduleDayFlightsDAL().Update(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[ScheduleFlightDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new ScheduleDayFlightsDAL().Delete(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[ScheduleFlightDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new ScheduleDayFlightsDAL().Delete(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[ScheduleFlightDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "NOK";
            return "OK";
        }
        private string mbtnAddNewFlightDetailOnclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ScheduleDayFlights>(ThamSo, dateTimeConverter);
                var kq = new ScheduleDayFlightsDAL().InsertReturnId(obj);
                string ax = kq.ToString() == "-1" ? "Insert sussess" : "Insert error";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[ScheduleFlightDAL]", 0, $"[InsertReturnId] [{ax}]", 0.0);
                return kq.ToString();
            }
            catch (Exception ex) { return "-1"; }
        }

        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var kq = btnUpdateOnclick(thamso);
            return kq == "Update sussess" ? "OK" : "NOK";
        }

        private clsDaylyFlightSearch GetObjSearch()
        {
            clsDaylyFlightSearch obj = new clsDaylyFlightSearch();
            if (!string.IsNullOrEmpty(txtFromDate.Value))
                obj.StartDate = UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy");
            if (!string.IsNullOrEmpty(txtToDate.Value))
                obj.FinishDate = UltilFunc.ToDate(txtToDate.Value, "dd/MM/yyyy");
            if(txtFLIGHTDATE_search.Text!="") obj.FLIGHTDATE = UltilFunc.ToDate(txtFLIGHTDATE_search.Text,"dd/MM/yyyy");
            if(txtDATE_OLD_Search.Text!="") obj.DATE_OLD = UltilFunc.ToDate(txtDATE_OLD_Search.Text, "dd/MM/yyyy");
            obj.PURPOSE = ddlPURPOSE_Search.SelectedValue;
            obj.CRAFT_TYPE = ddlCRAFT_Search.SelectedValue;
            obj.FROM_AIRP = ddlFROM_AIRP_Search.SelectedValue;
            obj.TO_AIRP = ddlTO_AIRP_Search.SelectedValue;
            obj.FLIGHT_TYPE = ddlFLIGHT_TYPE_Search.SelectedValue;
            obj.PERMTYPE = ddlPERMTYPE_Search.SelectedValue;
            obj.ETD = txtETD_Search.Text;
            obj.ETA = txtETA_Search.Text;
            obj.VIA = txtVIA_Search.Text;
            obj.PERMNBR = txtPERMNBR_Search.Text;
            obj.FLIGHTNBR = txtFLIGHTNBR_Search.Text;
            if (!string.IsNullOrEmpty(txtVALIDHOURS_Search.Text))
                obj.VALIDHOURS = Convert.ToInt64(txtVALIDHOURS_Search.Text);
            return obj;
        }
        #endregion
        protected void btnCreateDayFlight_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Value == "")
                {
                    this.AlertMessage("Please choise date from: From date");
                    txtFromDate.Focus();
                    return;
                }
                var ax = new ScheduleDayFlightsDAL().RenderTo_DayFlight(UltilFunc.ToDate(txtFromDate.Value, "dd/MM/yyyy"), _user.UserID.ToString());
                this.AlertMessage(ax == -1 ? "Error!" : "Sussess!");
            }
            catch (Exception ex)
            {
                this.AlertMessage(ex.ToString());
            }
            finally
            {
                this.AlertMessage("Sussess!");
            }
            this.AlertMessage("Sussess!");
        }
        protected void btnKeHoachBay_Click(object sender, EventArgs e)
        {

        }

        protected void rpSourceBody_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string id = ((Label)e.Item.FindControl("lblId")).Text;
                Label lblHis = (Label)e.Item.FindControl("lblHis");
                lblHis.Text = $"<tr class=\"detail-row\"><td colspan=\"22\" class=\"text-left\">{rListHistoryFlightDetails(new ScheduleDayFlightsDAL().GetHisById(id), id)}</td></tr>";
            }
        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }
        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            LinkHideDelete.Visible = !LinkHideDelete.Visible;
            lnkDelete.Visible = !lnkDelete.Visible;
            LoadData();
        }

        protected void lnkClear_Click(object sender, EventArgs e)
        {
            txtDATE_OLD_Search.Text = "";
            txtDOF_Search.Text = "";
            txtETA_Search.Text = "";
            txtETD_Search.Text = "";
            txtFLIGHTDATE_search.Text = "";
            txtFLIGHTNBR_Search.Text = "";
            //txtFLIGHT_TYPE_Search.Text = "";
            txtMTOW_Search.Text = "";
            txtNBR_Search.Text = "";
            txtPERMNBR_Search.Text = "";
            //txtPERMTYPE_Search.Text = "";
            txtREGISTRATION_Search.Text = "";
            txtREMARK_Search.Text = "";
            txtVALIDHOURS_Search.Text = "";
            txtVIA_Search.Text = "";
            ddlCRAFT_Search.SelectedIndex = 0;
            ddlFROM_AIRP_Search.SelectedIndex = 0;
            ddlPURPOSE_Search.SelectedIndex = 0;
            ddlTO_AIRP_Search.SelectedIndex = 0;
        }
        #region export
        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            clsDaylyFlightSearch objSearch = GetObjSearch();
            DataTable dt = new DataTable();
            objSearch.PageIndex = 0;
            objSearch.PageSize = 99999999;
            dt = new ScheduleDayFlightsDAL().GetTableWithValueSearch(objSearch);
            GridView gr = new GridView();
            gr.DataSource = dt;
            gr.DataBind();
            this.CreatePDF(this.RenderToHTML(gr), "ScheduleFlight.pdf", "", System.Drawing.Printing.PaperKind.A4);
        }

        protected void btnExportExel_Click(object sender, EventArgs e)
        {
            clsDaylyFlightSearch objSearch = GetObjSearch();
            DataTable dt = new DataTable();
            objSearch.PageIndex = 0;
            objSearch.PageSize = 99999999;
            dt = new ScheduleDayFlightsDAL().GetTableWithValueSearch(objSearch);
            GridView gr = new GridView();
            gr.DataSource = dt;
            gr.DataBind();
            this.CreateExcel(this.RenderToHTML(gr), "ScheduleFlight.xls");
        }

        #endregion

        protected void btnRenderCustom_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rpSourceBody.Items)
            {
                var chk = (CheckBox)item.FindControl("chkItem");
                if(chk.Checked)
                {
                    var ax = new clsResuftAPI().GetValueApiExtension("SCHEDULEDAYFLIGHTS_PKG", "RenderKhbById", new { P_ID= Convert.ToInt64(chk.Attributes["data-Id"].ToString())});
                }
            }
            LoadData();
        }
    }
}