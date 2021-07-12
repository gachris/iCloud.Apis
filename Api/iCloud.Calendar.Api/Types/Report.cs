using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "report", Namespace = "DAV:")]
    public class Report
    {
        [XmlElement(ElementName = "acl-principal-prop-set", Namespace = "DAV:")]
        public string Aclprincipalpropset { get; set; }

        [XmlElement(ElementName = "principal-match", Namespace = "DAV:")]
        public string Principalmatch { get; set; }

        [XmlElement(ElementName = "principal-property-search", Namespace = "DAV:")]
        public string Principalpropertysearch { get; set; }

        [XmlElement(ElementName = "expand-property", Namespace = "DAV:")]
        public string Expandproperty { get; set; }

        [XmlElement(ElementName = "sync-collection", Namespace = "DAV:")]
        public string Synccollection { get; set; }

        [XmlElement(ElementName = "verify-calendar-user", Namespace = "http://me.com/_namespace/")]
        public Verifycalendaruser Verifycalendaruser { get; set; }

        [XmlElement(ElementName = "calendarserver-principal-search", Namespace = "http://calendarserver.org/ns/")]
        public Calendarserverprincipalsearch Calendarserverprincipalsearch { get; set; }
    }
}
