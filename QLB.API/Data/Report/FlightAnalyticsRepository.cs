using System;
using QLB.API.Models;
using QLB.BusinessLogic;
using QLB.Info;

namespace QLB.API.Data
{
    internal static class FlightAnalyticsResponse
    {
        internal static ReponseReportEntity Run(Func<object> action)
        {
            try { return new ReponseReportEntity { Code="00", Message="Lấy dữ liệu thành công", ListValue=action() }; }
            catch (ArgumentException ex) { return new ReponseReportEntity { Code="-1", Message=ex.Message }; }
            catch (Exception) { return new ReponseReportEntity { Code="-99", Message="Có lỗi trong quá trình lấy dữ liệu" }; }
        }
    }
    public class FlightStatusRateRepository { public ReponseReportEntity GetData(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetStatus(r));} public ReponseReportEntity GetOperators(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetOperators(r));} }
    public class FlightOperationOverviewRepository { public ReponseReportEntity GetData(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetOverview(r));} }
    public class FlightTrendAnalysisRepository { public ReponseReportEntity GetTrend(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetTrend(r));} public ReponseReportEntity GetOperators(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetOperators(r));} }
    public class CivilFlightSummaryRepository { public ReponseReportEntity GetData(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetCivil(r));} }
    public class MilitaryFlightReportRepository { public ReponseReportEntity GetData(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetMilitary(r));} }
    public class ChartReportRepository { public ReponseReportEntity GetData(FlightAnalyticsRequest r){return FlightAnalyticsResponse.Run(()=>new FlightAnalyticsDAL().GetDashboard(r));} }
}
