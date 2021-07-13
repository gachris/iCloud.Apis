using iCloud.Apis.Auth;
using iCloud.Apis.People;
using System;

namespace iCloud.Integration.Implementation
{
    public class ICloudContactGroupsService
    {

        public static ContactGroupsList GetContactGroups(UserCredential credential, string resourceName)
        {
            var service = GetService(credential);
            return service.ContactGroups.List(resourceName).Execute();
        }

        public static ContactGroup GetContactGroup(UserCredential credential, string uniqueId, string resourceName)
        {
            var service = GetService(credential);
            return service.ContactGroups.Get(uniqueId, resourceName).Execute();
        }

        public static ContactGroup AddContactGroup(UserCredential credential, ContactGroup contactGroup, string resourceName)
        {
            var service = GetService(credential);
            return service.ContactGroups.Insert(contactGroup, resourceName).Execute();
        }

        public static object UpdateContactGroup(UserCredential credential, ContactGroup contactGroup, string uniqueId, string resourceName)
        {
            var service = GetService(credential);
            return service.ContactGroups.Update(contactGroup, uniqueId, resourceName).Execute();
        }

        public static object DeleteContactGroup(UserCredential credential, string uniqueId, string resourceName)
        {
            var service = GetService(credential);
            return service.ContactGroups.Delete(uniqueId, resourceName).Execute();
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