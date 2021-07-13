using iCloud.Apis.Auth;
using iCloud.Apis.People;
using System;

namespace iCloud.Integration.Implementation
{
    public class ICloudContactsSercive
    {
        public static PersonList GetContacts(UserCredential credential, string resourceName, string nextPageToken = null)
        {
            var service = GetService(credential);
            PeopleResource.ListRequest request = service.People.List(resourceName);
            request.MaxResults = 1000;
            request.PageToken = nextPageToken;
            return request.Execute();
        }

        public static Person GetContact(UserCredential credential, string personId, string resourceName)
        {
            var service = GetService(credential);
            PeopleResource.GetRequest request = service.People.Get(personId, resourceName);
            return request.Execute();
        }

        public static Person AddContact(UserCredential credential, Person person, string resourceName)
        {
            var service = GetService(credential);
            PeopleResource.InsertRequest request = service.People.Insert(person, resourceName);
            return request.Execute();
        }

        public static Person UpdateContact(UserCredential credential, Person person, string personId, string etag, string resourceName)
        {
            var service = GetService(credential);
            PeopleResource.UpdateRequest request = service.People.Update(person, personId, etag, resourceName);
            return request.Execute();
        }

        public static string DeleteContact(UserCredential credential, string personId, string resourceName)
        {
            var service = GetService(credential);
            PeopleResource.DeleteRequest request = service.People.Delete(personId, resourceName);
            return request.Execute();
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