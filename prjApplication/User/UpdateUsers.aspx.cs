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
using prjInfo;
using prjBusinessLogic;
using prjComponents;

namespace prjApplication.User
{
    public partial class UpdateUsers : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {           
           
            _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (!IsPostBack)
            {
                DataBind();
            }
               
        }


        public T_Users SetItem()
        {
            T_Users _obj = new T_Users();
            if (Page.Request.Params["id"] != null)
                _obj.UserID = int.Parse(Page.Request["id"].ToString());
            else _obj.UserID = 0;
            _obj.UserName = UltilFunc.SqlFormatText(this.txtUserName.Text.Trim());
            _obj.UserPass = HPCSecurity.Encrypt(txtPass.Text.Trim());
            _obj.UserEmail = this.txtEmail.Text.Trim();
            if (this.ckActivec.Checked)
                _obj.UserActive = 1;
            else
                _obj.UserActive = 0;
            if (this.txtBirth.Value.Length > 0)
            {
                if (this.txtBirth.Value.ToString() != "__/__/____")
                    _obj.UserBirthday = UltilFunc.ToDate(this.txtBirth.Value, "dd/MM/yyyy");
            }
            _obj.UserAddress = this.txtAddress.Text.Trim();
            _obj.UserFullName = this.txtFullName.Text.Trim();
            _obj.UserMobile = this.txtPhoneNumber.Text.Trim();
            _obj.DateCreated = DateTime.Now;
            _obj.DateModify = DateTime.Now;
            _obj.UserCreate = _user.UserID;
            //_obj.Group_ID = 0;// COMMENT 10/08/2010 
            _obj.Group_ID = 0;// Convert.ToInt32(ddlFontDisplay.SelectedValue);// ADD BY BOCT
            _obj.IsReporter = 0;// Convert.ToInt32(this.ddlChkType.SelectedValue);
            _obj.RedirectPages = "";//this.txtRedirectPages.Text.Trim();
            return _obj;
        }
        private void PopulateItem(int _id)
        {
            T_Users obj = new T_Users();
            UserDAL objControl = new UserDAL();
            obj = objControl.GetOneFromT_UsersByID(_id);
            this.txtUserName.Text = obj.UserName;
            txtUserName.ReadOnly = true;
            this.txtEmail.Text = obj.UserEmail;
            this.txtFullName.Text = obj.UserFullName;
            this.txtPhoneNumber.Text = obj.UserMobile;
            this.txtAddress.Text = obj.UserAddress;
            if (obj.UserActive == 1)

                this.ckActivec.Checked = true;

            else
                this.ckActivec.Checked = false;

            this.txtAddress.Text = obj.UserAddress;
            if (obj.UserBirthday != null && obj.UserBirthday != DateTime.MaxValue && obj.UserBirthday != DateTime.MinValue)
                this.txtBirth.Value = obj.UserBirthday.ToString("dd/MM/yyyy");
            //ddlChkType.SelectedIndex = UltilFunc.GetIndexControl(ddlChkType, obj.IsReporter.ToString());
            //this.ddlChkType.SelectedValue= obj.IsReporter.ToString();
            //this.txtRedirectPages.Text = obj.RedirectPages;
            //ddlChkType.Items.Clear();
            //UltilFunc.BindCombox(ddlChkType, "Group_ID", "Group_Name", "T_Groups", " 1=1  Order by Group_Name", "");
            //ddlChkType.SelectedIndex = CommonLib.GetIndexControl(ddlChkType, obj.IsReporter.ToString());

            //ddlFontDisplay.SelectedIndex = CommonLib.GetIndexControl(ddlFontDisplay, obj.Group_ID.ToString());

        }
        protected bool CheckValidate()
        {
            bool _return = true;
            if (this.txtUserName.Text.Length <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Tên truy cập không được rỗng!');", true);
                _return = false;
            }
            if (this.txtUserName.Text.Length < 5)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Tên truy cập lớn hơn hoặc bằng 6 ký tự!');", true);
                _return = false;
            }
            if (pnlPass.Visible)
            {
                if (this.txtPass.Text.Length <= 0)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu không được rỗng!');", true);
                    _return = false;
                }
                if (this.txtPass.Text.Length < 6)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu lớn hơn 6 ký tự!');", true);
                    _return = false;
                }
            }
            if (this.txtFullName.Text.Length <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Họ & tên không được rỗng!');", true);
                _return = false;
            }
            if (this.txtFullName.Text.Length < 6)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Họ & tên lớn hơn 6 ký tự!');", true);
                _return = false;
            }
            return _return;
        }
        protected void linkSave_Click(object sender, EventArgs e)
        {
            T_Users _obj = SetItem();
            T_Users _Isobj;
            int UserID = 0;
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
                UserID = int.Parse(Request["ID"].ToString());
            _Isobj = _userDAL.GetUSERNAME(_obj.UserName, UserID);
            if (_Isobj != null)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("VALIDATE_USERNAMEExits") + "');", true);
                return;
            }
            if (CheckValidate())
            {
                if (Page.IsValid)
                {
                    int _return = _userDAL.InsertT_Users(_obj);
                    string strLog = "";
                    if (Request["ID"] == null)
                    {
                        strLog = "[Thêm mới USER]-->[Thao tác Thêm][User:" + _userDAL.GetOneFromT_UsersByID(_return).UserFullName + " ]";
                        WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], strLog, 0);
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("VALIDATE_ADDNEWS") + "');", true);
                        Page.Response.Redirect("~/User/ListUser.aspx?Menu_ID=" + this.Page.Request["Menu_ID"].ToString());
                    }
                    else
                    {
                        strLog = "[Sửa USER]-->[Thao tác sửa][User:" + _userDAL.GetOneFromT_UsersByID(_return).UserFullName + " ]";
                        WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], strLog, 0);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("UpdateSuccessfully") + "');", true);
                    }
                }
            }
        }
        protected void LinkCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/Common/Reports.aspx");
        }
        public override void DataBind()
        {
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["ID"]) == true)
                {
                    pnlPass.Visible = false;
                    RequiredFieldValidator2.Visible = false;
                    CompareValidator1.Visible = false;
                    int _id = Convert.ToInt32(Request["ID"].ToString());
                    PopulateItem(_id);
                }
            }
            else
            {
                pnlPass.Visible = true;
                RequiredFieldValidator2.Visible = true;
                CompareValidator1.Visible = true;
            }
        }
    }
}