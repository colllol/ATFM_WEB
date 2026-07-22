using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class FlightStatusRateController:ApiController { [AcceptVerbs("Post","Put")] public ReponseReportEntity GetData(FlightAnalyticsRequest r){return new FlightStatusRateRepository().GetData(r);} [AcceptVerbs("Post","Put")] public ReponseReportEntity GetOperators(FlightAnalyticsRequest r){return new FlightStatusRateRepository().GetOperators(r);} }
    public class FlightOperationOverviewController:ApiController { [AcceptVerbs("Post","Put")] public ReponseReportEntity GetData(FlightAnalyticsRequest r){return new FlightOperationOverviewRepository().GetData(r);} }
    public class FlightTrendAnalysisController:ApiController { [AcceptVerbs("Post","Put")] public ReponseReportEntity GetTrend(FlightAnalyticsRequest r){return new FlightTrendAnalysisRepository().GetTrend(r);} [AcceptVerbs("Post","Put")] public ReponseReportEntity GetOperators(FlightAnalyticsRequest r){return new FlightTrendAnalysisRepository().GetOperators(r);} }
    public class CivilFlightSummaryController:ApiController { [AcceptVerbs("Post","Put")] public ReponseReportEntity GetData(FlightAnalyticsRequest r){return new CivilFlightSummaryRepository().GetData(r);} }
    public class MilitaryFlightReportController:ApiController { [AcceptVerbs("Post","Put")] public ReponseReportEntity GetData(FlightAnalyticsRequest r){return new MilitaryFlightReportRepository().GetData(r);} }
    public class ChartReportController:ApiController { [AcceptVerbs("Post","Put")] public ReponseReportEntity GetData(FlightAnalyticsRequest r){return new ChartReportRepository().GetData(r);} }
}
