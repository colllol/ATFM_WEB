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


namespace QLB.BusinessLogic
{
   public class ErrorsDAL
    {
        public DataTable GetPageErrors(int page_size, int page_index, string where)
        {
            try
            {

                DataSet ax = new DataSet();
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", where));
                return new oDataProvider().ExecuteDatase("CANT_PKG", "ERRORS_GET_PAGE", lis.ToArray()).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
