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
    public class GadDAL
    {
        public DataSet GetAllGad()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_D_GET_ALL");
        }
        public DataSet GetPageGad(int page_size, int page_index, string where)
        {            
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_D_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageGadExport(string where)
        {            
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_D_GET_PAGE_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdGad(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "G_A_D_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteGad(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "G_A_D_DELETE", new OracleParameter("p_ID", ID));
        }
        public int CreateGad(Gad obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "G_A_D_INSERT", obj);
        }
        public int UpdateGad(Gad obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "G_A_D_UPDATE", obj);
        }
    }
}
