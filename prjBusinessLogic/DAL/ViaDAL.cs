using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class ViaDAL
    {
        public List<Via> GetListAll()
        {
            return new clsResuftAPI<Via>().GetListObj("api/Via/GetAllVia");
        }

        public List<Via> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Via>().GetListObj("api/Via/GetPageVia/", pageSize, pageIndex, where);
        }
        public List<Via> GetListPageExport(string where)
        {
            return new clsResuftAPI<Via>().GetListObjExportData("api/Via/GetPageViaExport", where);
        }
        public Via GetViaById(string id)
        {
            return new clsResuftAPI<Via>().GetOneObj(id, "api/Via/GetByIdVia/");
        }

        public bool InsertVia(Via obj)
        {
            return new clsResuftAPI<Via>().InsertObj(obj, "api/Via/CreateVia");
        }
        public bool UpdateVia(Via obj)
        {
            return new clsResuftAPI<Via>().UpdateObj(obj, "api/Via/UpdateVia");
        }
        public bool DeleteVia(int id)
        {
            return new clsResuftAPI<Via>().DeleteObj(id.ToString(), "api/Via/DeleteVia/");
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Via/GetAllVia");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Via/GetPageVia/", pageSize, pageIndex, where);
        }
        public bool DeleteByVia(ViaSearch obj)
        {
            return new clsResuftAPI().GetValueWithObject("api/Via/DeleteViaImport", obj).ToString()=="1"?true:false;
        }
        public System.Data.DataTable GetTableBySearch(ViaSearch obj)
        {
            return new clsResuftAPI().GetPostTableWithObject("api/Via/GetViaBySearch/", obj);
        }
        public int CreateVia(Via obj)
        {
            return Convert.ToInt32(new clsResuftAPI<Via>().InsertObj(obj, "api/Via/InsertViaImport") ? 1 : 0);
        }
    }
}
