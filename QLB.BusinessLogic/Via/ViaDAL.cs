using System;
using System.Collections.Generic;
using System.Linq;
using ShareDLL;
using System.Data;
using QLB.Info;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class ViaDAL
    {
        public DataSet GetAllVia()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "VIA_GET_ALL");
        }
        public DataSet GetPageVia(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "VIA_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageViaExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "VIA_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdVia(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "VIA_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteVia(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "VIA_DELETE", new OracleParameter("p_ID", ID));
        }
        public Int64 CreateVia(Via obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "VIA_INSERT", obj);            
        }
        public Int64 UpdateVia(Via obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "VIA_UPDATE", obj);
        }
        public DataTable GetBySearch(ViaSearch obj)
        {
            return new oDataProvider().ExecuteDatase("PERM_IMP_PKG", "GetViaBySearch", obj).Tables[0];
        }
        public Int64 DeleteViaBy(ViaSearch obj)
        {
            return Convert.ToInt64(new oDataProvider().ExecuteScalar("PERM_IMP_PKG", "DeleteByOper", obj).ToString());
        }
        public Int64 InsertViaImport(ViaSearch obj)
        {
            return Convert.ToInt64(new oDataProvider().ExecuteScalar("PERM_IMP_PKG", "InsertViaImport", obj).ToString());
        }

        #region VIA REPORT

        public DataSet GetPageM_VIA_ARIPORT(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "M_VIA_ARIPORT_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageM_VIA_ARIPORTExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "M_VIA_ARIPORT_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdM_VIA_ARIPORT(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "M_VIA_ARIPORT_GET_ID", new OracleParameter("p_ID", ID));
        }
      
        public Int64 CreateM_VIA_ARIPORT(M_VIA_ARIPORT obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "M_VIA_ARIPORT_INSERT", obj);
        }
        public Int64 UpdateM_VIA_ARIPORT(M_VIA_ARIPORT obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "M_VIA_ARIPORT_UPDATE", obj);
        }
                
        public void DeleteM_VIA_ARIPORT(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "M_VIA_ARIPORT_DELETE", new OracleParameter("p_ID", ID));
        }
        #endregion




    }
}
