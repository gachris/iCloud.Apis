using iCloud.Apis.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iCloud.Apis.Auth.Requests
{
    /// <summary>
    ///  OAuth 2.0 request for an access token using an authorization code as specified in
    ///  http://tools.ietf.org/html/rfc6749#section-4.1.3.
    /// </summary>
    public class AuthorizationCodeTokenRequest : TokenRequest
    {
        /// <summary>Gets or sets the authorization code received from the authorization server.</summary>
        [RequestParameter("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the redirect URI parameter matching the redirect URI parameter in the authorization request.
        /// </summary>
        [RequestParameter("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// Constructs a new authorization code token request and sets grant_type to <c>authorization_code</c>.
        /// </summary>
        public AuthorizationCodeTokenRequest()
        {
            this.GrantType = "authorization_code";
        }
    }
}
