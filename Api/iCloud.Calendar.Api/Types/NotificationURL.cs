using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "notification-URL", Namespace = "http://calendarserver.org/ns/")]
    public class NotificationURL
    {
        public static NotificationURL Empty = new NotificationURL();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
