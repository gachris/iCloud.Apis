using vCards;

namespace iCloud.Apis.Util
{
    public class vCardAddress : vCardDeliveryAddress
    {
        public string Id { get; set; }
        public string CountryCode { get; set; }
        public bool IsPreferred { get; set; }
    }
}