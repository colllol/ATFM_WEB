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
    public class AeroDAL
    {
        public DataSet GetAllAero()
        {
             return new oDataProvider().ExecuteDatase("DIC_PKG", "AERO_GET_ALL");
        }
        public DataSet GetAeroVV()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AERO_GET_VV");
        }
        public DataSet GetPageAero(int page_size, int page_index,string where)
        {
             return new oDataProvider().ExecuteDatase("DIC_PKG", "AERO_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageAeroExport(string where)
        {
               return new oDataProvider().ExecuteDatase("DIC_PKG", "AERO_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdAero(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "AERO_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteAero(Int32 ID)
        {
             new oDataProvider().ExecuteNonQuery("DIC_PKG", "AERO_DELETE", new OracleParameter("p_ID", ID));
        }
        public Int64 CreateAero(Aero obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "AERO_INSERT", obj);  
            //return (Int64) new oDataProvider().ExecuteReturnID("DIC_PKG", "AERO_INSERT", obj);
        }
        public Int64 UpdateAero(Aero obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "AERO_UPDATE", obj);
        }
    }
}
