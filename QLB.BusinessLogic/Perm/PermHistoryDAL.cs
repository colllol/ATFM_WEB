using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic.Perm
{
    public class PermHistoryDAL
    {
        public DataSet GetByIdPermMasterNoBk (Int64 Id)
        {
             return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_BK_GET_ID", new OracleParameter("p_ID", Id));
        }
        public DataSet GetByIdPermMasterScBk (Int64 Id)
        {
             return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_SC_BK_GET_ID", new OracleParameter("p_ID", Id));
        }
        public DataSet GetByIdPermDetailNoBk(Int64 Id)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_NO_BK_GET_ID", new OracleParameter("p_ID", Id));
        }
        public DataSet GetByIdPermDetailScBk(Int64 Id)
        {
             return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_SC_BK_GET_ID", new OracleParameter("p_ID", Id));
        }
        public DataSet GetPagePermMasterScBk(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_SC_BK_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPagePermMasterNoBk(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_BK_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPagePermDetailScBk(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_SC_BK_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPagePermDetailNoBk(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_NO_BK_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
    }
}
