using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic.Perm
{
    public class PermMasterScDAL
    {
        public DataSet GetAllPermMasterSc()
        {
            return DataProvider.Instance().GetStoreDataSet_Oracle("PERM_PKG.PERMMASTER_SC_GET_ALL", new string[] { }, new object[] { });
        }
        public DataSet GetPagePermMasterSc(int page_size, int page_index, string where)
    {           
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_SC_GET_PAGE"
                , new OracleParameter("P_PAGE_SIZE", page_size)
                , new OracleParameter("P_PAGE_INDEX", page_index)
                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPagePermMasterScExport(string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_SC_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdPermMasterSc(Int64 Id)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_SC_GET_ID", new OracleParameter("p_ID", Id));
        }
        public void DeletePermMasterSc(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMMASTER_SC_DELETE", new OracleParameter("p_ID", ID));
        }

        public Int64 CreatePermMasterSc(PermMasterSc obj)
        {
            var u = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMMASTER_SC_INSERT", obj);
            return (Int64)u;
        }
        public Int64 UpdatePermMasterSc(PermMasterSc obj)
        {
            var u = new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMMASTER_SC_UPDATE", obj);
            return (Int64)u;
        }
        public DataTable GetDeleted(string where)
        {
            if (string.IsNullOrEmpty(where)) where = " 1=1";
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_SC_GetRecordDeleted"
                , new OracleParameter("P_WHERE", where)
                , new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
    }
}
