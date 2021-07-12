using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace iCloud.Apis.Calendar
{
    [TypeConverter(typeof(CalendarListConverter))]
    public class CalendarList : List<CalendarListEntry>, IList<CalendarListEntry>, IEnumerable<CalendarListEntry>, IEnumerable
    {
    }
}
