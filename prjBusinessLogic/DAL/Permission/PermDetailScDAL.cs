using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class PermDetailScDAL
    {
        public bool InsertObject(PermDetailSc obj)
        {
            return new clsResuftAPI<PermDetailSc>().InsertObj(obj, "api/PermDetailSc/CreatePermDetailSc");
        }
        public bool UpdateObject(PermDetailSc obj)
        {
            return new clsResuftAPI<PermDetailSc>().UpdateObj(obj, "api/PermDetailSc/UpdatePermDetailSc");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<PermDetailSc>().DeleteObj(id, "api/PermDetailSc/DeletePermDetailSc/");
        }
        public PermDetailSc GetOneObject(string id)
        {
            return new clsResuftAPI<PermDetailSc>().GetOneObj(id, "api/PermDetailSc/GetByIdPermDetailSc/");
        }
        public List<PermDetailSc> GetAllObject()
        {
            return new clsResuftAPI<PermDetailSc>().GetListObj("api/PermDetailSc/GetAllPermDetailSc");
        }
        public List<PermDetailSc> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<PermDetailSc>().GetListObj("api/PermDetailSc/GetPagePermDetailSc/", pageSize, pageIndex, where);
        }
        public List<PermDetailSc> GetPagePermDetailScExport(string where)
        {
            return new clsResuftAPI<PermDetailSc>().GetListObjExportData("api/PermDetailSc/GetPagePermDetailScExport/", where);
        }
        public string InsertReturnId(PermDetailSc obj)
        {
            return new clsResuftAPI<PermDetailSc>().InsertReturnId(obj, "api/PermDetailSc/CreatePermDetailSc");
        }
        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/PermDetailSc/GetAllPermDetailSc");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermDetailSc/GetPagePermDetailSc/", pageSize, pageIndex, where);
        }
        public System.Data.DataTable GetHistoryById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/PermHistory/GetByIdPermDetailScBk", id);
        }
        public bool RestoreRecode(string id, string version, string idUser)
        {
            return new clsResuftAPI().RestoreRecord("api/RestorePerm/RestorePermDetailSc", id,version, idUser);
        }
        public System.Data.DataTable GetTableDelete(string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermDetailSc/GetRecordDeleted", where);
        }
        public String CreateFlight(string fromdate,string todate)
        {
            return (new clsResuftAPI().GetPostValueApiExtension("PERMISSION2FLYPLAN_PKG", "GEN_FLYPLAN".ToUpper(), new { P_FROM = fromdate, P_TO= todate }).ToString());
            
        }
        public System.Data.DataTable GetBySearch(PermDetailSc_Search obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/PermDetailSc/GetBySearch", obj);
        }
    }
}
