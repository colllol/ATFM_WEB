using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic.Perm
{
   public class PermDetailScDAL
    {
        public DataSet GetAllPermDetailSc()
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_SC_GET_ALL", new string[] { }, new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPagePermDetailSc(int page_size, int page_index, string where)
        {
            try
            {
                //DataTable dt1 = DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_SC_GET_COUNT", new string[] { "P_WHERE" }, new object[] //{ where }).Tables[0];
                // DataTable dt2 = 
                //return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_SC_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE" }, new object[] { page_size, page_index, where });
                return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_SC_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPagePermDetailScExport(string where)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_SC_GET_PAGE_EXPORT", new string[] {"P_WHERE" }, new object[] {where });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCountPermDetailSc(string where)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_SC_GET_COUNT", new string[] { "P_WHERE" }, new object[] { where });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetByIdPermDetailSc(Int32 Id)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_SC_GET_ID", new string[] { "p_ID" }, new object[] { Id });
                return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_SC_GET_ID", new OracleParameter("p_ID", Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePermDetailSc(Int32 ID)
        {
            try
            {
                //DataProvider.Instance().ExecStore_Oracle("PERM_PKG.PERMDETAIL_SC_DELETE", new string[] { "p_ID" }, new object[] { ID });
                new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMDETAIL_SC_DELETE", new OracleParameter("p_ID", ID));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 CreatePermDetailSc(PermDetailSc obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "PERM_PKG.PERMDETAIL_SC_INSERT");
            var u = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMDETAIL_SC_INSERT", obj);
            return (Int64)u;

        }
        public Int64 UpdatePermDetailSc(PermDetailSc obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "PERM_PKG.PERMDETAIL_SC_UPDATE");
            var u = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMDETAIL_SC_UPDATE", obj);
            return (Int64)u;
        }
        public Int64 UpdatePermDetailSc_GenBack(PermDetailSc obj)
        {            
            var u = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMDETAIL_SC_UPDATE_GEN_BACK", obj);
            return (Int64)u;
        }
        public DataTable GetDeleted(string where)
        {
            if (string.IsNullOrEmpty(where)) where = " 1=1";
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_SC_GetRecordDeleted"
                , new OracleParameter("P_WHERE", where)
                , new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
        public DataTable GetBySearch(PermDetailSc_Search obj)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PermDetail_SC_GetBySerach", obj).Tables[0];
        }
    }
}
