using ShareDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
namespace QLB.BusinessLogic.Perm
{
    public class RestorePermDAL
    {
        public bool RestorePermMasterNo(Int64 Id, int Version, int UserId)
        {
            List<OracleParameter> lis = new List<OracleParameter>();
            lis.Add(new OracleParameter("P_ID", Id));
            lis.Add(new OracleParameter("P_Version", Version));
            lis.Add(new OracleParameter("P_IdUser", UserId));
            lis.Add(new OracleParameter("P_Out", OracleDbType.Int64, ParameterDirection.Output));
            Int64 ax = (Int64)new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMMASTER_NO_RestoreRecord", lis.ToArray());
            if (ax < 0) return false;
            return true;
        }
        public bool RestorePermMasterSc(Int64 Id, int Version, int UserId)
        {
            List<OracleParameter> lis = new List<OracleParameter>();
            lis.Add(new OracleParameter("P_ID", Id));
            lis.Add(new OracleParameter("P_Version", Version));
            lis.Add(new OracleParameter("P_IdUser", UserId));
            lis.Add(new OracleParameter("P_Out", OracleDbType.Int64, ParameterDirection.Output));            
            Int64 ax = (Int64)new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMMASTER_SC_RestoreRecord", lis.ToArray());
            if (ax < 0) return false;
            return true;
        }
        public bool RestorePermDetailNo(Int64 Id, int Version, int UserId)
        {
            List<OracleParameter> lis = new List<OracleParameter>();
            lis.Add(new OracleParameter("P_ID", Id));
            lis.Add(new OracleParameter("P_Version", Version));
            lis.Add(new OracleParameter("P_IdUser", UserId));
            lis.Add(new OracleParameter("P_Out", OracleDbType.Int64, ParameterDirection.Output));
            Int64 ax = (Int64)new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMDETAIL_NO_RestoreRecord", lis.ToArray());
            
            if (ax < 0) return false;
            return true;
        }
        public bool RestorePermDetailSc (Int64 Id, int Version, int UserId)
        {
            List<OracleParameter> lis = new List<OracleParameter>();
            lis.Add(new OracleParameter("P_ID", Id));
            lis.Add(new OracleParameter("P_Version", Version));
            lis.Add(new OracleParameter("P_IdUser", UserId));
            lis.Add(new OracleParameter("P_Out", OracleDbType.Int64, ParameterDirection.Output));
            Int64 ax = (Int64)new oDataProvider().ExecuteNonQuery("PERM_PKG", "PERMDETAIL_SC_RestoreRecord", lis.ToArray()); 
            if (ax < 0) return false;
            return true;
        }
    }
}
