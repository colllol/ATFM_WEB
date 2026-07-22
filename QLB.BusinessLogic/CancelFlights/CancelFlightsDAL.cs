using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ShareDLL;
using Oracle.DataAccess.Client;
using QLB.Info;

namespace QLB.BusinessLogic
{
    public class CancelFlightsDAL
    {
        public DataTable GetPage(int page_size, int page_index, string where)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("CANCEL_FLIGHTS_PKG", "GETPAGE"
                            , new OracleParameter("P_PAGE_SIZE", page_size)
                            , new OracleParameter("P_PAGE_INDEX", page_index)
                            , new OracleParameter("P_WHERE", where)).Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GET_CANCEL_FLIGHTS(clsFinishedFlightSearch obj)
        {
            return new oDataProvider().ExecuteDatase("CANCEL_FLIGHTS_PKG", "GET_T_CANCELED_FLIGHTS_BY_TIME", obj).Tables[0];
        }
    }
}
