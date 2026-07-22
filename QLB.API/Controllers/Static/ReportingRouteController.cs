using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class ReportingRouteController : ApiController
    {
        ReportingRouteRepository ReportingRouteRepository = null;

        public ReportingRouteController()
        {
            if (ReportingRouteRepository == null)
            {
                ReportingRouteRepository = new ReportingRouteRepository();
            }
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageReportingRoute(int page_size, int page_index,string where)
        {
            return ReportingRouteRepository.GetPageReportingRoute(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageReportingSector(int page_size, int page_index, string where)
        {
            return ReportingRouteRepository.GetPageReportingSector(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetPageReportingRouteExport(string where)
        {
            return ReportingRouteRepository.GetPageReportingRouteExport(where);
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetAllReportingRoute()
        {
            return ReportingRouteRepository.GetAllReportingRoute();
        }

        [AcceptVerbs("Get")]
        public ReponseEntity GetByIdReportingRoute(int Id)
        {
            return ReportingRouteRepository.GetByIdReportingRoute(Id);
        }
        [AcceptVerbs("Post")]
        public ReponseEntity CreateReportingRoute(ReportingRoute oREPORTINGROUTE)
        {
            return ReportingRouteRepository.CreateReportingRoute(oREPORTINGROUTE);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateReportingRoute(ReportingRoute oREPORTINGROUTE)
        {
            return ReportingRouteRepository.UpdateReportingRoute(oREPORTINGROUTE);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteReportingRoute(int Id)
        {
            return ReportingRouteRepository.DeleteReportingRoute(Id);
        }

        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteReportingSector(int Id)
        {
            return ReportingRouteRepository.DeleteReportingSector(Id);
        }
    }
}
