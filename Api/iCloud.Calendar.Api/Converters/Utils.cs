using iCloud.Apis.Calendar.Types;
using iCloud.Apis.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Apis.Calendar
{
    internal static class Utils
    {
        public static CalendarList ConvertToCalendarList(this List<Response<Prop>> responses)
        {
            if (responses != null && responses.Any())
            {
                var calendarList = new CalendarList();
                foreach (var response in responses)
                {
                    var calendarListEntry = response.ConvertToCalendarListEntry();
                    if (calendarListEntry != null)
                        calendarList.Add(calendarListEntry);
                }
                return calendarList;
            }
            return default;
        }

        public static CalendarListEntry ConvertToCalendarListEntry(this Response<Prop> response)
        {
            if (response != null)
            {
                var CalendarUrl = response.Url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (CalendarUrl.Count() == 3)
                {
                    return new CalendarListEntry()
                    {
                        Id = CalendarUrl.Last(),
                        Summary = response.Propstat.Prop?.Displayname?.Value ?? CalendarUrl.Last(),
                        Url = response.Url,
                        //FullUrl = ((UserCredential)HttpClientInitializer).Token.Tokeninfo.CalendarPrincipal.HomeSetUrl + multistatusItem.Url,
                        Privileges = response.Propstat.Prop.Currentuserprivilegeset.Privilege
                    };
                }
            }
            return default;
        }
    }
}
