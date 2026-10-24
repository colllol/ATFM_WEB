using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class ConfigDAL
    {
        public List<Config> GetListAll()
        {
            return new clsResuftAPI<Config>().GetListObj("api/Config/GetAllConfig");
        }

        public List<Config> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Config>().GetListObj("api/Config/GetPageConfig/", pageSize, pageIndex, where);
        }
        public List<Config> GetListPageExport(string where)
        {
            return new clsResuftAPI<Config>().GetListObjExportData("api/Config/GetPageConfigExport", where);
        }
        public Config GetConfigById(string id)
        {
            return new clsResuftAPI<Config>().GetOneObj(id, "api/Config/GetByIdConfig/");
        }

        public bool InsertConfig(Config obj)
        {
            return new clsResuftAPI<Config>().InsertObj(obj, "api/Config/CreateConfig");
        }
        public bool UpdateConfig(Config obj)
        {
            return new clsResuftAPI<Config>().UpdateObj(obj, "api/Config/UpdateConfig");
        }
        public bool DeleteConfig(int id)
        {
            return new clsResuftAPI<Config>().DeleteObj(id.ToString(), "api/Config/DeleteConfig");
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Config/GetAllConfig");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Config/GetPageConfig/", pageSize, pageIndex, where);
        }
    }
}
