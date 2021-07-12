using System.Collections.Generic;

namespace iCloud.Apis.People.Responses
{
    public class ModifyContactGroupMembersResponse
    {
        public ModifyContactGroupMembersResponse()
        {

        }

        public virtual IList<string> CanNotRemoveLastContactGroupResourceNames { get; set; }

        public virtual IList<string> NotFoundResourceNames { get; set; }

        public virtual string ETag { get; set; }
    }
}
