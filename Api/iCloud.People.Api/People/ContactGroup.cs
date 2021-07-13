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
        public virtual string ETag { get; set; }

        [JsonProperty("groupType")]
        public virtual string GroupType { get; set; }

        [JsonProperty("memberCount")]
        public virtual int? MemberCount { get; set; }

        [JsonProperty("memberResourceNames")]
        public virtual IList<string> MemberResourceNames { get; }

        [JsonProperty("resourceName")]
        public virtual string ResourceName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("full_url")]
        public string FullUrl { get; set; }

        public ContactGroup() : base()
        {
            GroupType = "group";
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

        public bool AddMemberResource(string uniqueId)
        {
            if (!this.MemberResourceNames.Contains(uniqueId))
            {
                this.MemberResourceNames.Add(uniqueId);
                return true;
            }
            return false;
        }

        public bool RemoveMemberResource(string uniqueId)
        {
            if (this.MemberResourceNames.Contains(uniqueId))
            {
                this.MemberResourceNames.Remove(uniqueId);
                return true;
            }
            return false;
        }
    }
}
