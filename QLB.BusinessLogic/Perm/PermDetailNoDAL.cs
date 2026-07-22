using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic.Perm
{
    public class PermDetailNoDAL
    {
        public DataSet GetAllPermDetailNo()
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_NO_GET_ALL", new string[] { }, new object[] { });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPagePermDetailNo(int page_size, int page_index, string where)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_NO_GET_PAGE", new string[] { "P_PAGE_SIZE", "P_PAGE_INDEX", "P_WHERE" }, new object[] { page_size, page_index, where });
                return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_NO_GET_PAGE"
                        , new OracleParameter("P_PAGE_SIZE", page_size)
                        , new OracleParameter("P_PAGE_INDEX", page_index)
                        , new OracleParameter("P_WHERE", where));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetPagePermDetailNoExport(string where)
        {
            try
            {                
                return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_NO_GET_PAGE_EXPORT", new string[] {"P_WHERE" }, new object[] {where });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCountPermDetailNo(string where)
        {
            try
            {
                return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_NO_GET_COUNT", new string[] { "P_WHERE" }, new object[] { where });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetByIdPermDetailNo(Int64 Id)
        {
            try
            {
                //return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMDETAIL_NO_GET_ID", new string[] { "p_ID" }, new object[] { Id });
                return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_NO_GET_ID", new OracleParameter("p_ID", Id));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeletePermDetailNo(Int64 ID)
        {
            try
            {
                //DataProvider.Instance().ExecStore_Oracle("PERM_PKG.PERMDETAIL_NO_DELETE", new string[] { "p_ID" }, new object[] { ID });
               new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMDETAIL_NO_DELETE", new OracleParameter("p_ID", ID));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int64 CreatePermDetailNo(PermDetailNo obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "PERM_PKG.PERMDETAIL_NO_INSERT");
            var u = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMDETAIL_NO_INSERT", obj);
            return (Int64)u;
        }
        public Int64 UpdatePermDetailNo(PermDetailNo obj)
        {
            //return DataProvider.Instance().InsertObjectReturn_Oracle(obj, "PERM_PKG.PERMDETAIL_NO_UPDATE");
            var u = new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMDETAIL_NO_UPDATE", obj);
            return (Int64)u;
        }
        public Int64 UpdatePermDetailNo_GenBack(PermDetailNo obj)
        {           
            var u = new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMDETAIL_NO_UPDATE_GEN_BACK", obj);
            return (Int64)u;
        }
        public DataTable GetDeleted(string where)
        {
            if (string.IsNullOrEmpty(where)) where = " 1=1";
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMDETAIL_NO_GetRecordDeleted"
                , new OracleParameter("P_WHERE", where)
                , new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
        public DataTable GetBySearch(PermDetailNo_Search obj)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PermDetail_No_GetBySearch", obj).Tables[0];
        }
    }
}
