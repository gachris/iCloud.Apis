using iCloud.Apis.Auth.Flows;
using iCloud.Apis.Util.Store;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    public class AuthorizationBroker
    {
        /// <summary>The folder which is used by the <see cref="T:iCloud.Apis.Util.Store.FileDataStore" />.</summary>
        /// <remarks>
        /// The reason that this is not 'private const' is that a user can change it and store the credentials in a
        /// different location.
        /// </remarks>
        public static string Folder = "iCloud.Apis.Auth";

        public static async Task<UserCredential> AuthorizeAsync(string user, NetworkCredential networdCredentials, CancellationToken cancellationToken, IDataStore dataStore)
        {
            AuthorizationCodeFlow.Initializer initializer = new AuthorizationCodeFlow.Initializer();
            return await AuthorizationBroker.AuthorizeAsync(initializer, networdCredentials, user, cancellationToken, dataStore).ConfigureAwait(false);
        }

        /// <summary>The core logic for asynchronously authorizing the specified user.</summary>
        /// <param name="initializer">The authorization code initializer.</param>
        /// <param name="networdCredentials">
        /// The netword credentials which indicate the iCloud API access your application is requesting.
        /// </param>
        /// <param name="user">The user to authorize.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel an operation.</param>
        /// <param name="dataStore">The data store, if not specified a file data store will be used.</param>
        /// <returns>User credential.</returns>
        public static async Task<UserCredential> AuthorizeAsync(AuthorizationCodeFlow.Initializer initializer, NetworkCredential networdCredentials, string user, CancellationToken taskCancellationToken, IDataStore dataStore = null)
        {
            initializer.DataStore = dataStore ?? new FileDataStore(AuthorizationBroker.Folder, false);
            return await new AuthorizationCodeInstalledApp(new AuthorizationCodeFlow(initializer), new LocalServerCodeReceiver()).AuthorizeAsync(user, networdCredentials, taskCancellationToken).ConfigureAwait(false);
        }
    }
}
