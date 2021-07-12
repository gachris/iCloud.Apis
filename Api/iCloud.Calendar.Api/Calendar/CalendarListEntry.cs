using iCloud.Apis.Calendar.Types;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Apis.Calendar
{
    [TypeConverter(typeof(CalendarListEntryConverter))]
    public class CalendarListEntry
    {
        public string Id { get; set; }
        public string Summary { get; set; }
        public string Url { get; set; }
        public string FullUrl { get; set; }
        public List<Privilege> Privileges { get; set; }
    }
}
