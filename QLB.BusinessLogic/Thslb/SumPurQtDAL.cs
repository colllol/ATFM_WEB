using System.Text;
using System.Threading.Tasks;
using System.Data;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Collections.Generic;
using System;

namespace QLB.BusinessLogic
{
   public class SumPurQtDAL
    {
        public DataTable SumPurQt(string date1)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                return new oDataProvider().ExecuteDatase("CANT_PKG", "CANT_REALIZE_GET_PAGE", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
