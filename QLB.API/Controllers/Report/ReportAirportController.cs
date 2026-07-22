using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class ChartReportAirportController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetData(ReportAirportRangeRequest request) { return new ReportAirportRepository().GetChartData(request); }
    }

    public class AirportTakeoffLandingController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetSummary(ReportAirportSummaryRequest request) { return new ReportAirportRepository().GetSummary(request); }
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetDetails(ReportAirportDetailsRequest request) { return new ReportAirportRepository().GetDetails(request); }
    }
}
