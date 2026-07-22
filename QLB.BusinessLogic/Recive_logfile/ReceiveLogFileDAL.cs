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
    public class ReceiveLogFileDAL
    {
        public DataTable GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            
                DataSet ax = new DataSet();               
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", "1=1"));
                lis.Add(new OracleParameter("P_FROM_DATE", DateTimeHelper.ConvertToDateTime(FromDate).Date));
                lis.Add(new OracleParameter("P_TO_DATE", DateTimeHelper.ConvertToDateTime(ToDate).Date));
                return new oDataProvider().ExecuteDatase("RECEIVELOGFILE_PKG", "GetPage", lis.ToArray()).Tables[0];
            
        }
        public DataTable GetPage(clsSearchReceiveLogFile obj)
        {
            return new oDataProvider().ExecuteDatase("RECEIVELOGFILE_PKG", "GetPage", obj).Tables[0];

        }
        public DataTable GetById(Int64 Id)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("RECEIVELOGFILE_PKG", "GETID", new OracleParameter("p_ID", Id)).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
