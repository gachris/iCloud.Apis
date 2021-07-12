using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "getctag", Namespace = "http://calendarserver.org/ns/")]
    public class Getctag
    {
        public static readonly Getctag Empty = new Getctag();

        [XmlText]
        public string Value { get; set; }
    }
}
