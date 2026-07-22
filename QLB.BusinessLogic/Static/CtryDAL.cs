using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
    public class CtryDAL
    {                              
        public DataSet GetAllCtry()
        {
             return new oDataProvider().ExecuteDatase("DIC_PKG", "CTRY_GET_ALL");
        }
        public DataSet GetPageCtry(int page_size, int page_index,string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CTRY_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageCtryExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CTRY_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdCtry(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CTRY_GET_ID"
                , new OracleParameter("p_ID", Id));  
        }
        public void DeleteCtry(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "CTRY_DELETE"
                , new OracleParameter("p_ID", ID));
        }        
        public int CreateCtry(Ctry obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "CTRY_INSERT", obj);
        }
        public int UpdateCtry(Ctry obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "CTRY_UPDATE", obj);
        }
    }
}
