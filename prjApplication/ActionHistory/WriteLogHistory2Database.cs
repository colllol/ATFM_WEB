using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using prjBusinessLogic;
using prjInfo;

namespace prjBusinessLogic
{
    public class WriteLogHistory2Database
    {
      
        public static void WriteHistory2Database(double UserID,string UserFullName,string strNotes, object MenuID, string ActionCode, double intNewID)
        {
            ActionHistoryDAL actionDAL = new ActionHistoryDAL();
            T_ActionHistory action = new T_ActionHistory();
            try
            {                
                action.UserID = UserID;
                action.FullName = UserFullName;
                action.Notes = strNotes;
                action.ActionsCode = ActionCode;
                action.HostIP = IpAddress();
                action.DateModify = DateTime.Now;
                action.News_ID = intNewID;
                action.Menu_ID = Convert.ToInt32(MenuID);
                actionDAL.InserT_ActionOrc(action);
            }
            catch (Exception ex)
            {
 
            }

        }
        public static void WriteLogin(Int32 UserID,string UserName, Int32 ISLOGIN)
        {

            ActionHistoryDAL actionDAL = new ActionHistoryDAL();           
            try
            {
                actionDAL.InsertLogView(UserID,UserName, ISLOGIN, DateTime.Now.ToString("MM-dd-yyyy HH mm ss"), IpAddress(), GetBrowre());
            }
            catch (Exception ex)
            {

            }

        }
        protected static string GetBrowre()
        {
            System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            string s = "Browser Capabilities\n"
                + "Type = " + browser.Type + "\n"
                + "Name = " + browser.Browser + "\n"
                + "Version = " + browser.Version + "\n"
                + "Major Version = " + browser.MajorVersion + "\n"
                + "Minor Version = " + browser.MinorVersion + "\n"
                + "Platform = " + browser.Platform + "\n"
                + "Is Beta = " + browser.Beta + "\n"
                + "Is Crawler = " + browser.Crawler + "\n"
                + "Is AOL = " + browser.AOL + "\n"
                + "Is Win16 = " + browser.Win16 + "\n"
                + "Is Win32 = " + browser.Win32 + "\n"
                + "Supports Frames = " + browser.Frames + "\n"
                + "Supports Tables = " + browser.Tables + "\n"
                + "Supports Cookies = " + browser.Cookies + "\n"
                + "Supports VBScript = " + browser.VBScript + "\n"
                + "Supports JavaScript = " +
                    browser.EcmaScriptVersion.ToString() + "\n"
                + "Supports Java Applets = " + browser.JavaApplets + "\n"
                + "Supports ActiveX Controls = " + browser.ActiveXControls
                      + "\n"
                + "Supports JavaScript Version = " +
                    browser["JavaScriptVersion"] + "\n";
            return s;
        }
        protected static string IpAddress()
        {
            string strIp;
            strIp = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (strIp == null)
            {
                strIp = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return strIp;
        }
        
    }
}
