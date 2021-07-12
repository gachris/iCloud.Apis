using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "schedule-outbox-URL", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class ScheduleoutboxURL
    {
        public static ScheduleoutboxURL Empty = new ScheduleoutboxURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
