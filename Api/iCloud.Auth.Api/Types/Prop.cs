using System.Xml.Serialization;

namespace iCloud.Apis.Auth.Types
{
    [XmlRoot(ElementName = "prop", Namespace = "DAV:")]
    public class Prop
    {
        [XmlElement(ElementName = "current-user-principal", Namespace = "DAV:")]
        public Currentuserprincipal Currentuserprincipal { get; set; }

        [XmlElement(ElementName = "calendar-home-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Calendarhomeset Calendarhomeset { get; set; }

        [XmlElement(ElementName = "displayname", Namespace = "DAV:")]
        public Displayname Displayname { get; set; }

        [XmlElement(ElementName = "addressbook-home-set", Namespace = "urn:ietf:params:xml:ns:carddav")]
        public Addressbookhomeset Addressbookhomeset { get; set; }

        [XmlElement(ElementName = "calendar-user-address-set", Namespace = "urn:ietf:params:xml:ns:caldav")]
        public Calendaruseraddressset Calendaruseraddressset { get; set; }
    }
}
