using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class SlotsTableController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetData(SlotsTableRequest request) { return new SlotsRepository().GetTableData(request); }
    }

    public class KhhController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetData(SlotsTableRequest request) { request = request ?? new SlotsTableRequest(); request.Source = "KHH"; return new SlotsRepository().GetTableData(request); }
    }

    public class SlotAeroController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetData(SlotsTableRequest request) { request = request ?? new SlotsTableRequest(); request.Source = "SLOT_AERO"; return new SlotsRepository().GetTableData(request); }
    }
}
