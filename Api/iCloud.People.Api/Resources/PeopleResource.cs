using iCloud.Apis.Core.Responses;
using iCloud.Apis.Core.Services;
using iCloud.Apis.People.Request;
using iCloud.Apis.People.Types;
using iCloud.Apis.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iCloud.Apis.People
{
    /// <summary>The "calendarList" collection of methods.</summary>
    public class PeopleResource
    {
        private const string Resource = "people";

        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService service;

        /// <summary>Constructs a new resource.</summary>
        public PeopleResource(IClientService service)
        {
            this.service = service;
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual DeleteRequest Delete(string uniqueId, string resourceName)
        {
            return new DeleteRequest(service, uniqueId, resourceName);
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual GetRequest Get(string uniqueId, string resourceName)
        {
            return new GetRequest(service, uniqueId, resourceName);
        }

        /// <summary>Inserts an existing calendar into the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        public virtual InsertRequest Insert(Person body, string resourceName)
        {
            return new InsertRequest(service, body, resourceName);
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public virtual ListRequest List(string resourceName)
        {
            return new ListRequest(service, resourceName);
        }

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        /// <param name="body">The body of the request.</param>
        /// <param name="calendarId">Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you
        /// want to access the primary calendar of the currently logged in user, use the "primary" keyword.</param>
        public virtual UpdateRequest Update(Person body, string uniqueId, string etag, string resourceName)
        {
            return new UpdateRequest(service, body, uniqueId, etag, resourceName);
        }

        /// <summary>Removes a calendar from the user's calendar list.</summary>
        public class DeleteRequest : PeopleBaseServiceRequest<string>
        {
            /// <summary>Constructs a new Delete request.</summary>
            public DeleteRequest(IClientService service, string uniqueId, string resourceName) : base(service)
            {
                this.UniqueId = uniqueId;
                this.ResourceName = resourceName;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; private set; }

            [RequestParameter("resourceName", RequestParameterType.Path)]
            public string ResourceName { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "delete";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.DELETE;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            /// <summary>Initializes Delete parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                this.RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }

        /// <summary>Returns a calendar from the user's calendar list.</summary>
        public class GetRequest : PeopleBaseServiceRequest<Person>
        {
            /// <summary>Constructs a new Get request.</summary>
            public GetRequest(IClientService service, string uniqueId, string resourceName) : base(service)
            {
                this.UniqueId = uniqueId;
                this.ResourceName = resourceName;
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            [RequestParameter("resourceName", RequestParameterType.Path)]
            public string ResourceName { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "get";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.GET;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            /// <summary>Initializes Get parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                this.RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }

            protected override async Task<Person> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string content = await this.Service.DeserializeResponse(response);
                    if (!String.IsNullOrEmpty(content))
                        return new Person(content);
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
        public class InsertRequest : PeopleBaseServiceRequest<Person>
        {
            /// <summary>Constructs a new Insert request.</summary>
            public InsertRequest(IClientService service, Person body, string resourceName) : base(service)
            {
                this.Body = body;

                if (string.IsNullOrEmpty(this.Body.UniqueId))
                    this.Body.UniqueId = Guid.NewGuid().ToString().ToUpper();
                this.UniqueId = this.Body.UniqueId;
                this.ResourceName = resourceName;

                this.InitParameters();
            }

            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public virtual string UniqueId { get; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Person Body { get; set; }

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter textWriter = new StreamWriter(stream))
                    {
                        CardStandardWriter writer = new CardStandardWriter();
                        writer.Write(this.Body, textWriter, Encoding.UTF8.WebName);
                        textWriter.Flush();

                        stream.Seek(0, SeekOrigin.Begin);
                        using (StreamReader streamReader = new StreamReader(stream))
                            return streamReader.ReadToEnd();
                    }
                }
            }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "insert";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "If-Non-Match", "*" }
            };

            public override string ContentType => ApiContentType.TEXT_VCARD;

            /// <summary>Initializes Insert parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();

                this.RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                this.RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }

            protected override async Task<Person> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string status = await this.Service.DeserializeResponse(response);
                    return Body;
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
        public class ListRequest : PeopleBaseServiceRequest<PersonList>
        {
            /// <summary>Constructs a new List request.</summary>
            public ListRequest(IClientService service, string resourceName) : base(service)
            {
                this.InitParameters();
                ResourceName = resourceName;
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
            public override string MethodName => ApiMethod.REPORT;

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.REPORT;

            ///<summary>Gets the resource name.</summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; }

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}";

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Depth", "1" }
            };

            protected override object GetBody()
            {
                var addressBookQuery = new Addressbookquery
                {
                    Prop = new Prop()
                    {
                        Getetag = Getetag.Empty,
                        Addressdata = new Addressdata()
                    }
                };
                return addressBookQuery;
            }

            /// <summary>Initializes List parameter list.</summary>
            protected override void InitParameters()
            {
                base.InitParameters();
                RequestParameters.Add(
                    "resourceName", new Parameter
                    {
                        Name = "resourceName",
                        IsRequired = true,
                        ParameterType = "path",
                        DefaultValue = null,
                        Pattern = null,
                    });
            }

            protected override async Task<PersonList> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    Multistatus<Prop> multistatus = await this.Service.DeserializeResponse<Multistatus<Prop>>(response);
                    if (multistatus != null)
                    {
                        try
                        {
                            var personList = new PersonList();
                            var contactGroupList = new ContactGroupList();

                            foreach (var multistatusItem in multistatus.Responses)
                            {
                                if (multistatusItem.Propstat.Prop.Addressdata.Value.Contains("X-ADDRESSBOOKSERVER-KIND:group"))
                                {
                                    var contactGroup = new ContactGroup(multistatusItem.Propstat.Prop.Addressdata.Value) { Etag = multistatusItem.Propstat.Prop.Getetag.Value };
                                    contactGroupList.Add(contactGroup);
                                }
                                else
                                {
                                    var person = new Person(multistatusItem.Propstat.Prop.Addressdata.Value) { Etag = multistatusItem.Propstat.Prop.Getetag.Value };
                                    personList.Add(person);
                                }
                            }

                            foreach (var contactGoup in contactGroupList)
                                personList.Where(x => contactGoup.MemberResourceNames.Contains(x.UniqueId)).ToList().ForEach(x => x.Memberships.Add(new Membership()
                                {
                                    ETag = contactGoup.Etag,
                                    ContactGroupMembership = new ContactGroupMembership()
                                    {
                                        ETag = contactGoup.Etag,
                                        ContactGroupId = contactGoup.UniqueId,
                                        ContactGroupResourceName = contactGoup.FullUrl
                                    }
                                }));

                            return personList;
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

        /// <summary>Updates an existing calendar on the user's calendar list.</summary>
        public class UpdateRequest : PeopleBaseServiceRequest<Person>
        {
            /// <summary>Constructs a new Update request.</summary>
            public UpdateRequest(IClientService service, Person body, string uniqueId, string etag, string resourceName) : base(service)
            {
                this.ResourceName = resourceName;
                this.Body = body;
                UniqueId = uniqueId;
                this.Etag = etag;
                Headers.Add("If-Match", Etag);
                this.InitParameters();
            }

            /// <summary>Calendar identifier. To retrieve calendar IDs call the calendarList.list method. If you want to
            /// access the primary calendar of the currently logged in user, use the "primary" keyword.</summary>
            [RequestParameter("resourceName", RequestParameterType.Path)]
            public virtual string ResourceName { get; private set; }

            /// <summary>Gets or sets the body of this request.</summary>
            private Person Body { get; set; }

            [RequestParameter("uniqueId", RequestParameterType.Path)]
            public string UniqueId { get; }

            public string Etag { get; }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "update";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PUT;

            ///<summary>Gets the REST path.</summary>
            public override string RestPath => "{resourceName}/{uniqueId}.vcf";

            public override string ContentType => ApiContentType.TEXT_VCARD;

            public override IDictionary<string, string> Headers => new Dictionary<string, string>();

            ///<summary>Returns the body of the request.</summary>
            protected override object GetBody()
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter textWriter = new StreamWriter(stream))
                    {
                        CardStandardWriter writer = new CardStandardWriter();
                        writer.Write(this.Body, textWriter, Encoding.UTF8.WebName);
                        textWriter.Flush();

                        stream.Seek(0, SeekOrigin.Begin);
                        using (StreamReader streamReader = new StreamReader(stream))
                            return streamReader.ReadToEnd();
                    }
                }
            }

            protected override async Task<Person> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    string status = await this.Service.DeserializeResponse(response);
                    return Body;
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

                this.RequestParameters.Add("resourceName", new Parameter
                {
                    Name = "resourceName",
                    IsRequired = true,
                    ParameterType = "path",
                });
                this.RequestParameters.Add("uniqueId", new Parameter
                {
                    Name = "uniqueId",
                    IsRequired = true,
                    ParameterType = "path",
                });
            }
        }
    }
}
