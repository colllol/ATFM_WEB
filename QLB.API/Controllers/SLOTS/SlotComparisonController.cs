using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class SlotComparisonController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetBootstrap() { return new SlotsRepository().GetComparisonBootstrap(); }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetResults(SlotComparisonRequest request) { return new SlotsRepository().GetComparisonResults(request); }
    }
}
