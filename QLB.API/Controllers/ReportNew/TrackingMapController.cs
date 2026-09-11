using System.Web.Http;
using QLB.API.Data;
using QLB.API.Models;
using QLB.Info;

namespace QLB.API.Controllers
{
    // API cho trang SLOTS/FlightTrackingMap.aspx
    // (lookup metadata chuyen bay theo danh sach callsign).
    // Nguon: TRACKING_MAP_PKG.
    public class TrackingMapController : ApiController
    {
        [AcceptVerbs("Post", "Put")]
        public ReponseReportEntity GetFlightMeta(TrackingMapRequest request)
        {
            return new ReportNewRepository().GetFlightMeta(request);
        }
    }
}
