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

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public ReponseReportEntity GetPage(int status = -1, int page = 1)
        {
            if ((status != -1 && status != 0 && status != 1) || page < 1 || page > 1000000)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return repository.GetPage(status, page);
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public ReponseReportEntity MarkRead(long id)
        {
            if (id <= 0)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return repository.MarkRead(id);
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public ReponseReportEntity MarkAllRead()
        {
            return repository.MarkAllRead();
        }
    }
}
