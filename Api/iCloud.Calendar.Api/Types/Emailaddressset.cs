using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "email-address-set", Namespace = "http://calendarserver.org/ns/")]
    public class Emailaddressset
    {
        public static Emailaddressset Empty = new Emailaddressset();

        [XmlText]
        public string Value { get; set; }
    }
}
