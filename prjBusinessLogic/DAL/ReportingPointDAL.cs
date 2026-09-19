using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class ReportingPointDAL
    {
        public bool InsertReportingPoint(ReportingPoint obj)
        {
            return new clsResuftAPI<ReportingPoint>().InsertObj(obj, "api/ReportingPoint/CreateReportingPoint");
        }
        public bool UpdateReportingPoint(ReportingPoint obj)
        {
            return new clsResuftAPI<ReportingPoint>().UpdateObj(obj, "api/ReportingPoint/UpdateReportingPoint");
        }
        public bool DeleteReportingPoint(string id)
        {
            return new clsResuftAPI<ReportingPoint>().DeleteObj(id, "api/ReportingPoint/DeleteReportingPoint/");
        }

        public ReportingPoint GetOneReportingPoint(string id)
        {
            return new clsResuftAPI<ReportingPoint>().GetOneObj(id, "api/ReportingPoint/GetByIdReportingPoint/");
        }
        public List<ReportingPoint> GetAddReportingPoint(string Route_Id)
        {
            return new clsResuftAPI<ReportingPoint>().GetListObjAdd(Route_Id, "api/ReportingPoint/GetAddReportingPoint");
        }

        public List<Sectors> GetAddSectorPoint(string Route_Id)
        {
            return new clsResuftAPI<Sectors>().GetListObjAdd(Route_Id, "api/ReportingPoint/GetAddSectorPoint");
        }

        public List<ReportingPoint> GetAllReportingPoint()
        {
            return new clsResuftAPI<ReportingPoint>().GetListObj("api/ReportingPoint/GetAllReportingPoint");
        }
        public List<ReportingPoint> GetPageReportingPoint(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<ReportingPoint>().GetListObj("api/ReportingPoint/GetPageReportingPoint", pageSize, pageIndex, where);
        }

        public List<ReportingPoint> GetPageReportingPointExport(string where)
        {
            return new clsResuftAPI<ReportingPoint>().GetListObjExportData("api/ReportingPoint/GetPageReportingPointExport/", where);
        }

        public System.Data.DataTable GetTableReportingPoint()
        {
            return new clsResuftAPI().GetTableObj("api/ReportingPoint/GetAllReportingPoint");
        }
        public System.Data.DataTable GetTableReportingPoint(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/ReportingPoint/GetPageReportingPoint/", pageSize, pageIndex, where);
        }
    }
}
