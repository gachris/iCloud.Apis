namespace iCloud.Apis.Core.Services
{
    /// <summary>
    /// Specifies a list of features which can be defined within the discovery document of a service.
    /// </summary>
    public enum Features
    {
        /// <summary>
        /// If this feature is specified, then the data of a response is encapsulated within a "data" resource.
        /// </summary>
        [StringValue("dataWrapper")] LegacyDataResponse,
    }
}
