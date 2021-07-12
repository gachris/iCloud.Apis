using System.Collections.Generic;
using System.Xml.Serialization;

namespace iCloud.Apis.Calendar.Types
{
    [XmlRoot(ElementName = "supported-report-set", Namespace = "DAV:")]
    public class Supportedreportset
    {
        [XmlElement(ElementName = "supported-report", Namespace = "DAV:")]
        public List<Supportedreport> Supportedreport { get; set; }
    }
}
