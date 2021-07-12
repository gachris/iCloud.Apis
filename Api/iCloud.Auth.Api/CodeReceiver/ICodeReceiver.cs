using iCloud.Apis.Auth.Requests;
using iCloud.Apis.Auth.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    /// <summary>OAuth 2.0 verification code receiver.</summary>
    public interface ICodeReceiver
    {
        /// <summary>Gets the redirected URI.</summary>
        string RedirectUri { get; }

        /// <summary>Receives the authorization code.</summary>
        /// <param name="url">The authorization code request URL</param>
        /// <param name="taskCancellationToken">Cancellation token</param>
        /// <returns>The authorization code response</returns>
        Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken);
    }
}