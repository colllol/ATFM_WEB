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
using System.Text;
using prjInfo;
using prjBusinessLogic;
using prjComponents;
using System.Data.SqlClient;
using HPCServerDataAccess;

namespace prjApplication.Masters
{
    public partial class Atfm_Nofooter : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
                T_Users user = null;
                user = _userDAL.GetUserByUserName(_name);
                if (user != null)
                {

                    litMenu.Text = BindNavigation(user.UserID);
                    literFullName.Text = user.UserFullName;
                    literChangPass.Text = "<a href=\"" + Global.ApplicationPath + "/User/ChangePass.aspx?url=" + Request.Url.AbsoluteUri + "\"><i class=\"ace-icon fa fa-cog\"></i>" + Global.RM.GetString("CHANGE_PASS") + "</a>";
                    literEditProfile.Text = "<a href=\"" + Global.ApplicationPath + "/User/UpdateUsers.aspx?ID=" + user.UserID + "\"><i class=\"ace-icon fa fa-user\"></i>Profile</a>";
                    lb_Exit.Visible = true;

                }
                else { lb_Exit.Visible = false; }
            }
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
        public string BindNavigation(int UserID)
        {
            int MenuID = 0;
            StringBuilder _sbHeader = new StringBuilder();
            StringBuilder _sb = new StringBuilder();
            string _tmp = string.Empty; _tmp = GetMenu4User(UserID);
            DataTable _dt;
            DataTable _dtChild;
            string _sql = string.Empty; string _sqlChild = string.Empty;
            _sql = "CMS_BindNavigationByUserID";
            prjBusinessLogic.UltilFunc _untilDAL = new prjBusinessLogic.UltilFunc();
            UserDAL _objDAL = new UserDAL();
            if (Request["Menu_ID"] != null && Request["Menu_ID"].ToString() != "" && Request["Menu_ID"].ToString() != String.Empty)
            {
                if (CommonLib.IsNumeric(Request["Menu_ID"]) == true)
                {

                    MenuID = UltilFunc.GetLatestID("T_Menus", "ParrentID", " WHERE ID=" + Convert.ToInt32(Request["Menu_ID"]));
                }

            }

            try
            {
                //Luu y viet mot ham tuong tu
                //_dt = _untilDAL.GetStoreDataSet(_sql, new string[] { "@User_ID" }, new object[] { UserID }).Tables[0];
                _dt = _objDAL.BindNavigationByUserID(UserID);
                if (_dt.Rows.Count > 0)
                {

                    for (int i = 0; i < _dt.Rows.Count; i++)
                    {
                        if (MenuID != 0)
                        {
                            if (MenuID == Convert.ToInt32(_dt.Rows[i]["ID"]))
                                _sb.Append("<li class=\"active open hover\">");
                            else
                                _sb.Append("<li class=\"hover\">");
                        }
                        else
                        {
                            if (i == 0)
                                _sb.Append("<li class=\"active open hover\">");
                            else
                                _sb.Append("<li class=\"hover\">");
                        }

                        if (CommonLib.CheckNullInt(_dt.Rows[i]["NODE"]) > 0)
                        {
                            if (CommonLib.CheckNullStr(_dt.Rows[i]["MenuIcon"]).Length > 0)
                            {
                                _sb.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><i class=\"menu-icon\"><img src =\"" + Global.ApplicationPath + CommonLib.CheckNullStr(_dt.Rows[i]["MenuIcon"]) + "\" style=\"width:28px; height: 28px;\" /></i><span class=\"menu-text\">" + CommonLib.CheckNullStr(_dt.Rows[i]["MenuName"]) + "</span><b class=\"arrow fa fa-angle-down\"></b></a>");

                            }
                            else
                            {
                                _sb.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><i class=\"menu-icon fa fa-list-alt\"></i><span class=\"menu-text\">" + CommonLib.CheckNullStr(_dt.Rows[i]["MenuName"]) + "</span><b class=\"arrow fa fa-angle-down\"></b></a>");

                            }

                            if (_tmp.Length == 1)
                            {
                                //_sqlChild = "SELECT MenuDesc,MenuName,MenuURL,MenuOrder,ID,MenuIcon,(select count(*) FROM T_Menus WHERE  T_Menus.isDisplay=1 and  ParrentID = mn.ID) childnodecount ";
                                //_sqlChild += " FROM T_Menus mn WHERE mn.isDisplay=1 and ParrentID = " + _dt.Rows[i]["ID"] + " AND ID IN (" + _tmp + ") ORDER BY mn.MenuOrder";
                                _sqlChild = " and ParrentID = " + _dt.Rows[i]["ID"] + " AND ID IN (" + _tmp + ") ORDER BY mn.MenuOrder";
                            }
                            else
                            {
                                //_sqlChild = " SELECT MenuDesc,MenuName,MenuURL,MenuOrder,ID,MenuIcon,(select count(*) FROM T_Menus WHERE T_Menus.isDisplay=1  and ParrentID = mn.ID) childnodecount";
                                //_sqlChild += " FROM T_Menus mn WHERE mn.isDisplay=1 and ParrentID = " + _dt.Rows[i]["ID"] + " AND ID IN (" + _tmp + ") ORDER BY mn.MenuOrder";
                                _sqlChild = " and ParrentID = " + _dt.Rows[i]["ID"] + " AND ID IN (" + _tmp + ") ORDER BY mn.MenuOrder";
                            }


                            try
                            {
                                //_dtChild = _untilDAL.ExecSqlDataSet(_sqlChild).Tables[0];
                                _dtChild = _objDAL.BindNavigationMaster(_sqlChild);
                                if (_dtChild.Rows.Count > 0)
                                {
                                    _sb.Append("<b class=\"arrow\"></b>");
                                    _sb.Append("<ul  class=\"submenu\">");
                                    for (int j = 0; j < _dtChild.Rows.Count; j++)
                                    {
                                        _sb.Append("<li class=\"hover\">");
                                        _sb.Append("<a href='" + Global.ApplicationPath + "/" + _dtChild.Rows[j]["MenuURL"].ToString() + "?Menu_ID=" + _dtChild.Rows[j]["ID"].ToString() + "'>");
                                        _sb.Append("<i class=\"menu-icon fa fa-caret-right\"></i>" + CommonLib.CheckNullStr(_dtChild.Rows[j]["MenuName"]));
                                        _sb.Append("</a><b class=\"arrow\"></b></li>");
                                    }
                                    _sb.Append("</ul>");

                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }


                        }
                        else
                        {
                            if (CommonLib.CheckNullStr(_dt.Rows[i]["MenuIcon"]).Length > 0)
                            {
                                _sb.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><i class=\"menu-icon\"><img src =\"" + Global.ApplicationPath + CommonLib.CheckNullStr(_dt.Rows[i]["MenuIcon"]) + "\" style=\"width:28px; height: 28px;\" /></i><span class=\"menu-text\">" + CommonLib.CheckNullStr(_dt.Rows[i]["MenuName"]) + "</span><b class=\"arrow fa fa-angle-down\"></b></a>");

                            }
                            else
                            {
                                _sb.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\"><i class=\"menu-icon fa fa-list-alt\"></i><span class=\"menu-text\">" + CommonLib.CheckNullStr(_dt.Rows[i]["MenuName"]) + "</span><b class=\"arrow fa fa-angle-down\"></b></a>");

                            }
                        }
                        _sb.Append(" </li>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _sb.ToString();
        }

        private string GetMenuName(int Menu_ID)
        {
            UserDAL _userDAL = new UserDAL();
            try
            {
                DataTable _dt = _userDAL.GetMenuNameMaster(Menu_ID);
                string strParrent = "";
                string str = "";
                if (_dt.Rows.Count > 0)
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