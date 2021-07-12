using Ical.Net;
using Ical.Net.Interfaces;
using iCloud.Apis.Auth;
using iCloud.Apis.Calendar.Request;
using iCloud.Apis.Calendar.Types;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.Core.Services;
using iCloud.Apis.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iCloud.Apis.Calendar
{
    /// <summary>The "events" collection of methods.</summary>
    public class EventsResource
    {
        private const string Resource = "events";
        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService service;

        /// <summary>Constructs a new resource.</summary>
        public EventsResource(IClientService service)
        {
            this.service = service;
        }

        /// <summary>Deletes an event.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        /// <param name="eventId">Event identifier.</param>
        public virtual DeleteRequest Delete(string calendarId, string eventId)
        {
            return new DeleteRequest(this.service, calendarId, eventId);
        }

        /// <summary>Returns an event.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        /// <param name="eventId">Event identifier.</param>
        public virtual GetRequest Get(string calendarId, string eventId)
        {
            return new GetRequest(this.service, calendarId, eventId);
        }

        /// <summary>Imports an event. This operation is used to add a private copy of an existing event to a
        /// calendar.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual ImportRequest Import(Event body, string calendarId)
        {
            return new ImportRequest(this.service, body, calendarId);
        }

        /// <summary>Creates an event.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual InsertRequest Insert(Event body, string calendarId)
        {
            return new InsertRequest(this.service, body, calendarId);
        }

        /// <summary>Returns instances of the specified recurring event.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        /// <param name="eventId">Recurring event identifier.</param>
        public virtual InstancesRequest Instances(string calendarId, string eventId)
        {
            return new InstancesRequest(this.service, calendarId, eventId);
        }

        /// <summary>Returns events on the specified calendar.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual ListRequest List(string calendarId)
        {
            return new ListRequest(this.service, calendarId);
        }

        /// <summary>Moves an event to another calendar, i.e. changes an event's organizer.</summary>
        /// <param name="calendarId">Calendar identifier of the source calendar where the event currently is on.</param>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="destination">Calendar identifier of the target
        /// calendar where the event is to be moved to.</param>
        public virtual MoveRequest Move(string calendarId, string eventId, string destination)
        {
            return new MoveRequest(this.service, calendarId, eventId, destination);
        }

        /// <summary>Updates an event. This method supports patch semantics.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        /// <param name="eventId">Event identifier.</param>
        public virtual PatchRequest Patch(Event body, string calendarId, string eventId)
        {
            return new PatchRequest(this.service, body, calendarId, eventId);
        }

        /// <summary>Creates an event based on a simple text string.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        /// <param name="text">The text describing the event to be created.</param>
        public virtual QuickAddRequest QuickAdd(string calendarId, string text)
        {
            return new QuickAddRequest(this.service, calendarId, text);
        }

        /// <summary>Updates an event.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        /// <param name="eventId">Event identifier.</param>
        public virtual UpdateRequest Update(Event body, string calendarId, string eventId)
        {
            return new UpdateRequest(this.service, body, calendarId, eventId);
        }

        /// <summary>Watch for changes to Events resources.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual WatchRequest Watch(Channel body, string calendarId)
        {
            return new WatchRequest(this.service, body, calendarId);
        }

        /// <summary>Deletes an event.</summary>
        public class DeleteRequest : CalendarBaseServiceRequest<string>
        {
            /// <summary>Constructs a new Delete request.</summary>
            public DeleteRequest(IClientService service, string calendarId, string eventId) : base(service)
            {
                this.CalendarId = calendarId;
                this.EventId = eventId;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "delete";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.DELETE;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
                //{ "If-None-Match", etag },
                //{ "If-Match", "*" }
            };

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
            }
        }

        /// <summary>Returns an event.</summary>
        public class GetRequest : CalendarBaseServiceRequest<IICalendarCollection>
        {
            /// <summary>Constructs a new Get request.</summary>
            public GetRequest(IClientService service, string calendarId, string eventId) : base(service)
            {
                this.CalendarId = calendarId;
                this.EventId = eventId;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Deprecated and ignored. A value will always be returned in the email field for the organizer,
            /// creator and attendees, even if no real email address is available (i.e. a generated, non-working value
            /// will be provided).</summary>
            [RequestParameter("alwaysIncludeEmail", RequestParameterType.Query)]
            public virtual bool? AlwaysIncludeEmail { get; set; }

            /// <summary>The maximum number of attendees to include in the response. If there are more than the
            /// specified number of attendees, only the participant is returned. Optional.</summary>
            /// [minimum: 1]
            [RequestParameter("maxAttendees", RequestParameterType.Query)]
            public virtual int? MaxAttendees { get; set; }

            /// <summary>Time zone used in the response. Optional. The default is the time zone of the
            /// calendar.</summary>
            [RequestParameter("timeZone", RequestParameterType.Query)]
            public virtual string TimeZone { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.GET;

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.GET;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
            };

            /// <summary>Initializes Get parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("alwaysIncludeEmail", new Parameter()
                {
                    Name = "alwaysIncludeEmail",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxAttendees", new Parameter()
                {
                    Name = "maxAttendees",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeZone", new Parameter()
                {
                    Name = "timeZone",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            protected override async Task<IICalendarCollection> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    var input = await this.Service.DeserializeResponse(response).ConfigureAwait(false);
                    byte[] byteArray = Encoding.UTF8.GetBytes(input);
                    MemoryStream stream = new MemoryStream(byteArray);
                    return Ical.Net.Calendar.LoadFromStream<Ical.Net.Calendar>(stream);
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }
        }

        /// <summary>Imports an event. This operation is used to add a private copy of an existing event to a
        /// calendar.</summary>
        public class ImportRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>Constructs a new Import request.</summary>
            public ImportRequest(IClientService service, Event body, string calendarId)
              : base(service)
            {
                this.CalendarId = calendarId;
                this.Body = body;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Version number of conference data supported by the API client. Version 0 assumes no conference
            /// data support and ignores conference data in the event's body. Version 1 enables support for copying of
            /// ConferenceData as well as for creating new conferences using the createRequest field of conferenceData.
            /// The default is 0.</summary>
            /// 
            ///             [minimum: 0]
            ///             [maximum: 1]
            [RequestParameter("conferenceDataVersion", RequestParameterType.Query)]
            public virtual int? ConferenceDataVersion { get; set; }

            /// <summary>Whether API client performing operation supports event attachments. Optional. The default is
            /// False.</summary>
            [RequestParameter("supportsAttachments", RequestParameterType.Query)]
            public virtual bool? SupportsAttachments { get; set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Event Body { get; set; }

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                return Body;
            }

            /// <summary>Gets the method name.</summary>
            public override string MethodName
            {
                get
                {
                    return "import";
                }
            }

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod
            {
                get
                {
                    return "POST";
                }
            }

            /// <summary>Gets the REST path.</summary>
            public override string RestPath
            {
                get
                {
                    return "calendars/{calendarId}/events/import";
                }
            }

            /// <summary>Initializes Import parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("conferenceDataVersion", new Parameter()
                {
                    Name = "conferenceDataVersion",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("supportsAttachments", new Parameter()
                {
                    Name = "supportsAttachments",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }
        }

        /// <summary>Creates an event.</summary>
        public class InsertRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>Constructs a new Insert request.</summary>
            public InsertRequest(IClientService service, Event body, string calendarId) : base(service)
            {
                this.CalendarId = calendarId;
                if (String.IsNullOrEmpty(body.Uid))
                    body.Uid = Guid.NewGuid().ToString();
                this.EventId = body.Uid;
                this.Body = body;
                this.InitParameters();
            }

            /// <summary>Event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Version number of conference data supported by the API client. Version 0 assumes no conference
            /// data support and ignores conference data in the event's body. Version 1 enables support for copying of
            /// ConferenceData as well as for creating new conferences using the createRequest field of conferenceData.
            /// The default is 0.</summary>
            /// 
            ///             [minimum: 0]
            ///             [maximum: 1]
            [RequestParameter("conferenceDataVersion", RequestParameterType.Query)]
            public virtual int? ConferenceDataVersion { get; set; }

            /// <summary>The maximum number of attendees to include in the response. If there are more than the
            /// specified number of attendees, only the participant is returned. Optional.</summary>
            /// 
            ///             [minimum: 1]
            [RequestParameter("maxAttendees", RequestParameterType.Query)]
            public virtual int? MaxAttendees { get; set; }

            /// <summary>Deprecated. Please use sendUpdates instead.
            /// 
            /// Whether to send notifications about the creation of the new event. Note that some emails might still be
            /// sent even if you set the value to false. The default is false.</summary>
            [RequestParameter("sendNotifications", RequestParameterType.Query)]
            public virtual bool? SendNotifications { get; set; }

            /// <summary>Whether to send notifications about the creation of the new event. Note that some emails might
            /// still be sent. The default is false.</summary>
            [RequestParameter("sendUpdates", RequestParameterType.Query)]
            public virtual EventsResource.InsertRequest.SendUpdatesEnum? SendUpdates { get; set; }

            /// <summary>Whether API client performing operation supports event attachments. Optional. The default is
            /// False.</summary>
            [RequestParameter("supportsAttachments", RequestParameterType.Query)]
            public virtual bool? SupportsAttachments { get; set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Event Body { get; set; }

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                Ical.Net.Calendar calendar = new Ical.Net.Calendar();
                calendar.Events.Add(Body);
                return new Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer().SerializeToString(calendar);
            }

            protected override async Task<Event> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    var input = await this.Service.DeserializeResponse(response).ConfigureAwait(false);
                    //byte[] byteArray = Encoding.UTF8.GetBytes(input);
                    //MemoryStream stream = new MemoryStream(byteArray);
                    return default;
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "insert";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            /// <summary>Gets the HTTP method.</summary>
            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"}
            };

            /// <summary>Gets the Content-Type header of this request.</summary>
            public override string ContentType => ApiContentType.TEXT_CALENDAR;

            /// <summary>Initializes Insert parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("conferenceDataVersion", new Parameter()
                {
                    Name = "conferenceDataVersion",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxAttendees", new Parameter()
                {
                    Name = "maxAttendees",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendNotifications", new Parameter()
                {
                    Name = "sendNotifications",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendUpdates", new Parameter()
                {
                    Name = "sendUpdates",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("supportsAttachments", new Parameter()
                {
                    Name = "supportsAttachments",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            /// <summary>Whether to send notifications about the creation of the new event. Note that some emails might
            /// still be sent. The default is false.</summary>
            public enum SendUpdatesEnum
            {
                /// <summary>Notifications are sent to all guests.</summary>
                [StringValue("all")] All,
                /// <summary>Notifications are sent to non-icloud Calendar guests only.</summary>
                [StringValue("externalOnly")] ExternalOnly,
                /// <summary>No notifications are sent. This value should only be used for migration use cases (note
                /// that in most migration cases the import method should be used).</summary>
                [StringValue("none")] None,
            }
        }

        /// <summary>Returns instances of the specified recurring event.</summary>
        public class InstancesRequest : CalendarBaseServiceRequest<Ical.Net.Calendar>
        {
            /// <summary>Constructs a new Instances request.</summary>
            public InstancesRequest(IClientService service, string calendarId, string eventId)
              : base(service)
            {
                this.CalendarId = calendarId;
                this.EventId = eventId;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Recurring event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Deprecated and ignored. A value will always be returned in the email field for the organizer,
            /// creator and attendees, even if no real email address is available (i.e. a generated, non-working value
            /// will be provided).</summary>
            [RequestParameter("alwaysIncludeEmail", RequestParameterType.Query)]
            public virtual bool? AlwaysIncludeEmail { get; set; }

            /// <summary>The maximum number of attendees to include in the response. If there are more than the
            /// specified number of attendees, only the participant is returned. Optional.</summary>
            /// 
            ///             [minimum: 1]
            [RequestParameter("maxAttendees", RequestParameterType.Query)]
            public virtual int? MaxAttendees { get; set; }

            /// <summary>Maximum number of events returned on one result page. By default the value is 250 events. The
            /// page size can never be larger than 2500 events. Optional.</summary>
            /// 
            ///             [minimum: 1]
            [RequestParameter("maxResults", RequestParameterType.Query)]
            public virtual int? MaxResults { get; set; }

            /// <summary>The original start time of the instance in the result. Optional.</summary>
            [RequestParameter("originalStart", RequestParameterType.Query)]
            public virtual string OriginalStart { get; set; }

            /// <summary>Token specifying which result page to return. Optional.</summary>
            [RequestParameter("pageToken", RequestParameterType.Query)]
            public virtual string PageToken { get; set; }

            /// <summary>Whether to include deleted events (with status equals "cancelled") in the result. Cancelled
            /// instances of recurring events will still be included if singleEvents is False. Optional. The default is
            /// False.</summary>
            [RequestParameter("showDeleted", RequestParameterType.Query)]
            public virtual bool? ShowDeleted { get; set; }

            /// <summary>Upper bound (exclusive) for an event's start time to filter by. Optional. The default is not to
            /// filter by start time. Must be an RFC3339 timestamp with mandatory time zone offset.</summary>
            [RequestParameter("timeMax", RequestParameterType.Query)]
            public virtual DateTime? TimeMax { get; set; }

            /// <summary>Lower bound (inclusive) for an event's end time to filter by. Optional. The default is not to
            /// filter by end time. Must be an RFC3339 timestamp with mandatory time zone offset.</summary>
            [RequestParameter("timeMin", RequestParameterType.Query)]
            public virtual DateTime? TimeMin { get; set; }

            /// <summary>Time zone used in the response. Optional. The default is the time zone of the
            /// calendar.</summary>
            [RequestParameter("timeZone", RequestParameterType.Query)]
            public virtual string TimeZone { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName
            {
                get
                {
                    return "instances";
                }
            }

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod
            {
                get
                {
                    return "GET";
                }
            }

            /// <summary>Gets the REST path.</summary>
            public override string RestPath
            {
                get
                {
                    return "calendars/{calendarId}/events/{eventId}/instances";
                }
            }

            /// <summary>Initializes Instances parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("alwaysIncludeEmail", new Parameter()
                {
                    Name = "alwaysIncludeEmail",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxAttendees", new Parameter()
                {
                    Name = "maxAttendees",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxResults", new Parameter()
                {
                    Name = "maxResults",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("originalStart", new Parameter()
                {
                    Name = "originalStart",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("pageToken", new Parameter()
                {
                    Name = "pageToken",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("showDeleted", new Parameter()
                {
                    Name = "showDeleted",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeMax", new Parameter()
                {
                    Name = "timeMax",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeMin", new Parameter()
                {
                    Name = "timeMin",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeZone", new Parameter()
                {
                    Name = "timeZone",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }
        }

        /// <summary>Returns events on the specified calendar.</summary>
        public class ListRequest : CalendarBaseServiceRequest<IICalendarCollection>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service, string calendarId) : base(service)
            {
                this.CalendarId = calendarId;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Deprecated and ignored. A value will always be returned in the email field for the organizer,
            /// creator and attendees, even if no real email address is available (i.e. a generated, non-working value
            /// will be provided).</summary>
            [RequestParameter("alwaysIncludeEmail", RequestParameterType.Query)]
            public virtual bool? AlwaysIncludeEmail { get; set; }

            /// <summary>Specifies event ID in the iCalendar format to be included in the response. Optional.</summary>
            [RequestParameter("iCalUID", RequestParameterType.Query)]
            public virtual string ICalUID { get; set; }

            /// <summary>The maximum number of attendees to include in the response. If there are more than the
            /// specified number of attendees, only the participant is returned. Optional.</summary>
            /// 
            ///             [minimum: 1]
            [RequestParameter("maxAttendees", RequestParameterType.Query)]
            public virtual int? MaxAttendees { get; set; }

            /// <summary>Maximum number of events returned on one result page. The number of events in the resulting
            /// page may be less than this value, or none at all, even if there are more events matching the query.
            /// Incomplete pages can be detected by a non-empty nextPageToken field in the response. By default the
            /// value is 250 events. The page size can never be larger than 2500 events. Optional.</summary>
            /// 
            ///             [default: 250]
            ///             [minimum: 1]
            [RequestParameter("maxResults", RequestParameterType.Query)]
            public virtual int? MaxResults { get; set; }

            /// <summary>The order of the events returned in the result. Optional. The default is an unspecified, stable
            /// order.</summary>
            [RequestParameter("orderBy", RequestParameterType.Query)]
            public virtual EventsResource.ListRequest.OrderByEnum? OrderBy { get; set; }

            /// <summary>Token specifying which result page to return. Optional.</summary>
            [RequestParameter("pageToken", RequestParameterType.Query)]
            public virtual string PageToken { get; set; }

            /// <summary>Extended properties constraint specified as propertyName=value. Matches only private
            /// properties. This parameter might be repeated multiple times to return events that match all given
            /// constraints.</summary>
            [RequestParameter("privateExtendedProperty", RequestParameterType.Query)]
            public virtual Repeatable<string> PrivateExtendedProperty { get; set; }

            /// <summary>Free text search terms to find events that match these terms in any field, except for extended
            /// properties. Optional.</summary>
            [RequestParameter("q", RequestParameterType.Query)]
            public virtual string Q { get; set; }

            /// <summary>Extended properties constraint specified as propertyName=value. Matches only shared properties.
            /// This parameter might be repeated multiple times to return events that match all given
            /// constraints.</summary>
            [RequestParameter("sharedExtendedProperty", RequestParameterType.Query)]
            public virtual Repeatable<string> SharedExtendedProperty { get; set; }

            /// <summary>Whether to include deleted events (with status equals "cancelled") in the result. Cancelled
            /// instances of recurring events (but not the underlying recurring event) will still be included if
            /// showDeleted and singleEvents are both False. If showDeleted and singleEvents are both True, only single
            /// instances of deleted events (but not the underlying recurring events) are returned. Optional. The
            /// default is False.</summary>
            [RequestParameter("showDeleted", RequestParameterType.Query)]
            public virtual bool? ShowDeleted { get; set; }

            /// <summary>Whether to include hidden invitations in the result. Optional. The default is False.</summary>
            [RequestParameter("showHiddenInvitations", RequestParameterType.Query)]
            public virtual bool? ShowHiddenInvitations { get; set; }

            /// <summary>Whether to expand recurring events into instances and only return single one-off events and
            /// instances of recurring events, but not the underlying recurring events themselves. Optional. The default
            /// is False.</summary>
            [RequestParameter("singleEvents", RequestParameterType.Query)]
            public virtual bool? SingleEvents { get; set; }

            /// <summary>Token obtained from the nextSyncToken field returned on the last page of results from the
            /// previous list request. It makes the result of this list request contain only entries that have changed
            /// since then. All events deleted since the previous list request will always be in the result set and it
            /// is not allowed to set showDeleted to False. There are several query parameters that cannot be specified
            /// together with nextSyncToken to ensure consistency of the client state.
            /// 
            /// These are: - iCalUID - orderBy - privateExtendedProperty - q - sharedExtendedProperty - timeMin -
            /// timeMax - updatedMin If the syncToken expires, the server will respond with a 410 GONE response code and
            /// the client should clear its storage and perform a full synchronization without any syncToken. Learn more
            /// about incremental synchronization. Optional. The default is to return all entries.</summary>
            [RequestParameter("syncToken", RequestParameterType.Query)]
            public virtual string SyncToken { get; set; }

            /// <summary>Upper bound (exclusive) for an event's start time to filter by. Optional. The default is not to
            /// filter by start time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
            /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
            /// timeMin is set, timeMax must be greater than timeMin.</summary>
            [RequestParameter("timeMax", RequestParameterType.Query)]
            public virtual DateTime? TimeMax { get; set; }

            /// <summary>Lower bound (exclusive) for an event's end time to filter by. Optional. The default is not to
            /// filter by end time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
            /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
            /// timeMax is set, timeMin must be smaller than timeMax.</summary>
            [RequestParameter("timeMin", RequestParameterType.Query)]
            public virtual DateTime? TimeMin { get; set; }

            /// <summary>Time zone used in the response. Optional. The default is the time zone of the
            /// calendar.</summary>
            [RequestParameter("timeZone", RequestParameterType.Query)]
            public virtual string TimeZone { get; set; }

            /// <summary>Lower bound for an event's last modification time (as a RFC3339 timestamp) to filter by. When
            /// specified, entries deleted since this time will always be included regardless of showDeleted. Optional.
            /// The default is not to filter by last modification time.</summary>
            [RequestParameter("updatedMin", RequestParameterType.Query)]
            public virtual DateTime? UpdatedMin { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "list";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.REPORT;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            /// <summary>Gets the HTTP method.</summary>
            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
            };

            protected override object GetBody()
            {
                //if (start != null)
                //{
                //    calendarquery.Filter.Compfilter.Child.Timerange = new Timerange { Start = start.ToStartTime() };

                //    if (end != null)
                //        calendarquery.Filter.Compfilter.Child.Timerange.End = end.ToEndTime();
                //}

                return new Calendarquery()
                {
                    Prop = new Prop() { Calendardata = Calendardata.Empty, Getetag = Getetag.Empty },
                    Filter = new Filter() { Compfilter = new Compfilter() { Name = "VCALENDAR", Child = new Compfilter() { Name = "VEVENT" } } }
                };
            }

            protected override async Task<IICalendarCollection> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    Ical.Net.Calendar events = new Ical.Net.Calendar();
                    string calendarData = string.Empty;
                    var multistatus = await this.Service.DeserializeResponse<Multistatus<Prop>>(response).ConfigureAwait(false);
                    foreach (var multistatusItem in multistatus.Responses)
                    {
                        if (multistatusItem.Propstat != null && multistatusItem.Propstat.Prop != null && multistatusItem.Propstat.Prop.Calendardata != null)
                            calendarData += multistatusItem.Propstat.Prop.Calendardata.Value;
                    }
                    byte[] byteArray = Encoding.UTF8.GetBytes(calendarData);
                    MemoryStream stream = new MemoryStream(byteArray);
                    return Ical.Net.Calendar.LoadFromStream<Ical.Net.Calendar>(stream);
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }

            /// <summary>Initializes List parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("alwaysIncludeEmail", new Parameter()
                {
                    Name = "alwaysIncludeEmail",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("iCalUID", new Parameter()
                {
                    Name = "iCalUID",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxAttendees", new Parameter()
                {
                    Name = "maxAttendees",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxResults", new Parameter()
                {
                    Name = "maxResults",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = "250",
                    Pattern = null
                });
                this.RequestParameters.Add("orderBy", new Parameter()
                {
                    Name = "orderBy",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("pageToken", new Parameter()
                {
                    Name = "pageToken",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("privateExtendedProperty", new Parameter()
                {
                    Name = "privateExtendedProperty",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("q", new Parameter()
                {
                    Name = "q",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sharedExtendedProperty", new Parameter()
                {
                    Name = "sharedExtendedProperty",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("showDeleted", new Parameter()
                {
                    Name = "showDeleted",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("showHiddenInvitations", new Parameter()
                {
                    Name = "showHiddenInvitations",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("singleEvents", new Parameter()
                {
                    Name = "singleEvents",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("syncToken", new Parameter()
                {
                    Name = "syncToken",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeMax", new Parameter()
                {
                    Name = "timeMax",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeMin", new Parameter()
                {
                    Name = "timeMin",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeZone", new Parameter()
                {
                    Name = "timeZone",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("updatedMin", new Parameter()
                {
                    Name = "updatedMin",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            /// <summary>The order of the events returned in the result. Optional. The default is an unspecified, stable
            /// order.</summary>
            public enum OrderByEnum
            {
                /// <summary>Order by the start date/time (ascending). This is only available when querying single
                /// events (i.e. the parameter singleEvents is True)</summary>
                [StringValue("startTime")] StartTime,
                /// <summary>Order by last modification time (ascending).</summary>
                [StringValue("updated")] Updated,
            }
        }

        /// <summary>Moves an event to another calendar, i.e. changes an event's organizer.</summary>
        public class MoveRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>Constructs a new Move request.</summary>
            public MoveRequest(
              IClientService service,
              string calendarId,
              string eventId,
              string destination)
              : base(service)
            {
                this.CalendarId = calendarId;
                this.EventId = eventId;
                this.Destination = destination;
                this.InitParameters();
            }

            /// <summary>Calendar identifier of the source calendar where the event currently is on.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Calendar identifier of the target calendar where the event is to be moved to.</summary>
            [RequestParameter("destination", RequestParameterType.Query)]
            public virtual string Destination { get; private set; }

            /// <summary>Deprecated. Please use sendUpdates instead.
            /// 
            /// Whether to send notifications about the change of the event's organizer. Note that some emails might
            /// still be sent even if you set the value to false. The default is false.</summary>
            [RequestParameter("sendNotifications", RequestParameterType.Query)]
            public virtual bool? SendNotifications { get; set; }

            /// <summary>Guests who should receive notifications about the change of the event's organizer.</summary>
            [RequestParameter("sendUpdates", RequestParameterType.Query)]
            public virtual EventsResource.MoveRequest.SendUpdatesEnum? SendUpdates { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName
            {
                get
                {
                    return "move";
                }
            }

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod
            {
                get
                {
                    return "POST";
                }
            }

            /// <summary>Gets the REST path.</summary>
            public override string RestPath
            {
                get
                {
                    return "calendars/{calendarId}/events/{eventId}/move";
                }
            }

            /// <summary>Initializes Move parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("destination", new Parameter()
                {
                    Name = "destination",
                    IsRequired = true,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendNotifications", new Parameter()
                {
                    Name = "sendNotifications",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendUpdates", new Parameter()
                {
                    Name = "sendUpdates",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            /// <summary>Guests who should receive notifications about the change of the event's organizer.</summary>
            public enum SendUpdatesEnum
            {
                /// <summary>Notifications are sent to all guests.</summary>
                [StringValue("all")] All,
                /// <summary>Notifications are sent to non-icloud Calendar guests only.</summary>
                [StringValue("externalOnly")] ExternalOnly,
                /// <summary>No notifications are sent. This value should only be used for migration use cases (note
                /// that in most migration cases the import method should be used).</summary>
                [StringValue("none")] None,
            }
        }

        /// <summary>Updates an event. This method supports patch semantics.</summary>
        public class PatchRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>Constructs a new Patch request.</summary>
            public PatchRequest(IClientService service, Event body, string calendarId, string eventId)
              : base(service)
            {
                this.CalendarId = calendarId;
                this.EventId = eventId;
                this.Body = body;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Deprecated and ignored. A value will always be returned in the email field for the organizer,
            /// creator and attendees, even if no real email address is available (i.e. a generated, non-working value
            /// will be provided).</summary>
            [RequestParameter("alwaysIncludeEmail", RequestParameterType.Query)]
            public virtual bool? AlwaysIncludeEmail { get; set; }

            /// <summary>Version number of conference data supported by the API client. Version 0 assumes no conference
            /// data support and ignores conference data in the event's body. Version 1 enables support for copying of
            /// ConferenceData as well as for creating new conferences using the createRequest field of conferenceData.
            /// The default is 0.</summary>
            /// 
            ///             [minimum: 0]
            ///             [maximum: 1]
            [RequestParameter("conferenceDataVersion", RequestParameterType.Query)]
            public virtual int? ConferenceDataVersion { get; set; }

            /// <summary>The maximum number of attendees to include in the response. If there are more than the
            /// specified number of attendees, only the participant is returned. Optional.</summary>
            /// 
            ///             [minimum: 1]
            [RequestParameter("maxAttendees", RequestParameterType.Query)]
            public virtual int? MaxAttendees { get; set; }

            /// <summary>Deprecated. Please use sendUpdates instead.
            /// 
            /// Whether to send notifications about the event update (for example, description changes, etc.). Note that
            /// some emails might still be sent even if you set the value to false. The default is false.</summary>
            [RequestParameter("sendNotifications", RequestParameterType.Query)]
            public virtual bool? SendNotifications { get; set; }

            /// <summary>Guests who should receive notifications about the event update (for example, title changes,
            /// etc.).</summary>
            [RequestParameter("sendUpdates", RequestParameterType.Query)]
            public virtual EventsResource.PatchRequest.SendUpdatesEnum? SendUpdates { get; set; }

            /// <summary>Whether API client performing operation supports event attachments. Optional. The default is
            /// False.</summary>
            [RequestParameter("supportsAttachments", RequestParameterType.Query)]
            public virtual bool? SupportsAttachments { get; set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Event Body { get; set; }

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                return Body;
            }

            /// <summary>Gets the method name.</summary>
            public override string MethodName
            {
                get
                {
                    return "patch";
                }
            }

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod
            {
                get
                {
                    return "PATCH";
                }
            }

            /// <summary>Gets the REST path.</summary>
            public override string RestPath
            {
                get
                {
                    return "calendars/{calendarId}/events/{eventId}";
                }
            }

            /// <summary>Initializes Patch parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("alwaysIncludeEmail", new Parameter()
                {
                    Name = "alwaysIncludeEmail",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("conferenceDataVersion", new Parameter()
                {
                    Name = "conferenceDataVersion",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxAttendees", new Parameter()
                {
                    Name = "maxAttendees",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendNotifications", new Parameter()
                {
                    Name = "sendNotifications",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendUpdates", new Parameter()
                {
                    Name = "sendUpdates",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("supportsAttachments", new Parameter()
                {
                    Name = "supportsAttachments",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            /// <summary>Guests who should receive notifications about the event update (for example, title changes,
            /// etc.).</summary>
            public enum SendUpdatesEnum
            {
                /// <summary>Notifications are sent to all guests.</summary>
                [StringValue("all")] All,
                /// <summary>Notifications are sent to non-icloud Calendar guests only.</summary>
                [StringValue("externalOnly")] ExternalOnly,
                /// <summary>No notifications are sent. This value should only be used for migration use cases (note
                /// that in most migration cases the import method should be used).</summary>
                [StringValue("none")] None,
            }
        }

        /// <summary>Creates an event based on a simple text string.</summary>
        public class QuickAddRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>Constructs a new QuickAdd request.</summary>
            public QuickAddRequest(IClientService service, string calendarId, string text)
              : base(service)
            {
                this.CalendarId = calendarId;
                this.Text = text;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>The text describing the event to be created.</summary>
            [RequestParameter("text", RequestParameterType.Query)]
            public virtual string Text { get; private set; }

            /// <summary>Deprecated. Please use sendUpdates instead.
            /// 
            /// Whether to send notifications about the creation of the event. Note that some emails might still be sent
            /// even if you set the value to false. The default is false.</summary>
            [RequestParameter("sendNotifications", RequestParameterType.Query)]
            public virtual bool? SendNotifications { get; set; }

            /// <summary>Guests who should receive notifications about the creation of the new event.</summary>
            [RequestParameter("sendUpdates", RequestParameterType.Query)]
            public virtual EventsResource.QuickAddRequest.SendUpdatesEnum? SendUpdates { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName
            {
                get
                {
                    return "quickAdd";
                }
            }

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod
            {
                get
                {
                    return "POST";
                }
            }

            /// <summary>Gets the REST path.</summary>
            public override string RestPath
            {
                get
                {
                    return "calendars/{calendarId}/events/quickAdd";
                }
            }

            /// <summary>Initializes QuickAdd parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("text", new Parameter()
                {
                    Name = "text",
                    IsRequired = true,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendNotifications", new Parameter()
                {
                    Name = "sendNotifications",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sendUpdates", new Parameter()
                {
                    Name = "sendUpdates",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            /// <summary>Guests who should receive notifications about the creation of the new event.</summary>
            public enum SendUpdatesEnum
            {
                /// <summary>Notifications are sent to all guests.</summary>
                [StringValue("all")] All,
                /// <summary>Notifications are sent to non-icloud Calendar guests only.</summary>
                [StringValue("externalOnly")] ExternalOnly,
                /// <summary>No notifications are sent. This value should only be used for migration use cases (note
                /// that in most migration cases the import method should be used).</summary>
                [StringValue("none")] None,
            }
        }

        /// <summary>Updates an event.</summary>
        public class UpdateRequest : CalendarBaseServiceRequest<Event>
        {
            /// <summary>Constructs a new Update request.</summary>
            public UpdateRequest(IClientService service, Event body, string calendarId, string eventId) : base(service)
            {
                this.CalendarId = calendarId;
                this.EventId = eventId;
                this.Body = body;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Event identifier.</summary>
            [RequestParameter("eventId", RequestParameterType.Path)]
            public virtual string EventId { get; private set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Event Body { get; set; }

            /// <summary>Gets the method name.</summary>
            public override string MethodName => "update";

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            /// <summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}/{eventId}.ics";

            /// <summary>Gets the Content-Type header of this request.</summary>
            public override string ContentType => ApiContentType.TEXT_CALENDAR;

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                Ical.Net.Calendar calendar = new Ical.Net.Calendar();
                calendar.Events.Add(Body);
                return new Ical.Net.Serialization.iCalendar.Serializers.CalendarSerializer().SerializeToString(calendar);
            }

            protected override async Task<Event> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    var input = await this.Service.DeserializeResponse(response).ConfigureAwait(false);
                    //byte[] byteArray = Encoding.UTF8.GetBytes(input);
                    //MemoryStream stream = new MemoryStream(byteArray);
                    return default;
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }

            /// <summary>Initializes Update parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("eventId", new Parameter()
                {
                    Name = "eventId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
            }
        }

        /// <summary>Watch for changes to Events resources.</summary>
        public class WatchRequest : CalendarBaseServiceRequest<Channel>
        {
            /// <summary>Constructs a new Watch request.</summary>
            public WatchRequest(IClientService service, Channel body, string calendarId)
              : base(service)
            {
                this.CalendarId = calendarId;
                this.Body = body;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Deprecated and ignored. A value will always be returned in the email field for the organizer,
            /// creator and attendees, even if no real email address is available (i.e. a generated, non-working value
            /// will be provided).</summary>
            [RequestParameter("alwaysIncludeEmail", RequestParameterType.Query)]
            public virtual bool? AlwaysIncludeEmail { get; set; }

            /// <summary>Specifies event ID in the iCalendar format to be included in the response. Optional.</summary>
            [RequestParameter("iCalUID", RequestParameterType.Query)]
            public virtual string ICalUID { get; set; }

            /// <summary>The maximum number of attendees to include in the response. If there are more than the
            /// specified number of attendees, only the participant is returned. Optional.</summary>
            /// 
            ///             [minimum: 1]
            [RequestParameter("maxAttendees", RequestParameterType.Query)]
            public virtual int? MaxAttendees { get; set; }

            /// <summary>Maximum number of events returned on one result page. The number of events in the resulting
            /// page may be less than this value, or none at all, even if there are more events matching the query.
            /// Incomplete pages can be detected by a non-empty nextPageToken field in the response. By default the
            /// value is 250 events. The page size can never be larger than 2500 events. Optional.</summary>
            /// 
            ///             [default: 250]
            ///             [minimum: 1]
            [RequestParameter("maxResults", RequestParameterType.Query)]
            public virtual int? MaxResults { get; set; }

            /// <summary>The order of the events returned in the result. Optional. The default is an unspecified, stable
            /// order.</summary>
            [RequestParameter("orderBy", RequestParameterType.Query)]
            public virtual EventsResource.WatchRequest.OrderByEnum? OrderBy { get; set; }

            /// <summary>Token specifying which result page to return. Optional.</summary>
            [RequestParameter("pageToken", RequestParameterType.Query)]
            public virtual string PageToken { get; set; }

            /// <summary>Extended properties constraint specified as propertyName=value. Matches only private
            /// properties. This parameter might be repeated multiple times to return events that match all given
            /// constraints.</summary>
            [RequestParameter("privateExtendedProperty", RequestParameterType.Query)]
            public virtual Repeatable<string> PrivateExtendedProperty { get; set; }

            /// <summary>Free text search terms to find events that match these terms in any field, except for extended
            /// properties. Optional.</summary>
            [RequestParameter("q", RequestParameterType.Query)]
            public virtual string Q { get; set; }

            /// <summary>Extended properties constraint specified as propertyName=value. Matches only shared properties.
            /// This parameter might be repeated multiple times to return events that match all given
            /// constraints.</summary>
            [RequestParameter("sharedExtendedProperty", RequestParameterType.Query)]
            public virtual Repeatable<string> SharedExtendedProperty { get; set; }

            /// <summary>Whether to include deleted events (with status equals "cancelled") in the result. Cancelled
            /// instances of recurring events (but not the underlying recurring event) will still be included if
            /// showDeleted and singleEvents are both False. If showDeleted and singleEvents are both True, only single
            /// instances of deleted events (but not the underlying recurring events) are returned. Optional. The
            /// default is False.</summary>
            [RequestParameter("showDeleted", RequestParameterType.Query)]
            public virtual bool? ShowDeleted { get; set; }

            /// <summary>Whether to include hidden invitations in the result. Optional. The default is False.</summary>
            [RequestParameter("showHiddenInvitations", RequestParameterType.Query)]
            public virtual bool? ShowHiddenInvitations { get; set; }

            /// <summary>Whether to expand recurring events into instances and only return single one-off events and
            /// instances of recurring events, but not the underlying recurring events themselves. Optional. The default
            /// is False.</summary>
            [RequestParameter("singleEvents", RequestParameterType.Query)]
            public virtual bool? SingleEvents { get; set; }

            /// <summary>Token obtained from the nextSyncToken field returned on the last page of results from the
            /// previous list request. It makes the result of this list request contain only entries that have changed
            /// since then. All events deleted since the previous list request will always be in the result set and it
            /// is not allowed to set showDeleted to False. There are several query parameters that cannot be specified
            /// together with nextSyncToken to ensure consistency of the client state.
            /// 
            /// These are: - iCalUID - orderBy - privateExtendedProperty - q - sharedExtendedProperty - timeMin -
            /// timeMax - updatedMin If the syncToken expires, the server will respond with a 410 GONE response code and
            /// the client should clear its storage and perform a full synchronization without any syncToken. Learn more
            /// about incremental synchronization. Optional. The default is to return all entries.</summary>
            [RequestParameter("syncToken", RequestParameterType.Query)]
            public virtual string SyncToken { get; set; }

            /// <summary>Upper bound (exclusive) for an event's start time to filter by. Optional. The default is not to
            /// filter by start time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
            /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
            /// timeMin is set, timeMax must be greater than timeMin.</summary>
            [RequestParameter("timeMax", RequestParameterType.Query)]
            public virtual DateTime? TimeMax { get; set; }

            /// <summary>Lower bound (exclusive) for an event's end time to filter by. Optional. The default is not to
            /// filter by end time. Must be an RFC3339 timestamp with mandatory time zone offset, for example,
            /// 2011-06-03T10:00:00-07:00, 2011-06-03T10:00:00Z. Milliseconds may be provided but are ignored. If
            /// timeMax is set, timeMin must be smaller than timeMax.</summary>
            [RequestParameter("timeMin", RequestParameterType.Query)]
            public virtual DateTime? TimeMin { get; set; }

            /// <summary>Time zone used in the response. Optional. The default is the time zone of the
            /// calendar.</summary>
            [RequestParameter("timeZone", RequestParameterType.Query)]
            public virtual string TimeZone { get; set; }

            /// <summary>Lower bound for an event's last modification time (as a RFC3339 timestamp) to filter by. When
            /// specified, entries deleted since this time will always be included regardless of showDeleted. Optional.
            /// The default is not to filter by last modification time.</summary>
            [RequestParameter("updatedMin", RequestParameterType.Query)]
            public virtual DateTime? UpdatedMin { get; set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Channel Body { get; set; }

            /// <summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                return Body;
            }

            /// <summary>Gets the method name.</summary>
            public override string MethodName
            {
                get
                {
                    return "watch";
                }
            }

            /// <summary>Gets the HTTP method.</summary>
            public override string HttpMethod
            {
                get
                {
                    return "POST";
                }
            }

            /// <summary>Gets the REST path.</summary>
            public override string RestPath
            {
                get
                {
                    return "calendars/{calendarId}/events/watch";
                }
            }

            /// <summary>Initializes Watch parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                this.RequestParameters.Add("calendarId", new Parameter()
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("alwaysIncludeEmail", new Parameter()
                {
                    Name = "alwaysIncludeEmail",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("iCalUID", new Parameter()
                {
                    Name = "iCalUID",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxAttendees", new Parameter()
                {
                    Name = "maxAttendees",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("maxResults", new Parameter()
                {
                    Name = "maxResults",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = "250",
                    Pattern = null
                });
                this.RequestParameters.Add("orderBy", new Parameter()
                {
                    Name = "orderBy",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("pageToken", new Parameter()
                {
                    Name = "pageToken",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("privateExtendedProperty", new Parameter()
                {
                    Name = "privateExtendedProperty",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("q", new Parameter()
                {
                    Name = "q",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("sharedExtendedProperty", new Parameter()
                {
                    Name = "sharedExtendedProperty",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("showDeleted", new Parameter()
                {
                    Name = "showDeleted",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("showHiddenInvitations", new Parameter()
                {
                    Name = "showHiddenInvitations",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("singleEvents", new Parameter()
                {
                    Name = "singleEvents",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("syncToken", new Parameter()
                {
                    Name = "syncToken",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeMax", new Parameter()
                {
                    Name = "timeMax",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeMin", new Parameter()
                {
                    Name = "timeMin",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("timeZone", new Parameter()
                {
                    Name = "timeZone",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
                this.RequestParameters.Add("updatedMin", new Parameter()
                {
                    Name = "updatedMin",
                    IsRequired = false,
                    ParameterType = "query",
                    DefaultValue = null,
                    Pattern = null
                });
            }

            /// <summary>The order of the events returned in the result. Optional. The default is an unspecified, stable
            /// order.</summary>
            public enum OrderByEnum
            {
                /// <summary>Order by the start date/time (ascending). This is only available when querying single
                /// events (i.e. the parameter singleEvents is True)</summary>
                [StringValue("startTime")] StartTime,
                /// <summary>Order by last modification time (ascending).</summary>
                [StringValue("updated")] Updated,
            }
        }
    }
}
