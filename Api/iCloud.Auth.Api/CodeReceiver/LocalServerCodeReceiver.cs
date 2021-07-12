using iCloud.Apis.Auth.Requests;
using iCloud.Apis.Auth.Responses;
using iCloud.Apis.Core.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace iCloud.Apis.Auth
{
    /// <summary>
    /// OAuth 2.0 verification code receiver that runs a local server on a free port and waits for a call with the
    /// authorization verification code.
    /// </summary>
    public class LocalServerCodeReceiver : ICodeReceiver
    {
        private static readonly ILogger Logger = ApplicationContext.Logger.ForType<LocalServerCodeReceiver>();

        public string RedirectUri
        {
            get { return string.Empty; }
        }

        public Task<AuthorizationCodeResponseUrl> ReceiveCodeAsync(AuthorizationCodeRequestUrl url, CancellationToken taskCancellationToken)
        {
            var tcs = new TaskCompletionSource<AuthorizationCodeResponseUrl>();

            try
            {
                //string fileName = url.Build().ToString();
                AuthorizationCodeResponseUrl authorizationCodeResponseUrl = new AuthorizationCodeResponseUrl();

                //try
                //{
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(string.Concat(url.Credentials.UserName, ":", url.Credentials.Password));
                var basicAuthorization = Convert.ToBase64String(plainTextBytes);

                //    AuthService.GetToken(url.Credentials);
                //}
                //catch (WebException ex)
                //{
                //    if (ex.Response is HttpWebResponse response)
                //    {
                //        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
                //        {
                //            authorizationCodeResponseUrl.Error = "ICLOUDAUTHSCREEN/INVALIDCREDENTIALS";
                //        }
                //        else authorizationCodeResponseUrl.Error = "SYNCHRONIZATION/UNKNOWNERROR";
                //    }
                //    else authorizationCodeResponseUrl.Error = "SYNCHRONIZATION/UNKNOWNERROR";
                //}
                //catch (Exception)
                //{
                //    authorizationCodeResponseUrl.Error = "SYNCHRONIZATION/UNKNOWNERROR";
                //}

                authorizationCodeResponseUrl.Code = basicAuthorization;

                tcs.SetResult(authorizationCodeResponseUrl);

                tcs.Task.Wait(taskCancellationToken);
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                    tcs.SetException(e.InnerException);
                else
                    tcs.SetException(e);
            }

            return tcs.Task;
        }
    }
}