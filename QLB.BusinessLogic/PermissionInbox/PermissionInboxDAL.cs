using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class PermissionInboxDAL
    {
        public DataTable GetPage(clsSearchPermissionInbox obj)
        {
            return new oDataProvider().ExecuteDatase("PERMISSIONINBOX_PKG", "GetPage", obj).Tables[0];

        }

        public DataTable GetByNBR(string Nbr)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("PERMISSIONINBOX_PKG", "GetByNBR", new OracleParameter("P_NBR", Nbr)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
