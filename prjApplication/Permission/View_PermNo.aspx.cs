using Newtonsoft.Json.Converters;
using prjBusinessLogic;
using prjComponents;
using prjInfo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace prjApplication.Permission
{
    public partial class View_PermNo : PageBaseCallBack
    {
        
        public string _phanCach = "::::";
        public string _ObjRender
        {
            get
            {
                PermDetailNo obj = new PermDetailNo();
                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddl_Load();
                GetInfoByFirstLoad();
                //LoadDataGrid();
            }
        }


        public void ddl_Load()
        {
            this.FillDropdownList(ddlAUTHOR_ID, new FpAuthorDAL().GetAllObject(), "AUTHOR_NAME", "AUTHOR_CODE");
            //this.FillDropdownList(ddlOPER_ID, new OperDAL().GetAllObject(), "OPER_NAME", "OPER_ICAO");

            //this.FillDropdownList<FlyPurpose>(ddlPURPOSE_ID, new FlyPurposeDAL().GetAllObject(), "PURPOSE_NAME", "PURPOSE_CODE");
            //this.FillDropdownList<Aero>(ddlTO_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");
            //this.FillDropdownList<Aero>(ddlFROM_AIRP, new AeroDAL().GetListAll(), "AE_NAME", "AE_ID");

            //ddlCRAFT_ID ddlPURPOSE_ID  ddlTO_AIRP  ddlFROM_AIRP  
            var s_Purpose = new FlyPurposeDAL().GetAllObject();
            var s_Aero = new AeroDAL().GetListAll();
            var s_Craft = new CraftTypeDAL().GetAllCraftType();
            foreach (var item in s_Purpose)
            {
                ListItem li = new ListItem();
                li.Value = item.PURPOSE_CODE;
                li.Text = item.PURPOSE_CODE + " - " + item.PURPOSE_NAME;
                ddlPURPOSE_ID.Items.Add(li);
            }
            foreach (var item in s_Aero)
            {
                ListItem li = new ListItem();
                li.Value = item.AE_CODE;
                li.Text = item.AE_CODE + " - " + item.AE_IATA;
                ddlFROM_AIRP.Items.Add(li);
                ddlTO_AIRP.Items.Add(li);
            }
            foreach (var item in s_Craft)
            {
                ListItem li = new ListItem();
                li.Value = item.CRAFT_ID.ToString();
                li.Text = item.MA + " - " + item.TAITRONG;
                ddlCRAFT_ID.Items.Add(li);
            }
            ddlPURPOSE_ID.Items.Insert(0, new ListItem("--", "0"));
            ddlFROM_AIRP.Items.Insert(0, new ListItem("--", "0"));
            ddlTO_AIRP.Items.Insert(0, new ListItem("--", "0"));
            ddlCRAFT_ID.Items.Insert(0, new ListItem("--", "0"));

            ddlPURPOSE_ID.DataBind();
            ddlFROM_AIRP.DataBind();
            ddlTO_AIRP.DataBind();
            ddlCRAFT_ID.DataBind();
        }

        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "LoadDataGrid":
                    kq = LoadDataGrid(_arg[0]);
                    break;
                case "GetOneFlight":
                    kq = GetOneFlight(_arg[0]);
                    break;
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
                case "btnCreateOnclick":
                    kq = btnCreateOnclick(_arg[0]);
                    break;
                case "mbtnAddNewFlightDetailOnclick":
                    kq = mbtnAddNewFlightDetailOnclick(_arg[0]);
                    break;
                case "mShowDetail":
                    kq = mShowDetail(_arg[0]);
                    break;
                case "mbtnUpdateFlightDetailOnclick":
                    kq = mbtnUpdateFlightDetailOnclick(_arg[0]);
                    break;
                case "btnDeleteOnclick":
                    kq = mDeleteRowOnclick(_arg[0]);
                    break;
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
                case "btnSearch_Click":
                    kq = btnSearch_Click(_arg[0]);
                    break;
                case "uploadComplete":
                    kq = uploadComplete(ThamSo);
                    break;
                case "GetListFilePerm":
                    kq = GetListFilePerm(_arg[0]);
                    break;
                case "deleteFile":
                    kq = deleteFile(ThamSo);
                    break;
            }
            return kq;
        }
        private string GetOneFlight(string ThamSo)
        {
            PermMasterNo obj = new PermMasterNoDAL().GetOneObject(ThamSo);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private PermDetailNo_Search GetObjectDetail_Search()
        {
            PermDetailNo_Search obj = new PermDetailNo_Search();
            obj.CRAFT_ID = Convert.ToInt32(ddlCRAFT_ID.SelectedValue);            
            obj.PERM_ID = Request.QueryString["ID"] == null ? 0 : Convert.ToInt32(Request.QueryString["ID"]);
            obj.DAYSFLIGHT = txtDAYSFLIGHT.Value;
            obj.FLIGHTNBR = txtFLIGHTNBR.Value;
            obj.REGISTRATION = txtREGISTRATION.Value;
            obj.FROM_AIRP = ddlFROM_AIRP.SelectedValue;
            obj.TO_AIRP = ddlTO_AIRP.SelectedValue;
            obj.ETD = txtETD.Value;
            obj.ETD = txtETD.Value;
            obj.VIA = txtVIA.Value;
            obj.PURPOSE_ID = ddlPURPOSE_ID.SelectedValue;
            obj.REMARK = txtREMARK.Value;
            //obj.REMARK_SEND = txtREMARK_SEND.Value;
            obj.PageSize = 10000000;
            return obj;
        }
        protected void GetInfoByFirstLoad()
        {
            string kq = GetOneFlight(Request.Params["ID"]==null?"0": Request.Params["ID"]);
            this.ExcuteJavascript($"ReadInfoPerm('{ kq }');");
        }
        private string btnUpdateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterNo>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserName.ToString();
            bool kq = new PermMasterNoDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterNoDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterNo>(ThamSo, dateTimeConverter);
            obj.ID = 0;
            var kq = new PermMasterNoDAL().InsertReturnId(obj);
            string ax = kq.ToString() != "-1" ? "Insert sussess " : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterNoDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            return kq;
        }
        protected string LoadDataGrid()
        {
            var ax = new PermDetailNoDAL().GetBySearch(GetObjectDetail_Search());
            rptSource.DataSource = ax;
            rptSource.DataBind();
            return this.RenderToHTML(rptSource);
        }
        private string LoadDataGrid(string thamso)
        {
            return btnSearch_Click(thamso);
        }
        private string btnSearch_Click(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailNo_Search>(thamso, dateTimeConverter);
            obj.PageSize = 10000000;
            rptSource.DataSource = new PermDetailNoDAL().GetBySearch(obj);
            rptSource.DataBind();
            return this.RenderToHTML(rptSource);
        }
        private string mbtnAddNewFlightDetailOnclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailNo>(ThamSo, dateTimeConverter);
                obj.ID = 0;
                var kq = new PermDetailNoDAL().InsertReturnId(obj);
                string ax = kq.ToString() == "-1" ? "Insert error" : "Insert sussess";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[InsertReturnId] [{ax}]", 0.0);
                return kq.ToString();
            }
            catch (Exception ex) { return "-1"; }
        }

        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var kq = btnUpdateDetailsOnclick(thamso);
            return kq == "Update sussess" ? "OK" : "NOK";
        }
        private string btnUpdateDetailsOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailNo>(ThamSo, dateTimeConverter);
            if (obj.ID == 0)
                return "Update error";            
            bool kq = new PermDetailNoDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }

        private string mShowDetail(string id)
        {
            PermDetailNo obj = new PermDetailNoDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string mDeleteRowOnclick(string thamso)
        {
            bool kq = new PermDetailNoDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new PermDetailNoDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailNoDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        #region upload file
        private string CreatePathFolder
        {
            get
            {
                string kq = Server.MapPath(@"~/FileUploadPermission/" + DateTime.Today.Year + @"/" + DateTime.Today.Month + "//" + "NO//");
                if (!Directory.Exists(kq))
                {
                    Directory.CreateDirectory(kq);
                }
                return kq;
            }
        }
        protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            this.ExcuteJavascript($"preloadImg('lblLinkFile', 'laaa', '15px','15px');");
            string fileName = $"{DateTime.Now.ToString("ddMMyyyy hhmmss")}_{_user.UserName}{Path.GetExtension(AsyncFileUpload1.FileName)}";
            string bx = AsyncFileUpload1.FileName;
            string pathFile = CreatePathFolder + fileName;
            AsyncFileUpload1.SaveAs(pathFile);
            Session.Add("fileUpNo", pathFile);
            Session.Add("fileurlNo", Global.ApplicationPath + @"/FileUploadPermission/" + DateTime.Today.Year + @"/" + DateTime.Today.Month + "//" + "NO//");
        }
        private string uploadComplete(string[] thamso)
        {
            var kq = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("PERM_PKG", "fileUploadPermission", new { P_PERMID = thamso[0], P_PATHFILE = Session["fileUpNo"].ToString(), P_PERMTYPE = "NO", P_USERCREATE = _user.UserName, P_URLPATH = Session["fileurlNo"].ToString(), P_FILENAME = Path.GetFileName(Session["fileUpNo"].ToString()) }).ToString());
            if (kq == -1) return "Upload error!";
            return GetListFilePerm(thamso[0]);
        }
        private string GetListFilePerm(string thamso)
        {
            DataTable dt = new clsResuftAPI().GetTableApiExtension("PERM_PKG", "fileUpload_Select", new { P_PERMID = thamso, P_PERMTYPE = "NO" });
            if (dt == null) return "";
            string ax = string.Empty;
            foreach (DataRow r in dt.Rows)
            {
                ax += $"<a target='_blank' href='{r["URLPATH"]}{Path.GetFileName(r["PATHFILE"].ToString())}'>{Path.GetFileName(r["PATHFILE"].ToString())}</a><i onclick='deleteFile({r["ID"]});' class='ace-icon fa fa-trash-o bigger-100'></i></br>";
            }
            return ax;
        }
        private string deleteFile(string[] thamso)
        {
            var ax = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("PERM_PKG", "deleteFileUpload", new { P_ID = thamso[0] }).ToString());
            if (ax == 1) return "Delete susses!";
            return "Delete error!";
        }
        #endregion

    }
}