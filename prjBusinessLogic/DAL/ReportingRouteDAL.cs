using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;
using System.Net.Http;
using System.Net.Http.Headers;

namespace prjBusinessLogic.DAL
{
    public class ReportingRouteDAL
    {
        public ReportingRoute GetOneRoute(int id)
        {
            return new clsResuftAPI<ReportingRoute>().GetOneObj(id.ToString(), "api/RouteList/GetByIdRoutePoint/");
        }
        public bool InsertReportingRoute(ReportingRoute obj)
        {
            return new clsResuftAPI<ReportingRoute>().InsertObj(obj, "api/ReportingRoute/CreateReportingRoute");
        }
        public bool UpdateReportingRoute(ReportingRoute obj)
        {
            return new clsResuftAPI<ReportingRoute>().UpdateObj(obj, "api/ReportingRoute/UpdateReportingRoute");
        }
        public bool DeleteReportingRoute(string id)
        {
            return new clsResuftAPI<ReportingRoute>().DeleteObj(id, "api/ReportingRoute/DeleteReportingRoute/");
        }

        public bool DeleteReportingSector(string id)
        {
            return new clsResuftAPI<ReportingRoute>().DeleteObj(id, "api/ReportingRoute/DeleteReportingSector/");
        }

        //public List<ReportingRoute> GetOneObject(string id)
        //{
        //    return new clsResuftAPI<ReportingRoute>().GetListObjAdd(id, "api/ReportingRoute/GetByIdReportingRoute/");
        //}
        public List<ReportingRoute> GetAllReportingRoute()
        {
            return new clsResuftAPI<ReportingRoute>().GetListObj("api/ReportingRoute/GetAllReportingRoute");
        }
        public List<ReportingRoute> GetPageReportingRoute(int pageSize, int pageIndex, string where)
        {
                return new clsResuftAPI<ReportingRoute>().GetListObj("api/ReportingRoute/GetPageReportingRoute", pageSize, pageIndex, where);       
        }

        public List<SectorRoute> GetPageReportingSector(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<SectorRoute>().GetListObj("api/ReportingRoute/GetPageReportingSector", pageSize, pageIndex, where);
        }

        public List<ReportingRoute> GetPageReportingRouteExport(string where)
        {
            return new clsResuftAPI<ReportingRoute>().GetListObjExportData("api/ReportingRoute/GetPageReportingRouteExport", where); 
        }
        public System.Data.DataTable GetTableReportingRoute()
        {
            return new clsResuftAPI().GetTableObj("api/ReportingRoute/GetAllReportingRoute");
        }
        public System.Data.DataTable GetTableReportingRoute(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/ReportingRoute/GetPageReportingRoute/", pageSize, pageIndex, where);
        }
        public bool SetPointIdIntoRouteID(string routeId, string pointId, string status)
        {
            #region code old
            //try
            //{
            //    var client = new HttpClient();
            //    client.BaseAddress = new Uri(System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = client.GetAsync($"sfsfsdf/id=sfssf").Result;
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        var re = response.Content.ReadAsAsync<dynamic>().Result;
            //        if (re.Code == "00")
            //        {
            //            //dynamic oValue = re.ListValue;
            //            var ax = re.ListValue;
            //            if (ax.ToString() == "1") return true;
            //        }
            //    }
            //    return false;
            //}
            //catch (Exception)
            //{
            //    return false;
            //}

            //var ax = new clsResuftAPI().GetValueApiExtension("DIC_PKG", "REPORTING_ROUTE_ADD_POINT", new { P_ROUTE_ID = Int64.Parse(routeId), P_POINT_ID = Int64.Parse(pointId), P_STATUS = Int64.Parse(status) }).ToString();
            //if (ax == "1") return true;
            //return false;
            #endregion

            var ax = new clsResuftAPI().GetValueApiExtension("DIC_PKG", "REPORTING_ROUTE_ADD_POINT", new { P_ROUTE_ID = routeId, P_POINT_ID = pointId, P_STATUS = Int64.Parse(status) }).ToString();
            if (ax == "1") return true;
            return false;           
        }

        public bool SetSectorIdIntoRouteID(string routeId, string sectorId, string status)
        {
            
            var ax = new clsResuftAPI().GetValueApiExtension("DIC_PKG", "REPORTING_ROUTE_ADD_SECTOR", new { P_ROUTE_ID = routeId, P_SECTOR_ID = sectorId, P_STATUS = Int64.Parse(status) }).ToString();
            if (ax == "1") return true;
            return false;
        }
    }
}
