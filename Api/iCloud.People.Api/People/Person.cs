using iCloud.Apis.Util;
using System.Collections.Generic;
using System.IO;
using vCards;

namespace iCloud.Apis.People
{
    public class Person : vCard
    {
        public Person() : base()
        {
            Memberships = new List<Membership>();
            Addresses = new vCardAddressCollection();
        }

        public Person(TextReader input) : base(input)
        {

        }

        public Person(byte[] bytes) : base(bytes)
        {
        }

        public Person(string content) : this()
        {
            var standardReader = new CardStandardReader();
            standardReader.ReadInto(this, new StringReader(content));
        }

        public string Etag { get; set; }

        public virtual IList<Membership> Memberships { get; set; }

        public vCardAddressCollection Addresses { get; }
    }
}