using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
   public class ReportingRouteDAL
    {
        public DataSet GetAllReportingRoute()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_ROUTE_GET_ALL");
        }
        public DataSet GetPageReportingRoute(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_ROUTE_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }

        public DataSet GetPageReportingSector(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_ROUTESECTOR_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }

        public void DeleteReportingSector(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORTING_ROUTE_SECTOR_DELETE", new OracleParameter("p_ID", ID));
        }

        public DataSet GetPageReportingRouteExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_ROUTE_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdReportingRoute(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_ROUTE_GET_ID", new OracleParameter("p_ID", Id));
        }
        public void DeleteReportingRoute(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORTING_ROUTE_DELETE", new OracleParameter("p_ID", ID));
        }
        public int CreateReportingRoute(ReportingRoute obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORTING_ROUTE_INSERT", obj);
        }
        public int UpdateReportingRoute(ReportingRoute obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "ROUTE_UPDATE", obj);
        }

        
    }
}
