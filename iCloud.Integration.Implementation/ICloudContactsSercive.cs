using iCloud.Apis.Auth;
using iCloud.Apis.People;
using System;

namespace iCloud.Integration.Implementation
{
    public class ICloudContactsSercive
    {
        public static PersonList GetContacts(UserCredential credential, string nextPageToken = null)
        {
            var service = GetService(credential);
            PeopleResource.ListRequest request = service.People.List("card");
            request.MaxResults = 1000;
            request.PageToken = nextPageToken;
            return request.Execute();
        }

        public static Person GetContact(UserCredential credential, string personId)
        {
            var service = GetService(credential);
            PeopleResource.GetRequest request = service.People.Get(personId, "card");
            return request.Execute();
        }

        public static Person AddContact(UserCredential credential, Person person)
        {
            var service = GetService(credential);
            PeopleResource.InsertRequest request = service.People.Insert(person, "card");
            return request.Execute();
        }

        public static Person UpdateContact(UserCredential credential, Person person, string personId, string etag)
        {
            var service = GetService(credential);
            PeopleResource.UpdateRequest request = service.People.Update(person, personId, etag, "card");
            return request.Execute();
        }

        public static string DeleteContact(UserCredential credential, string personId)
        {
            var service = GetService(credential);
            PeopleResource.DeleteRequest request = service.People.Delete(personId, "card");
            return request.Execute();
        }

        #region Services

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

        #endregion
    }
}