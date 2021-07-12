using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "resource-id", Namespace = "DAV:")]
    public class Resourceid
    {
        public static readonly Resourceid Empty = new Resourceid();

        [XmlText]
        public string Value { get; set; }
    }
}
