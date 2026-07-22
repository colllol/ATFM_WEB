using System;
using System.Data;
using QLB.Info.System;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Collections.Generic;

namespace QLB.BusinessLogic
{
    public class ActionHistoryDAL
    {
        public DataSet GetAllActionHistory()
        {
            return new oDataProvider().ExecuteDatase("SYS_PKG", "T_ACTIONHISTORY_GET_ALL");
        }
        public DataSet GetPageActionHistory(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("SYS_PKG", "ACTIONHISTORY_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataTable GetPage(int page_size, int page_index, string where, string FromDate, string ToDate, string FullName)
        {
            try
            {
                List<OracleParameter> lis = new List<OracleParameter>();
                lis.Add(new OracleParameter("P_PAGE_SIZE", page_size));
                lis.Add(new OracleParameter("P_PAGE_INDEX", page_index));
                lis.Add(new OracleParameter("P_WHERE", "1=1"));
                lis.Add(new OracleParameter("P_FROM_DATE", DateTimeHelper.ConvertToDateTime(FromDate).Date));
                lis.Add(new OracleParameter("P_TO_DATE", DateTimeHelper.ConvertToDateTime(ToDate).Date));
                lis.Add(new OracleParameter("P_FULLNAME", FullName));
                return new oDataProvider().ExecuteDatase("SYS_PKG", "T_ACTIONHISTORY_GET_PAGE", lis.ToArray()).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteActionHistory(Int32 ID)
        {
            try
            {
                DataProvider.Instance().ExecStore_Oracle("SYS_PKG.T_ACTIONHISTORY_DELETE", new string[] { "p_ID" }, new object[] { ID });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 CreateActionHistory(ActionHistory obj)
        {
            var u = new oDataProvider().ExecuteReturnID("SYS_PKG", "T_ACTIONHISTORY_INSERT", obj);
            return (Int64)u;
        }
        public Int64 UpdateActionHistory(ActionHistory obj)
        {
            var u = new oDataProvider().ExecuteReturnID("SYS_PKG", "T_ACTIONHISTORY_UPDATE", obj);
            return (Int64)u;
        }
    }
}
