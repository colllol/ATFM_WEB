using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class NotCompletedFlightsDAL
    {
        public DataTable GetPage(int page_size, int page_index, string where, string FromDate, string ToDate)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", "1=1"));
                lis.Add(new OracleParameter("P_FROM_DATE", DateTimeHelper.ConvertToDateTime(FromDate).Date));
                lis.Add(new OracleParameter("P_TO_DATE", DateTimeHelper.ConvertToDateTime(ToDate).Date));
                return new oDataProvider().ExecuteDatase("FINISH_FLIGHTS_PKG", "NOTCOMPLETED_FLIGHTS_GET_PAGE", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetById(Int64 Id)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("FINISH_FLIGHTS_PKG.NOTCOMPLETED_FLIGHTS_GET_ID", new string[] { "P_FLIGHT_ID" }, new object[] { Id }).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
