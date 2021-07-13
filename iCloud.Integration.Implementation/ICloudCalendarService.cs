using Ical.Net;
using Ical.Net.Interfaces;
using iCloud.Apis.Auth;
using iCloud.Apis.Calendar;
using System;

namespace iCloud.Integration.Implementation
{
    public class ICloudCalendarService
    {
        public static CalendarListEntry InsertCalendar(UserCredential credential, CalendarListEntry calendarListEntry)
        {
            var service = GetService(credential);
            return service.CalendarList.Insert(calendarListEntry).Execute();
        }

        public static CalendarListEntry GetCalendar(UserCredential credential, string calendarId)
        {
            var service = GetService(credential);
            return service.CalendarList.Get(calendarId).Execute();
        }

        public static CalendarList GetCalendars(UserCredential credential)
        {
            var service = GetService(credential);
            return service.CalendarList.List().Execute();
        }

        public static string DeleteCalendar(UserCredential credential, string calendarId)
        {
            var service = GetService(credential);
            return service.CalendarList.Delete(calendarId).Execute();
        }

        public static CalendarListEntry UpdateCalendar(UserCredential credential, CalendarListEntry calendarListEntry, string calendarId)
        {
            var service = GetService(credential);
            return service.CalendarList.Update(calendarListEntry, calendarId).Execute();
        }

        public static IICalendarCollection GetEvents(UserCredential credential, DateTime? timeFrom, DateTime? timeTo, string calendarId, string nextPageToken = null)
        {
            var service = GetService(credential);
            EventsResource.ListRequest request = service.Events.List(calendarId);
            request.TimeMin = timeFrom;
            request.TimeMax = timeTo;
            request.ShowDeleted = false;
            request.PageToken = nextPageToken;
            request.MaxResults = 2500;
            request.SingleEvents = true;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            return request.Execute();
        }

        public static IICalendarCollection GetEvent(UserCredential credential, string calendarId, string eventId)
        {
            var service = GetService(credential);
            EventsResource.GetRequest request = service.Events.Get(calendarId, eventId);
            return request.Execute();
        }

        public static Event AddEvent(UserCredential credential, Event newEvent, string calendarId)
        {
            var service = GetService(credential);
            EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
            return request.Execute();
        }

        public static Event UpdateEvent(UserCredential credential, Event editEvent, string eventId, string calendarId)
        {
            var service = GetService(credential);
            EventsResource.UpdateRequest request = service.Events.Update(editEvent, calendarId, eventId);
            return request.Execute();
        }

        public static string DeleteEvent(UserCredential credential, string calendarId, string eventId)
        {
            var service = GetService(credential);
            EventsResource.DeleteRequest request = service.Events.Delete(calendarId, eventId);
            return request.Execute();
        }

        private static CalendarService GetService(UserCredential credential)
        {
            if (credential != null)
            {
                return new CalendarService(new iCloud.Apis.Core.Services.BaseClientService.Initializer()
                {
                    ApplicationName = "iCloud.SyncApp",
                    HttpClientInitializer = credential
                });
            }
            else throw new UnauthorizedAccessException();
        }
    }
}