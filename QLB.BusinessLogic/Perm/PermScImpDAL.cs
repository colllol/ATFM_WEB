using System;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class PermScImpDAL
    {
        public DataSet GetPagePermScIMP(int page_size, int page_index, string where)
        {
            try
            {
                return new oDataProvider().ExecuteDatase("PERM_IMP_PKG", "PERMSC_IMP_GET_PAGE"
                    , new OracleParameter("P_PAGE_SIZE", page_size)
                    , new OracleParameter("P_PAGE_INDEX", page_index)
                    , new OracleParameter("P_WHERE", where));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPagePermScIMPExport(string where)
        {
            return new oDataProvider().ExecuteDatase("PERM_IMP_PKG", "PERMSC_IMP_GET_PAGE_EXPORT"
                , new OracleParameter("P_WHERE", where));
        }

        public DataSet GetAllPermSCIMP()
        {
            return new oDataProvider().ExecuteDatase("PERM_IMP_PKG", "PERMSC_IMP_GET_ALL");
        }

        public DataSet GetByIdPERMSC_IMP(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("PERM_IMP_PKG", "PERMSC_IMP_GET_ID"
                , new OracleParameter("p_ID", Id));
        }
        public void DeletePERMSC_IMP(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("PERM_IMP_PKG", "PERMSC_IMP_DELETE"
                , new OracleParameter("p_ID", ID));
        }
        
        public int UpdatePERMSC_IMP(PermScIMP obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("PERM_IMP_PKG", "PERMSC_IMP_UPDATE", obj);
        }
    }
}
