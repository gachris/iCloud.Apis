using iCloud.Apis.Auth;
using iCloud.Apis.People;
using System;

namespace iCloud.Integration.Implementation
{
    public class IdentityCardService
    {
        public static IdentityCardList GetIdentityCards(UserCredential credential)
        {
            var service = GetService(credential);
            return service.IdentityCard.List().Execute();
        }

        private static PeopleService GetService(UserCredential credential)
        {
            if (credential != null)
            {
                return new PeopleService(new iCloud.Apis.Core.Services.BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            else throw new UnauthorizedAccessException();
        }
    }
}