namespace iCloud.Apis.People
{
    public class Membership
    {
        public virtual ContactGroupMembership ContactGroupMembership { get; set; }

        public virtual string ETag { get; set; }
    }
}