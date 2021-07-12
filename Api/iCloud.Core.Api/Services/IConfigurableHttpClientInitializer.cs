namespace iCloud.Apis.Core.Services
{
    public interface IConfigurableHttpClientInitializer
    {
        /// <summary>Initializes a HTTP client after it was created.</summary>
        void Initialize(ConfigurableHttpClient httpClient);
    }
}
