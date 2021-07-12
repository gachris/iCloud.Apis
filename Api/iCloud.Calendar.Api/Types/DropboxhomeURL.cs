using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "dropbox-home-URL", Namespace = "http://calendarserver.org/ns/")]
    public class DropboxhomeURL
    {
        public static readonly DropboxhomeURL Empty = new DropboxhomeURL();

        [XmlElement(ElementName = "href", Namespace = "http://calendarserver.org/ns/")]
        public Url Url { get; set; }
    }
}
