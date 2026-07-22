using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class CraftTypeDAL
    {
        public DataSet GetAllCraftType()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CRAFT_TYPE_GET_ALL");
        }
        public DataSet GetPageCraftType(int page_size, int page_index,string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CRAFT_TYPE_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageCraftTypeExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CRAFT_TYPE_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdCraftType(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "CRAFT_TYPE_GET_ID"
                , new OracleParameter("p_ID", ID));
        }
        public void DeleteCraftType(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "CRAFT_TYPE_DELETE"
                , new OracleParameter("p_ID", ID));
        }
        public Int64 CreateCraftType(CraftType obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "CRAFT_TYPE_INSERT", obj);
        }
        public int UpdateCraftType(CraftType obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "CRAFT_TYPE_UPDATE", obj);
        }
    }
}
