using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Auth.Types;
using iCloud.Apis.Core.Request;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.Core.Services;
using iCloud.Apis.Util;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth.Requests
{
    /// <summary>Extension methods to <see cref="T:ICloud.Apis.Auth.OAuth2.Requests.TokenRequest" />.</summary>
    public static class TokenRequestExtenstions
    {
        /// <summary>
        /// Executes the token request in order to receive a
        /// <see cref="T:ICloud.Apis.Auth.OAuth2.Responses.TokenResponse" />. In case the token server returns an
        /// error, a <see cref="T:ICloud.Apis.Auth.OAuth2.Responses.TokenResponseException" /> is thrown.
        /// </summary>
        /// <param name="request">The token request.</param>
        /// <param name="httpClient">The HTTP client used to create an HTTP request.</param>
        /// <param name="tokenServerUrl">The token server URL.</param>
        /// <param name="taskCancellationToken">Cancellation token to cancel operation.</param>
        /// <param name="clock">
        /// The clock which is used to set the
        /// <see cref="P:ICloud.Apis.Auth.OAuth2.Responses.TokenResponse.Issued" /> property.
        /// </param>
        /// <returns>Token response with the new access token.</returns>
        public static async Task<TokenResponse> ExecuteAsync(this TokenRequest request, HttpClient httpClient, string tokenServerUrl, CancellationToken taskCancellationToken, IClock clock)
        {
            HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), tokenServerUrl)
            {
                Content = ParameterUtils.CreateFormUrlEncodedContent(request)
            }, taskCancellationToken).ConfigureAwait(false);
            string input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new TokenResponseException(NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(input));
            TokenResponse tokenResponse = NewtonsoftJsonSerializer.Instance.Deserialize<TokenResponse>(input);
            tokenResponse.Issued = clock.Now;
            return tokenResponse;
        }

        public static async Task<TokenResponse> ExecuteAsync(this AuthorizationCodeTokenRequest request, HttpClient httpClient, string CalendarServerUrl, string ContactsServerUrl, CancellationToken taskCancellationToken, IClock clock)
        {
            TokenResponse tokenResponse = new TokenResponse();

            var principalRequest = new Propfind<Prop>() { Prop = new Prop() { Currentuserprincipal = Currentuserprincipal.Empty } };
            var content = XmlSerializerHelper.Serialize(principalRequest);

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {request.Code}");
            httpClient.DefaultRequestHeaders.Add("UserAgent", "curl/7.37.0");
            httpClient.DefaultRequestHeaders.Add("Depth", "0");

            HttpResponseMessage response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), CalendarServerUrl)
            {
                Content = new StringContent(content),
            }, taskCancellationToken).ConfigureAwait(false);

            string input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new TokenResponseException(new TokenErrorResponse() { Error = response.ReasonPhrase, ErrorDescription = response.StatusCode.ToString() });

            var multistatus = XmlSerializerHelper.Deserialize<Multistatus<Prop>>(input);

            tokenResponse.Tokeninfo = new Tokeninfo
            {
                CalendarPrincipal = new CalendarPrincipal()
                {
                    Id = multistatus.Responses.FirstOrDefault().Propstat.Prop.Currentuserprincipal.Url.Value.Split(new char[] { '/' })[1],
                    Url = ApiConstants.CalendarServerUrl + multistatus.Responses.FirstOrDefault().Propstat.Prop.Currentuserprincipal.Url.Value
                }
            };

            response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), ContactsServerUrl)
            {
                Content = new StringContent(content),
            }, taskCancellationToken).ConfigureAwait(false);

            input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new TokenResponseException(NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(input));

            multistatus = XmlSerializerHelper.Deserialize<Multistatus<Prop>>(input);

            tokenResponse.Tokeninfo.ContactsPrincipal = new ContactsPrincipal()
            {
                Id = multistatus.Responses.FirstOrDefault().Propstat.Prop.Currentuserprincipal.Url.Value.Split(new char[] { '/' })[1],
                Url = ApiConstants.ContactsServerUrl + multistatus.Responses.FirstOrDefault().Propstat.Prop.Currentuserprincipal.Url.Value
            };

            tokenResponse.AccessToken = request.Code;
            tokenResponse.Issued = clock.Now;
            tokenResponse.TokenType = request.GrantType;

            principalRequest = new Propfind<Prop>()
            {
                Prop = new Prop()
                {
                    Calendaruseraddressset = Calendaruseraddressset.Empty,
                    Calendarhomeset = Calendarhomeset.Empty,
                    Currentuserprincipal = Currentuserprincipal.Empty,
                    Displayname = Displayname.Empty
                }
            };
            content = XmlSerializerHelper.Serialize(principalRequest);

            response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), tokenResponse.Tokeninfo.CalendarPrincipal.Url)
            {
                Content = new StringContent(content),
            }, taskCancellationToken).ConfigureAwait(false);

            input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new TokenResponseException(NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(input));

            multistatus = XmlSerializerHelper.Deserialize<Multistatus<Prop>>(input);

            tokenResponse.Tokeninfo.CalendarPrincipal.Calendaruseraddressset = multistatus.Responses.FirstOrDefault().Propstat.Prop.Calendaruseraddressset;
            tokenResponse.Tokeninfo.CalendarPrincipal.HomeSetUrl = GetBaseCalendarListUrl(ApiConstants.CalendarHomeSetPort, multistatus.Responses.FirstOrDefault().Propstat.Prop.Calendarhomeset.Url.Value);
            tokenResponse.Tokeninfo.CalendarPrincipal.PrincipalHomeSetUrl = multistatus.Responses.FirstOrDefault().Propstat.Prop.Calendarhomeset.Url.Value;
            tokenResponse.Tokeninfo.CalendarPrincipal.DisplayName = multistatus.Responses.FirstOrDefault().Propstat.Prop.Displayname.Value;
            tokenResponse.Tokeninfo.CalendarPrincipal.CurrentUserPrincipal = multistatus.Responses.FirstOrDefault().Propstat.Prop.Currentuserprincipal.Url.Value;
            tokenResponse.Tokeninfo.Name = multistatus.Responses.FirstOrDefault().Propstat.Prop.Displayname.Value;
            tokenResponse.Tokeninfo.Email = multistatus.Responses.FirstOrDefault().Propstat.Prop.Calendaruseraddressset.Href.Where(x => x.Value.StartsWith("mailto:")).FirstOrDefault().Value.Replace("mailto:", "");

            principalRequest = new Propfind<Prop>()
            {
                Prop = new Prop()
                {
                    Addressbookhomeset = Addressbookhomeset.Empty,
                    Currentuserprincipal = Currentuserprincipal.Empty,
                    Displayname = Displayname.Empty
                }
            };
            content = XmlSerializerHelper.Serialize(principalRequest);

            response = await httpClient.SendAsync(new HttpRequestMessage(new HttpMethod(ApiMethod.PROPFIND), tokenResponse.Tokeninfo.ContactsPrincipal.Url)
            {
                Content = new StringContent(content),
            }, taskCancellationToken).ConfigureAwait(false);

            input = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new TokenResponseException(NewtonsoftJsonSerializer.Instance.Deserialize<TokenErrorResponse>(input));

            multistatus = XmlSerializerHelper.Deserialize<Multistatus<Prop>>(input);

            tokenResponse.Tokeninfo.ContactsPrincipal.HomeSetUrl = GetBaseCalendarListUrl(ApiConstants.ContactsHomeSetPort, multistatus.Responses.FirstOrDefault().Propstat.Prop.Addressbookhomeset.Url.Value);
            tokenResponse.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl = multistatus.Responses.FirstOrDefault().Propstat.Prop.Addressbookhomeset.Url.Value;
            tokenResponse.Tokeninfo.ContactsPrincipal.DisplayName = multistatus.Responses.FirstOrDefault().Propstat.Prop.Displayname.Value;
            tokenResponse.Tokeninfo.ContactsPrincipal.CurrentUserPrincipal = multistatus.Responses.FirstOrDefault().Propstat.Prop.Currentuserprincipal.Url.Value;

            return tokenResponse;
        }

        private static string GetBaseCalendarListUrl(string temp, string calendarListUrl)
        {
            Match match = Regex.Match(calendarListUrl, $@"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{{1,256}}\.[a-zA-Z0-9()]{{1,6}}\b([{temp}]*)?");
            return match?.Value ?? null;
        }
    }
}
