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
    public partial class EditPermSC : PageBaseCallBack
    {
        public int _ID
        {
            get
            {
                if (!string.IsNullOrEmpty("" + Request["ID"]))
                    return Convert.ToInt32(Request["ID"]);
                else return 0;
            }
        }
        public string NAMECHA { get { if (string.IsNullOrEmpty(_ID.ToString())) return ""; return new PermMasterScDAL().GetOneObject(_ID.ToString()).PERMNBR_ID; } }
        public string _ObjRender
        {
            get
            {
                PermDetailSc obj = new PermDetailSc();
                var ser = new JavaScriptSerializer();
                ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
                return ser.Serialize(obj);
            }

        }

        public string _phanCach = "::::";
        public string _IdSelect = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                GetInfoByFirstLoad();
                //LoadDataGrid();


            }
        }
        

        public override string GetCallbackResult()
        {
            if (_EventArgument == "") return "";
            string kq = "";
            string[] _arg = _EventArgument.Split(new string[] { "_____" }, StringSplitOptions.RemoveEmptyEntries);
            string[] ThamSo = _arg[0].Split(new string[] { "::::" }, StringSplitOptions.RemoveEmptyEntries);
            switch (_arg[1])
            {
                case "GetOneFlight":
                    kq = GetOneFlight(_arg[0]);
                    break;
                case "btnUpdateOnclick":
                    kq = btnUpdateOnclick(_arg[0]);
                    break;
                case "btnCreateOnclick":
                    kq = btnCreateOnclick(_arg[0]);
                    break;
                case "btnCreate_Details_Onclick":
                    kq = btnCreate_Details_Onclick(_arg[0]);
                    break;
                case "mShowDetail":
                    kq = mShowDetail(_arg[0]);
                    break;
                case "mbtnUpdateFlightDetailOnclick":
                    kq = mbtnUpdateFlightDetailOnclick(_arg[0]);
                    break;
                case "btnDeleteOnclick":
                    kq = btnDeleteOnclick(_arg[0]);
                    break;
                case "RestoreHistory":
                    kq = RestoreHistory(ThamSo);
                    break;
                case "btnSearch_Click":
                    kq = btnSearch_Click(_arg[0]);
                    break;
                case "LoadDataGrid":
                    kq = LoadDataGrid(_arg[0]);
                    break;
                case "LoadGrdSourceScroll":
                    kq = LoadGrdSourceScroll(_arg[0]);
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
            PermMasterSc obj = new PermMasterScDAL().GetOneObject(ThamSo);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string mShowDetail(string id)
        {
            return GetOneObject(id);
        }

        protected void GetInfoByFirstLoad()
        {
            if (Request.QueryString["ID"] == null)
                return;
            string kq = GetOneFlight(Request.Params["ID"]);
            this.ExcuteJavascript($"ReadInfoPerm('{kq}'); listfile();");

        }

        private PermDetailSc_Search GetObjectSearch()
        {
            PermDetailSc_Search obj = new PermDetailSc_Search();
            obj.BEGINDATE = txtBEGINDATE_SC.Value == "" ? DateTime.MinValue : UltilFunc.ToDate(txtBEGINDATE_SC.Value, "dd/MM/yyyy");
            obj.ENDDATE = txtENDDATE_SC.Value == "" ? DateTime.MinValue : UltilFunc.ToDate(txtENDDATE_SC.Value, "dd/MM/yyyy");
            //obj.CRAFT_ID = Convert.ToInt32(ddlCRAFT_ID.SelectedValue);
            obj.DAY1 = chkDay1.Checked ? "1" : "0";
            obj.DAY2 = chkDay2.Checked ? "2" : "0";
            obj.DAY3 = chkDay3.Checked ? "3" : "0";
            obj.DAY4 = chkDay4.Checked ? "4" : "0";
            obj.DAY5 = chkDay5.Checked ? "5" : "0";
            obj.DAY6 = chkDay6.Checked ? "6" : "0";
            obj.DAY7 = chkDay7.Checked ? "7" : "0";
            obj.ETA = txtETA.Value;
            obj.ETD = txtETD.Value;
            obj.FLIGHTNBR = txtFLIGHTNBR.Value;
            //obj.FROM_AIRP = ddlFROM_AIRP.SelectedValue;
            obj.LASTUSER = _user.UserName;
            //obj.PURPOSE_ID = ddlPURPOSE_ID.SelectedValue;
            obj.REGISTRATION = txtREGISTRATION.Value;
            //obj.TO_AIRP = ddlTO_AIRP.SelectedValue;
            obj.VIA = txtVIA.Value;
            obj.REMARK = txtREMARK.Value;
            //NVTHAI 27.02.2020
            //obj.PageSize = 50000000;
            obj.PageSize = 100;
            obj.LASTUSER = _user.UserName;
            obj.PERM_ID = Request.QueryString["ID"] != null ? Convert.ToInt64(Request.QueryString["ID"].ToString()) : 0;
            return obj;
        }
        protected void LoadDataGrid()
        {
            var s = new PermDetailScDAL().GetBySearch(GetObjectSearch());
            rptSource.DataSource = s;
            rptSource.DataBind();
        }
        protected string LoadDataGrid(string thamso)
        {
            return btnSearch_Click(thamso);
        }
        private string LoadGrdSourceScroll(string thamso)
        {
            return btnSearch_Click(thamso);
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        #region perm maseter function
        private string btnUpdateOnclick(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterSc>(thamso, dateTimeConverter);
            obj.LASTUSER = _user.UserName;
            bool kq = new PermMasterScDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnCreateOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermMasterSc>(ThamSo, dateTimeConverter);
            obj.LASTUSER = _user.UserName.ToString();
            obj.ID = 0;
            var kq = new PermMasterScDAL().InsertReturnId(obj);
            string ax = kq.ToString() != "-1" ? "Insert sussess" : "Insert error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermMasterScDAL]", 0, $"[InsertObject] [{ax}]", 0.0);
            if (kq == "-1")
                return $"<script>Alert('Insert error');</script>";
            return $"{kq.ToString()}";
        }
        #endregion

        #region perm detail function
        private string btnCreate_Details_Onclick(string ThamSo)
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailSc>(ThamSo, dateTimeConverter);
                if (obj.PERM_ID == 0)
                    return "-1";
                var kq = new PermDetailScDAL().InsertReturnId(obj);
                string ax = kq.ToString() == "-1" ? "Insert error" : "Insert success";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[CreateObject] [{ax}]", 0.0);
                if (kq == "-1")
                    return "";
                return kq.ToString();
            }
            catch (Exception ex) { return "-1"; }
        }
        private string btnUpdateDetailsOnclick(string ThamSo)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailSc>(ThamSo, dateTimeConverter);
            bool kq = new PermDetailScDAL().UpdateObject(obj);
            string ax = kq.ToString() == true.ToString() ? "Update sussess" : "Update error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[UpdateObject] [{ax}]", 0.0);
            if (!kq)
                return "Update error";
            return "Update sussess";
        }
        private string btnDeleteOnclick(string thamso)
        {
            bool kq = new PermDetailScDAL().DeleteObject(thamso);
            string ax = kq.ToString() == true.ToString() ? "Delete sussess" : "Delete error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[DeleteObject] [{ax}]", 0.0);
            if (!kq)
                return "Delete error";
            return "Delete sussess";
        }
        private string GetOneObject(string id)
        {
            PermDetailSc obj = new PermDetailScDAL().GetOneObject(id);
            var ser = new JavaScriptSerializer();
            ser.RegisterConverters(new JavaScriptConverter[] { new DateTimeConverter() });
            return ser.Serialize(obj);
        }
        private string btnSearch_Click(string thamso)
        {
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<PermDetailSc_Search>(thamso, dateTimeConverter);
            //obj.PageSize = 500;
            obj.PageSize = 100;
            DataTable dt = new PermDetailScDAL().GetBySearch(obj);
            if (dt == null)
                return "";
            rptSource.DataSource = dt;
            rptSource.DataBind();
            return this.RenderToHTML(rptSource) + $"<script>$('#lblTotalRecord').text('Total: {dt.Rows[0]["RECORD_SUM"]}')</script>";
        }
        private string mbtnUpdateFlightDetailOnclick(string thamso)
        {
            var kq = btnUpdateDetailsOnclick(thamso);
            return kq == "Update sussess" ? "OK" : "NOK";
        }
        #endregion

        #region history function
        private string RestoreHistory(string[] thamso)
        {
            bool kq = new PermDetailScDAL().RestoreRecode(thamso[0], thamso[1], thamso[2]);
            string ax = kq.ToString() == true.ToString() ? "Restore sussess" : "Restore error";
            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[PermDetailScDAL]", 0, $"[RestoreRecode] [{ax}]", 0.0);
            return kq ? "OK" : "NOK";
        }
        #endregion

        #region btn event
        protected void btnSearch_Onclick(object sender, EventArgs e)
        {
            LoadDataGrid();
        }
        /// <summary>
        /// Create Perm Detail by Perm_id = parms["ID"]
        /// </summary>        
        protected void likCreate_Click(object sender, EventArgs e)
        {
            PermDetailSc obj = GetObjectSearch();

        }

        protected void likSave_Click(object sender, EventArgs e)
        {

        }

        protected void lnkCancel_Click(object sender, EventArgs e)
        {

        }

        protected void lbkClear_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region upload file
        private string CreatePathFolder
        {
            get
            {
                string kq = Server.MapPath(@"~/FileUploadPermission/" + DateTime.Today.Year + @"/" + DateTime.Today.Month + "//" + "SC//");
                if (!Directory.Exists(kq))
                {
                    Directory.CreateDirectory(kq);
                }
                return kq;
            }
        }
       
        private string uploadComplete(string[] thamso)
        {
            var kq = Convert.ToInt64(new clsResuftAPI().GetValueApiExtension("PERM_PKG", "fileUploadPermission", new { P_PERMID = thamso[0], P_PATHFILE = Session["fileUp"].ToString(), P_PERMTYPE = "SC", P_USERCREATE = _user.UserName, P_URLPATH = Session["fileurl"].ToString(), P_FILENAME = Path.GetFileName(Session["fileUp"].ToString()) }).ToString());
            if (kq == -1) return "Upload error!";
            return GetListFilePerm(thamso[0]);
        }
        private string GetListFilePerm(string thamso)
        {
            DataTable dt = new clsResuftAPI().GetTableApiExtension("PERM_PKG", "fileUpload_Select", new { P_PERMID = thamso, P_PERMTYPE = "SC" });
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