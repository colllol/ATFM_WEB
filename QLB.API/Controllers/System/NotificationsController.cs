using System.Web.Http;
using QLB.API.Data;
using QLB.API.Filters;
using QLB.API.Models;

namespace QLB.API.Controllers
{
    public class NotificationsController : ApiController
    {
        private readonly NotificationsRepository repository = new NotificationsRepository();

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public ReponseReportEntity GetState()
        {
            return repository.GetState();
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public ReponseReportEntity MarkAllRead()
        {
            return repository.MarkAllRead();
        }
    }
}
