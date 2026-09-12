using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic.DAL
{
    public class AirlinesDAL
    {
        public bool InsertAirlines(Airlines obj)
        {
            return new clsResuftAPI<Airlines>().InsertObj(obj, "api/Airlines/CreateAirlines");
        }
        public bool UpdateAirlines(Airlines obj)
        {
            return new clsResuftAPI<Airlines>().UpdateObj(obj, "api/Airlines/UpdateAirlines");
        }
        public bool DeleteAirlines(string id)
        {
            return new clsResuftAPI<Airlines>().DeleteObj(id, "api/Airlines/DeleteAirlines/");
        }

        public Airlines GetOneAirlines(string id)
        {
            return new clsResuftAPI<Airlines>().GetOneObj(id, "api/Airlines/GetByIdAirlines/");
        }
        public List<Airlines> GetAllAirlines()
        {
            return new clsResuftAPI<Airlines>().GetListObj("api/Airlines/GetAllAirlines");
        }
        public List<Airlines> GetPageAirlines(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Airlines>().GetListObj("api/Airlines/GetPageAirlines/", pageSize, pageIndex, where);
        }
        public List<Airlines> GetPageAirlinesExport(string where)
        {
            return new clsResuftAPI<Airlines>().GetListObjExportData("api/Airlines/GetPageAirlinesExport", where);
        }

        public System.Data.DataTable GetTableAirlines()
        {
            return new clsResuftAPI().GetTableObj("api/Airlines/GetAllAirlines");
        }
        public System.Data.DataTable GetTableAirlines(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Airlines/GetPageAirlines/", pageSize, pageIndex, where);
        }
    }
}
