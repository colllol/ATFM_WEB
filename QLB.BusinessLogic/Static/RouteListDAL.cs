using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
   public class RouteListDAL
    {
        public DataSet GetAllRouteList()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ROUTE_LIST_GET_ALL");
        }
        public DataSet GetPageRouteList(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ROUTE_LIST_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageRouteListExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ROUTE_LIST_GET_PAGE_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdRouteList(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ROUTE_LIST_GET_ID", new OracleParameter("p_ID", Id));
        }
        public DataSet GetByIdRoutePoint(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "ROUTE_POINT_GET_ID", new OracleParameter("p_ROUTE_ID", Id));
        }
        
        public void DeleteRouteList(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "ROUTE_LIST_DELETE", new OracleParameter("p_ID", ID));
        }

        public int CreateRouteList(RouteList obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "ROUTE_LIST_INSERT", obj);
        }
        public int UpdateRouteList(RouteList obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "ROUTE_LIST_UPDATE", obj);
        }
    }
}
