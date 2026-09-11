using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    // API cho cac trang ReportNew dung chung bo phan loai trang thai chuyen bay:
    //   FlightStatusRate.aspx, FlightOperationOverview.aspx,
    //   AirportTakeoffLanding.aspx, FlightTrendAnalysis.aspx.
    // Nguon: FLIGHT_STATUS_PKG.
    public class FlightStatusController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetStatus(FlightStatusRequest request)
        {
            return new ReportNewRepository().GetStatus(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetCancelled(DateRangeRequest request)
        {
            return new ReportNewRepository().GetCancelled(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetOperators(DateRangeRequest request)
        {
            return new ReportNewRepository().GetStatusOperators(request);
        }
    }
}
