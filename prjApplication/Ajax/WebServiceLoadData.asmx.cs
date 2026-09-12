using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using prjBusinessLogic;
using prjComponents;

namespace prjApplication.Ajax
{
    /// <summary>
    /// Summary description for WebServiceLoadData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceLoadData : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string GetTotals()
        {
            StringBuilder _strbd = new StringBuilder();
                       
            try
            {
                AlarmDAL _dal = new AlarmDAL();
                DataTable _dt = _dal.GetTableCountAlarm();
                if(_dt.Rows.Count > 0)
                    _strbd.Append(_dt.Rows[0]["TOTAL"].ToString());
                else
                    _strbd.Append("0");
            }
            catch (Exception ex) { ex.StackTrace.ToString(); }
            finally { }
            return _strbd.ToString();
        }
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [WebMethod]
        public string UpdateOnline()
        {
            string _strbd = "";
            prjBusinessLogic.UserDAL _userDAL = new UserDAL();
            prjInfo.T_Users _user = null;
            _user = _userDAL.GetUserByUserName(HPCSecurity.CurrentUser.Identity.Name);
            try
            {
                WriteLogHistory2Database.WriteLogin(_user.UserID, _user.UserFullName, 1);
            }
            catch (Exception ex) { ex.StackTrace.ToString(); }
            finally { }
            return _strbd;
        }
    }
}
