using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class AeroDAL
    {
        public List<Aero> GetListAll()
        {
            return new clsResuftAPI<Aero>().GetListObj("api/Aero/GetAllAero");
        }
        public List<Aero> GetListAeroVV()
        {
            return new clsResuftAPI<Aero>().GetListObj("api/Aero/GetAeroVV");
        }
        public List<Aero> GetListPage(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Aero>().GetListObj("api/Aero/GetPageAero/", pageSize, pageIndex, where);
        }
        public List<Aero> GetListPageExport(string where)
        {
            return new clsResuftAPI<Aero>().GetListObjExportData("api/Aero/GetPageAeroExport", where);
        }
        public Aero GetAeroById(string id)
        {
            return new clsResuftAPI<Aero>().GetOneObj(id, "api/Aero/GetByIdAero/");
        }

        public bool InsertAero(Aero obj)
        {
            return new clsResuftAPI<Aero>().InsertObj(obj, "api/Aero/CreateAero");
        }
        public bool UpdateAero(Aero obj)
        {
            return new clsResuftAPI<Aero>().UpdateObj(obj, "api/Aero/UpdateAero");
        }
        public bool DeleteAero(int id)
        {
            return new clsResuftAPI<Aero>().DeleteObj(id.ToString(), "api/Aero/DeleteAero/");
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/Aero/GetAllAero");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Aero/GetPageAero/", pageSize, pageIndex, where);
        }
    }
}
