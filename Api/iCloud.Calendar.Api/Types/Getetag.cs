using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "getetag", Namespace = "DAV:")]
    public class Getetag
    {
        public static Getetag Empty = new Getetag();

        [XmlText]
        public string Value { get; set; }
    }
}
