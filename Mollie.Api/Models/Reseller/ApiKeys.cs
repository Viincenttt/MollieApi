using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("api_keys")]
    public class ApiKeys
    {
        [XmlElement("test")]
        public string TestApiKey { get; set; }

        [XmlElement("live")]
        public string LiveApiKey { get; set; }
    }
}
