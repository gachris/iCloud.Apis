using System;

namespace iCloud.Apis.Auth.Responses
{
    /// <summary>
    /// Token response exception which is thrown in case of receiving a token error when an authorization code or an
    /// access token is expected.
    /// </summary>
    public class TokenResponseException : Exception
    {
        /// <summary>The error information.</summary>
        public TokenErrorResponse Error { get; private set; }

        /// <summary>Constructs a new token response exception from the given error.</summary>
        public TokenResponseException(TokenErrorResponse error)
          : base(error.ToString())
        {
            this.Error = error;
        }
    }
}
