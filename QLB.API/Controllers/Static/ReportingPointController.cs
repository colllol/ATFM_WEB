using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class ReportingPointController : ApiController
    {
        ReportingPointRepository ReportingPointRepository = null;

        public ReportingPointController()
        {
            if (ReportingPointRepository == null)
            {
                ReportingPointRepository = new ReportingPointRepository();
            }
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageReportingPoint(int page_size, int page_index, string where)
        {
            return ReportingPointRepository.GetPageReportingPoint(page_size, page_index, where);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetPageReportingPointExport(string where)
        {
            return ReportingPointRepository.GetPageReportingPointExport(where);
        }

        [AcceptVerbs("Get")]


        public ReponseEntity GetAllReportingPoint()
        {
            return ReportingPointRepository.GetAllReportingPoint();
        }


        [AcceptVerbs("Get")]

        public ReponseEntity GetByIdReportingPoint(int ID)
        {

            return ReportingPointRepository.GetByIdReportingPoint(ID);
        }
        [AcceptVerbs("Get")]

        public ReponseEntity GetAddReportingPoint(int Route_Id)
        {

            return ReportingPointRepository.GetAddReportingPoint(Route_Id);
        }

        [AcceptVerbs("Get")]

        public ReponseEntity GetAddSectorPoint(int Route_Id)
        {

            return ReportingPointRepository.GetAddSectorPoint(Route_Id);
        }


        [AcceptVerbs("Post")]
        public ReponseEntity CreateReportingPoint(ReportingPoint oREPORTINGPOINT)
        {
            return ReportingPointRepository.CreateReportingPoint(oREPORTINGPOINT);
        }

        [AcceptVerbs("Put")]
        public ReponseEntity UpdateReportingPoint(ReportingPoint oREPORTINGPOINT)
        {
            return ReportingPointRepository.UpdateReportingPoint(oREPORTINGPOINT);
        }
        [AcceptVerbs("Delete")]
        public ReponseEntity DeleteReportingPoint(int ID)
        {
            return ReportingPointRepository.DeleteReportingPoint(ID);
        }
    }
}
