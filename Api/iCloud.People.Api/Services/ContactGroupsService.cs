using iCloud.Apis.Auth;
using iCloud.Apis.Core.Request;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.People.Request;
using iCloud.Apis.People.Responses;
using iCloud.Apis.People.Types;
using iCloud.Apis.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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
                    Etag = response.Propstat.Prop.Getetag.Value,
                    FullUrl = String.Concat(credential.Token.Tokeninfo.ContactsPrincipal.HomeSetUrl, response.Url)
                };
                listItems.Add(card);
            }

            return listItems;
        }

        public ModifyContactGroupMembersResponse ModifyMembers(UserCredential credential, ModifyContactGroupMembersRequest request, string resourceName, string uniqueId)
        {
            if (credential == null)
            {
                throw new UnauthorizedAccessException();
            }

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

            var addressbookurl = string.Concat(credential.Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl, resourceName);

            Multistatus<Prop> addressBookDataMultistatus = HttpWebRequestHelper.ExectueMethod<Multistatus<Prop>>(addressbookurl, ApiMethod.REPORT, addressBookQueryContent, ApiContentType.APPLICATION_XML, headers);

            var response = addressBookDataMultistatus.Responses.FirstOrDefault();
            var contactGroup = new ContactGroup(response.Propstat.Prop.Addressdata.Value)
            {
                Url = response.Url,
                Etag = response.Propstat.Prop.Getetag.Value,
                FullUrl = String.Concat(credential.Token.Tokeninfo.ContactsPrincipal.HomeSetUrl, response.Url)
            };

            if (request.ResourceNamesToAdd?.Any() ?? false)
            {
                foreach (var resourceNameToAdd in request.ResourceNamesToAdd)
                {
                    if (!contactGroup.MemberResourceNames.Contains(resourceNameToAdd))
                        contactGroup.MemberResourceNames.Add(resourceNameToAdd);
                }
            }

            if (request.ResourceNamesToRemove?.Any() ?? false)
            {
                foreach (var resourceNameToRemove in request.ResourceNamesToRemove)
                {
                    if (contactGroup.MemberResourceNames.Contains(resourceNameToRemove))
                        contactGroup.MemberResourceNames.Remove(resourceNameToRemove);
                }
            }

            var contactgroupurl = string.Concat(addressbookurl, "/", uniqueId, ".vcf");

            headers.Add("If-Match", request.ETag);

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter textWriter = new StreamWriter(stream))
                {
                    CardStandardWriter writer = new CardStandardWriter();
                    writer.Write(contactGroup, textWriter, Encoding.UTF8.WebName);
                    textWriter.Flush();

                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        var content = streamReader.ReadToEnd();
                        HttpWebRequestHelper.ExectueMethod(contactgroupurl, ApiMethod.PUT, content, ApiContentType.TEXT_VCARD, headers);
                        return new ModifyContactGroupMembersResponse();
                    }
                }
            }
        }
    }
}