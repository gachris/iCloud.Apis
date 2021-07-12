using iCloud.Apis.Core.Services;

namespace iCloud.Apis.People.Request
{
    public abstract class PeopleBaseServiceRequest<TResponse> : ClientServiceRequest<TResponse>
    {
        ///<summary>Constructs a new CalendarBaseServiceRequest instance.</summary>
        protected PeopleBaseServiceRequest(IClientService service) : base(service)
        {
        }
    }
}
