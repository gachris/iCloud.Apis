using Newtonsoft.Json;

namespace iCloud.Apis.Auth
{
    public class Tokeninfo
    {
        [JsonProperty("email")]
        public virtual string Email { get; set; }

        [JsonProperty("name")]
        public virtual string Name { get; set; }

        [JsonProperty("calendar_principal")]
        public CalendarPrincipal CalendarPrincipal { get; set; }

        [JsonProperty("contacts_principal")]
        public ContactsPrincipal ContactsPrincipal { get; set; }
    }
}
