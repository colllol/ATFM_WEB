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
    public  class SectorsDAL
    {
        public DataSet GetAllSectors()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SECTOR_GET_ALL");
        }
       
        public DataSet GetPageSectors(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SECTOR_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageSectorsExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SECTOR_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdSectors(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SECTOR_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteSectors(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "AERO_DELETE", new OracleParameter("p_ID", ID));
        }
        public Int64 CreateSectors(Sectors obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "SECTOR_INSERT", obj);            
        }
        public Int64 UpdateSectors(Sectors obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "SECTOR_UPDATE", obj);
        }
    }
}

