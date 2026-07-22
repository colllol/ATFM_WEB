using System;
using QLB.API.Models;
using QLB.BusinessLogic;
using QLB.Info;

namespace QLB.API.Data
{
    public class ReportAirportRepository
    {
        public ReponseReportEntity GetChartData(ReportAirportRangeRequest request) { return Execute(() => new ReportAirportDAL().GetChartData(request)); }
        public ReponseReportEntity GetSummary(ReportAirportSummaryRequest request) { return Execute(() => new ReportAirportDAL().GetSummary(request)); }
        public ReponseReportEntity GetDetails(ReportAirportDetailsRequest request) { return Execute(() => new ReportAirportDAL().GetDetails(request)); }
        private static ReponseReportEntity Execute(Func<object> action) { try { return new ReponseReportEntity { Code="00", Message="Lấy dữ liệu thành công", ListValue=action() }; } catch(ArgumentException ex) { return new ReponseReportEntity { Code="-1", Message=ex.Message }; } catch(Exception) { return new ReponseReportEntity { Code="-99", Message="Có lỗi trong quá trình lấy dữ liệu" }; } }
    }
}
