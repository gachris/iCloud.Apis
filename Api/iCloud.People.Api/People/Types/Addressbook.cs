using System.Xml.Serialization;

namespace iCloud.Apis.People.Types
{
    [XmlRoot(ElementName = "addressbook", Namespace = "urn:ietf:params:xml:ns:carddav")]
    public class Addressbook
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
}
