using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prjInfo;

namespace prjBusinessLogic
{
    public class ReportStaticDAL
    {
        public List<ReportStatic> GetAllReportStatic()
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/ReportStatic/GetAllReportStatic");
        }
        public List<ReportNameStatic> GetAllReportNameStatic()
        {
            return new clsResuftAPI<ReportNameStatic>().GetListObj("api/ReportStatic/GetAllReportNameStatic");
        }
        public List<ReportStatic> GetPageReportStatic(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI<ReportStatic>().GetListObj("api/ReportStatic/GetPageReportStatic/", pageSize, pageIndex, where);
        }
        public List<ReportStatic> GetPageReportStaticExport(string where)
        {
            return new clsResuftAPI<ReportStatic>().GetListObjExportData("api/ReportStatic/GetPageReportStaticExport", where);
        }
        public ReportStatic GetReportStaticById(string id)
        {
            return new clsResuftAPI<ReportStatic>().GetOneObj(id, "api/ReportStatic/GetByIdReportStatic/");
        }

        public bool InsertReportStatic(ReportStatic obj)
        {
            return new clsResuftAPI<ReportStatic>().InsertObj(obj, "api/ReportStatic/CreateReportStatic");
        }
        public bool UpdateReportStatic(ReportStatic obj)
        {
            return new clsResuftAPI<ReportStatic>().UpdateObj(obj, "api/ReportStatic/UpdateReportStatic");
        }
        public bool DeleteReportStatic(int id)
        {
            return new clsResuftAPI<ReportStatic>().DeleteObj(id.ToString(), "api/ReportStatic/DeleteReportStatic/");
        }

        public System.Data.DataTable GetTableObject()
        {
            return new clsResuftAPI().GetTableObj("api/ReportStatic/GetAllReportStatic");
        }
        public System.Data.DataTable GetTableObject(int pageSize, int pageIndex, string where)
        {
            return new clsResuftAPI().GetTableObj("api/ReportStatic/GetPageReportStatic/", pageSize, pageIndex, where);
        }
    }
}
