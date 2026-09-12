using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using prjBusinessLogic;
using prjInfo;
using System.Web.Script.Serialization;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace prjApplication.FlightManagement
{
    public partial class ListFlight : PageBaseCallBack
    {
        public string _phanCach = "::::";
        public string _IdSelect = "0";
        private string _AliasSession = "ListFlight";
        public string _ObjRender
        {
            get
            {
                FlightPermission obj = new FlightPermission();

                var ser = new JavaScriptSerializer();                
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        //public object JsonConvert { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CustomPaging1.ValueSearch = Session[_AliasSession].ToString();
                    Session.SetValueForControlSearch(this, _AliasSession);
                }
                catch
                {
                    Session.SetValueSearch(_AliasSession, " 1=1");
                }
                
                LoadData();
                ddlTypeFlight_Load();
            }
        }

        #region Call back

        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
                case "btnCreateOnclick":
                    kq = btnCreateOnclick(_arg[0]);
                    break;
                case "GetOneFlight":
                    kq = GetOneFlight(ThamSo);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(ThamSo);
                    break;
                case "btnDeleteOnclick":
                    kq = btnDeleteOnclick(_arg[0]);
                    break;
            }
            return kq;
        }

        #endregion

        #region function
        void LoadData()
        {
            List<FlightPermission> t = new FlightPermissionDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<FlightPermission>();
            grdSource.DataSource = t;
            grdSource.DataBind();
        }
        void ddlTypeFlight_Load()
        {
            //List<FlightPermission> t = new FlightPermissionDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            //this.FillDropdownList<FlightPermission>(ddlTypeFlight, t, "CTR_ENAME","CTR_CODE");
        }

        private string LoadDataGrid(string[] ThamSo)
        {
            List<FlightPermission> t = new FlightPermissionDAL().GetPageObject(CustomPaging1.PageSize, CustomPaging1.PageIndex, CustomPaging1.ValueSearch);
            CustomPaging1.TotalsRecord = t.GetTotalRecord<FlightPermission>();
            grdSource.DataSource = t;
            grdSource.DataBind();
            StringBuilder sb = new StringBuilder();
            StringWriter tw = new StringWriter(sb);
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            grdSource.RenderControl(hw);
            return sb.ToString();
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPermission>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            bool kq = new FlightPermissionDAL().UpdateObject(obj);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new FlightPermissionDAL().DeleteObject(thamso);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string btnCreateOnclick(string ThamSo)
        {
            
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPermission>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserID.ToString();
            obj.USER_NAME = _user.UserID.ToString();
            obj.ID = 0;
            //bool kq = new FlightPermissionDAL().InsertObject(obj);
            string _obj = new FlightPermissionDAL().InsertObjectReturn(obj);
            if (_obj== "0")
                return "Insert error";
            else
            {
                CreateSeason(_obj, ThamSo);
                return "Insert sussess";
            }
            
        }
        public void CreateSeason(string _obj,string Thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FlightPermission>(Thamso, dateTimeConverter);
            var ax = new clsResuftAPI().GetPostValueApiExtension("PERMISSION_SEASON", "GEN_SEASON_CALENDAR", new { PERM_TYPE= obj.TYPE, SEASON= obj.SEASON, YEAR= obj.YEAR, CALENDAR_ID=_obj});
        }
        private string GetOneFlight(string[] ThamSo)
        {
            FlightPermission obj = new FlightPermissionDAL().GetOneObject(Convert.ToInt32(ThamSo[0]));
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private void PutclsSearchDetail()
        {
            List<clsSearchDetail> lisSearch = new List<clsSearchDetail>();
            lisSearch.Add(new clsSearchDetail("CTR_CODE", "txtSearch_UserName", txtSearch_UserName.Text.Trim()));
            Session.SetValueSearch(_AliasSession, lisSearch);
        }

        #endregion

        #region control event
        protected void linkSearch_Click(object sender, EventArgs e)
        {
            string where = " 1=1";
            if (!String.IsNullOrEmpty(txtSearch_UserName.Text.Trim()))
                where += " AND " + string.Format(" CTR_CODE like N'%{0}%'", UltilFunc.SqlFormatText(this.txtSearch_UserName.Text.Trim()));
            PutclsSearchDetail();
            Session.SetValueSearch(_AliasSession, where);
            CustomPaging1.PageIndex = 0;
            CustomPaging1.ValueSearch = where;
            LoadData();
        }

        protected void btnAddCountry_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region page event

        #endregion

        #region grd event
        protected void grdSource_EditCommand(object source, DataGridCommandEventArgs e)
        {
           
        }

        protected void grdSource_DeleteCommand(object source, DataGridCommandEventArgs e)
        {

        }
        protected void grdSource_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "RedectDetail":
                    Response.Redirect("~/FlightManagement/FlightDetail.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString() + "&ID=" + e.CommandArgument.ToString());
                    break;
            }
        }

        #endregion


    }
}