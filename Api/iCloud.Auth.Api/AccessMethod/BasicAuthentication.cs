using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace iCloud.Apis.Auth
{
    /// <summary>
    /// OAuth 2.0 helper for accessing protected resources using the Bearer token as specified in
    /// http://tools.ietf.org/html/rfc6750.
    /// </summary>
    public class BasicAuthentication
    {
        /// <summary>
        /// Thread-safe OAuth 2.0 method for accessing protected resources using the Authorization header as specified
        /// in http://tools.ietf.org/html/rfc6750#section-2.1.
        /// </summary>
        public class AuthorizationHeaderAccessMethod : IAccessMethod
        {
            private const string Schema = "Basic";

            public void Intercept(HttpRequestMessage request, string accessToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", accessToken);
            }

            public string GetAccessToken(HttpRequestMessage request)
            {
                if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Basic")
                    return request.Headers.Authorization.Parameter;
                return null;
            }
        }
    }
}
