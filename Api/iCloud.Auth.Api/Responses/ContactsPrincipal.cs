using Newtonsoft.Json;

namespace iCloud.Apis.Auth
{
    public class ContactsPrincipal
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("current_user_principal")]
        public string CurrentUserPrincipal { get; set; }

        [JsonProperty("home_set_url")]
        public string HomeSetUrl { get; set; }

        [JsonProperty("principal_home_set_url")]
        public string PrincipalHomeSetUrl { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
