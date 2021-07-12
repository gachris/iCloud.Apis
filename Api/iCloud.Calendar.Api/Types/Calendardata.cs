using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "calendar-data", Namespace = "DAV:")]
    public class Calendardata
    {
        public static Calendardata Empty = new Calendardata();

        [XmlText]
        public string Value { get; set; }
    }
}
