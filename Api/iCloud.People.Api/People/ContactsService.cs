using iCloud.Apis.Auth;
using iCloud.Apis.Core.Responses;
using iCloud.Apis.People.Request;
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
    public class ContactsService
    {
        public IList<Person> GetContacts(UserCredential credential, string resourceName)
        {
            if (credential == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var headers = new WebHeaderCollection
            {
                 { "Authorization", $"Basic {credential.Token.AccessToken}" },
                 { "UserAgent" , "curl/7.37.0" },
                 { "Depth", "1" }
            };

            var addressBookQuery = new Addressbookquery
            {
                Prop = new Prop()
                {
                    Getetag = new Getetag(),
                    Addressdata = new Addressdata()
                }
            };

            var addressBookQueryContent = XmlSerializerHelper.Serialize(addressBookQuery);

            Multistatus<Prop> addressBookDataMultistatus = HttpWebRequestHelper.ExectueMethod<Multistatus<Prop>>(credential.Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl + resourceName, ApiMethod.REPORT, addressBookQueryContent, ApiContentType.APPLICATION_XML, headers);

            var persons = new List<Person>();
            var contactGroups = new List<ContactGroup>();

            foreach (var response in addressBookDataMultistatus.Responses)
            {
                if (response.Propstat.Prop.Addressdata.Value.Contains("X-ADDRESSBOOKSERVER-KIND:group"))
                {
                    var contactGroup = new ContactGroup(response.Propstat.Prop.Addressdata.Value) { Etag = response.Propstat.Prop.Getetag.Value };
                    contactGroups.Add(contactGroup);
                }
                else
                {
                    var person = new Person(response.Propstat.Prop.Addressdata.Value) { Etag = response.Propstat.Prop.Getetag.Value };
                    persons.Add(person);
                }
            }

            foreach (var contactGoup in contactGroups)
                persons.Where(x => contactGoup.MemberResourceNames.Contains(x.UniqueId)).ToList().ForEach(x => x.Memberships.Add(new Membership()
                {
                    ETag = contactGoup.Etag,
                    ContactGroupMembership = new ContactGroupMembership()
                    {
                        ETag = contactGoup.Etag,
                        ContactGroupId = contactGoup.UniqueId,
                        ContactGroupResourceName = contactGoup.FullUrl
                    }
                }));

            return persons;
        }

        public Person AddContact(UserCredential credential, Person person, string resourceName)
        {
            if (credential == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var headers = new WebHeaderCollection
            {
                { "Authorization", $"Basic {credential.Token.AccessToken}" },
                { "UserAgent" , "curl/7.37.0" },
                { "If-Non-Match", "*" }
            };

            if (string.IsNullOrEmpty(person.UniqueId))
                person.UniqueId = Guid.NewGuid().ToString().ToUpper();

            var url = string.Concat(credential.Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl, resourceName, "/", person.UniqueId, ".vcf");

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter textWriter = new StreamWriter(stream))
                {
                    CardStandardWriter writer = new CardStandardWriter();
                    writer.Write(person, textWriter, Encoding.UTF8.WebName);
                    textWriter.Flush();

                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        var content = streamReader.ReadToEnd();
                        HttpWebRequestHelper.ExectueMethod(url, ApiMethod.PUT, content, ApiContentType.TEXT_VCARD, headers);
                        return person;
                    }
                }
            }
        }

        public Person UpdateContact(UserCredential credential, Person person, string personId, string etag, string resourceName)
        {
            if (credential == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (person == null)
            {
                throw new ArgumentNullException(nameof(person));
            }

            if (string.IsNullOrEmpty(etag))
            {
                throw new ArgumentNullException(nameof(etag));
            }

            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            var headers = new WebHeaderCollection
            {
                { "Authorization", $"Basic {credential.Token.AccessToken}" },
                { "UserAgent" , "curl/7.37.0" },
                { "If-Match", etag }
            };

            var url = string.Concat(credential.Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl, resourceName, "/", personId, ".vcf");

            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter textWriter = new StreamWriter(stream))
                {
                    CardStandardWriter writer = new CardStandardWriter();
                    writer.Write(person, textWriter, "utf-8");
                    textWriter.Flush();

                    stream.Seek(0, SeekOrigin.Begin);
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        var content = streamReader.ReadToEnd();

                        HttpWebRequestHelper.ExectueMethod(url, ApiMethod.PUT, content, ApiContentType.TEXT_VCARD, headers);
                        return person;
                    }
                }
            }
        }
    }
}