using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjBusinessLogic;
using prjInfo;
using prjComponents;

namespace prjApplication
{
    public class PageCoreAdmin:System.Web.UI.Page
    {  
        protected override void OnLoad(EventArgs e)
        {
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {
                    if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                        Response.Redirect("~/Errors/AccessDenied.aspx");
                    _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
                    _Role = _userDAL.GetRole4UserMenu(_user.UserID, Convert.ToInt32(Request["Menu_ID"]));                    
                }
            }
            base.OnLoad(e);            
        }

        #region instan
        protected prjInfo.T_RolePermission _Role = null;
        private bool _refreshState;
        private bool _isRefresh;
        private int UserID
        {
            get { if (ViewState["UserID"] != null) return Convert.ToInt32(ViewState["UserID"]); else return 0; }

            set { ViewState["UserID"] = value; }
        }
        //protected override void LoadViewState(object savedState)
        //{
        //    try
        //    {
        //        object[] AllStates = (object[])savedState;
        //        base.LoadViewState(AllStates[0]);
        //        _refreshState = bool.Parse(AllStates[1].ToString());
        //        _isRefresh = _refreshState ==
        //        bool.Parse(Session["__ISREFRESH"].ToString());
        //    }
        //    catch
        //    { }
        //}
        //protected override object SaveViewState()
        //{
        //    Session["__ISREFRESH"] = _refreshState;
        //    object[] AllStates = new object[2];
        //    AllStates[0] = base.SaveViewState();
        //    AllStates[1] = !(_refreshState);
        //    return AllStates;
        //}
        protected prjBusinessLogic.UserDAL _userDAL = new UserDAL();
        protected prjInfo.T_Users _user = null;
        #endregion
    }
}