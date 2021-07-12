using iCloud.Apis.Auth;
using iCloud.Apis.Core.Services;
using System.Collections.Generic;

namespace iCloud.Apis.Calendar
{
    public class CalendarService : BaseClientService
    {
        public const string Version = "v1";

        public CalendarService() : this(new Initializer())
        {
        }

        public CalendarService(Initializer initializer) : base(initializer)
        {
            CalendarList = new CalendarListResource(this);
            Events = new EventsResource(this);
            BaseUri = ((UserCredential)initializer.HttpClientInitializer).Token.Tokeninfo.CalendarPrincipal.PrincipalHomeSetUrl;
            BasePath = ((UserCredential)initializer.HttpClientInitializer).Token.Tokeninfo.CalendarPrincipal.HomeSetUrl;
        }

        public override string Name => "calendar";

        public override string BaseUri { get; }

        public override string BasePath { get; }

        public override IList<string> Features => new string[0];

        /// <summary>Gets the CalendarList resource.</summary>
        public virtual CalendarListResource CalendarList { get; }

        /// <summary>Gets the Events resource.</summary>
        public virtual EventsResource Events { get; }
    }
}