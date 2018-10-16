using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("response")]
    public class CreateProfileResponse : BaseResellerResponse
    {
        [XmlElement("profile")]
        public WebsiteProfile Profile { get; set; }
    }
}
