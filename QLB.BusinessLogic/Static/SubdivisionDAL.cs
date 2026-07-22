using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using QLB.Info;
using ShareDLL;
using Oracle.DataAccess.Client;

namespace QLB.BusinessLogic
{
    public class SubdivisionDAL
    {
        public DataSet GetAllSubdivision()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SUBDIVISION_GET_ALL");
        }
        public DataSet GetPageSubdivision(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SUBDIVISION_GET_PAGE"
                                , new OracleParameter("P_PAGE_SIZE", page_size)
                                , new OracleParameter("P_PAGE_INDEX", page_index)
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageSubdivisionExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SUBDIVISION_GET_PAGE_EXPORT"
                                , new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdSubdivision(Int32 Id)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "SUBDIVISION_GET_ID", new OracleParameter("p_ID", Id));
        }
        public void DeleteSubdivision(Int32 ID)
        {
            DataProvider.Instance().ExecStore_Oracle("DIC_PKG.SUBDIVISION_DELETE", new string[] { "p_ID" }, new object[] { ID });
        }
        public int CreateSubdivision(Subdivision obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "SUBDIVISION_INSERT", obj);
        }
        public int UpdateSubdivision(Subdivision obj)
        {
            return (int)new oDataProvider().ExecuteNonQuery("DIC_PKG", "SUBDIVISION_UPDATE", obj);
        }
    }
}
