using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Core.Services;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    /// <summary>
    /// Thread-safe OAuth 2.0 authorization code flow for an installed application that persists end-user credentials.
    /// </summary>
    /// <remarks>
    /// Incremental authorization (https://developers.ICloud.com/+/web/api/rest/oauth) is currently not supported
    /// for Installed Apps.
    /// </remarks>
    public class AuthorizationCodeInstalledApp : IAuthorizationCodeInstalledApp
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<AuthorizationCodeInstalledApp>();
        private readonly IAuthorizationCodeFlow flow;
        private readonly ICodeReceiver codeReceiver;

        /// <summary>
        /// Constructs a new authorization code installed application with the given flow and code receiver.
        /// </summary>
        public AuthorizationCodeInstalledApp(IAuthorizationCodeFlow flow, ICodeReceiver codeReceiver)
        {
            this.flow = flow;
            this.codeReceiver = codeReceiver;
        }

        /// <summary>Gets the authorization code flow.</summary>
        public IAuthorizationCodeFlow Flow
        {
            get { return this.flow; }
        }

        /// <summary>Gets the code receiver which is responsible for receiving the authorization code.</summary>
        public ICodeReceiver CodeReceiver
        {
            get { return this.codeReceiver; }
        }

        public async Task<UserCredential> AuthorizeAsync(string userId, NetworkCredential networdCredentials, CancellationToken taskCancellationToken)
        {
            TokenResponse token = await this.Flow.LoadTokenAsync(userId, taskCancellationToken).ConfigureAwait(false);
            if (this.ShouldRequestAuthorizationCode(token))
            {
                AuthorizationCodeResponseUrl authorizationCode = await this.CodeReceiver.ReceiveCodeAsync(this.Flow.CreateAuthorizationCodeRequest(this.CodeReceiver.RedirectUri, networdCredentials), taskCancellationToken).ConfigureAwait(false);
                if (string.IsNullOrEmpty(authorizationCode.Code))
                {
                    TokenErrorResponse error = new TokenErrorResponse(authorizationCode);
                    AuthorizationCodeInstalledApp.Logger.Info("Received an error. The response is: {0}", (object)error);
                    throw new TokenResponseException(error);
                }
                AuthorizationCodeInstalledApp.Logger.Debug("Received \"{0}\" code", (object)authorizationCode.Code);
                token = await this.Flow.ExchangeCodeForTokenAsync(userId, authorizationCode.Code, this.CodeReceiver.RedirectUri, taskCancellationToken).ConfigureAwait(false);
            }
            return new UserCredential(this.flow, userId, token);
        }

        /// <summary>
        /// Determines the need for retrieval of a new authorization code, based on the given token and the
        /// authorization code flow.
        /// </summary>
        public bool ShouldRequestAuthorizationCode(TokenResponse token)
        {
            if (this.Flow.ShouldForceTokenRetrieval() || token == null)
                return true;
            return false;
        }
    }
}