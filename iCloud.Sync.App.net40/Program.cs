using Ical.Net;
using Ical.Net.DataTypes;
using iCloud.Apis.Calendar;
using iCloud.Apis.People;
using iCloud.Integration.Implementation;
using System;
using System.Linq;
using System.Net;

namespace iCloud.Sync.App.net40
{
    internal class Program
    {
        private static readonly ICloudAuthorizationService _icloudAuthorizationService = new ICloudAuthorizationService("local.dataStore.userid", new NetworkCredential("Apple ID", "App-specific pass"));

        private static void Main()
        {
            SignIn();
            var identityCards = GetIdentityCards();
            GetContacts(identityCards.FirstOrDefault().ResourceName);
        }

        private static void SignIn()
        {
            _icloudAuthorizationService.SignIn();
        }

        private static void SignOut()
        {
            _icloudAuthorizationService.SignOut();
        }

        private static IdentityCardList GetIdentityCards()
        {
            var getIdentityCardsResponse = IdentityCardService.GetIdentityCards(_icloudAuthorizationService.Credential);
            return getIdentityCardsResponse;
        }

        private static void GetContacts(string resourceName)
        {
            var getContactsResponse = ICloudContactsSercive.GetContacts(_icloudAuthorizationService.Credential, resourceName);
        }

        private static void GetContact(string resourceName)
        {
            var getContactResponse = ICloudContactsSercive.GetContact(_icloudAuthorizationService.Credential, "Contact Unique ID", resourceName);
        }

        private static void AddContact(string resourceName)
        {
            var person = new Apis.People.Person();

            person.FormattedName = "Contact Description";
            person.FamilyName = "Contact Description";

            var addContactResponse = ICloudContactsSercive.AddContact(_icloudAuthorizationService.Credential, person, resourceName);
        }

        private static void UpdateContact(Apis.People.Person person, string resourceName)
        {
            person.FormattedName = "New Contact Description";
            person.FamilyName = "New Contact Description";

            var updateContactsResponse = ICloudContactsSercive.UpdateContact(_icloudAuthorizationService.Credential, person, person.UniqueId, person.Etag, resourceName);
        }

        private static void DeleteContact(string resourceName)
        {
            var getContactResponse = ICloudContactsSercive.DeleteContact(_icloudAuthorizationService.Credential, "Contact Unique Id", resourceName);
        }

        private static void GetContactGroups(string resourceName)
        {
            var getContactsResponse = ICloudContactGroupsService.GetContactGroups(_icloudAuthorizationService.Credential, resourceName);
        }

        private static void GetContactGroup(string resourceName)
        {
            var getContactGroupResponse = ICloudContactGroupsService.GetContactGroup(_icloudAuthorizationService.Credential, "A47000E6-81F8-4785-ABEB-624B597C9527", resourceName);
        }

        private static void AddContactGroup(string resourceName)
        {
            var getContactGroupResponse = ICloudContactGroupsService.AddContactGroup(_icloudAuthorizationService.Credential, new ContactGroup()
            {
                FamilyName = "New Contact Group Description",
                FormattedName = "New Contact Group Description"
            }, resourceName);
        }

        private static void UpdateContactGroup(ContactGroup contactGroup, string resourceName)
        {
            contactGroup.FamilyName = "New Contact Group Description";
            contactGroup.FormattedName = "New Contact Group Description";
            contactGroup.AddMemberResource("Contact Unique Id");

            var updateContactGroupResponse = ICloudContactGroupsService.UpdateContactGroup(_icloudAuthorizationService.Credential, contactGroup, "Contact Group Unique Id", resourceName);
        }

        private static void DeleteContactGroup(string resourceName)
        {
            var getContactGroupResponse = ICloudContactGroupsService.DeleteContactGroup(_icloudAuthorizationService.Credential, "Contact Group Unique Id", resourceName);
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
