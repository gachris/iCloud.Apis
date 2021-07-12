using Ical.Net;
using Ical.Net.DataTypes;
using iCloud.Apis.Calendar;
using iCloud.Integration.Implementation;
using System;
using System.Net;

namespace iCloud.Sync.App.net40
{
    internal class Program
    {
        private static readonly ICloudAuthorizationService _icloudAuthorizationService = new ICloudAuthorizationService("userId.DataStore", new NetworkCredential("Apple ID", "App-specific pass"));

        private static void Main(string[] args)
        {
        }

        private static void SignIn()
        {
            _icloudAuthorizationService.SignIn();
        }

        private static void GetCalendar()
        {
            var getCalendarResponse = ICloudCalendarService.GetCalendar(_icloudAuthorizationService.Credential, "CalendarId");
        }

        private static void GetCalendars()
        {
            var getCalendarsResponse = ICloudCalendarService.GetCalendars(_icloudAuthorizationService.Credential);
        }

        private static void InsertCalendar()
        {
            var insertResponse = ICloudCalendarService.InsertCalendar(_icloudAuthorizationService.Credential, new CalendarListEntry()
            {
                Id = Guid.NewGuid().ToString(),
                Summary = "Calendar Description"
            });
        }

        private static void DeleteCalendar()
        {
            var deleteResponse = ICloudCalendarService.DeleteCalendar(_icloudAuthorizationService.Credential, "CalendarId");
        }

        private static void GetEvents()
        {
            var getEventsResponse = ICloudCalendarService.GetEvents(_icloudAuthorizationService.Credential, null, null, "CalendarId");
        }

        private static void GetEvent()
        {
            var getEventResponse = ICloudCalendarService.GetEvent(_icloudAuthorizationService.Credential, "CalendarId", "EventId");
        }

        private static void AddEvent()
        {
            var addEventResponse = ICloudCalendarService.AddEvent(_icloudAuthorizationService.Credential, new Event()
            {
                Uid = Guid.NewGuid().ToString(),
                Summary = "Event Description",
                Start = new CalDateTime(2021, 07, 08),
                End = new CalDateTime(2021, 07, 08)
            }, "CalendarId");
        }

        private static void UpdateEvent(Event @event)
        {
            var updateEventResponse = ICloudCalendarService.UpdateEvent(_icloudAuthorizationService.Credential, @event, "EventId", "CalendarId");
        }

        private static void DeleteEvent()
        {
            var deleteEventResponse = ICloudCalendarService.DeleteEvent(_icloudAuthorizationService.Credential, "CalendarId", "EventId");
        }
    }
}
