using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic
{
   public  class FpauthorDAL
    {
        public DataSet GetAllFpauthor()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FPAUTHOR_GET_ALL");
        }
        public DataSet GetPageFpauthor(int page_size, int page_index,string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FPAUTHOR_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }

        public DataSet GetPageFpauthorExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FPAUTHOR_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdFpauthor(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "FPAUTHOR_GET_ID"
                , new OracleParameter("p_ID", ID));
        }
        public void DeleteFpauthor(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "FPAUTHOR_DELETE"
                , new OracleParameter("p_ID", ID));
        }
        public int CreateFpauthor(Fpauthor obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "FPAUTHOR_INSERT", obj);
        }
        public int UpdateFpauthor(Fpauthor obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "FPAUTHOR_UPDATE", obj);
        }
    }
}
