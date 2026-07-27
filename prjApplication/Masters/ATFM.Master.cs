using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using prjInfo;
using prjBusinessLogic;
using prjComponents;
using System.Data.SqlClient;
using HPCServerDataAccess;
using System.Diagnostics;


namespace prjApplication.Masters
{
    public partial class ATFM : System.Web.UI.MasterPage
    {
        private const string CurrentUserSessionKey = "ATFM_CURRENT_USER";
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Stopwatch masterTimer = Stopwatch.StartNew();
                //BEGIN MENUNAME
                if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
                {
                    if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                    {
                        if (!HPCSecurity.IsAccept(Convert.ToInt32(Request["Menu_ID"])))
                            Response.Redirect("~/Errors/AccessDenied.aspx");
                        //this.litImageIcon.Text = "<img src=\" ../Images/Settings.png \">";
                        this.litTitleMenuName.Text = GetMenuName(Convert.ToInt32(Page.Request["Menu_ID"].ToString()));
                    }
                    
                }
                else
                {
                    //this.litImageIcon.Text = "<img src=\" ../Images/Settings.png \">";
                    this.litTitleMenuName.Text = "";
                }
                //END
                //T_RolePermission _role;
                string _name = HPCSecurity.CurrentUser.Identity.Name;
                UserDAL _userDAL = new UserDAL();
                Stopwatch userTimer = Stopwatch.StartNew();
                T_Users user = Session[CurrentUserSessionKey] as T_Users;
                bool userFromSession = user != null
                    && string.Equals(user.UserName, _name, StringComparison.OrdinalIgnoreCase);
                if (!userFromSession)
                {
                    user = _userDAL.GetUserByUserName(_name);
                    if (user != null) Session[CurrentUserSessionKey] = user;
                }
                userTimer.Stop();
                TracePerformance(userFromSession ? "UserFromSession" : "UserFromApi", userTimer.ElapsedMilliseconds);
                if (user != null)
                {
                    Stopwatch menuTimer = Stopwatch.StartNew();
                    
                    litMenu.Text = BindNavigation(user.UserID);

                    menuTimer.Stop();
                    TracePerformance("BindNavigation", menuTimer.ElapsedMilliseconds);
                    literFullName.Text = user.UserFullName;
                    literChangPass.Text = "<a href=\"" + Global.ApplicationPath + "/User/ChangePass.aspx?url=" + Request.Url.AbsoluteUri + "\"><i class=\"ace-icon fa fa-cog\"></i>" + Global.RM.GetString("CHANGE_PASS") + "</a>";
                    literEditProfile.Text = "<a href=\"" + Global.ApplicationPath + "/User/UpdateUsers.aspx?ID=" + user.UserID + "\"><i class=\"ace-icon fa fa-user\"></i>Profile</a>";                    
                    lb_Exit.Visible = true;
                    
                }
                else { lb_Exit.Visible = false; }

                masterTimer.Stop();
                TracePerformance("MasterTotal", masterTimer.ElapsedMilliseconds);
                Response.AppendHeader("Server-Timing", "master;dur=" + masterTimer.ElapsedMilliseconds);
            }
        }

        private static void TracePerformance(string phase, long elapsedMilliseconds)
        {
            System.Diagnostics.Trace.TraceInformation(
                "[ATFM.Performance] {0}: {1} ms", phase, elapsedMilliseconds);
        }


        #region Menu Bind Data
        protected string GetMenu4User(int UserID)
        {
            prjBusinessLogic.UltilFunc _untilDAL = new prjBusinessLogic.UltilFunc();
            UserDAL _objDAL = new UserDAL();
            string _sql = "[CMS_GetMenu4User]";
            DataTable _dt;
            string _tmp = string.Empty;
            try
            {
                //_dt = _untilDAL.GetStoreDataSet(_sql, new string[] { "@User_ID" }, new object[] { UserID }).Tables[0];
                _dt = _objDAL.GetMenu4User(UserID);
                if (_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (_tmp.Trim() == "")
                            _tmp = _dt.Rows[i]["ID"].ToString();
                        else
                            _tmp = _tmp + "," + _dt.Rows[i]["ID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _tmp;
        }
        private string isParent()
        {
            int Menu_ID = CommonLib.CheckNullInt(Request["Menu_ID"]);
            if (Menu_ID > 0)
            {
                prjBusinessLogic.UltilFunc ultilDAL = new UltilFunc();
                if (CommonLib.isParrentMenu(Menu_ID))
                    return Menu_ID.ToString();
                else
                    return ultilDAL.GetColumnValues("T_Menus", "ParrentID", " ID=" + Menu_ID.ToString());
            }
            else return "0";
        }
        [Serializable]
        private sealed class NavigationMenuItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public int NodeCount { get; set; }
            public List<NavigationMenuItem> Children { get; set; }
        }

        private string GetNavigationCacheKey(int userID)
        {
            return "ATFM_NAVIGATION_TREE_" + userID;
        }

        private List<NavigationMenuItem> LoadNavigationTree(int userID)
        {
            string allowedMenuIDs = GetMenu4User(userID);
            UserDAL userDAL = new UserDAL();
            DataTable parentTable = userDAL.BindNavigationByUserID(userID);
            DataTable childTable = null;
            bool canGroupChildren = false;
            List<NavigationMenuItem> menuTree = new List<NavigationMenuItem>();

            // Lay tat ca menu con duoc cap quyen trong mot lan goi API, sau do nhom
            // trong bo nho. Cach nay tranh mot lan goi CMS_BindNavigation cho moi
            // menu cha (N+1 requests) khi session chua co navigation cache.
            if (!string.IsNullOrWhiteSpace(allowedMenuIDs))
            {
                string childFilter = " and ParrentID > 0 AND ID IN (" + allowedMenuIDs
                    + ") ORDER BY mn.MenuOrder";
                childTable = userDAL.BindNavigationMaster(childFilter);
                canGroupChildren = childTable != null && childTable.Columns.Contains("ParrentID");
            }

            foreach (DataRow parentRow in parentTable.Rows)
            {
                NavigationMenuItem parent = new NavigationMenuItem
                {
                    ID = CommonLib.CheckNullInt(parentRow["ID"]),
                    Name = CommonLib.CheckNullStr(parentRow["MenuName"]),
                    Icon = CommonLib.CheckNullStr(parentRow["MenuIcon"]),
                    NodeCount = CommonLib.CheckNullInt(parentRow["NODE"]),
                    Children = new List<NavigationMenuItem>()
                };

                if (parent.NodeCount > 0)
                {
                    DataRow[] childRows;
                    if (canGroupChildren)
                    {
                        childRows = childTable.Select("ParrentID = " + parent.ID);
                    }
                    else if (!string.IsNullOrWhiteSpace(allowedMenuIDs))
                    {
                        // Mot so phien ban API CMS_BindNavigation khong tra ve cot
                        // ParrentID. Trong truong hop do, quay ve loc theo tung menu cha
                        // de dam bao dung du lieu va khong phat sinh EvaluateException.
                        string parentFilter = " and ParrentID = " + parent.ID
                            + " AND ID IN (" + allowedMenuIDs + ") ORDER BY mn.MenuOrder";
                        DataTable parentChildTable = userDAL.BindNavigationMaster(parentFilter);
                        childRows = parentChildTable == null
                            ? new DataRow[0]
                            : parentChildTable.Select();
                    }
                    else
                    {
                        childRows = new DataRow[0];
                    }

                    foreach (DataRow childRow in childRows)
                    {
                        parent.Children.Add(new NavigationMenuItem
                        {
                            ID = CommonLib.CheckNullInt(childRow["ID"]),
                            Name = CommonLib.CheckNullStr(childRow["MenuName"]),
                            Icon = CommonLib.CheckNullStr(childRow["MenuIcon"]),
                            Url = CommonLib.CheckNullStr(childRow["MenuURL"]),
                            Children = new List<NavigationMenuItem>()
                        });
                    }
                }

                menuTree.Add(parent);
            }

            return menuTree;
        }

        private List<NavigationMenuItem> GetNavigationTree(int userID)
        {
            string cacheKey = GetNavigationCacheKey(userID);
            List<NavigationMenuItem> menuTree = Session[cacheKey] as List<NavigationMenuItem>;

            if (menuTree == null)
            {
                Stopwatch loadTimer = Stopwatch.StartNew();
                menuTree = LoadNavigationTree(userID);
                Session[cacheKey] = menuTree;
                loadTimer.Stop();
                TracePerformance("NavigationCacheMiss", loadTimer.ElapsedMilliseconds);
            }
            else
            {
                TracePerformance("NavigationCacheHit", 0);
            }

            return menuTree;
        }

        public string BindNavigation(int UserID)
        {
            List<NavigationMenuItem> menuTree = GetNavigationTree(UserID);
            StringBuilder html = new StringBuilder();
            int requestedMenuID = CommonLib.CheckNullInt(Request["Menu_ID"]);
            int activeParentID = 0;

            if (requestedMenuID > 0)
            {
                foreach (NavigationMenuItem parent in menuTree)
                {
                    if (parent.ID == requestedMenuID || parent.Children.Exists(child => child.ID == requestedMenuID))
                    {
                        activeParentID = parent.ID;
                        break;
                    }
                }
            }

            for (int i = 0; i < menuTree.Count; i++)
            {
                NavigationMenuItem parent = menuTree[i];
                bool isActive = activeParentID > 0 ? parent.ID == activeParentID : i == 0;
                html.Append(isActive ? "<li class=\"active open hover\">" : "<li class=\"hover\">");

                html.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">");
                if (!string.IsNullOrWhiteSpace(parent.Icon))
                {
                    html.Append("<i class=\"menu-icon\"><img src=\"")
                        .Append(HttpUtility.HtmlAttributeEncode(Global.ApplicationPath + parent.Icon))
                        .Append("\" style=\"width:28px; height:28px;\" /></i>");
                }
                else
                {
                    html.Append("<i class=\"menu-icon fa fa-list-alt\"></i>");
                }

                html.Append("<span class=\"menu-text\">")
                    .Append(HttpUtility.HtmlEncode(parent.Name))
                    .Append("</span><b class=\"arrow fa fa-angle-down\"></b></a>");

                if (parent.Children.Count > 0)
                {
                    html.Append("<b class=\"arrow\"></b><ul class=\"submenu\">");
                    foreach (NavigationMenuItem child in parent.Children)
                    {
                        string separator = child.Url.Contains("?") ? "&" : "?";
                        string childUrl = Global.ApplicationPath + "/" + child.Url + separator + "Menu_ID=" + child.ID;
                        html.Append("<li class=\"hover\"><a href=\"")
                            .Append(HttpUtility.HtmlAttributeEncode(childUrl))
                            .Append("\"><i class=\"menu-icon fa fa-caret-right\"></i>")
                            .Append(HttpUtility.HtmlEncode(child.Name))
                            .Append("</a><b class=\"arrow\"></b></li>");
                    }
                    html.Append("</ul>");
                }

                html.Append("</li>");
            }

            return html.ToString();
        }

        private string GetMenuName(int Menu_ID)
        {
            UserDAL _userDAL = new UserDAL();
            try
            {
                DataTable _dt =  _userDAL.GetMenuNameMaster(Menu_ID);
                string strParrent = "";
                string str = "";
                if (_dt.Rows.Count>0)
                {
                    str = _dt.Rows[0]["MenuName"].ToString();
                    strParrent = _dt.Rows[0]["MenuParrent"].ToString();
                }
                //_service.AddParameter("@Menu_ID", SqlDbType.Int, Menu_ID);
                //SqlDataReader _drMenu = null;
                //_drMenu = _service.ExecuteSPReader("[CMS_GetMenuNameMaster]");
                //if (_drMenu.HasRows)
                //{
                //    while (_drMenu.Read())
                //    {
                //        str = _drMenu["MenuName"].ToString();
                //        strParrent = _drMenu["MenuParrent"].ToString();
                //    }
                //}
                //_drMenu.Close();
                //_service.CloseConnect();
                //_service.Disconnect();

                return "&nbsp;<h1 style=\"font-family: 'Tahoma';\">" + strParrent + "<small><i class=\"ace-icon fa fa-angle-double-right\"></i>" + str + "</small><h1>";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    _service.CloseConnect();
            //    _service.Disconnect();
            //}
        }
        #endregion




        protected void lb_Exit_Click(object sender, EventArgs e)
        {
            UserDAL _userDAL = new UserDAL();
            T_Users user = null;
            user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            WriteLogHistory2Database.WriteHistory2Database(user.UserID, user.UserFullName, "[Thoát]", 0, "[Thoát] [Thoát khỏi hệ thống]", 0.0);
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            Page.Response.Cookies.Clear();
            Page.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            FormsAuthentication.SignOut();
            Page.Response.Cookies.Remove("prjInfomation");
            Page.Response.Cookies["prjInfomation"].Expires = DateTime.Now.AddMilliseconds(-1);
            Page.Response.Redirect(Global.ApplicationPath + "/Login.aspx");
        }
    }
}
