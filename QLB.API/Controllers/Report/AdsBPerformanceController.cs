using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    public class AdsBPerformanceController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetData(AdsBPerformanceRequest request)
        {
            return new AdsBPerformanceRepository().GetData(request);
        }

        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetOperators(AdsBPerformanceRequest request)
        {
            return new AdsBPerformanceRepository().GetOperators(request);
        }
    }
}
