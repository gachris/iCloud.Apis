using iCloud.Apis.Core.Services;
using System;
using System.Net;

namespace iCloud.Apis.Auth.Requests
{
    /// <summary>
    /// ICloud-specific implementation of the OAuth 2.0 URL for an authorization web page to allow the end user to
    /// authorize the application to access their protected resources and that returns an authorization code, as
    /// specified in https://developers.ICloud.com/accounts/docs/OAuth2WebServer.
    /// </summary>
    public class AuthorizationCodeRequestUrl : AuthorizationRequestUrl
    {
        /// <summary>
        /// Constructs a new authorization code request with the given authorization server URL. This constructor sets
        /// the <see cref="P:ICloud.Apis.Auth.OAuth2.Requests.ICloudAuthorizationCodeRequestUrl.AccessType" /> to <c>offline</c>.
        /// </summary>
        public AuthorizationCodeRequestUrl(Uri authorizationServerUrl, NetworkCredential networkCredential) : base(authorizationServerUrl)
        {
            Credentials = networkCredential;
        }
    }
}
