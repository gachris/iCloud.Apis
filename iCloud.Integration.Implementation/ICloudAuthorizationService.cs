using iCloud.Apis.Auth;
using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Util.Store;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Integration.Implementation
{
    public class ICloudAuthorizationService
    {
        public event EventHandler OnSignIn;
        public event EventHandler OnSignOut;

        public ICloudAuthorizationService(string userId, NetworkCredential networkCredential)
        {
            UserId = userId;
            NetworkCredential = networkCredential;
        }

        public UserCredential Credential { get; private set; }


        private IDataStore _tokenDataStore;
        private IDataStore TokenDataStore
        {
            get
            {
                if (_tokenDataStore == null) _tokenDataStore = new FileDataStore("icloudStore");
                return _tokenDataStore;
            }
        }

        public bool IsAuthenticated
        {
            get { return Credential != null; }
        }

        public string UserId { get; }

        public NetworkCredential NetworkCredential { get; }

        public bool SignIn()
        {
            Credential = AuthorizationBroker.AuthorizeAsync(UserId, NetworkCredential, CancellationToken.None, TokenDataStore).ConfigureAwait(false).GetAwaiter().GetResult();
            OnSignIn?.Invoke(this, new EventArgs());
            return IsAuthenticated;
        }

        public bool SignOut()
        {
            if (Credential != null)
            {
                var result = Credential.RevokeTokenAsync(CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                OnSignOut?.Invoke(this, new EventArgs());
                Credential = null;
                return result;
            }
            else throw new UnauthorizedAccessException();
        }

        protected async Task<TokenResponse> GetTokenResponse()
        {
            return await TokenDataStore.GetAsync<TokenResponse>(UserId);
        }
    }
}
