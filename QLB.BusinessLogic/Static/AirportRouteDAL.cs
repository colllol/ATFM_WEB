using System;
using System.Collections.Generic;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
   public class AirportRouteDAL
    {
        public DataSet GetAllAirportRoute()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRPORT_ROUTE_GET_ALL");
        }
        public DataSet GetPageAirportRoute(int page_size, int page_index,string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRPORT_ROUTE_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageAirportRouteExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRPORT_ROUTE_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdAirportRoute(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AIRPORT_ROUTE_GET_ID"
                , new OracleParameter("p_ID", ID));
        }
        public void DeleteAirportRoute(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "AIRPORT_ROUTE_DELETE"
                , new OracleParameter("p_ID", ID));
        }
        public int CreateAirportRoute(AirportRoute obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "AIRPORT_ROUTE_INSERT", obj);
        }
        public int UpdateAirportRoute(AirportRoute obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "AIRPORT_ROUTE_UPDATE", obj);
        }
    }
}
