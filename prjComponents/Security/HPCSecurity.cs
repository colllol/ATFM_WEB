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
            if (MenuID <= 0)
                return false;

            UserDAL userDAL = new UserDAL();
            var user = MenuCache.ResolveCurrentUser(userDAL);
            if (user == null)
                return false;

            DataTable menuRows = MenuCache.GetOrLoad(user.UserID, userDAL);
            return MenuCache.ContainsMenu(menuRows, MenuID);
        }
    }
}

