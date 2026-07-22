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
    public class DocumentDAL
    {
        public DataSet GetAllDocument()
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Document_GET_ALL");
        }
        public DataSet GetPageDocument(int page_size, int page_index, string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Document_GET_PAGE", new OracleParameter("P_PAGE_SIZE", page_size), new OracleParameter("P_PAGE_INDEX", page_index), new OracleParameter("P_WHERE", where));
        }
        public DataSet GetPageDocumentExport(string where)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Document_GET_PAGE_EXPORT", new OracleParameter("P_WHERE", where));
        }
        public DataSet GetByIdDocument(Int32 ID)
        {
            return new oDataProvider().ExecuteDatase("DIC_PKG", "Document_GET_ID", new OracleParameter("p_ID", ID));
        }
        public void DeleteDocument(Int32 ID)
        {
            new oDataProvider().ExecuteNonQuery("DIC_PKG", "Document_DELETE", new OracleParameter("P_ID", ID));
        }
        public Int64 CreateDocument(Document obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "Document_INSERT", obj);            
        }
        public Int64 UpdateDocument(Document obj)
        {
            return (Int64)new oDataProvider().ExecuteNonQuery("DIC_PKG", "Document_UPDATE", obj);
        }
    }
}
