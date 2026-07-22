using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class AirlinesDAL
    {
        public DataSet GetAllAirlines()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRLINES_GET_ALL");
        }
        public DataSet GetPageAirlines(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRLINES_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageAirlinesExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRLINES_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }        
        public DataSet GetByIdAirlines(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRLINES_GET_ID"
                , new OracleParameter("p_ID", Id));                
        }
        public void DeleteAirlines(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "AIRLINES_GET_ID"
                , new OracleParameter("p_ID", ID));
        }
        public int CreateAirlines(Airlines obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "AIRLINES_INSERT", obj);
        }
        public int UpdateAirlines(Airlines obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "AIRLINES_UPDATE", obj);            
        }
    }
}
