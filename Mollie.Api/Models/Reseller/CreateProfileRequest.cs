using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("request")]
    public class CreateProfileRequest : BaseResellerRequest
    {
        public CreateProfileRequest(string partnerId, string profileKey) : base(partnerId, profileKey) { }

        [XmlElement("partner_id_customer")]
        public string PartnerIdCustomer { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("website")]
        public string Website { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("phone")]
        public string Phone { get; set; }

        [XmlElement("category")]
        public Category Category { get; set; }
    }
}