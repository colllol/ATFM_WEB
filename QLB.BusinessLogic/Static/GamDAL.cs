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
   public class GamDAL
    {
        public DataSet GetAllGam()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_M_GET_ALL");
        }
        public DataSet GetPageGam(int page_size, int page_index,string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_M_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageGamExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_M_GET_PAGE_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdGam(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_M_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteGam(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "G_A_M_DELETE", new OracleParameter("p_ID", ID));
        }
        public int CreateGam(Gam obj)
        {
            return (int) new oDataProvider().ExecuteNonQuery("DIC_PKG", "G_A_M_INSERT", obj);
        }
        public int UpdateGam(Gam obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "G_A_M_UPDATE", obj);
        }
    }
}
