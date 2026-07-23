using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace QLB.API.Filters
{
    public sealed class ApiKeyAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private const string HeaderName = "X-API-Key";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            string configuredKey = Environment.GetEnvironmentVariable("ATFM_API_KEY");
            if (string.IsNullOrWhiteSpace(configuredKey))
                configuredKey = ConfigurationManager.AppSettings["APIKey"];
            string suppliedKey = null;
            System.Collections.Generic.IEnumerable<string> values;

            if (string.IsNullOrWhiteSpace(configuredKey))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.ServiceUnavailable, "API authentication is not configured.");
                return;
            }

            if (actionContext.Request.Headers.TryGetValues(HeaderName, out values))
                suppliedKey = values.FirstOrDefault();

            if (!FixedTimeEquals(configuredKey, suppliedKey))
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Invalid API credentials.");
            }
        }

        private static bool FixedTimeEquals(string expected, string actual)
        {
            if (actual == null) return false;

            byte[] expectedHash;
            byte[] actualHash;
            using (SHA256 sha = SHA256.Create())
            {
                expectedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(expected));
                actualHash = sha.ComputeHash(Encoding.UTF8.GetBytes(actual));
            }

            int difference = expectedHash.Length ^ actualHash.Length;
            for (int i = 0; i < expectedHash.Length && i < actualHash.Length; i++)
                difference |= expectedHash[i] ^ actualHash[i];
            return difference == 0;
        }
    }
}
