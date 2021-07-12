using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Core.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    public class UserCredential : ICredential, IConfigurableHttpClientInitializer, ITokenAccess, IHttpExecuteInterceptor, IHttpUnsuccessfulResponseHandler
    {
        protected static readonly ILogger Logger = ApplicationContext.Logger.ForType<UserCredential>();
        private readonly object lockObject = new object();
        private TokenResponse token;
        private readonly IAuthorizationCodeFlow flow;
        private readonly string userId;

        /// <summary>Gets or sets the token response which contains the access token.</summary>
        public TokenResponse Token
        {
            get
            {
                lock (this.lockObject)
                    return this.token;
            }
            set
            {
                lock (this.lockObject)
                    this.token = value;
            }
        }

        /// <summary>Gets the user identity.</summary>
        public string UserId
        {
            get
            {
                return this.userId;
            }
        }

        /// <summary>Constructs a new credential instance.</summary>
        /// <param name="flow">Authorization code flow.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">An initial token for the user.</param>
        public UserCredential(IAuthorizationCodeFlow flow, string userId, TokenResponse token)
        {
            this.flow = flow;
            this.userId = userId;
            this.token = token;
        }

        /// <summary>
        /// Default implementation is to try to refresh the access token if there is no access token or if we are 1
        /// minute away from expiration. If token server is unavailable, it will try to use the access token even if
        /// has expired. If successful, it will call <see cref="M:ICloud.Apis.Auth.OAuth2.IAccessMethod.Intercept(System.Net.Http.HttpRequestMessage,System.String)" />.
        /// </summary>
        public async Task InterceptAsync(HttpRequestMessage request, CancellationToken taskCancellationToken)
        {
            this.GetAccessTokenForRequestAsync(request.RequestUri.ToString(), taskCancellationToken);
            this.flow.AccessMethod.Intercept(request, this.Token.AccessToken);
#if net40
            await TaskEx.Delay(0);
#endif
#if others_frameworks
            await Task.Delay(0);
#endif
        }

        public async Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
        {
            if (args.Response.StatusCode != HttpStatusCode.Unauthorized)
                return false;
            bool flag = !object.Equals(Token.AccessToken, this.flow.AccessMethod.GetAccessToken(args.Request));
#if net40
            await TaskEx.Delay(0);
#endif
#if others_frameworks
            await Task.Delay(0);
#endif
            return flag;
        }

        public void Initialize(ConfigurableHttpClient httpClient)
        {
            httpClient.MessageHandler.AddExecuteInterceptor(this);
            httpClient.MessageHandler.AddUnsuccessfulResponseHandler(this);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Token.AccessToken);
        }

        public virtual string GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default)
        {
            if (this.Token != null)
                return this.token.AccessToken;
            else return null;
        }

        /// <summary>
        /// Asynchronously revokes the token by calling
        /// <see cref="M:ICloud.Apis.Auth.OAuth2.Flows.IAuthorizationCodeFlow.RevokeTokenAsync(System.String,System.String,System.Threading.CancellationToken)" />.
        /// </summary>
        /// <param name="taskCancellationToken">Cancellation token to cancel an operation.</param>
        /// <returns><c>true</c> if the token was revoked successfully.</returns>
        public async Task<bool> RevokeTokenAsync(CancellationToken taskCancellationToken)
        {
            if (this.Token == null)
            {
                UserCredential.Logger.Warning("Token is already null, no need to revoke it.");
                return false;
            }
            await this.flow.RevokeTokenAsync(this.userId, this.Token.AccessToken, taskCancellationToken).ConfigureAwait(false);
            UserCredential.Logger.Info("Access token was revoked successfully");
            return true;
        }
    }
}