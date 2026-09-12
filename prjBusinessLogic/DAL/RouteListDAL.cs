using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;


namespace prjBusinessLogic
{
    public class RouteListDAL
    {
        public bool InsertRouteList(RouteList obj)
        {
            return new clsResuftAPI<RouteList>().InsertObj(obj, "api/RouteList/CreateRouteList");
        }
        public bool UpdateRouteList(RouteList obj)
        {
            return new clsResuftAPI<RouteList>().UpdateObj(obj, "api/RouteList/UpdateRouteList");
        }
        public bool DeleteRouteList(string id)
        {
            return new clsResuftAPI<RouteList>().DeleteObj(id, "api/RouteList/DeleteRouteList/");
        }

        public RouteList GetOneRouteList(int id)
        {
            return new clsResuftAPI<RouteList>().GetOneObj(id.ToString(), "api/RouteList/GetByIdRouteList/");
        }
        public List<RouteList> GetAllRouteList()
        {
            return new clsResuftAPI<RouteList>().GetListObj("api/RouteList/GetAllRouteList");
        }
        public List<RouteList> GetPageRouteList(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<RouteList>().GetListObj("api/RouteList/GetPageRouteList/", pageSize, pageIndex, where);
        }

        public List<RouteList> GetPageRouteListExport(string where)
        {
            return new clsResuftAPI<RouteList>().GetListObjExportData("api/RouteList/GetPageRouteListExport/", where);
        }

        public System.Data.DataTable GetTableRouteList()
        {
            return new clsResuftAPI().GetTableObj("api/RouteList/GetAllRouteList");
        }
        public System.Data.DataTable GetTableRouteList(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/RouteList/GetPageRouteList/", pageSize, pageIndex, where);
        }
    }
}
