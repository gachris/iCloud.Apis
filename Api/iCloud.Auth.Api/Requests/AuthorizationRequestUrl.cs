using iCloud.Apis.Core.Services;
using System;
using System.Net;

namespace iCloud.Apis.Auth.Requests
{
    /// <summary>
    /// OAuth 2.0 request URL for an authorization web page to allow the end user to authorize the application to
    /// access their protected resources, as specified in http://tools.ietf.org/html/rfc6749#section-3.1.
    /// </summary>
    public class AuthorizationRequestUrl
    {
        private readonly Uri authorizationServerUrl;

        /// <summary>
        /// Gets or sets the response type which must be <c>code</c> for requesting an authorization code or
        /// <c>token</c> for requesting an access token (implicit grant), or space separated registered extension
        /// values. See http://tools.ietf.org/html/rfc6749#section-3.1.1 for more details
        /// </summary>
        [RequestParameter("response_type", RequestParameterType.Query)]
        public string ResponseType { get; set; }

        /// <summary>Gets or sets the client identifier.</summary>
        [RequestParameter("client_id", RequestParameterType.Query)]
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the URI that the authorization server directs the resource owner's user-agent back to the
        /// client after a successful authorization grant, as specified in
        /// http://tools.ietf.org/html/rfc6749#section-3.1.2 or <c>null</c> for none.
        /// </summary>
        [RequestParameter("redirect_uri", RequestParameterType.Query)]
        public string RedirectUri { get; set; }

        /// <summary>
        /// Gets or sets space-separated list of scopes, as specified in http://tools.ietf.org/html/rfc6749#section-3.3
        /// or <c>null</c> for none.
        /// </summary>
        [RequestParameter("credentials", RequestParameterType.Query)]
        public NetworkCredential Credentials { get; set; }

        /// <summary>
        /// Gets or sets the state (an opaque value used by the client to maintain state between the request and
        /// callback, as mentioned in http://tools.ietf.org/html/rfc6749#section-3.1.2.2 or <c>null</c> for none.
        /// </summary>
        [RequestParameter("state", RequestParameterType.Query)]
        public string State { get; set; }

        /// <summary>Gets the authorization server URI.</summary>
        public Uri AuthorizationServerUrl
        {
            get
            {
                return this.authorizationServerUrl;
            }
        }

        /// <summary>Constructs a new authorization request with the specified URI.</summary>
        /// <param name="authorizationServerUrl">Authorization server URI</param>
        public AuthorizationRequestUrl(Uri authorizationServerUrl)
        {
            this.authorizationServerUrl = authorizationServerUrl;
        }
    }
}
