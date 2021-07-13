using iCloud.Apis.Auth;
using iCloud.Apis.Core.Services;
using System.Collections.Generic;

namespace iCloud.Apis.People
{
    public class PeopleService : BaseClientService
    {
        public const string Version = "v1";

        public PeopleService() : this(new Initializer())
        {
        }

        public PeopleService(Initializer initializer) : base(initializer)
        {
            People = new PeopleResource(this);
            ContactGroups = new ContactGroupsResource(this);
            IdentityCard = new IdentityCardResource(this);
            BaseUri = ((UserCredential)initializer.HttpClientInitializer).Token.Tokeninfo.ContactsPrincipal.PrincipalHomeSetUrl;
            BasePath = ((UserCredential)initializer.HttpClientInitializer).Token.Tokeninfo.ContactsPrincipal.HomeSetUrl;
        }

        public override string Name => "people";

        public override string BaseUri { get; }

        public override string BasePath { get; }

        public override IList<string> Features => new string[0];

        /// <summary>Gets the People resource.</summary>
        public virtual PeopleResource People { get; }

        /// <summary>Gets the ContactGroups resource.</summary>
        public virtual ContactGroupsResource ContactGroups { get; }

        /// <summary>Gets the Card resource.</summary>
        public virtual IdentityCardResource IdentityCard { get; }
    }
}