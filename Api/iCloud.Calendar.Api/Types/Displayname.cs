using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "displayname", Namespace = "DAV:")]
    public class Displayname
    {
        public static readonly Displayname Empty = new Displayname();

        [XmlText]
        public string Value { get; set; }
    }
}
