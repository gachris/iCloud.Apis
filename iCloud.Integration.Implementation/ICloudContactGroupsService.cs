using iCloud.Apis.Auth;
using iCloud.Apis.People;
using iCloud.Apis.People.Responses;
using System;
using System.Collections.Generic;

namespace iCloud.Integration.Implementation
{
    public class ICloudContactGroupsService
    {
        #region Retrieve ContactGroups

        public static IList<ContactGroup> GetContactGroups(UserCredential credential)
        {
            var service = GetService(credential);
            return service.GetContactGroups(credential);
        }

        #endregion

        public static ModifyContactGroupMembersResponse ModifyMembers(UserCredential credential, IList<string> resourceNamesToAdd, IList<string> resourceNamesToRemove, string etag, string uniqueId)
        {
            var service = GetService(credential);
            ModifyContactGroupMembersRequest request = new ModifyContactGroupMembersRequest
            {
                ResourceNamesToAdd = resourceNamesToAdd,
                ResourceNamesToRemove = resourceNamesToRemove,
                ETag = etag
            };
            return service.ModifyMembers(credential, request, "card", uniqueId);
        }

        #region Services

        private static ContactGroupsService GetService(UserCredential credential)
        {
            if (credential != null)
            {
                return new ContactGroupsService();
            }
            else throw new UnauthorizedAccessException();
        }

        #endregion
    }
}