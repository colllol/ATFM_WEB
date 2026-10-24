using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class AirportRouteDAL
    {
        public bool InsertAirportRoute(AirportRoute obj)
        {
            return new clsResuftAPI<AirportRoute>().InsertObj(obj, "api/AirportRoute/CreateAirportRoute");
        }
        public bool UpdateAirportRoute(AirportRoute obj)
        {
            return new clsResuftAPI<AirportRoute>().UpdateObj(obj, "api/AirportRoute/UpdateAirportRoute");
        }
        public bool DeleteAirportRoute(string Id)
        {
            return new clsResuftAPI<AirportRoute>().DeleteObj(Id, "api/AirportRoute/DeleteAirportRoute");
        }

        public AirportRoute GetOneAirportRoute(int Id)
        {
            return new clsResuftAPI<AirportRoute>().GetOneObj(Id.ToString(), "api/AirportRoute/GetByIdAirportRoute");
        }

        public List<AirportRoute> GetAllAirportRoute()
        {
            return new clsResuftAPI<AirportRoute>().GetListObj("api/AirportRoute/GetAllAirportRoute");
        }

        public List<AirportRoute> GetPageAirportRoute(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<AirportRoute>().GetListObj("api/AirportRoute/GetPageAirportRoute", pageSize, pageIndex, where);
        }
        public List<AirportRoute> GetPageAirportRouteExport(string where)
        {
            return new clsResuftAPI<AirportRoute>().GetListObjExportData("api/AirportRoute/GetPageAirportRouteExport", where);
        }
    }
}
