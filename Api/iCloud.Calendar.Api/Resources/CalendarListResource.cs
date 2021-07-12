using iCloud.Apis.Auth;
using iCloud.Apis.Calendar.Request;
using iCloud.Apis.Calendar.Types;
using iCloud.Apis.Core.Request;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.Core.Services;
using iCloud.Apis.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCloud.Apis.Calendar
{
    /// <summary>The "calendarList" collection of methods.</summary>
    public class CalendarListResource
    {
        private const string Resource = "calendarList";

        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService service;

        /// <summary>Constructs a new resource.</summary>
        public CalendarListResource(IClientService service)
        {
            this.service = service;
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual DeleteRequest Delete(string calendarId)
        {
            return new DeleteRequest(service, calendarId);
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual GetRequest Get(string calendarId)
        {
            return new GetRequest(service, calendarId);
        }

        /// <summary>Inserts an existing calendar into the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(CalendarListEntry body)
        {
            return new InsertRequest(service, body);
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public virtual ListRequest List()
        {
            return new ListRequest(service);
        }

        /// <summary>Updates an existing calendar on the user's calendar list. This method supports patch
        /// semantics.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual PatchRequest Patch(CalendarListEntry body, string calendarId)
        {
            return new PatchRequest(service, body, calendarId);
        }

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual UpdateRequest Update(CalendarListEntry body, string calendarId)
        {
            return new UpdateRequest(service, body, calendarId);
        }

        /// <summary>Watch for changes to CalendarList resources.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual WatchRequest Watch(object body)
        {
            return new WatchRequest(service, body);
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        public class DeleteRequest : CalendarBaseServiceRequest<string>
        {
            /// <summary>Constructs a new Delete request.</summary>
            public DeleteRequest(IClientService service, string calendarId) : base(service)
            {
                CalendarId = calendarId;
                InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.DELETE;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.DELETE;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
            };

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add(
                    "calendarId", new Parameter
                    {
                        Name = "calendarId",
                        IsRequired = true,
                        ParameterType = "path",
                        DefaultValue = null,
                        Pattern = null,
                    });
            }
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        public class GetRequest : CalendarBaseServiceRequest<CalendarListEntry>
        {
            /// <summary>Constructs a new Get request.</summary>
            public GetRequest(IClientService service, string calendarId) : base(service)
            {
                CalendarId = calendarId;
                InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.PROPFIND;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPFIND;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            protected override object GetBody()
            {
                var calendar = new Propfind<Prop>()
                {
                    Prop = new Prop
                    {
                        Displayname = Displayname.Empty,
                        Getctag = Getctag.Empty,
                        Currentuserprivilegeset = Currentuserprivilegeset.Empty,
                        Value = PropValue.Calendardata,
                    }
                };
                return XmlSerializerHelper.Serialize(calendar);
            }

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
            };

            /// <summary>Initializes Get parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null,
                });
            }

            protected override async Task<CalendarListEntry> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    Multistatus<Prop> multistatus = await this.Service.DeserializeResponse<Multistatus<Prop>>(response);
                    if (multistatus != null)
                    {
                        try
                        {
                            TypeConverter converter = TypeDescriptor.GetConverter(typeof(CalendarListEntry));
                            bool canConvertFrom = converter.CanConvertFrom(multistatus.GetType());
                            if (canConvertFrom)
                                return (CalendarListEntry)converter.ConvertFrom(multistatus);
                            else return default;
                        }
                        catch (InvalidCastException)
                        {
                            return default;
                        }
                    }
                    else return default;
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }
        }

        /// <summary>Inserts an existing calendar into the user's calendar list.</summary>
        public class InsertRequest : CalendarBaseServiceRequest<CalendarListEntry>
        {
            /// <summary>Constructs a new Insert request.</summary>
            public InsertRequest(IClientService service, CalendarListEntry body) : base(service)
            {
                Body = body;
                if (string.IsNullOrEmpty(Body.Id))
                    Body.Id = Guid.NewGuid().ToString();
                CalendarId = Body.Id;
                InitParameters();
            }

            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Whether to use the foregroundColor and backgroundColor fields to write the calendar colors
            /// (RGB). If this feature is used, the index-based colorId field will be set to the best matching option
            /// automatically. Optional. The default is False.</summary>
            //[RequestParameter("colorRgbFormat", RequestParameterType.Query)]
            //public virtual bool? ColorRgbFormat { get; set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private CalendarListEntry Body { get; set; }

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                var mkCalendar = new MkCalendar()
                {
                    Set = new Set()
                    {
                        Prop = new Prop()
                        {
                            Displayname = new Displayname() { Value = Body.Summary }
                        }
                    }
                };
                return XmlSerializerHelper.Serialize(mkCalendar);
            }

            //         <?xml version = "1.0" encoding="utf-8" ?>
            //<C:mkcalendar xmlns:D="DAV:" xmlns:C="urn:ietf:params:xml:ns:caldav">
            //  <D:set>
            //    <D:prop>
            //      <D:displayname>Lisa's Events</D:displayname>
            //      <C:calendar-description xml:lang="en">Calendar restricted to events.</C:calendar-description>
            //      <C:supported-calendar-component-set>
            //         <C:comp name = "VEVENT" />
            //      </ C:supported-calendar-component-set>
            //      <C:calendar-timezone><![CDATA[BEGIN:VCALENDAR
            //PRODID:-//Example Corp.//CalDAV Client//EN
            //VERSION:2.0
            //BEGIN:VTIMEZONE
            //TZID:US-Eastern
            //LAST-MODIFIED:19870101T000000Z
            //BEGIN:STANDARD
            //DTSTART:19671029T020000
            //RRULE:FREQ=YEARLY; BYDAY=-1SU;BYMONTH=10
            //TZOFFSETFROM:-0400
            //TZOFFSETTO:-0500
            //TZNAME:Eastern Standard Time(US & Canada)
            //END:STANDARD
            //BEGIN:DAYLIGHT
            //DTSTART:19870405T020000
            //RRULE:FREQ=YEARLY;BYDAY=1SU;BYMONTH=4
            //TZOFFSETFROM:-0500
            //TZOFFSETTO:-0400
            //TZNAME:Eastern Daylight Time(US & Canada)
            //END:DAYLIGHT
            //END:VTIMEZONE
            //END:VCALENDAR
            //]]></C:calendar-timezone>
            //    </D:prop>
            //  </D:set>
            //</C:mkcalendar>

            ///<summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.MKCALENDAR;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.MKCALENDAR;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{calendarId}";

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
            };

            /// <summary>Initializes Insert parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                //RequestParameters.Add(
                //    "colorRgbFormat", new Parameter
                //    {
                //        Name = "colorRgbFormat",
                //        IsRequired = false,
                //        ParameterType = "query",
                //        DefaultValue = null,
                //        Pattern = null,
                //    });

                RequestParameters.Add("calendarId", new Parameter
                {
                    Name = "calendarId",
                    IsRequired = true,
                    ParameterType = "path",
                    DefaultValue = null,
                    Pattern = null,
                });
            }

            protected override async Task<CalendarListEntry> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    var multistatus = await this.Service.DeserializeResponse(response);
                    if (multistatus != null)
                    {
                        try
                        {
                            TypeConverter converter = TypeDescriptor.GetConverter(typeof(CalendarListEntry));
                            bool canConvertFrom = converter.CanConvertFrom(multistatus.GetType());
                            if (canConvertFrom)
                                return (CalendarListEntry)converter.ConvertFrom(multistatus);
                            else return default;
                        }
                        catch (InvalidCastException)
                        {
                            return default;
                        }
                    }
                    else return default;
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public class ListRequest : CalendarBaseServiceRequest<CalendarList>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service) : base(service)
            {
                InitParameters();
            }

            /// <summary>Maximum number of entries returned on one result page. By default the value is 100 entries. The
            /// page size can never be larger than 250 entries. Optional.</summary>
            /// [minimum: 1]
            public virtual int? MaxResults { get; set; }

            /// <summary>The minimum access role for the user in the returned entries. Optional. The default is no
            /// restriction.</summary>
            public virtual MinAccessRoleEnum? MinAccessRole { get; set; }

            /// <summary>The minimum access role for the user in the returned entries. Optional. The default is no
            /// restriction.</summary>
            public enum MinAccessRoleEnum
            {
                /// <summary>The user can read free/busy information.</summary>
                [StringValue("freeBusyReader")]
                FreeBusyReader,
                /// <summary>The user can read and modify events and access control lists.</summary>
                [StringValue("owner")]
                Owner,
                /// <summary>The user can read events that are not private.</summary>
                [StringValue("reader")]
                Reader,
                /// <summary>The user can read and modify events.</summary>
                [StringValue("writer")]
                Writer,
            }

            /// <summary>Token specifying which result page to return. Optional.</summary>
            public virtual string PageToken { get; set; }

            /// <summary>Whether to include deleted calendar list entries in the result. Optional. The default is
            /// False.</summary>
            public virtual bool? ShowDeleted { get; set; }

            /// <summary>Whether to show hidden entries. Optional. The default is False.</summary>
            public virtual bool? ShowHidden { get; set; }

            /// <summary>Token obtained from the nextSyncToken field returned on the last page of results from the
            /// previous list request. It makes the result of this list request contain only entries that have changed
            /// since then. If only read-only fields such as calendar properties or ACLs have changed, the entry won't
            /// be returned. All entries deleted and hidden since the previous list request will always be in the result
            /// set and it is not allowed to set showDeleted neither showHidden to False. To ensure client state
            /// consistency minAccessRole query parameter cannot be specified together with nextSyncToken. If the
            /// syncToken expires, the server will respond with a 410 GONE response code and the client should clear its
            /// storage and perform a full synchronization without any syncToken. Learn more about incremental
            /// synchronization. Optional. The default is to return all entries.</summary>
            public virtual string SyncToken { get; set; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => ApiMethod.PROPFIND;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPFIND;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath { get; }

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Authorization", $"Basic {((UserCredential)Service.HttpClientInitializer).Token.AccessToken}"},
                { "Depth", "1" }
            };

            protected override object GetBody()
            {
                var calendar = new Propfind<Prop>()
                {
                    Prop = new Prop
                    {
                        Displayname = Displayname.Empty,
                        Getctag = Getctag.Empty,
                        Currentuserprivilegeset = Currentuserprivilegeset.Empty,
                        Value = PropValue.Calendardata,
                    }
                };
                return calendar;
            }

            /// <summary>Initializes List parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
            }

            protected override async Task<CalendarList> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    Multistatus<Prop> multistatus = await this.Service.DeserializeResponse<Multistatus<Prop>>(response);
                    if (multistatus != null)
                    {
                        try
                        {
                            TypeConverter converter = TypeDescriptor.GetConverter(typeof(CalendarList));
                            bool canConvertFrom = converter.CanConvertFrom(multistatus.GetType());
                            if (canConvertFrom)
                                return (CalendarList)converter.ConvertFrom(multistatus);
                            else return default;
                        }
                        catch (InvalidCastException)
                        {
                            return default;
                        }
                    }
                    else return default;
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }
        }

        /// <summary>Updates an existing calendar on the user's calendar list. This method supports patch
        /// semantics.</summary>
        public class PatchRequest : CalendarBaseServiceRequest<CalendarListEntry>
        {
            /// <summary>Constructs a new Patch request.</summary>
            public PatchRequest(IClientService service, CalendarListEntry body, string calendarId) : base(service)
            {
                CalendarId = calendarId;
                Body = body;
                InitParameters();
            }


            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Whether to use the foregroundColor and backgroundColor fields to write the calendar colors
            /// (RGB). If this feature is used, the index-based colorId field will be set to the best matching option
            /// automatically. Optional. The default is False.</summary>
            [RequestParameter("colorRgbFormat", RequestParameterType.Query)]
            public virtual bool? ColorRgbFormat { get; set; }


            /// <summary>Gets or sets the body of this request.</summary>
            private CalendarListEntry Body { get; set; }

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody() => Body;

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "patch";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => "PATCH";

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "users/me/calendarList/{calendarId}";

            /// <summary>Initializes Patch parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add(
                    "calendarId", new Parameter
                    {
                        Name = "calendarId",
                        IsRequired = true,
                        ParameterType = "path",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "colorRgbFormat", new Parameter
                    {
                        Name = "colorRgbFormat",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
            }

        }

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        public class UpdateRequest : CalendarBaseServiceRequest<CalendarListEntry>
        {
            /// <summary>Constructs a new Update request.</summary>
            public UpdateRequest(IClientService service, CalendarListEntry body, string calendarId) : base(service)
            {
                CalendarId = calendarId;
                Body = body;
                InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("calendarId", RequestParameterType.Path)]
            public virtual string CalendarId { get; private set; }

            /// <summary>Whether to use the foregroundColor and backgroundColor fields to write the calendar colors
            /// (RGB). If this feature is used, the index-based colorId field will be set to the best matching option
            /// automatically. Optional. The default is False.</summary>
            [RequestParameter("colorRgbFormat", RequestParameterType.Query)]
            public virtual System.Nullable<bool> ColorRgbFormat { get; set; }


            /// <summary>Gets or sets the body of this request.</summary>
            private CalendarListEntry Body { get; set; }

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody() => Body;

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "update";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => "PUT";

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "users/me/calendarList/{calendarId}";

            /// <summary>Initializes Update parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add(
                    "calendarId", new Parameter
                    {
                        Name = "calendarId",
                        IsRequired = true,
                        ParameterType = "path",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "colorRgbFormat", new Parameter
                    {
                        Name = "colorRgbFormat",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
            }

        }

        /// <summary>Watch for changes to CalendarList resources.</summary>
        public class WatchRequest : CalendarBaseServiceRequest<object>
        {
            /// <summary>Constructs a new Watch request.</summary>
            public WatchRequest(IClientService service, object body)
                : base(service)
            {
                Body = body;
                InitParameters();
            }


            /// <summary>Maximum number of entries returned on one result page. By default the value is 100 entries. The
            /// page size can never be larger than 250 entries. Optional.</summary>
            /// [minimum: 1]
            [RequestParameter("maxResults", RequestParameterType.Query)]
            public virtual System.Nullable<int> MaxResults { get; set; }

            /// <summary>The minimum access role for the user in the returned entries. Optional. The default is no
            /// restriction.</summary>
            [RequestParameter("minAccessRole", RequestParameterType.Query)]
            public virtual System.Nullable<MinAccessRoleEnum> MinAccessRole { get; set; }

            /// <summary>The minimum access role for the user in the returned entries. Optional. The default is no
            /// restriction.</summary>
            public enum MinAccessRoleEnum
            {
                /// <summary>The user can read free/busy information.</summary>
                [StringValue("freeBusyReader")]
                FreeBusyReader,
                /// <summary>The user can read and modify events and access control lists.</summary>
                [StringValue("owner")]
                Owner,
                /// <summary>The user can read events that are not private.</summary>
                [StringValue("reader")]
                Reader,
                /// <summary>The user can read and modify events.</summary>
                [StringValue("writer")]
                Writer,
            }

            /// <summary>Token specifying which result page to return. Optional.</summary>
            [RequestParameter("pageToken", RequestParameterType.Query)]
            public virtual string PageToken { get; set; }

            /// <summary>Whether to include deleted calendar list entries in the result. Optional. The default is
            /// False.</summary>
            [RequestParameter("showDeleted", RequestParameterType.Query)]
            public virtual System.Nullable<bool> ShowDeleted { get; set; }

            /// <summary>Whether to show hidden entries. Optional. The default is False.</summary>
            [RequestParameter("showHidden", RequestParameterType.Query)]
            public virtual System.Nullable<bool> ShowHidden { get; set; }

            /// <summary>Token obtained from the nextSyncToken field returned on the last page of results from the
            /// previous list request. It makes the result of this list request contain only entries that have changed
            /// since then. If only read-only fields such as calendar properties or ACLs have changed, the entry won't
            /// be returned. All entries deleted and hidden since the previous list request will always be in the result
            /// set and it is not allowed to set showDeleted neither showHidden to False. To ensure client state
            /// consistency minAccessRole query parameter cannot be specified together with nextSyncToken. If the
            /// syncToken expires, the server will respond with a 410 GONE response code and the client should clear its
            /// storage and perform a full synchronization without any syncToken. Learn more about incremental
            /// synchronization. Optional. The default is to return all entries.</summary>
            [RequestParameter("syncToken", RequestParameterType.Query)]
            public virtual string SyncToken { get; set; }


            /// <summary>Gets or sets the body of this request.</summary>
            private object Body { get; set; }

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody() => Body;

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "watch";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => "POST";

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "users/me/calendarList/watch";

            /// <summary>Initializes Watch parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                RequestParameters.Add(
                    "maxResults", new Parameter
                    {
                        Name = "maxResults",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "minAccessRole", new Parameter
                    {
                        Name = "minAccessRole",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "pageToken", new Parameter
                    {
                        Name = "pageToken",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "showDeleted", new Parameter
                    {
                        Name = "showDeleted",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "showHidden", new Parameter
                    {
                        Name = "showHidden",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
                RequestParameters.Add(
                    "syncToken", new Parameter
                    {
                        Name = "syncToken",
                        IsRequired = false,
                        ParameterType = "query",
                        DefaultValue = null,
                        Pattern = null,
                    });
            }
        }
    }
}
