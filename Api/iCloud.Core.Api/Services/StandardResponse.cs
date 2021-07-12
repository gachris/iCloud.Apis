using Newtonsoft.Json;

namespace iCloud.Apis.Core.Services
{
    public sealed class StandardResponse<InnerType>
    {
        /// <summary>May be null if call failed.</summary>
        [JsonProperty("data")]
        public InnerType Data { get; set; }

        /// <summary>May be null if call succedded.</summary>
        [JsonProperty("error")]
        public RequestError Error { get; set; }
    }
}
