using iCloud.Apis.Core.Services;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace iCloud.Apis.Calendar
{
    public class Channel : IDirectResponseSchema
    {
        /// <summary>The address where notifications are delivered for this channel.</summary>
        [JsonProperty("address")]
        public virtual string Address { get; set; }

        /// <summary>Date and time of notification channel expiration, expressed as a Unix timestamp, in milliseconds.
        /// Optional.</summary>
        [JsonProperty("expiration")]
        public virtual long? Expiration { get; set; }

        /// <summary>A UUID or similar unique string that identifies this channel.</summary>
        [JsonProperty("id")]
        public virtual string Id { get; set; }

        /// <summary>Identifies this as a notification channel used to watch for changes to a resource, which is
        /// "api#channel".</summary>
        [JsonProperty("kind")]
        public virtual string Kind { get; set; }

        /// <summary>Additional parameters controlling delivery channel behavior. Optional.</summary>
        [JsonProperty("params")]
        public virtual IDictionary<string, string> Params__ { get; set; }

        /// <summary>A Boolean value to indicate whether payload is wanted. Optional.</summary>
        [JsonProperty("payload")]
        public virtual bool? Payload { get; set; }

        /// <summary>An opaque ID that identifies the resource being watched on this channel. Stable across different
        /// API versions.</summary>
        [JsonProperty("resourceId")]
        public virtual string ResourceId { get; set; }

        /// <summary>A version-specific identifier for the watched resource.</summary>
        [JsonProperty("resourceUri")]
        public virtual string ResourceUri { get; set; }

        /// <summary>An arbitrary string delivered to the target address with each notification delivered over this
        /// channel. Optional.</summary>
        [JsonProperty("token")]
        public virtual string Token { get; set; }

        /// <summary>The type of delivery mechanism used for this channel.</summary>
        [JsonProperty("type")]
        public virtual string Type { get; set; }

        /// <summary>The ETag of the item.</summary>
        public virtual string ETag { get; set; }
    }
}
