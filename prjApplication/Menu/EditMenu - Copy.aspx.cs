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
using prjBusinessLogic;
using prjInfo;
using prjComponents;
namespace prjApplication.Menu
{
    public partial class EditMenu : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        public string urlimages = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                if (UltilFunc.IsNumeric(Request["Menu_ID"]))
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    if (!IsPostBack)
                    {
                        LoadCombo();
                        DataBind();
                    }
                    LoadImage();

                }
            }
        }
        public void LoadImage()
        {           
            if (txtThumbnail.Text != "")
                this.ImgViews.Src = prjComponents.Global.TinPath_CMS() + txtThumbnail.Text;
            else this.ImgViews.Attributes.CssStyle.Add("display", "none");
        }
        protected void LinkCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/Menu/ListMenu.aspx?Menu_ID=" + Request["Menu_ID"].ToString());
        }
        protected void linkSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                MenuDAL _menuDAL = new MenuDAL();
                T_Menus _menu = SetItem();
                int menuID = 0;
                if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
                    menuID = int.Parse(Request["ID"].ToString());
                string _return = _menuDAL.Insert_T_MenusOrc(_menu);
                if (Page.Request.Params["id"] == null)
                {
                    WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "Thêm mới", Request["Menu_ID"].ToString(), "[Chức năng hệ thống] [Thao tác thêm: " + _menuDAL.GetOneFromT_MenusByID(Convert.ToInt32(_return)).MenuName + "]", 0);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("VALIDATE_ADDNEWS") + "');", true);
                }
                if (Page.Request.Params["id"] != null)
                {
                    WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"].ToString(), "[Chức năng hệ thống] [Thao tác sửa: " + _menuDAL.GetOneFromT_MenusByID(Convert.ToInt32(Request["id"])).MenuName + "]", 0);
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("UpdateSuccessfully") + "');", true);
                }
            }
        }
        private void Clear()
        {
            this.ddlParrentID.SelectedIndex = 0;
            this.txtMenuName.Text = "";
        }
        private void LoadCombo()
        {
            ddlParrentID.Items.Clear();
            UltilFunc.BindCombox(this.ddlParrentID, "ID", "MenuName", "T_Menus", " 1=1 ", "---", "ParrentID");
        }
        public override void DataBind()
        {
            if (Request["ID"] != null && Request["ID"].ToString() != "" &&
                Request["ID"].ToString() != String.Empty)
            {                
                if (CommonLib.IsNumeric(Request["ID"]) == true)
                {
                    int _id = Convert.ToInt32(Request["ID"].ToString());
                    PopulateItem(_id);
                }
            }
            else
                this.ImgViews.Attributes.CssStyle.Add("display", "none");
        }
        private void PopulateItem(int _id)
        {
            T_Menus _objSet = new T_Menus();
            MenuDAL _menuDAL = new MenuDAL();
            _objSet = _menuDAL.GetOneFromT_MenusByID(_id);
            ddlParrentID.SelectedIndex = UltilFunc.GetIndexControl(ddlParrentID, _objSet.ParrentID.ToString());
            this.txtMenuName.Text = _objSet.MenuName;
            this.txtMenuURL.Text = _objSet.MenuURL;
            this.txtThumbnail.Text = _objSet.MenuIcon;
            if (_objSet.MenuIcon != "")
                this.ImgViews.Src = prjComponents.Global.TinPath+ prjComponents.Global.UploadPath + _objSet.MenuIcon;
            else this.ImgViews.Attributes.CssStyle.Add("display", "none");
            this.txtMenuDesc.Text = _objSet.MenuDesc;
            this.txtMenuOrder.Text = _objSet.MenuOrder.ToString();
            if (_objSet.isDisplay==1)
                this.chkIsDisplay.Checked = true;
            else
                this.chkIsDisplay.Checked = false;
            if (_objSet.ActiveSync == 1)
                this.chkActiveSync.Checked = true;
            else
                this.chkActiveSync.Checked = false;

            if (_objSet.ActiveSyncImages == 1)
                this.chkActiveSyncImage.Checked = true;
            else
                this.chkActiveSyncImage.Checked = false;

            //this.chkActiveSync.Checked = _objSet.ActiveSync;
            // this.chkActiveSyncImage.Checked = _objSet.ActiveSyncImages;
        }
        private T_Menus SetItem()
        {
            T_Menus _obj = new T_Menus();
            if (Page.Request.Params["id"] != null)
                _obj.ID = int.Parse(Page.Request["id"].ToString());
            else _obj.ID = 0;
            _obj.MenuName = UltilFunc.SqlFormatText(this.txtMenuName.Text.Trim());
            _obj.MenuURL = this.txtMenuURL.Text.Trim();
            _obj.MenuDesc = this.txtMenuDesc.Text.Trim();
            _obj.MenuIcon = this.txtThumbnail.Text.Trim();
            if (txtMenuOrder.Text.Length > 0)
                if (UltilFunc.IsNumeric(this.txtMenuOrder.Text.Trim()))
                    _obj.MenuOrder = Convert.ToInt32(this.txtMenuOrder.Text.Trim());
            _obj.DateCreated = DateTime.Now;
            _obj.DateModify = DateTime.Now;
            if (this.chkIsDisplay.Checked)
                _obj.isDisplay = 1;
            else
                _obj.isDisplay = 0;

            if (this.chkIsDisplay.Checked)
                _obj.isDisplay = 1;
            else
                _obj.isDisplay = 0;

            if (this.chkActiveSyncImage.Checked)
                _obj.ActiveSyncImages = 1;
            else
                _obj.ActiveSyncImages = 0;

            // _obj.isDisplay = this.chkIsDisplay.Checked;
            // _obj.ActiveSync = this.chkActiveSync.Checked;
            //_obj.ActiveSyncImages = this.chkActiveSyncImage.Checked;
            _obj.UserCreate = _user.UserID;
            _obj.UserModify = _user.UserID;
            _obj.ParrentID = Convert.ToInt32(this.ddlParrentID.SelectedValue.ToString());
            return _obj;
        }
    }
}
