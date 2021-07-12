using System.Collections.Generic;

namespace iCloud.Apis.People.Responses
{
    public class ModifyContactGroupMembersRequest
    {
        public ModifyContactGroupMembersRequest()
        {
        }

        public virtual IList<string> ResourceNamesToAdd { get; set; }

        public virtual IList<string> ResourceNamesToRemove { get; set; }

        public virtual string ETag { get; set; }
    }
}
