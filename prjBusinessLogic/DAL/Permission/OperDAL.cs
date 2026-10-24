using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class OperDAL
    {

        public bool InsertObject(Oper obj)
        {
            return new clsResuftAPI<Oper>().InsertObj(obj, "api/Oper/CreateOper");
        }
        public bool UpdateObject(Oper obj)
        {
            return new clsResuftAPI<Oper>().UpdateObj(obj, "api/Oper/UpdateOper");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<Oper>().DeleteObj(id, "api/Oper/DeleteOper/");
        }

        public Oper GetOneObject(string id)
        {
            return new clsResuftAPI<Oper>().GetOneObj(id, "api/Oper/GetByIdOper/");
        }
        public List<Oper> GetAllObject()
        {
            return new clsResuftAPI<Oper>().GetListObj("api/Oper/GetAllOper");
        }
        public List<Oper> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Oper>().GetListObj("api/Oper/GetPageOper/", pageSize, pageIndex, where);
        }
        public List<Oper> GetPageOperExport(string where)
        {
            return new clsResuftAPI<Oper>().GetListObjExportData("api/Oper/GetPageOperExport", where);
        }
        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Oper/GetAllOper");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Oper/GetPageOper/", pageSize, pageIndex, where);
        }
    }
}
