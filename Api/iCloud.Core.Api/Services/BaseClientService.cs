using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCloud.Apis.Core.Services
{
    public abstract class BaseClientService : IClientService, IDisposable
    {
        /// <summary>The class logger.</summary>
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<BaseClientService>();
        /// <summary>The default maximum allowed length of a URL string for GET requests.</summary>
        public const uint DefaultMaxUrlLength = 2048;

        /// <summary>Constructs a new base client with the specified initializer.</summary>
        protected BaseClientService(BaseClientService.Initializer initializer)
        {
            this.GZipEnabled = initializer.GZipEnabled;
            this.Serializer = initializer.Serializer;
            this.ApiKey = initializer.ApiKey;
            this.ApplicationName = initializer.ApplicationName;
            if (this.ApplicationName == null)
                BaseClientService.Logger.Warning("Application name is not set. Please set Initializer.ApplicationName property");
            this.HttpClientInitializer = initializer.HttpClientInitializer;
            this.HttpClient = this.CreateHttpClient(initializer);
        }

        /// <summary>Returns <c>true</c> if this service contains the specified feature.</summary>
        private bool HasFeature(Features feature)
        {
            return this.Features.Contains(Utilities.GetEnumStringValue(feature));
        }

        private ConfigurableHttpClient CreateHttpClient(BaseClientService.Initializer initializer)
        {
            IHttpClientFactory httpClientFactory = initializer.HttpClientFactory ?? new HttpClientFactory();
            CreateHttpClientArgs args = new CreateHttpClientArgs()
            {
                GZipEnabled = this.GZipEnabled,
                ApplicationName = this.ApplicationName
            };
            if (this.HttpClientInitializer != null)
                args.Initializers.Add(this.HttpClientInitializer);
            if (initializer.DefaultExponentialBackOffPolicy != ExponentialBackOffPolicy.None)
                args.Initializers.Add(new ExponentialBackOffInitializer(initializer.DefaultExponentialBackOffPolicy, new Func<BackOffHandler>(this.CreateBackOffHandler)));
            ConfigurableHttpClient httpClient = httpClientFactory.CreateHttpClient(args);
            if (initializer.MaxUrlLength > 0U)
                httpClient.MessageHandler.AddExecuteInterceptor(new MaxUrlLengthInterceptor(initializer.MaxUrlLength));
            return httpClient;
        }

        /// <summary>
        /// Creates the back-off handler with <see cref="T:ICloud.Apis.Util.ExponentialBackOff" />.
        /// Overrides this method to change the default behavior of back-off handler (e.g. you can change the maximum
        /// waited request's time span, or create a back-off handler with you own implementation of
        /// <see cref="T:ICloud.Apis.Util.IBackOff" />).
        /// </summary>
        protected virtual BackOffHandler CreateBackOffHandler()
        {
            return new BackOffHandler(new ExponentialBackOff());
        }

        public ConfigurableHttpClient HttpClient { get; private set; }

        public IConfigurableHttpClientInitializer HttpClientInitializer { get; private set; }

        public bool GZipEnabled { get; private set; }

        public string ApiKey { get; private set; }

        public string ApplicationName { get; private set; }

        public void SetRequestSerailizedContent(HttpRequestMessage request, object body)
        {
            request.SetRequestSerailizedContent(this, body, this.GZipEnabled);
        }

        public ISerializer Serializer { get; private set; }

        public virtual string SerializeObject(object obj)
        {
            if (obj is string)
                return (string)obj;
            else if (!this.HasFeature(Services.Features.LegacyDataResponse))
                return this.Serializer.Serialize(obj);
            return this.Serializer.Serialize(new StandardResponse<object>() { Data = obj });
        }

        public virtual async Task<string> DeserializeResponse(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public virtual async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
        {
            object input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (Equals(typeof(T), typeof(string)))
                return (T)input;
            if (this.HasFeature(Services.Features.LegacyDataResponse))
            {
                StandardResponse<T> standardResponse;
                try
                {
                    standardResponse = this.Serializer.Deserialize<StandardResponse<T>>((string)input);
                }
                catch (JsonReaderException ex)
                {
                    throw new ICloudApiException(this.Name, "Failed to parse response from server as json [" + input + "]", ex);
                }
                if (standardResponse.Error != null)
                    throw new ICloudApiException(this.Name, "Server error - " + standardResponse.Error)
                    {
                        Error = standardResponse.Error
                    };
                if (standardResponse.Data == null)
                    throw new ICloudApiException(this.Name, "The response could not be deserialized.");
                return standardResponse.Data;
            }
            T obj2;
            try
            {
                obj2 = this.Serializer.Deserialize<T>((string)input);
            }
            catch (JsonReaderException ex)
            {
                throw new ICloudApiException(this.Name, "Failed to parse response from server as json [" + input + "]", ex);
            }
            string str = response.Headers.ETag != null ? response.Headers.ETag.Tag : null;
            if (obj2 is IDirectResponseSchema && str != null)
                (obj2 as IDirectResponseSchema).ETag = str;
            return obj2;
        }

        public virtual async Task<RequestError> DeserializeError(HttpResponseMessage response)
        {
            StandardResponse<object> errorResponse;
            try
            {
                errorResponse = this.Serializer.Deserialize<StandardResponse<object>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                if (errorResponse?.Error == null)
                    throw new ICloudApiException(this.Name, "error response is null");
            }
            catch (Exception ex)
            {
                throw new ICloudApiException(this.Name, "An Error occurred, but the error response could not be deserialized", ex);
            }
            return errorResponse.Error;
        }

        public abstract string Name { get; }

        public abstract string BaseUri { get; }

        public abstract string BasePath { get; }

        public abstract IList<string> Features { get; }

        public virtual void Dispose()
        {
            if (this.HttpClient == null)
                return;
            this.HttpClient.Dispose();
        }

        /// <summary>An initializer class for the client service.</summary>
        public class Initializer
        {
            /// <summary>
            /// Gets or sets the factory for creating <see cref="T:System.Net.Http.HttpClient" /> instance. If this
            /// property is not set the service uses a new <see cref="T:ICloud.Apis.Http.HttpClientFactory" /> instance.
            /// </summary>
            public IHttpClientFactory HttpClientFactory { get; set; }

            /// <summary>
            /// Gets or sets a HTTP client initializer which is able to customize properties on
            /// <see cref="T:ICloud.Apis.Http.ConfigurableHttpClient" /> and
            /// <see cref="T:ICloud.Apis.Http.ConfigurableMessageHandler" />.
            /// </summary>
            public IConfigurableHttpClientInitializer HttpClientInitializer { get; set; }

            /// <summary>
            /// Get or sets the exponential back-off policy used by the service. Default value is
            /// <c>UnsuccessfulResponse503</c>, which means that exponential back-off is used on 503 abnormal HTTP
            /// response.
            /// If the value is set to <c>None</c>, no exponential back-off policy is used, and it's up to the user to
            /// configure the <see cref="T:ICloud.Apis.Http.ConfigurableMessageHandler" /> in an
            /// <see cref="T:ICloud.Apis.Http.IConfigurableHttpClientInitializer" /> to set a specific back-off
            /// implementation (using <see cref="T:ICloud.Apis.Http.BackOffHandler" />).
            /// </summary>
            public ExponentialBackOffPolicy DefaultExponentialBackOffPolicy { get; set; }

            /// <summary>Gets or sets whether this service supports GZip. Default value is <c>true</c>.</summary>
            public bool GZipEnabled { get; set; }

            /// <summary>
            /// Gets or sets the serializer. Default value is <see cref="T:ICloud.Apis.Json.NewtonsoftJsonSerializer" />.
            /// </summary>
            public ISerializer Serializer { get; set; }

            /// <summary>Gets or sets the API Key. Default value is <c>null</c>.</summary>
            public string ApiKey { get; set; }

            /// <summary>
            /// Gets or sets Application name to be used in the User-Agent header. Default value is <c>null</c>.
            /// </summary>
            public string ApplicationName { get; set; }

            /// <summary>
            /// Maximum allowed length of a URL string for GET requests. Default value is <c>2048</c>. If the value is
            /// set to <c>0</c>, requests will never be modified due to URL string length.
            /// </summary>
            public uint MaxUrlLength { get; set; }

            /// <summary>Constructs a new initializer with default values.</summary>
            public Initializer()
            {
                this.GZipEnabled = false;
                this.Serializer = new XmlSerializer();
                this.DefaultExponentialBackOffPolicy = ExponentialBackOffPolicy.UnsuccessfulResponse503;
                this.MaxUrlLength = 2048U;
            }
        }
    }
}