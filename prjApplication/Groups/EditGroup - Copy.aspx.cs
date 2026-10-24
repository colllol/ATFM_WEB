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
namespace prjApplication.Groups
{
    public partial class EditGroup : System.Web.UI.Page
    {
        prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        prjInfo.T_Users _user = null;
        protected prjInfo.T_RolePermission _Role = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    _Role = _userDAL.GetRole4UserMenu(_user.UserID, Convert.ToInt32(Request["Menu_ID"]));
                    this.linkSave.Visible = _Role.R_Edit;
                    if (!IsPostBack)
                        DataBind();
                }
            }
        }
        protected string IpAddress()
        {
            string strIp;
            strIp = Page.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIp == null)
            {
                strIp = Page.Request.ServerVariables["REMOTE_ADDR"];
            }
            return strIp;
        }
        protected void linkSave_Click(object sender, EventArgs e)
        {
            #region GhiLog
            UserDAL _usermenuDAL = new UserDAL();
            ActionHistoryDAL actionDAL = new ActionHistoryDAL();
            T_ActionHistory action = new T_ActionHistory();
            action.UserID = _user.UserID;
            action.FullName = _user.UserName;
            action.HostIP = IpAddress();
            action.DateModify = DateTime.Now;
            #endregion           
            if (!Page.IsValid) return;
            GroupDAL _nhomnguoidungDAL = new GroupDAL();
            T_Groups _nhomnguoidung = SetItem();
            T_Groups _isnhomnguoidung;
            int groupID = 0;
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
                groupID = int.Parse(Request["ID"].ToString());
            
            _isnhomnguoidung = _nhomnguoidungDAL.GetGroupName(_nhomnguoidung.Group_Name,groupID);
            if (_isnhomnguoidung != null)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("VALIDATE_NHOMNGUOIDUNG_EXIT") + "');", true);
                return;
            }
            string _return = _nhomnguoidungDAL.Insert_T_Group(_nhomnguoidung);
            if (Page.Request.Params["id"] == null)
            {
                action.ActionsCode = "[Thêm mới Group]-->[Thao tác Thêm][ID:" + _return.ToString() + " ]";
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("VALIDATE_NHOMNGUOIDUNG_AddNews") + "');", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('" + Global.RM.GetString("UpdateSuccessfully") + "');", true);
                action.ActionsCode = "[Sửa Group]-->[Thao tác Sửa][ID:" + Page.Request.Params["id"].ToString() + " ]";
            }
            actionDAL.InserT_ActionOrc(action);
        }
        public void Clear()
        {
            this.txtName.Text = "";
            this.txtDesc.Text = "";
        }
        private T_Groups SetItem()
        {
            T_Groups _obj = new T_Groups();
            if (Page.Request.Params["id"] != null)
            {
                _obj.Group_ID = int.Parse(Page.Request["id"].ToString());
            }
            else _obj.Group_ID = 0;
            _obj.Group_Name = this.txtName.Text.Trim();
            _obj.Group_Description = this.txtDesc.Text.Trim();
            _obj.DateCreated = DateTime.Now;
            _obj.DateModify = DateTime.Now;
            return _obj;
        }
        private void PopulateItem(int _id)
        {
            T_Groups _objSet = new T_Groups();
            GroupDAL _groupDAL = new GroupDAL();
            _objSet = _groupDAL.GetOneFromT_GroupsByID(_id);
            this.txtName.Text = _objSet.Group_Name;
            this.txtDesc.Text = _objSet.Group_Description;
        }
        public override void DataBind()
        {
            if (Request["ID"] != null && Request["ID"].ToString() != "" && Request["ID"].ToString() != String.Empty)
            {
                //this.pnlEditGroup.GroupingText = Global.RM.GetString("PANELEDITGROUPNAME");
                if (CommonLib.IsNumeric(Request["ID"]) == true)
                {
                    int _id = Convert.ToInt32(Request["ID"].ToString());
                    PopulateItem(_id);
                }
            }
            //else
            //{
            //    this.pnlEditGroup.GroupingText = Global.RM.GetString("PANELTHEMMOIGROUPNAME");
            //}
        }
        protected void LinkCancel_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("~/Groups/ListGroups.aspx?Menu_ID=" + Request["Menu_ID"].ToString());
        }
    }
}
