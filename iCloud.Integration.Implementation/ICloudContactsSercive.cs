using iCloud.Apis.Auth;
using iCloud.Apis.People;
using System;
using System.Collections.Generic;

namespace iCloud.Integration.Implementation
{
    public class ICloudContactsSercive
    {
        #region Retrieve Contacts

        public static IList<Person> GetContacts(UserCredential credential)
        {
            var service = GetService(credential);
            return service.GetContacts(credential, "card");
        }

        #endregion

        public static Person AddContact(UserCredential credential, Person contact)
        {
            var service = GetService(credential);
            return service.AddContact(credential, contact, "card");
        }

        public static Person UpdateContact(UserCredential credential, Person contact, string uniqueId, string etag)
        {
            var service = GetService(credential);
            return service.UpdateContact(credential, contact, uniqueId, etag, "card");
        }

        #region Services

        private static ContactsService GetService(UserCredential credential)
        {
            if (credential != null)
            {
                return new ContactsService();
            }
            else throw new UnauthorizedAccessException();
        }

        #endregion
    }
}