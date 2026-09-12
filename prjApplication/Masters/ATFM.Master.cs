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
                string _name = HPCSecurity.CurrentUser.Identity.Name;
                UserDAL _userDAL = new UserDAL();
                Stopwatch userTimer = Stopwatch.StartNew();
                T_Users user = Session[CurrentUserSessionKey] as T_Users;
                bool userFromSession = user != null
                    && string.Equals(user.UserName, _name, StringComparison.OrdinalIgnoreCase);
                if (user != null && !userFromSession)
                {
                    Session.Remove(CurrentUserSessionKey);
                    user = null;
                }
                if (!userFromSession)
                {
                    user = _userDAL.GetUserByUserName(_name);
                    if (user != null) Session[CurrentUserSessionKey] = user;
                }
                userTimer.Stop();
                TracePerformance(userFromSession ? "UserFromSession" : "UserFromApi", userTimer.ElapsedMilliseconds);
                if (user != null)
                {
                    DataTable menuRows;
                    try
                    {
                        menuRows = MenuCache.GetOrLoad(user.UserID, _userDAL);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("[ATFM.MenuCache] " + ex.Message);
                        throw new HttpException(503, "Không thể tải quyền menu.", ex);
                    }

                    int menuID = CommonLib.CheckNullInt(Request["Menu_ID"]);
                    if (menuID > 0 && !MenuCache.ContainsMenu(menuRows, menuID))
                    {
                        Response.Redirect("~/Errors/AccessDenied.aspx", false);
                        Context.ApplicationInstance.CompleteRequest();
                        return;
                    }

                    this.litTitleMenuName.Text = menuID > 0
                        ? GetMenuName(menuID, menuRows)
                        : string.Empty;

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
#if false
        // LEGACY MENU LOADER: giu lai de co the khoi phuc luong nhieu API cu.
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
#endif
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

#if false
        // LEGACY MENU LOADER: luong cu goi GetMenu4User, BindNavigationByUserID
        // va BindNavigationMaster, sau do cache cay menu trong Session.
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
#endif

        private List<NavigationMenuItem> GetNavigationTree(int userID)
        {
            DataTable rows = MenuCache.Get(userID);
            List<NavigationMenuItem> roots = new List<NavigationMenuItem>();
            Dictionary<int, NavigationMenuItem> items = new Dictionary<int, NavigationMenuItem>();

            if (rows == null)
                return roots;

            foreach (DataRow row in rows.Rows)
            {
                NavigationMenuItem item = new NavigationMenuItem
                {
                    ID = CommonLib.CheckNullInt(row["ID"]),
                    Name = CommonLib.CheckNullStr(row["MENUNAME"]),
                    Icon = CommonLib.CheckNullStr(row["MENUICON"]),
                    Url = CommonLib.CheckNullStr(row["MENUURL"]),
                    Children = new List<NavigationMenuItem>()
                };
                items[item.ID] = item;
            }

            foreach (DataRow row in rows.Rows)
            {
                int id = CommonLib.CheckNullInt(row["ID"]);
                int parentID = CommonLib.CheckNullInt(row["PARRENTID"]);
                NavigationMenuItem item;
                if (!items.TryGetValue(id, out item))
                    continue;

                NavigationMenuItem parent;
                if (parentID > 0 && items.TryGetValue(parentID, out parent))
                    parent.Children.Add(item);
                else if (parentID == 0)
                    roots.Add(item);
            }

            foreach (NavigationMenuItem root in roots)
                root.NodeCount = root.Children.Count;

            return roots;
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

        private string GetMenuName(int menuID, DataTable menuRows)
        {
            string menuName;
            string parentName;
            if (!MenuCache.TryGetMenuNames(menuRows, menuID, out menuName, out parentName))
                return string.Empty;

            return "&nbsp;<h1 style=\"font-family: 'Tahoma';\">"
                + HttpUtility.HtmlEncode(parentName)
                + "<small><i class=\"ace-icon fa fa-angle-double-right\"></i>"
                + HttpUtility.HtmlEncode(menuName)
                + "</small><h1>";
        }
        #endregion




        protected void lb_Exit_Click(object sender, EventArgs e)
        {
            UserDAL _userDAL = new UserDAL();
            T_Users user = Session[CurrentUserSessionKey] as T_Users;
            if (user == null)
                user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            if (user != null)
            {
                MenuCache.Remove(user.UserID);
                WriteLogHistory2Database.WriteHistory2Database(user.UserID, user.UserFullName, "[Thoát]", 0, "[Thoát] [Thoát khỏi hệ thống]", 0.0);
            }
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
