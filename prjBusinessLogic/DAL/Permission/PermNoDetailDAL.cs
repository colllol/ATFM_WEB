using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
namespace prjBusinessLogic
{
    public class PermDetailNoDAL
    {
        public bool InsertObject(PermDetailNo obj)
        {
            return new clsResuftAPI<PermDetailNo>().InsertObj(obj, "api/PermDetailNo/CreatePermDetailNo");
        }
        public bool UpdateObject(PermDetailNo obj)
        {
            return new clsResuftAPI<PermDetailNo>().UpdateObj(obj, "api/PermDetailNo/UpdatePermDetailNo");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<PermDetailNo>().DeleteObj(id, "api/PermDetailNo/DeletePermDetailNo/");
        }
        public string InsertReturnId(PermDetailNo obj)
        {
            return new clsResuftAPI<PermDetailNo>().InsertReturnId(obj, "api/PermDetailNo/CreatePermDetailNo");
        }
        public PermDetailNo GetOneObject(string id)
        {
            return new clsResuftAPI<PermDetailNo>().GetOneObj(id, "api/PermDetailNo/GetByIdPermDetailNo/");
        }
        public List<PermDetailNo> GetAllObject()
        {
            return new clsResuftAPI<PermDetailNo>().GetListObj("api/PermDetailNo/GetAllPermDetailNo");
        }
        public List<PermDetailNo> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<PermDetailNo>().GetListObj("api/PermDetailNo/GetPagePermDetailNo/", pageSize, pageIndex, where);
        }
        public List<PermDetailNo> GetPagePermDetailNoExport(string where)
        {
            return new clsResuftAPI<PermDetailNo>().GetListObjExportData("api/PermDetailNo/GetPagePermDetailNoExport/", where);
        }
        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/PermDetailNo/GetAllPermDetailNo");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermDetailNo/GetPagePermDetailNo/", pageSize, pageIndex, where);
        }
        public System.Data.DataTable GetHistoryById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/PermHistory/GetbyIdPermDetailNoBk", id);
        }
        public System.Data.DataTable GetActionHistoryByPermId(long permId)
        {
            return new clsResuftAPI().GetPostTableApiExtension(
                "PERMDETAIL_NO_HISTORY_PKG",
                "GET_BY_PERM_ID",
                new { P_PERM_ID = permId }
            );
        }
        public bool RestoreRecode(string id, string version, string idUser)
        {
            return new clsResuftAPI().RestoreRecord("api/RestorePerm/RestorePermDetailNo", id, version, idUser);
        }
        public System.Data.DataTable GetTableDelete(string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermDetailNo/GetRecordDeleted", where);
        }
        public System.Data.DataTable GetBySearch(PermDetailNo_Search obj)
        {
            return new clsResuftAPI().GetTableWithObject("api/PermDetailNo/GetBySearch", obj);
        }
    }
}
