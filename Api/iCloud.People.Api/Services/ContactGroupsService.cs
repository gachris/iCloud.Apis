using iCloud.Apis.Auth;
using iCloud.Apis.Core.Request;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.People.Request;
using iCloud.Apis.People.Types;
using iCloud.Apis.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace iCloud.Apis.People
{
    public class ContactGroupsService
    {
        public IList<ContactGroup> GetContactGroups(UserCredential credential)
        {
            if (credential == null)
            {
                throw new UnauthorizedAccessException();
            }

            var contactsListPropfind = new Propfind<Prop>()
            {
                Prop = new Prop
                {
                    Displayname = new Displayname(),
                    Resourcetype = new Resourcetype(),
                    Supportedreportset = new Supportedreportset()
                }
            };
            var contactsListPropfindContent = XmlSerializerHelper.Serialize(contactsListPropfind);

            var addressBookQuery = new Addressbookquery
            {
                Filter = new Filters { Type = "anyof" },
                Prop = new Prop()
                {
                    Getetag = new Getetag(),
                    Addressdata = new Addressdata()
                }
            };

            var propfilter = new Propfilter { Name = "X-ADDRESSBOOKSERVER-KIND", };
            var textmatch = new TextMatch
            {
                Collation = "i;unicode-casemap",
                Negatecondition = "no",
                Matchtype = "contains",
                Text = "group"
            };

            propfilter.Textmatch = new List<TextMatch> { textmatch };
            addressBookQuery.Filter.Propfilter = new List<Propfilter> { propfilter };

            var addressBookQueryContent = XmlSerializerHelper.Serialize(addressBookQuery);

            var headers = new WebHeaderCollection
            {
                 { "Authorization", $"Basic {credential.Token.AccessToken}" },
                 { "UserAgent" , "curl/7.37.0" },
                 { "Depth", "1" }
            };

            Multistatus<Prop> multistatus = HttpWebRequestHelper.ExectueMethod<Multistatus<Prop>>(credential.Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl, ApiMethod.PROPFIND, contactsListPropfindContent, ApiContentType.APPLICATION_XML, headers);

            var listItems = new List<ContactGroup>();

            ContactGroup card = null;

            foreach (var response in multistatus.Responses)
            {
                var cardUrl = response.Url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (cardUrl.Count() == 3)
                {
                    card = new ContactGroup()
                    {
                        FormattedName = "All Contacts",
                        UniqueId = cardUrl.Last(),
                        FamilyName = cardUrl.Last(),
                        Url = response.Url,
                        FullUrl = String.Concat(credential.Token.Tokeninfo.ContactsPrincipal.HomeSetUrl, response.Url)
                    };
                }
            }

            Multistatus<Prop> addressBookDataMultistatus = HttpWebRequestHelper.ExectueMethod<Multistatus<Prop>>(card.FullUrl, ApiMethod.REPORT, addressBookQueryContent, ApiContentType.APPLICATION_XML, headers);

            foreach (var response in addressBookDataMultistatus.Responses)
            {
                card = new ContactGroup(response.Propstat.Prop.Addressdata.Value)
                {
                    Url = response.Url,
                    ETag = response.Propstat.Prop.Getetag.Value,
                    FullUrl = String.Concat(credential.Token.Tokeninfo.ContactsPrincipal.HomeSetUrl, response.Url)
                };
                listItems.Add(card);
            }

            return listItems;
        }

        public ContactGroup GetContactGroup(UserCredential credential, string uniqueId)
        {
            var headers = new WebHeaderCollection
            {
                 { "Authorization", $"Basic {credential.Token.AccessToken}" },
                 { "UserAgent" , "curl/7.37.0" }
            };

            var addressBookQuery = new Addressbookquery
            {
                Filter = new Filters { Type = "anyof" },
                Prop = new Prop()
                {
                    Getetag = new Getetag(),
                    Addressdata = new Addressdata()
                }
            };

            var propfilter = new Propfilter { Name = "UID", };
            var textmatch = new TextMatch
            {
                Collation = "i;unicode-casemap",
                Negatecondition = "no",
                Matchtype = "equals",
                Text = uniqueId
            };

            propfilter.Textmatch = new List<TextMatch> { textmatch };
            addressBookQuery.Filter.Propfilter = new List<Propfilter> { propfilter };

            var addressBookQueryContent = XmlSerializerHelper.Serialize(addressBookQuery);

            var addressbookurl = string.Concat(credential.Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl, "card");

            Multistatus<Prop> addressBookDataMultistatus = HttpWebRequestHelper.ExectueMethod<Multistatus<Prop>>(addressbookurl, ApiMethod.REPORT, addressBookQueryContent, ApiContentType.APPLICATION_XML, headers);

            var response = addressBookDataMultistatus.Responses.FirstOrDefault();
            return new ContactGroup(response.Propstat.Prop.Addressdata.Value)
            {
                Url = response.Url,
                ETag = response.Propstat.Prop.Getetag.Value,
                FullUrl = String.Concat(credential.Token.Tokeninfo.ContactsPrincipal.HomeSetUrl, response.Url)
            };
        }
    }
}