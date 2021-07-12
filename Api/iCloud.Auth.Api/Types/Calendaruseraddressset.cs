using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Apis.Auth.Types
{
    [XmlRoot(ElementName = "calendar-user-address-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
    public class Calendaruseraddressset
    {
        public static Calendaruseraddressset Empty = new Calendaruseraddressset();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public List<Url> Href { get; set; }
    }
}
