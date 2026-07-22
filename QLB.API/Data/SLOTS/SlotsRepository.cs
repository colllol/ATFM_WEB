using System;
using QLB.API.Models;
using QLB.BusinessLogic;
using QLB.Info;

namespace QLB.API.Data
{
    public class SlotsRepository
    {
        public ReponseReportEntity GetTableData(SlotsTableRequest request) { return Execute(() => new SlotsTableDAL().GetData(request)); }
        public ReponseReportEntity GetComparisonBootstrap() { return Execute(() => new SlotComparisonDAL().GetBootstrap()); }
        public ReponseReportEntity GetComparisonResults(SlotComparisonRequest request) { return Execute(() => new SlotComparisonDAL().GetResults(request)); }

        private static ReponseReportEntity Execute(Func<object> action)
        {
            try { return new ReponseReportEntity { Code = "00", Message = "Lấy dữ liệu thành công", ListValue = action() }; }
            catch (ArgumentException ex) { return new ReponseReportEntity { Code = "-1", Message = ex.Message }; }
            catch (Exception) { return new ReponseReportEntity { Code = "-99", Message = "Có lỗi trong quá trình lấy dữ liệu" }; }
        }
    }
}
