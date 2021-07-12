using iCloud.Apis.Auth.Requests;
using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Util.Store;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    /// <summary>OAuth 2.0 authorization code flow that manages and persists end-user credentials.</summary>
    public interface IAuthorizationCodeFlow : IDisposable
    {
        /// <summary>Gets the method for presenting the access token to the resource server.</summary>
        IAccessMethod AccessMethod { get; }

        /// <summary>Gets the clock.</summary>
        IClock Clock { get; }

        /// <summary>Gets the data store used to store the credentials.</summary>
        IDataStore DataStore { get; }

        /// <summary>
        /// Asynchronously loads the user's token using the flow's
        /// <see cref="T:ICloud.Apis.Util.Store.IDataStore" />.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation</param>
        /// <returns>Token response</returns>
        Task<TokenResponse> LoadTokenAsync(string userId, CancellationToken taskCancellationToken);

        /// <summary>
        /// Asynchronously deletes the user's token using the flow's
        /// <see cref="T:ICloud.Apis.Util.Store.IDataStore" />.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken);

        /// <summary>Creates an authorization code request with the specified redirect URI.</summary>
        AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri, NetworkCredential networkCredential);

        /// <summary>Asynchronously exchanges code with a token.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="code">Authorization code received from the authorization server.</param>
        /// <param name="redirectUri">Redirect URI which is used in the token request.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>Token response which contains the access token.</returns>
        Task<TokenResponse> ExchangeCodeForTokenAsync(string userId, string code, string redirectUri, CancellationToken taskCancellationToken);

        /// <summary>Asynchronously refreshes an access token using a refresh token.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="refreshToken">Refresh token which is used to get a new access token.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>Token response which contains the access token and the input refresh token.</returns>
        Task<TokenResponse> RefreshTokenAsync(string userId, string refreshToken, CancellationToken taskCancellationToken);
        /// <summary>
        /// Asynchronously revokes the specified token. This method disconnects the user's account from the OAuth 2.0
        /// application. It should be called upon removing the user account from the site.</summary>
        /// <remarks>
        /// If revoking the token succeeds, the user's credential is removed from the data store and the user MUST
        /// authorize the application again before the application can access the user's private resources.
        /// </remarks>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Access token to be revoked.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <returns><c>true</c> if the token was revoked successfully.</returns>
        Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken);

        /// <summary>
        /// Indicates if a new token needs to be retrieved and stored regardless of normal circumstances.
        /// </summary>
        bool ShouldForceTokenRetrieval();
    }
}
