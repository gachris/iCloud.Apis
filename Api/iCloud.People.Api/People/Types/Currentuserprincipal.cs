using System.Xml.Serialization;

namespace iCloud.Apis.People.Types
{
    [XmlRoot(ElementName = "current-user-principal", Namespace = "DAV:")]
    public class Currentuserprincipal
    {
        public static Currentuserprincipal Empty = new Currentuserprincipal();

        [XmlElement(ElementName = "href", Namespace = "DAV:")]
        public Url Url { get; set; }
    }
}
