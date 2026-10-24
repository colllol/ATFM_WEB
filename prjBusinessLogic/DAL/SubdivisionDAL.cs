using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;


namespace prjBusinessLogic.DAL
{
    public class SubdivisionDAL
    {
        public bool InsertSubdivision(Subdivision obj)
        {
            return new clsResuftAPI<Subdivision>().InsertObj(obj, "api/Subdivision/CreateSubdivision");
        }
        public bool UpdateSubdivision(Subdivision obj)
        {
            return new clsResuftAPI<Subdivision>().UpdateObj(obj, "api/Subdivision/UpdateSubdivision");
        }
        public bool DeleteSubdivision(string id)
        {
            return new clsResuftAPI<Subdivision>().DeleteObj(id, "api/Subdivision/DeleteSubdivision/");
        }

        public Subdivision GetOneSubdivision(string id)
        {
            return new clsResuftAPI<Subdivision>().GetOneObj(id, "api/Subdivision/GetByIdSubdivision/");
        }
        public List<Subdivision> GetAllSubdivision()
        {
            return new clsResuftAPI<Subdivision>().GetListObj("api/Subdivision/GetAllSubdivision");
        }
        public List<Subdivision> GetPageSubdivision(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<Subdivision>().GetListObj("api/Subdivision/GetPageSubdivision/", pageSize, pageIndex, where);
        }

        public List<Subdivision> GetPageSubdivisionExport(string where)
        {
            return new clsResuftAPI<Subdivision>().GetListObjExportData("api/Subdivision/GetPageSubdivisionExport/", where);
        }

        public System.Data.DataTable GetTableSubdivision()
        {
            return new clsResuftAPI().GetTableObj("api/Subdivision/GetAllSubdivision");
        }
        public System.Data.DataTable GetTableSubdivision(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/Subdivision/GetPageSubdivision/", pageSize, pageIndex, where);
        }
    }
}
