using System.Xml.Serialization;

namespace iCloud.Apis.People.Types
{
    [XmlRoot(ElementName = "getetag", Namespace = "DAV:")]
    public class Getetag
    {
        public static Getetag Empty = new Getetag();

        [XmlText]
        public string Value { get; set; }
    }
}
