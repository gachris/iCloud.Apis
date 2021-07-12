using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "verify-calendar-user", Namespace = "http://me.com/_namespace/")]
    public class Verifycalendaruser
    {
        public static readonly Getctag Empty = new Getctag();

        [XmlText]
        public string Value { get; set; }
    }
}
