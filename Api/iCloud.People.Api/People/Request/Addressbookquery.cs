using iCloud.Apis.People.Types;
using System.Xml.Serialization;

namespace iCloud.Apis.People.Request
{
    [XmlRoot(ElementName = "addressbook-query", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public class Addressbookquery
    {
        [XmlElement(ElementName = "prop", Namespace = "DAV:")]
        public Prop Prop { get; set; }

        [XmlElement(ElementName = "filter", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public Filters Filter { get; set; }
    }
}
