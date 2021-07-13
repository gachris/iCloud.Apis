using iCloud.Apis.Auth;
using iCloud.Apis.Core.Request;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.Core.Services;
using iCloud.Apis.People.Request;
using iCloud.Apis.People.Types;
using iCloud.Apis.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace iCloud.Apis.People
{
    /// <summary>The "ContactGroups" collection of methods.</summary>
    public class IdentityCardResource
    {
        private const string _resource = "contactGroups";

        /// <summary>The service which this resource belongs to.</summary>
        private readonly IClientService service;

        /// <summary>Constructs a new resource.</summary>
        public IdentityCardResource(IClientService service)
        {
            this.service = service;
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public virtual ListRequest List()
        {
            return new ListRequest(service);
        }

        /// <summary>Returns the calendars on the user's calendar list.</summary>
        public class ListRequest : PeopleBaseServiceRequest<IdentityCardList>
        {
            /// <summary>Constructs a new Get request.</summary>
            public ListRequest(IClientService service) : base(service)
            {
                this.InitParameters();
            }

            ///<summary>Gets the method name.</summary>
            public override string MethodName => "get";

            ///<summary>Gets the HTTP method.</summary>
            public override string HttpMethod => ApiMethod.PROPFIND;

            public override string RestPath => string.Empty;

            public override IDictionary<string, string> Headers => new Dictionary<string, string>
            {
                { "Depth", "1" }
            };

            protected override object GetBody()
            {
                var contactsListPropfind = new Propfind<Prop>()
                {
                    Prop = new Prop
                    {
                        Resourcetype = new Resourcetype()
                    }
                };
                return contactsListPropfind;
            }

            protected override async Task<IdentityCardList> ParseResponse(HttpResponseMessage response)
            {
                if (response.IsSuccessStatusCode)
                {
                    Multistatus<Prop> multistatus = await this.Service.DeserializeResponse<Multistatus<Prop>>(response);
                    var listItems = new IdentityCardList();
                    foreach (var multistatusItem in multistatus.Responses)
                    {
                        var cardUrl = multistatusItem.Url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cardUrl.Count() == 3)
                        {
                            IdentityCard card = new IdentityCard()
                            {
                                ResourceName = cardUrl.Last(),
                                UniqueId = cardUrl.Last(),
                                Url = multistatusItem.Url,
                            };
                            listItems.Add(card);
                        }
                    }
                    return listItems;
                }
                RequestError requestError = await this.Service.DeserializeError(response).ConfigureAwait(false);
                throw new ICloudApiException(this.Service.Name, requestError.ToString())
                {
                    Error = requestError,
                    HttpStatusCode = response.StatusCode
                };
            }
        }
    }
}
