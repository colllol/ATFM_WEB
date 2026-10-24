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
using System.Text.RegularExpressions;

namespace prjApplication.User
{
    public partial class ChangePass : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (_userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name) != null)
                _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            else Response.Redirect("~/Errors/AccessDenied.aspx");
        }
        protected bool CheckValidate()
        {
            bool _return = true;
            if (this.txtPassOld.Text.Length < 6)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu cũ lớn hơn hoặc bằng 6 ký tự!');", true);
                _return = false;
            }
            if (this.txtPass.Text.Length <= 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu không được rỗng!');", true);
                _return = false;
            }
            if (this.txtPass.Text.Length < 8)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu lớn hơn hoặc bằng 6 ký tự!');", true);
                _return = false;
            }

            Match password = Regex.Match(this.txtPass.Text, @"
                                      ^              # Match the start of the string
                                       (?=.*\p{Lu})  # Positive lookahead assertion, is true when there is an uppercase letter
                                       (?=.*\P{L})   # Positive lookahead assertion, is true when there is a non-letter
                                       \S{8,}        # At least 8 non whitespace characters
                                      $              # Match the end of the string
                                     ", RegexOptions.IgnorePatternWhitespace);

            if (password.Success == false)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu lớn hơn bằng 8 ký tự, ít nhất một chữ hoa, ít nhất một ký tự đặc biệt');", true);
                _return = false;
            }

            return _return;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Request["url"] != null)
                Response.Redirect(Request["url"].ToString());
        }
        protected void linkSave_Click(object sender, EventArgs e)
        {
            string pass = prjComponents.HPCSecurity.Encrypt(txtPass.Text.Trim());
            UltilFunc _until = new UltilFunc();
            UserDAL _userDAL = new UserDAL();
            if (CheckValidate())
            {
                if (Page.IsValid)
                {
                    string Menu_ID = "";
                    if (Request["Menu_ID"] != null) Menu_ID = Request["Menu_ID"].ToString();
                    if (this.txtPassOld.Text.Length >= 6 && this.txtPass.Text.Length >= 6 && this.txtPassConfirm.Text.Length >= 6)
                    {
                        // Check PassOld
                        string PassOld = prjComponents.HPCSecurity.Encrypt(txtPassOld.Text.Trim());
                        if (!PassOld.Equals(_user.UserPass))
                        {
                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Menu_ID, "[Đổi mật khẩu] [Nhập sai mật khẩu cũ tài khoản: " + _user.UserFullName + "]", 0);
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Mật khẩu cũ không đúng!');", true);
                            return;
                        }
                        else
                        {
                            _userDAL.CMS_ResetPass(_user.UserID, pass);
                            //_until.ExecSql("UPDATE T_Users SET UserPass = '" + pass + "' WHERE UserID =" + _user.UserID);
                            string strLog = "[Đổi mật khẩu]-->[Đổi mật khẩu tài khoản thành công:" + _user.UserFullName + " ]";

                            WriteLogHistory2Database.WriteHistory2Database(_user.UserID, _user.UserFullName, "[Cập nhật]", Menu_ID, strLog, 0);
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("UpdateSuccessfully") + "');", true);
                        }
                    }
                }
            }
        }
    }
}
