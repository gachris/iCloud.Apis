using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "current-user-privilege-set", Namespace = "DAV:")]
    public class Currentuserprivilegeset
    {
        public static readonly Currentuserprivilegeset Empty = new Currentuserprivilegeset();

        [XmlElement(ElementName = "privilege", Type = typeof(Privilege), Namespace = "DAV:")]
        public List<Privilege> Privilege { get; set; }
    }
}