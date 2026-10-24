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
    public partial class UserChangeInfo : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name) != null)
            {
                _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                if (!IsPostBack)
                    DataBind();
            }
            else Response.Redirect("~/Errors/AccessDenied.aspx");
        }
        private Boolean CheckForm()
        {
            bool _return = true;
            if (this.txtFullName.Text.Length <= 6)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Họ và tên lớn hơn 6 ký tự!');", true);
                _return = false;
            }
            else if (this.txtEmail.Text.Length > 0)
            {
                if (!UltilFunc.IsEmailError(this.txtEmail.Text.Trim()))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Email sai định dạng');", true);
                    _return = false;
                }
            }
            else _return = true;
            return _return;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Request["url"] != null)
                Response.Redirect(Request["url"].ToString());
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            CheckForm();
            T_Users _obj = SetItem();
            if (Page.IsValid)
            {
                _userDAL.UpdateT_UsersInfo(_obj);
                string strLog = "Tài khoản cập nhật thông tin: " + _user.UserFullName + "";
                WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Request["Menu_ID"], strLog, 0);
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Cập nhật thông tin thành công!');", true);
            }
        }
        public T_Users SetItem()
        {
            T_Users _obj = new T_Users();
            _obj.UserID = _user.UserID;
            _obj.UserName = "";
            _obj.UserPass = "";
            _obj.UserEmail = this.txtEmail.Text.Trim();
            _obj.UserActive = 1;
            try
            {
                if (this.txtBirth.Text.Length > 0)// Ngay Sinh
                    _obj.UserBirthday = UltilFunc.ToDate(this.txtBirth.Text, "dd/MM/yyyy");
            }
            catch { System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Sai kiểu ngày tháng');", true); }
            _obj.UserAddress = this.txtAddress.Text.Trim();
            _obj.UserFullName = this.txtFullName.Text.Trim();
            _obj.UserMobile = this.txtPhoneNumber.Text.Trim();
            _obj.DateCreated = DateTime.Now;
            _obj.DateModify = DateTime.Now;
            _obj.UserCreate = _user.UserID;
            _obj.Group_ID = 0;
            _obj.IsReporter = 0;
            _obj.RedirectPages = "";
            return _obj;
        }
        private void PopulateItem()
        {
            this.txtEmail.Text = _user.UserEmail;
            this.txtFullName.Text = _user.UserFullName;
            this.txtPhoneNumber.Text = _user.UserMobile;
            this.txtAddress.Text = _user.UserAddress;
            if (_user.UserBirthday != null && _user.UserBirthday != DateTime.MaxValue && _user.UserBirthday != DateTime.MinValue)
                this.txtBirth.Text = _user.UserBirthday.ToString("dd/MM/yyyy");
        }
        public override void DataBind()
        {
            PopulateItem();
        }
    }
}
