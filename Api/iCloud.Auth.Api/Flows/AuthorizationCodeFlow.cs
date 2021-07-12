using iCloud.Apis.Auth.Requests;
using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Core.Services;
using iCloud.Apis.Util;
using iCloud.Apis.Util.Store;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth.Flows
{
    /// <summary>
    /// ICloud specific authorization code flow which inherits from <see cref="T:ICloud.Apis.Auth.OAuth2.Flows.AuthorizationCodeFlow" />.
    /// </summary>
    public class AuthorizationCodeFlow : IAuthorizationCodeFlow
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<AuthorizationCodeFlow>();
        private readonly IAccessMethod accessMethod;
        private readonly string _contactsServerUrl;
        private readonly string _calendarServerUrl;
        private readonly IDataStore dataStore;
        private readonly ConfigurableHttpClient httpClient;
        private readonly IClock clock;

        /// <summary>Gets the token server URL.</summary>
        public string ContactsServerUrl
        {
            get { return this._contactsServerUrl; }
        }

        /// <summary>Gets the authorization code server URL.</summary>
        public string CalendarServerUrl
        {
            get { return this._calendarServerUrl; }
        }

        /// <summary>Gets the data store used to store the credentials.</summary>
        public IDataStore DataStore
        {
            get { return this.dataStore; }
        }

        /// <summary>Gets the HTTP client used to make authentication requests to the server.</summary>
        public ConfigurableHttpClient HttpClient
        {
            get { return this.httpClient; }
        }

        public IAccessMethod AccessMethod
        {
            get { return this.accessMethod; }
        }

        public IClock Clock
        {
            get { return this.clock; }
        }

        /// <summary>Constructs a new ICloud authorization code flow.</summary>
        public AuthorizationCodeFlow(AuthorizationCodeFlow.Initializer initializer)
        {
            this.accessMethod = initializer.AccessMethod.ThrowIfNull("Initializer.AccessMethod");
            this.clock = initializer.Clock.ThrowIfNull("Initializer.Clock");
            this._contactsServerUrl = initializer.AuthorizationContactsServerUrl.ThrowIfNullOrEmpty("Initializer.TokenServerUrl");
            this._calendarServerUrl = initializer.AuthorizationCalendarServerUrl.ThrowIfNullOrEmpty("Initializer.AuthorizationServerUrl");
            this.dataStore = initializer.DataStore;
            if (this.dataStore == null)
                AuthorizationCodeFlow.Logger.Warning("Datastore is null, as a result the user's credential will not be stored");
            CreateHttpClientArgs args = new CreateHttpClientArgs();
            if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
                args.Initializers.Add(new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, () => new BackOffHandler(new ExponentialBackOff())));
            this.httpClient = (initializer.HttpClientFactory ?? new HttpClientFactory()).CreateHttpClient(args);
        }

        public async Task<TokenResponse> LoadTokenAsync(string userId, CancellationToken taskCancellationToken)
        {
            taskCancellationToken.ThrowIfCancellationRequested();
            if (this.DataStore == null)
                return null;
            return await this.DataStore.GetAsync<TokenResponse>(userId).ConfigureAwait(false);
        }

        public async Task DeleteTokenAsync(string userId, CancellationToken taskCancellationToken)
        {
            taskCancellationToken.ThrowIfCancellationRequested();
            if (this.DataStore == null)
                return;
            await this.DataStore.DeleteAsync<TokenResponse>(userId).ConfigureAwait(false);
        }

        public virtual AuthorizationCodeRequestUrl CreateAuthorizationCodeRequest(string redirectUri, NetworkCredential networkCredential)
        {
            AuthorizationCodeRequestUrl authorizationCodeRequestUrl = new AuthorizationCodeRequestUrl(new Uri(this.CalendarServerUrl), networkCredential);
            return authorizationCodeRequestUrl;
        }

        public async Task<TokenResponse> ExchangeCodeForTokenAsync(string userId, string code, string redirectUri, CancellationToken taskCancellationToken)
        {
            AuthorizationCodeTokenRequest codeTokenRequest = new AuthorizationCodeTokenRequest
            {
                RedirectUri = redirectUri,
                Code = code
            };
            TokenResponse token = await this.FetchTokenAsync(userId, codeTokenRequest, taskCancellationToken).ConfigureAwait(false);
            await this.StoreTokenAsync(userId, token, taskCancellationToken).ConfigureAwait(false);
            return token;
        }

        public Task<TokenResponse> RefreshTokenAsync(string userId, string refreshToken, CancellationToken taskCancellationToken)
        {
            throw new NotImplementedException("The Auth protocol does not support token refresh.");
        }

        public async Task RevokeTokenAsync(string userId, string token, CancellationToken taskCancellationToken)
        {
            await this.DeleteTokenAsync(userId, taskCancellationToken);
        }

        public virtual bool ShouldForceTokenRetrieval()
        {
            return false;
        }

        /// <summary>Stores the token in the <see cref="P:ICloud.Apis.Auth.OAuth2.Flows.AuthorizationCodeFlow.DataStore" />.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="token">Token to store.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        private async Task StoreTokenAsync(string userId, TokenResponse token, CancellationToken taskCancellationToken)
        {
            taskCancellationToken.ThrowIfCancellationRequested();
            if (this.DataStore == null)
                return;
            await this.DataStore.StoreAsync<TokenResponse>(userId, token).ConfigureAwait(false);
        }

        /// <summary>Retrieve a new token from the server using the specified request.</summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="request">Token request.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <returns>Token response with the new access token.</returns>
        public async Task<TokenResponse> FetchTokenAsync(string userId, AuthorizationCodeTokenRequest request, CancellationToken taskCancellationToken)
        {
            TokenResponseException tokenException;
            try
            {
                return await request.ExecuteAsync(httpClient, this.CalendarServerUrl, this.ContactsServerUrl, taskCancellationToken, this.Clock).ConfigureAwait(false);
            }
            catch (TokenResponseException ex)
            {
                tokenException = ex;
            }
            await this.DeleteTokenAsync(userId, taskCancellationToken).ConfigureAwait(false);
            throw tokenException;
        }

        public void Dispose()
        {
            if (this.HttpClient == null)
                return;
            this.HttpClient.Dispose();
        }

        /// <summary>An initializer class for the authorization code flow. </summary>
        public class Initializer
        {
            /// <summary>
            /// Gets or sets the method for presenting the access token to the resource server.
            /// The default value is
            /// <see cref="T:ICloud.Apis.Auth.OAuth2.BearerToken.AuthorizationHeaderAccessMethod" />.
            /// </summary>
            public IAccessMethod AccessMethod { get; set; }

            /// <summary>Gets or sets the authorization server URL.</summary>
            public string AuthorizationCalendarServerUrl { get; private set; }

            /// <summary>Gets or sets the authorization server URL.</summary>
            public string AuthorizationContactsServerUrl { get; private set; }

            /// <summary>Gets or sets the data store used to store the token response.</summary>
            public IDataStore DataStore { get; set; }

            /// <summary>
            /// Gets or sets the factory for creating <see cref="T:System.Net.Http.HttpClient" /> instance.
            /// </summary>
            public IHttpClientFactory HttpClientFactory { get; set; }

            /// <summary>
            /// Get or sets the exponential back-off policy. Default value is  <c>UnsuccessfulResponse503</c>, which
            /// means that exponential back-off is used on 503 abnormal HTTP responses.
            /// If the value is set to <c>None</c>, no exponential back-off policy is used, and it's up to user to
            /// configure the <see cref="T:ICloud.Apis.Http.ConfigurableMessageHandler" /> in an
            /// <see cref="T:ICloud.Apis.Http.IConfigurableHttpClientInitializer" /> to set a specific back-off
            /// implementation (using <see cref="T:ICloud.Apis.Http.BackOffHandler" />).
            /// </summary>
            public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

            /// <summary>
            /// Gets or sets the clock. The clock is used to determine if the token has expired, if so we will try to
            /// refresh it. The default value is <see cref="F:ICloud.Apis.Util.SystemClock.Default" />.
            /// </summary>
            public IClock Clock { get; set; }

            /// <summary>Constructs a new initializer.</summary>
            public Initializer()
            {
                this.AuthorizationCalendarServerUrl = ApiConstants.CalendarServerUrl;
                this.AuthorizationContactsServerUrl = ApiConstants.ContactsServerUrl;
                this.AccessMethod = new BasicAuthentication.AuthorizationHeaderAccessMethod();
                this.DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
                this.Clock = SystemClock.Default;
            }
        }
    }
}
