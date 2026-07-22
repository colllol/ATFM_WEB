using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
   public class ReportingPointDAL
    {
        public DataSet GetAllReportingPoint()
        {
            #region Code Old
            //try
            //{
            //    return DataProvider.Instance().GetStoreDataSet_Oracle("DIC_PKG.REPORTING_POINT_GET_ALL", new string[] { }, new object[] { });
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_POINT_GET_ALL");
        }
        public DataSet GetPageReportingPoint(int page_size, int page_index, string where)
        {
            #region Code Old
            //try
            //{
            //    return DataProvider.Instance().GetStoreDataSet_Oracle("DIC_PKG.REPORTING_POINT_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE" }, new object[] { page_size, page_index, where });
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion

            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_POINT_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageReportingPointExport(string where)
        {
            #region Code Old
            //try
            //{
            //    return DataProvider.Instance().GetStoreDataSet_Oracle("DIC_PKG.REPORTING_POINT_EXPORT", new string[] {"P_WHERE" }, new object[] {where });

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion

            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_POINT_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }
        
        public DataSet GetByIdReportingPoint(Int32 Id)
        {
            #region Code Old
            //try
            //{
            //    return DataProvider.Instance().GetStoreDataSet_Oracle("DIC_PKG.REPORTING_POINT_GET_ID", new string[] { "p_ID" }, new object[] { Id });
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion

            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_POINT_GET_ID", new OracleParameter("p_ID", Id));
        }
        public DataSet GetAddReportingPoint(Int32 Route_Id)
        {
            #region Code Old
            //try
            //{
            //    return DataProvider.Instance().GetStoreDataSet_Oracle("DIC_PKG.REPORTING_POINT_ADD", new string[] { "P_ROUTE_ID" }, new object[] { Route_Id });
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_POINT_ADD", new OracleParameter("P_ROUTE_ID", Route_Id));
        }

        public DataSet GetAddSectorPoint(Int32 Route_Id)
        {
           
            return new oDataProvider().ExecuteDatase("DIC_PKG", "REPORTING_SECTOR_ADD", new OracleParameter("P_ROUTE_ID", Route_Id));
        }



        public void DeleteReportingPoint(Int32 ID)
        {
            #region Code Old
            //DataProvider.Instance().ExecStore_Oracle("DIC_PKG.REPORTING_POINT_DELETE", new string[] { "p_ID" }, new object[] { ID });
            #endregion
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORTING_POINT_DELETE", new OracleParameter("p_ID", ID));
        }
        public int CreateReportingPoint(ReportingPoint obj)
        {
            #region Code Old
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "DIC_PKG.REPORTING_POINT_INSERT");
            #endregion
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORTING_POINT_INSERT", obj);
        }
        public int UpdateReportingPoint(ReportingPoint obj)
        {
            #region Code Old
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "DIC_PKG.REPORTING_POINT_UPDATE");
            #endregion
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "REPORTING_POINT_UPDATE", obj);
        }
    }
}
