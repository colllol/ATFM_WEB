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
    public class ConfigDAL
    {
        public DataSet GetAllConfig()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Config_GET_ALL");
        }
        public DataSet GetPageConfig(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Config_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageConfigExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Config_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdConfig(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Config_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteConfig(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "CONFIG_DELETE", new OracleParameter("P_ID", ID));
        }
        public Int64 CreateConfig(Config obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "Config_INSERT", obj);            
        }
        public Int64 UpdateConfig(Config obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "Config_UPDATE", obj);
        }
    }
}
