using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace prjBusinessLogic
{
    public class PermMasterNoDAL
    {
        public bool InsertObject(PermMasterNo obj)
        {
            return new clsResuftAPI<PermMasterNo>().InsertObj(obj, "api/PermMasterNo/CreatePermMasterNo");
        }
        public bool UpdateObject(PermMasterNo obj)
        {
            return new clsResuftAPI<PermMasterNo>().UpdateObj(obj, "api/PermMasterNo/UpdatePermMasterNo");
        }
        public bool DeleteObject(string id)
        {
            return new clsResuftAPI<PermMasterNo>().DeleteObj(id, "api/PermMasterNo/DeletePermMasterNo/");
        }

        public PermMasterNo GetOneObject(string id)
        {
            return new clsResuftAPI<PermMasterNo>().GetOneObj(id, "api/PermMasterNo/GetByIdPermMasterNo/");
        }
        public List<PermMasterNo> GetAllObject()
        {
            return new clsResuftAPI<PermMasterNo>().GetListObj("api/PermMasterNo/GetAllPermMasterNo");
        }
        public List<PermMasterNo> GetPageObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<PermMasterNo>().GetListObj("api/PermMasterNo/GetPagePermMasterNo/", pageSize, pageIndex, where);
        }
        public List<PermMasterNo> GetPagePermMasterNoExport(string where)
        {
            return new clsResuftAPI<PermMasterNo>().GetListObjExportData("api/PermMasterNo/GetPagePermMasterNoExport",where);
        }        
        public string InsertReturnId(PermMasterNo obj)
        {
            return new clsResuftAPI<PermMasterNo>().InsertReturnId(obj, "api/PermMasterNo/CreatePermMasterNo");
        }
        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/PermMasterNo/GetAllPermMasterNo");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermMasterNo/GetPagePermMasterNo/", pageSize, pageIndex, where);
        }
        public System.Data.DataTable GetHistoryById(string id)
        {
            return new clsResuftAPI().GetTableHistory("api/PermHistory/GetByIdPermMasterNoBk", id);
        }
        public bool RestoreRecode(string id, string version, string idUser)
        {
            return new clsResuftAPI().RestoreRecord("api/RestorePerm/RestorePermMasterNo", id, version, idUser);
        }
        public System.Data.DataTable GetTableDelete(string where)
        {
            return new clsResuftAPI().GetTableObj("api/PermMasterNo/GetRecordDeleted", where);
        }
    }
}
