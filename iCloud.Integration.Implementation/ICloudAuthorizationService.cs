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

        #region Events

        public event EventHandler OnSignIn;
        public event EventHandler OnSignOut;

        #endregion

        #region Constructor

        public ICloudAuthorizationService(string userId, NetworkCredential networkCredential)
        {
            UserId = userId;
            NetworkCredential = networkCredential;
            Init();
        }
        
        #endregion

        #region Init

        private async void Init()
        {
            try
            {
                var token = GetTokenResponse().Result;
                if (token != null)
                    SignInAsync();
            }
            catch
            {
                await TokenDataStore.DeleteAsync<TokenResponse>("gachris");
            }
        }

        #endregion

        #region Data     

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

        #endregion

        #region Functions

        public void SignInAsync()
        {
            Credential = AuthorizationBroker.AuthorizeAsync(UserId, NetworkCredential, CancellationToken.None, TokenDataStore).Result;
            if (Credential != null)
            {
                OnSignIn?.Invoke(Credential, new EventArgs());
            }
        }

        public async Task SignOutAsync()
        {
            if (Credential != null)
            {
                await Credential.RevokeTokenAsync(CancellationToken.None);
                OnSignOut?.Invoke(Credential, new EventArgs());
                Credential = null;
            }
            else throw new UnauthorizedAccessException();
        }

        protected async Task<TokenResponse> GetTokenResponse()
        {
            return await TokenDataStore.GetAsync<TokenResponse>(UserId);
        }

        #endregion
    }
}
