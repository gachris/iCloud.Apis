using Newtonsoft.Json;
using System;

namespace iCloud.Apis.Auth.Responses
{
    public class TokenResponse
    {
        [JsonProperty("token_info")]
        public Tokeninfo Tokeninfo { get; set; }

        /// <summary>Gets or sets the access token issued by the authorization server.</summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the token type as specified in http://tools.ietf.org/html/rfc6749#section-7.1.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>The date and time that this token was issued. <remarks>
        /// It should be set by the CLIENT after the token was received from the server.
        /// </remarks>
        /// </summary>
        public DateTime Issued { get; set; }
    }
}
