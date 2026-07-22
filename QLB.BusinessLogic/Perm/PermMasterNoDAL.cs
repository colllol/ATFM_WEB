using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;
using System.Collections.Generic;

namespace QLB.BusinessLogic.Perm
{
    public class PermMasterNoDAL
    {
        public DataSet GetAllPermMasterNo()
        {
             return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_GET_ALL", new Oracle.DataAccess.Client.OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, "", ParameterDirection.Output));
        }
        public DataSet GetPagePermMasterNo(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_GET_PAGE"
                    , new OracleParameter("P_PAGE_SIZE", page_size)
                    , new OracleParameter("P_PAGE_INDEX", page_index)
                    , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPagePermMasterNoExport(string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));        
        }
        public DataSet GetByIdPermMasterNo(Int64 Id)
        {
             return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_GET_ID", new OracleParameter("p_ID", Id));
        }
        public void DeletePermMasterNo(Int64 ID)
        {
            new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMMASTER_NO_DELETE", new OracleParameter("P_ID", ID));
        }

        public Int64 CreatePermMasterNo(PermMasterNo obj)
        {
            var ax = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMMASTER_NO_INSERT", obj);
            return (Int64)ax;
        }
        public Int64 UpdatePermMasterNo(PermMasterNo obj)
        {
            var u = new oDataProvider().ExecuteReturnID("PERM_PKG", "PERMMASTER_NO_UPDATE", obj);
            return (Int64)u;
        }
        public DataTable GetDeleted(string where)
        {
            if (string.IsNullOrEmpty(where)) where = " 1=1";
            return new oDataProvider().ExecuteDatase("PERM_PKG", "PERMMASTER_NO_GetRecordDeleted"
                , new OracleParameter("P_WHERE", where)
                , new OracleParameter("P_OUT_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)).Tables[0];
        }
    }
}
