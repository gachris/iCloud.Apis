using System.Xml.Serialization;

namespace iCloud.Apis.Auth.Types
{
    [XmlRoot(ElementName = "addressbook-home-set", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public class Addressbookhomeset
    {
        public static Addressbookhomeset Empty = new Addressbookhomeset();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
