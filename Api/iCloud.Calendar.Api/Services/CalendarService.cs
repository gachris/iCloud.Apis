using iCloud.Apis.Auth;
using iCloud.Apis.Core.Services;
using System.Collections.Generic;

namespace iCloud.Apis.Calendar
{
    public class CalendarService : BaseClientService
    {
        public const string Version = "v1";

        public CalendarService() : this(new BaseClientService.Initializer())
        {
        }

        public CalendarService(Initializer initializer) : base(initializer)
        {
            //Acl = new AclResource(this);
            CalendarList = new CalendarListResource(this);
            //Calendars = new CalendarsResource(this);
            //Channels = new ChannelsResource(this);
            //Colors = new ColorsResource(this);
            Events = new EventsResource(this);
            //Freebusy = new FreebusyResource(this);
            //Settings = new SettingsResource(this);
            BaseUri = ((UserCredential)initializer.HttpClientInitializer).Token.Tokeninfo.CalendarPrincipal.PrincipalHomeSetUrl;
            BasePath = ((UserCredential)initializer.HttpClientInitializer).Token.Tokeninfo.CalendarPrincipal.HomeSetUrl;
        }

        public override string Name => "calendar";

        public override string BaseUri { get; }

        public override string BasePath { get; }

        public override IList<string> Features => new string[0];

        ///// <summary>Gets the Acl resource.</summary>
        //public virtual AclResource Acl { get; }

        /// <summary>Gets the CalendarList resource.</summary>
        public virtual CalendarListResource CalendarList { get; }

        ///// <summary>Gets the Calendars resource.</summary>
        //public virtual CalendarsResource Calendars { get; }

        ///// <summary>Gets the Channels resource.</summary>
        //public virtual ChannelsResource Channels { get; }

        ///// <summary>Gets the Colors resource.</summary>
        //public virtual ColorsResource Colors { get; }

        /// <summary>Gets the Events resource.</summary>
        public virtual EventsResource Events { get; }

        ///// <summary>Gets the Freebusy resource.</summary>
        //public virtual FreebusyResource Freebusy { get; }

        ///// <summary>Gets the Settings resource.</summary>
        //public virtual SettingsResource Settings { get; }
    }
}