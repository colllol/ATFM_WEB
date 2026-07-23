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
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Web.Hosting;

namespace prjApplication
{
    public partial class Login : System.Web.UI.Page
    {
        public const string CurrentUserSessionKey = "ATFM_CURRENT_USER";
        public const string MenuUserSessionKey = "ATFM_MENU_USER_ID";
        public const string MenuRowsSessionKey = "ATFM_MENU_ROWS";
        private const string LegacyNavigationKeyPrefix = "ATFM_NAVIGATION_TREE_";
        private const string MenuServerCacheKeyPrefix = "ATFM_MENU_ROWS_CACHE_";

        UserDAL _userDAL = new UserDAL();
        T_Users user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //HttpCookie cookie = Request.Cookies["prjInfomation"];
            //if (cookie != null)
            //{
            //    object _Username = cookie["hpcUserNames"];
            //    object _Password = cookie["hpcPassword"];
            //    if (_Username != null && _Password != null)
            //    {
            //        user = _userDAL.GetUserByUserPass(_Username.ToString(), _Password.ToString());
            //        if (user != null)
            //        {
            //            if (user.UserName.Trim() == _Username.ToString() && _Password.ToString() == user.UserPass.Trim())
            //            {
            //                FormsAuthentication.SetAuthCookie(user.UserName, false);
            //                Response.Redirect(Global.ApplicationPath + "/Default.aspx");
            //            }
            //        }
            //    }
            //}
            //txtUserName.Focus();

          

        }


        protected void btLogon_Click(object sender, EventArgs e)
        {
            Stopwatch totalTimer = Stopwatch.StartNew();
            var regex = new Regex(@"^[a-zA-Z0-9]+$");

            if (!regex.IsMatch(txtUserName.Text)) return;
            if (!regex.IsMatch(txtPassWord.Text)) return;

            Stopwatch phaseTimer = Stopwatch.StartNew();
            user = _userDAL.GetUserByUserPass(txtUserName.Text, HPCSecurity.Encrypt(txtPassWord.Text));
            phaseTimer.Stop();
            TraceLoginTiming("Authenticate", phaseTimer.ElapsedMilliseconds);

            if (user != null && user.UserID > 0)
            {
                if (user.UserActive == 1)
                {
                    phaseTimer.Restart();
                    try
                    {
                        PrepareMenuSession(user.UserID);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError("[Login.Menu] " + ex.Message);
                        System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "MenuError",
                            "alert('Không thể tải quyền menu. Vui lòng thử đăng nhập lại.');", true);
                        return;
                    }

                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    Session[CurrentUserSessionKey] = user;
                    if (chkRemember.Checked)
                    {
                        HttpCookie cookie = new HttpCookie("prjInfomation");
                        cookie.Values.Add("hpcUserNames", txtUserName.Text.Trim());
                        cookie.Values.Add("hpcPassword", HPCSecurity.Encrypt(txtPassWord.Text.Trim()));
                        cookie.Expires = DateTime.Now.AddMinutes(60);
                        Response.Cookies.Add(cookie);
                    }
                    phaseTimer.Stop();
                    TraceLoginTiming("CreateAuthAndSession", phaseTimer.ElapsedMilliseconds);

                    QueueLoginHistory(user.UserID, user.UserFullName, "[Đăng nhập hệ thống]", 0,
                        "[Đăng nhập] [Đăng nhập hệ thống]");

                    totalTimer.Stop();
                    TraceLoginTiming("TotalBeforeRedirect", totalTimer.ElapsedMilliseconds);
                    Response.AppendHeader("Server-Timing", "login;dur=" + totalTimer.ElapsedMilliseconds);
                    Response.Redirect(Global.ApplicationPath + "/Common/ChartReport.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Tài khoản của bạn đang bị khóa. Liên hệ quản trị!');", true);
                }
            }
            else
            {
                QueueLoginHistory(0, txtUserName.Text, "[Error]", 1, "[Đăng nhập không thành công]");
                System.Web.UI.ScriptManager.RegisterStartupScript(this, typeof(string), "Message", "alert('Tên đăng nhập hoặc mật khẩu không đúng.');", true);
            }
        }

        private void PrepareMenuSession(int userID)
        {
            int sessionUserID = CommonLib.CheckNullInt(Session[MenuUserSessionKey]);
            DataTable sessionRows = Session[MenuRowsSessionKey] as DataTable;
            if (sessionUserID == userID && IsValidMenuTable(sessionRows))
                return;

            if (sessionUserID > 0 && sessionUserID != userID)
            {
                Session.Remove(CurrentUserSessionKey);
                Session.Remove(MenuRowsSessionKey);
                Session.Remove(LegacyNavigationKeyPrefix + sessionUserID);
            }

            string cacheKey = MenuServerCacheKeyPrefix + userID;
            DataTable menuRows = HttpRuntime.Cache[cacheKey] as DataTable;
            if (!IsValidMenuTable(menuRows))
            {
                menuRows = _userDAL.GetAllMenu4User(userID);
                if (!IsValidMenuTable(menuRows))
                    throw new InvalidOperationException("GetAllMenu4User returned invalid data.");

                HttpRuntime.Cache.Insert(
                    cacheKey,
                    menuRows.Copy(),
                    null,
                    DateTime.UtcNow.AddMinutes(30),
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }

            Session[MenuRowsSessionKey] = menuRows.Copy();
            Session[MenuUserSessionKey] = userID;
        }

        private static bool IsValidMenuTable(DataTable table)
        {
            return table != null
                && table.Columns.Contains("ID")
                && table.Columns.Contains("MENUNAME")
                && table.Columns.Contains("MENUORDER")
                && table.Columns.Contains("PARRENTID")
                && table.Columns.Contains("MENUURL")
                && table.Columns.Contains("MENUICON");
        }

        private static void QueueLoginHistory(double userID, string fullName, string notes, object menuID, string actionCode)
        {
            HostingEnvironment.QueueBackgroundWorkItem(delegate(CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested) return;

                try
                {
                    WriteLogHistory2Database.WriteHistory2Database(userID, fullName, notes, menuID, actionCode, 0.0);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError("[Login.Performance] Background history failed: " + ex.Message);
                }
            });
        }

        private static void TraceLoginTiming(string phase, long elapsedMilliseconds)
        {
            System.Diagnostics.Trace.TraceInformation(
                "[Login.Performance] {0}: {1} ms", phase, elapsedMilliseconds);
        }
    }
}
