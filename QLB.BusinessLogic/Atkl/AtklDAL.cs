using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using ShareDLL;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using QLB.Info;

namespace QLB.BusinessLogic
{
   public class AtklDAL
    {
        public DataTable GetPage(clsSearchAtkl obj)
        {
            return new oDataProvider().ExecuteDatase("ATKL_PKG", "ATKL_GET_PAGE", obj).Tables[0];

        }
        public DataSet GetById(Int64 Id)
        {
            return new oDataProvider().ExecuteDatase("ATKL_PKG", "ATKL_GET_ID", new OracleParameter("p_ID", Id));
        }
        public Int64 UpdateAtkl(Atkl obj)
        {
            var u = new oDataProvider().ExecuteNonQuery("ATKL_PKG", "Update_Atkl", obj);
            return (Int64)u;
        }
    }
}
