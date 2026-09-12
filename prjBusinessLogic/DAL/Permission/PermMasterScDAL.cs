using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class PermMasterScDAL
    {
        public bool InsertObject(PermMasterSc obj)
        {
            return new clsResuftAPI<PermMasterSc>().InsertObj(obj, "api/PermMasterSc/CreatePermMasterSc");
        }
        public bool UpdateObject(PermMasterSc obj)
        {
            return new clsResuftAPI<PermMasterSc>().UpdateObj(obj, "api/PermMasterSc/UpdatePermMasterSc");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<PermMasterSc>().DeleteObj(id, "api/PermMasterSc/DeletePermMasterSc/");
        }

        public PermMasterSc GetOneObject(string id)
        {
            return new clsResuftAPI<PermMasterSc>().GetOneObj(id, "api/PermMasterSc/GetByIdPermMasterSc/");
        }
        public List<PermMasterSc> GetAllObject()
        {
            return new clsResuftAPI<PermMasterSc>().GetListObj("api/PermMasterSc/GetAllPermMasterSc");
        }
        public List<PermMasterSc> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<PermMasterSc>().GetListObj("api/PermMasterSc/GetPagePermMasterSc/", pageSize, pageIndex, where);
        }
        public List<PermMasterSc> GetPagePermMasterScExport(string where)
        {
            return new clsResuftAPI<PermMasterSc>().GetListObjExportData("api/PermMasterSc/GetPagePermMasterScExport", where);
        }
        public string InsertReturnId(PermMasterSc obj)
        {
            return new clsResuftAPI<PermMasterSc>().InsertReturnId(obj, "api/PermMasterSc/CreatePermMasterSc");
        }
        public System.Data.DataTable GetHistoryById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/PermHistory/GetByIdPermMasterScBk", id);
        }
        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/PermMasterSc/GetAllPermMasterSc");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermMasterSc/GetPagePermMasterSc/", pageSize, pageIndex, where);
        }
        public bool RestoreRecode(string id, string version, string idUser)
        {
            return new clsResuftAPI().RestoreRecord("api/RestorePerm/RestorePermMasterSc", id, version, idUser);
        }
        public System.Data.DataTable GetTableDelete(string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermMasterSc/GetRecordDeleted", where);
        }
    }
}
