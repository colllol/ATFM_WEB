using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;

namespace QLB.API.Controllers
{
    // API cho trang ReportNew/AnomalyWarning.aspx (canh bao delay hom nay).
    // Nguon: DELAY_ALERT_PKG.
    public class DelayAlertController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetTodayFlights()
        {
            return new ReportNewRepository().GetTodayDelayFlights();
        }
    }
}
