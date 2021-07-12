using System.Xml.Serialization;

namespace iCloud.Apis.People.Types
{
    [XmlRoot(ElementName = "addressbook-multiget", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public class Addressbookmultiget
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
