using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "principal-collection-set", Namespace = "DAV:")]
    public class Principalcollectionset
    {
        public static Principalcollectionset Empty = new Principalcollectionset();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
