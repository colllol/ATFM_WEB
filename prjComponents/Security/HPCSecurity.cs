using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using prjBusinessLogic;
//using HPCInfo;
using prjComponents;
namespace prjComponents
{
    public class HPCSecurity
    {
        public static HPCPrincipal CurrentUser
        {
            get
            {
                HPCPrincipal r;

                if (HttpContext.Current.User is HPCPrincipal)
                    r = (HPCPrincipal)HttpContext.Current.User;

                else
                    r = new HPCPrincipal(HttpContext.Current.User.Identity, null);
                return r;
            }
            set
            {
                HttpContext.Current.User = value;
            }
        }
        public static string Encrypt(string cleanString)
        {
            byte[] clearBytes;
            byte[] hashedBytes;
            clearBytes = new UnicodeEncoding().GetBytes(cleanString);
            hashedBytes = ((HashAlgorithm)(CryptoConfig.CreateFromName("MD5"))).ComputeHash(clearBytes);
            return BitConverter.ToString(hashedBytes);
        }
        public static bool IsAccept(int MenuID)
        {
            prjBusinessLogic.UltilFunc _untilDAL = new prjBusinessLogic.UltilFunc();
            UserDAL _objDAL = new UserDAL();
            //DataSet _ds = _untilDAL.GetStoreDataSet("[CMS_GetRole4UserMenu]", new string[] { "@UserName", "@Menu_ID" }, new object[] { HPCSecurity.CurrentUser.Identity.Name, MenuID.ToString() });
            DataTable _dt = _objDAL.GetRole4UserMenu(HPCSecurity.CurrentUser.Identity.Name, MenuID);
            if (_dt.Rows.Count > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}

