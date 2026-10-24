using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class DocumentDAL
    {
        public List<Document> GetListAll()
        {
            return new clsResuftAPI<Document>().GetListObj("api/Document/GetAllDocument");
        }

        public List<Document> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Document>().GetListObj("api/Document/GetPageDocument/", pageSize, pageIndex, where);
        }
        public List<Document> GetListPageExport(string where)
        {
            return new clsResuftAPI<Document>().GetListObjExportData("api/Document/GetPageDocumentExport", where);
        }
        public Document GetDocumentById(string id)
        {
            return new clsResuftAPI<Document>().GetOneObj(id, "api/Document/GetByIdDocument/");
        }

        public bool InsertDocument(Document obj)
        {
            return new clsResuftAPI<Document>().InsertObj(obj, "api/Document/CreateDocument");
        }
        public bool UpdateDocument(Document obj)
        {
            return new clsResuftAPI<Document>().UpdateObj(obj, "api/Document/UpdateDocument");
        }
        public bool DeleteDocument(int id)
        {
            return new clsResuftAPI<Document>().DeleteObj(id.ToString(), "api/Document/DeleteDocument");
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Document/GetAllDocument");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Document/GetPageDocument/", pageSize, pageIndex, where);
        }
    }
}
