using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class FlyPurposeDAL
    {
        public DataSet GetAllFlyPurpose()
        {               
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FLY_PURPOSE_GET_ALL");
        }
        public DataSet GetPageFlyPurpose(int page_size, int page_index,string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FLY_PURPOSE_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageFlyPurposeExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FLY_PURPOSE_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }        
        public DataSet GetByIdFlyPurpose(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FLY_PURPOSE_GET_ID"
                , new OracleParameter("p_ID", ID));
        }
        public void DeleteFlyPurpose(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "FLY_PURPOSE_DELETE"
                , new OracleParameter("p_ID", ID));
        }
        public int CreateFlyPurpose(FlyPurpose obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "FLY_PURPOSE_INSERT", obj);
        }
        public int UpdateFlyPurpose(FlyPurpose obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "FLY_PURPOSE_UPDATE", obj);
        }
    }
}
