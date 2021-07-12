using iCloud.Apis.Util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using vCards;

namespace iCloud.Apis.People
{
    public class ContactGroup : vCard
    {
        [JsonProperty("etag")]
        public virtual string Etag { get; set; }

        [JsonProperty("groupType")]
        public virtual string GroupType { get; set; }

        [JsonProperty("memberCount")]
        public virtual int? MemberCount { get; set; }

        [JsonProperty("memberResourceNames")]
        public virtual IList<string> MemberResourceNames { get; set; }

        [JsonProperty("resourceName")]
        public virtual string ResourceName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("full_url")]
        public string FullUrl { get; set; }

        public ContactGroup() : base()
        {
            MemberResourceNames = new List<string>();
        }

        public ContactGroup(byte[] bytes) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(Encoding.UTF8.GetString(bytes)));
        }

        public ContactGroup(string content) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(content));
        }
    }
}
