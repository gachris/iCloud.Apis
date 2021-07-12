using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "schedule-inbox-URL", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class ScheduleinboxURL
    {
        public static ScheduleinboxURL Empty = new ScheduleinboxURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
