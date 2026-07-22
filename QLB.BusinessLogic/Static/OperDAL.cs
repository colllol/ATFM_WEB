using System;
using System.Data;
using QLB.Info.Perm;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic.Perm
{
   public class OperDAL
    {
        public DataSet GetAllOper()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "OPER_GET_ALL");
        }
        public DataSet GetPageOper(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "OPER_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageOperExport(string where)
        {            
            return new oDataProvider().ExecuteDatase("DIC_PKG", "OPER_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }       
        public DataSet GetByIdOper(Int64 Id)
        {            
            return new oDataProvider().ExecuteDatase("DIC_PKG", "OPER_GET_ID", new OracleParameter("p_ID", Id));
        }
        public void DeleteOper(Int64 ID)
        {            
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "OPER_DELETE", new OracleParameter("p_ID", ID));
        }

        public Int64 CreateOper(Oper obj)
        {
            return (Int64) new oDataProvider().ExecuteReturnID("DIC_PKG", "OPER_INSERT", obj);
        }
        public Int64 UpdateOper(Oper obj)
        {
            return (Int64)new oDataProvider().ExecuteReturnID("DIC_PKG", "OPER_UPDATE", obj);
        }
    }
}
