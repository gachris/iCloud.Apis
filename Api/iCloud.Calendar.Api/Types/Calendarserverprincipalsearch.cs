using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "calendarserver-principal-search", Namespace = "http://calendarserver.org/ns/")]
    public class Calendarserverprincipalsearch
    {
        public static readonly Calendarserverprincipalsearch Empty = new Calendarserverprincipalsearch();

        [XmlText]
        public string Value { get; set; }
    }
}
