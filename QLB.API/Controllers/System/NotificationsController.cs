using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using QLB.API.Data;
using QLB.API.Filters;
using QLB.API.Models;

namespace QLB.API.Controllers
{
    public class NotificationCreateRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<long> UserIds { get; set; }
    }

    public class NotificationsController : ApiController
    {
        private readonly NotificationsRepository repository = new NotificationsRepository();

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public ReponseReportEntity GetState(long userId)
        {
            if (userId <= 0)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return repository.GetState(userId);
        }

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public ReponseReportEntity GetPage(long userId, int status = -1, int page = 1)
        {
            if (userId <= 0 || (status != -1 && status != 0 && status != 1)
                || page < 1 || page > 1000000)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return repository.GetPage(userId, status, page);
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public ReponseReportEntity MarkRead(long userId, long id)
        {
            if (userId <= 0 || id <= 0)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return repository.MarkRead(userId, id);
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public ReponseReportEntity MarkAllRead(long userId)
        {
            if (userId <= 0)
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            return repository.MarkAllRead(userId);
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public ReponseReportEntity Create(NotificationCreateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title)
                || string.IsNullOrWhiteSpace(request.Content)
                || request.Title.Length > 250 || request.Content.Length > 2000
                || (request.UserIds != null && request.UserIds.Any(id => id <= 0)))
                throw new HttpResponseException(global::System.Net.HttpStatusCode.BadRequest);

            string userIds = request.UserIds == null || request.UserIds.Count == 0
                ? null
                : string.Join(",", request.UserIds.Distinct());
            return repository.Create(request.Title.Trim(), request.Content.Trim(), userIds);
        }
    }
}
