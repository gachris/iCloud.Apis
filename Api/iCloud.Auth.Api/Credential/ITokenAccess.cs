using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    /// <summary>
    /// Allows direct retrieval of access tokens to authenticate requests.
    /// This is necessary for workflows where you don't want to use
    /// <see cref="T:ICloud.Apis.Services.BaseClientService" /> to access the API.
    /// (e.g. gRPC that implemenents the entire HTTP2 stack internally).
    /// </summary>
    public interface ITokenAccess
    {
        /// <summary>
        /// Gets an access token to authorize a request.
        /// Implementations should handle automatic refreshes of the token
        /// if they are supported.
        /// The <paramref name="authUri" /> might be required by some credential types
        /// (e.g. the JWT access token) while other credential types
        /// migth just ignore it.
        /// </summary>
        /// <param name="authUri">The URI the returned token will grant access to.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The access token.</returns>
        string GetAccessTokenForRequestAsync(string authUri = null, CancellationToken cancellationToken = default);
    }
}
