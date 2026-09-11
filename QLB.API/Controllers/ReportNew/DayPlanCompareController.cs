using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    // API cho trang ReportNew/FlightPlanDailyComparison.aspx
    // (so sanh ke hoach bay ngay voi cung ky tuan truoc).
    // Nguon: DAYPLAN_COMPARE_PKG.
    public class DayPlanCompareController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetDayFlights(DayPlanCompareRequest request)
        {
            return new ReportNewRepository().GetDayFlights(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetAirports()
        {
            return new ReportNewRepository().GetDayPlanAirports();
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetOperators()
        {
            return new ReportNewRepository().GetDayPlanOperators();
        }
    }
}
