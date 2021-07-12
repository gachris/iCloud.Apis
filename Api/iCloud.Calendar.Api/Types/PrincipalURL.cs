using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "principal-URL", Namespace = "DAV:")]
    public class PrincipalURL
    {
        public static PrincipalURL Empty = new PrincipalURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
