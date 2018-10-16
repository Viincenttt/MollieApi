using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("response")]
    public class CreateAccountResponse : BaseResellerResponse
    {
        [XmlElement("username")]
        public string Username { get; set; }

        [XmlElement("password")]
        public string Password { get; set; }

        [XmlElement("partner_id")]
        public string PartnerId { get; set; }
    }
}
