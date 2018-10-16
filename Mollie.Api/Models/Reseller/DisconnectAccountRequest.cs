using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    public class DisconnectAccountRequest : BaseResellerRequest
    {
        public DisconnectAccountRequest(string partnerId, string profileKey) : base(partnerId, profileKey) { }

        [XmlElement("partner_id_customer")]
        public string PartnerIdCustomer { get; set; }
    }
}
