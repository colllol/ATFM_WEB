using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class AtsDAL
    {
        public DataSet GetPageAtsRoute(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ATSROUTE_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }

        public void DeleteAtsRoute(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "ATSROUTE_DELETE"
                , new OracleParameter("p_ID", ID));
        }

        public DataSet GetPageAtsFir(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ATSFIR_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }

        public void DeleteAtsFir(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "ATSFIR_DELETE"
                , new OracleParameter("p_ID", ID));
        }

        public DataSet GetPageAtsWay(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ATSWAY_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }

        public void DeleteAtsWay(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "ATSWAY_DELETE"
                , new OracleParameter("p_ID", ID));
        }

        public DataSet GetPageAtsAreo(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ATSAREO_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }

        public void DeleteAtsAreo(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "ATSAREO_DELETE"
                , new OracleParameter("p_ID", ID));
        }
    }
}
