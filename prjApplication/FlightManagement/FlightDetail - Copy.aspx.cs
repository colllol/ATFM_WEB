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
namespace prjApplication.FlightManagement
{
    public partial class FlightDetail : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";        
        private string _AliasSession = "FlightDetail";
        public string _ObjRender
        {
            get
            {
                FlightPermissionDetail obj = new FlightPermissionDetail();

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
            }
           
            //LoadMultiDropdownList();
        }
        protected void PhanTrang1_Paging_IndexChange(object sender, EventArgs e)
        {
            LoadData();
        }
        public string GetWhereConditionInGrid()
        {
            string where = " 1=1 ";
            var ax = Convert.ToInt32(Request["ID"]);
            where = where + " AND CALENDAR_ID = " + ax.ToString();
            return where;
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
            }

            return kq;
        }
        #endregion

        #region function
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //GridView ax = new GridView();
            //List<FlightPermissionDetail> t = new FlightPermissionDetailDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            //ax.DataSource = t;
            //ax.DataBind();
            //this.CreateExcel(this.RenderToHTML(ax), "LISTDAYFLIGHT_" + DateTime.Now.ToFileTime() + ".xls");
            this.CreateExcel(this.RenderToHTML(grid_List), "LISTDAYFLIGHT_.xls");
        }
        void LoadData()
        {
            List<FlightPermissionDetail> t = new FlightPermissionDetailDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            if (t.Count == 0)
                PhanTrang1.TotalRecord = 0;
            else
                t.GetTotalRecord<FlightPermissionDetail>();

            //grdSource.DataSource = t;
            //grdSource.DataBind();
            grid_List.DataSource = t;
            grid_List.DataBind();

        }
        private void LoadMultiDropdownList()
        {
            this.FillDropdownList<FlightPermission>(ddlCALENDAR_ID, new FlightPermissionDAL().GetAllObject(), "CALENDAR_NAME", "CALENDAR_ID");
            this.FillDropdownList<CraftType>(ddlCARAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(ddlPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");

            this.FillDropdownList<FlightPermission>(mCALENDAR_ID, new FlightPermissionDAL().GetAllObject(), "CALENDAR_NAME", "CALENDAR_ID");
            this.FillDropdownList<CraftType>(mCARAFT_ID, new CraftTypeDAL().GetAllCraftType(), "MA", "CRAFT_ID");
            this.FillDropdownList<FlyPurpose>(mPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            this.FillDropdownList<Aero>(mFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.FillDropdownList<Aero>(mTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            this.SetDisplayDropdownList(ddlCALENDAR_ID, Request["ID"]);
            this.SetDisplayDropdownList(mCALENDAR_ID, Request["ID"]);
        }
        private string LoadDataGrid(string[] ThamSo)
        {
            List<FlightPermissionDetail> t = new FlightPermissionDetailDAL().GetPageObject(PhanTrang1.PageSize, PhanTrang1.PageIndex, GetWhereConditionInGrid());
            PhanTrang1.TotalRecord = t.GetTotalRecord<FlightPermissionDetail>();
            //grdSource.DataSource = t;
            //grdSource.DataBind();
            grid_List.DataSource = t;
            grid_List.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            //grdSource.RenderControl(hw);
            grid_List.RenderControl(hw);
            return sb.ToString();
        }
        private string GetOneObject(string id)
        {
            FlightPermissionDetail obj = new FlightPermissionDetailDAL().GetOneObject(Convert.ToInt32(id));
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
            FlightPermissionDetail obj = new FlightPermissionDetailDAL().GetOneObject(Convert.ToInt32(id));
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPermissionDetail>(ThamSo, dateTimeConverter);
            obj.LAST_USER = _user.UserID.ToString();
            obj.USER_NAME = _user.UserID.ToString();
            obj.ID = 0;
            bool kq = new FlightPermissionDetailDAL().InsertObject(obj);
            if (!kq)
                return "Insert error";
            return "Insert sussess";
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPermissionDetail>(ThamSo, dateTimeConverter);
            obj.LAST_USER = _user.UserID.ToString();
            if (obj.ID == 0)
            {
                return "Update error";
            }
            bool kq = new FlightPermissionDetailDAL().UpdateObject(obj);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new FlightPermissionDetailDAL().DeleteObject(thamso);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new FlightPermissionDetailDAL().DeleteObject(thamso);
            if (!kq)
                return "NOK";
            return "OK";
        }
        private string mbtnAddNewFlightDetailOnclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPermissionDetail>(ThamSo, dateTimeConverter);
                obj.ID = 0;
                var kq = new FlightPermissionDetailDAL().InsertReturnId(obj);
                return kq.ToString();
            }
            catch (Exception ex){ return "-1"; }
        }        
        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var kq = btnUpdateOnclick(thamso);
            return kq == "Update sussess" ? "OK" : "NOK";
        }
        #endregion

        #region control event
        protected void linkSearch_Click(object sender, EventArgs e)
        {

        }
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
                tCell.Text = rListHistoryFlightDetails(new prjBusinessLogic.FlightPermissionDetailDAL().GetHistoryById(id), id);
                if (tCell.Text == "")
                    return;
                gvRow.Cells.Add(tCell);
                Table tbl = e.Row.Parent as Table;
                tbl.Rows.Add(gvRow);
            }
        }

        #endregion

    }
}