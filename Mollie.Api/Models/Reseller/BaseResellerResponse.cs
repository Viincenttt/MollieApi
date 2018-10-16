using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("response")]
    public class BaseResellerResponse
    {
        [XmlElement("success")]
        public bool Success { get; set; }

        [XmlElement("resultcode")]
        public int ResultCode { get; set; }

        [XmlElement("resultmessage")]
        public string ResultMessage { get; set; }
    }
}