using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public IHttpActionResult GetState(long userId)
        {
            if (userId <= 0)
                return Error(HttpStatusCode.BadRequest, "Tài khoản không hợp lệ.");

            return Execute(() =>
            {
                AiNotificationService.QueueSynchronization();
                EmailNotificationService.TrySynchronize();
                return repository.GetState(userId);
            });
        }

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public IHttpActionResult GetPage(long userId, int status = -1, int page = 1, string source = "ALL")
        {
            source = NormalizeSource(source);
            if (userId <= 0 || (status != -1 && status != 0 && status != 1)
                || page < 1 || page > 1000000 || source == null)
                return Error(HttpStatusCode.BadRequest, "Tài khoản, bộ lọc hoặc trang không hợp lệ.");

            return Execute(() =>
            {
                AiNotificationService.QueueSynchronization();
                EmailNotificationService.TrySynchronize();
                return repository.GetPage(userId, status, page, source);
            });
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public IHttpActionResult MarkRead(long userId, long id)
        {
            if (userId <= 0 || id <= 0)
                return Error(HttpStatusCode.BadRequest, "Tài khoản hoặc mã thông báo không hợp lệ.");

            return Execute(() => repository.MarkRead(userId, id));
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public IHttpActionResult MarkAllRead(long userId, string source = "ALL")
        {
            source = NormalizeSource(source);
            if (userId <= 0 || source == null)
                return Error(HttpStatusCode.BadRequest, "Tài khoản hoặc nguồn thông báo không hợp lệ.");

            return Execute(() => repository.MarkAllRead(userId, source));
        }

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public IHttpActionResult GetAiJob(long userId, long id)
        {
            if (userId <= 0 || id <= 0)
                return Error(HttpStatusCode.BadRequest, "Tài khoản hoặc mã thông báo không hợp lệ.");

            return Execute(() => new { Code = "00", Job = repository.GetAiJob(userId, id, false) });
        }

        [AcceptVerbs("Get")]
        [ApiKeyAuthorize]
        public IHttpActionResult GetAiResult(long userId, long id)
        {
            if (userId <= 0 || id <= 0)
                return Error(HttpStatusCode.BadRequest, "Tài khoản hoặc mã thông báo không hợp lệ.");

            return Execute(() => new { Code = "00", Result = repository.GetAiJob(userId, id, true) });
        }

        [AcceptVerbs("Post")]
        [ApiKeyAuthorize]
        public IHttpActionResult Create(NotificationCreateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title)
                || string.IsNullOrWhiteSpace(request.Content)
                || request.Title.Length > 250 || request.Content.Length > 2000
                || (request.UserIds != null && request.UserIds.Any(id => id <= 0)))
                return Error(HttpStatusCode.BadRequest, "Nội dung thông báo không hợp lệ.");

            string userIds = request.UserIds == null || request.UserIds.Count == 0
                ? null
                : string.Join(",", request.UserIds.Distinct());
            return Execute(() => repository.Create(request.Title.Trim(), request.Content.Trim(), userIds));
        }

        private IHttpActionResult Execute(Func<object> action)
        {
            try
            {
                object value = action();
                ReponseReportEntity response = value as ReponseReportEntity;
                if (response != null && response.Code != "00")
                    return Error(HttpStatusCode.InternalServerError, "Không thể xử lý dữ liệu thông báo.");
                return Ok(value);
            }
            catch (AiIntegrationException ex)
            {
                return Error((HttpStatusCode)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Trace.TraceError("[NotificationsController] " + ex.GetType().Name);
                return Error(HttpStatusCode.InternalServerError, "Không thể xử lý dữ liệu thông báo.");
            }
        }

        private IHttpActionResult Error(HttpStatusCode statusCode, string message)
        {
            return Content(statusCode, new { Code = "-99", Message = message });
        }

        private static string NormalizeSource(string source)
        {
            string value = (source ?? "ALL").Trim().ToUpperInvariant();
            return value == "ALL" || value == "AI_QUERY" ? value : null;
        }
    }
}
