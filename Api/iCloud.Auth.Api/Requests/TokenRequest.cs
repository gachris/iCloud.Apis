using iCloud.Apis.Core.Services;

namespace iCloud.Apis.Auth.Requests
{
    /// <summary>
    /// OAuth 2.0 request for an access token as specified in http://tools.ietf.org/html/rfc6749#section-4.
    /// </summary>
    public class TokenRequest
    {
        /// <summary>
        /// Gets or sets space-separated list of scopes as specified in http://tools.ietf.org/html/rfc6749#section-3.3.
        /// </summary>
        [RequestParameter("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Gets or sets the Grant type. Sets <c>authorization_code</c> or <c>password</c> or <c>client_credentials</c>
        /// or <c>refresh_token</c> or absolute URI of the extension grant type.
        /// </summary>
        [RequestParameter("grant_type")]
        public string GrantType { get; set; }

        /// <summary>Gets or sets the client Identifier.</summary>
        [RequestParameter("client_id")]
        public string ClientId { get; set; }

        /// <summary>Gets or sets the client Secret.</summary>
        [RequestParameter("client_secret")]
        public string ClientSecret { get; set; }
    }
}
