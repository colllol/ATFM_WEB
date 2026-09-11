using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    // API cho trang SLOTS/SlotComparison.aspx (phan doc du lieu so sanh slot).
    // Nguon: SLOT_COMPARE_PKG. Logic so sanh va luu ket qua van o SlotComparisonService.
    public class SlotCompareController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetDefaultDate()
        {
            return new ReportNewRepository().GetSlotDefaultDate();
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetOperators()
        {
            return new ReportNewRepository().GetSlotOperators();
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetSummary(SlotCompareRequest request)
        {
            return new ReportNewRepository().GetSlotSummary(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetResults(SlotCompareRequest request)
        {
            return new ReportNewRepository().GetSlotResults(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetSourceKhh(SlotCompareRequest request)
        {
            return new ReportNewRepository().GetSlotSourceKhh(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetSourceSlot(SlotCompareRequest request)
        {
            return new ReportNewRepository().GetSlotSourceSlot(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetSourcePerm(SlotCompareRequest request)
        {
            return new ReportNewRepository().GetSlotSourcePerm(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetOperatorMap()
        {
            return new ReportNewRepository().GetSlotOperatorMap();
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetAirportMap()
        {
            return new ReportNewRepository().GetSlotAirportMap();
        }
    }
}
