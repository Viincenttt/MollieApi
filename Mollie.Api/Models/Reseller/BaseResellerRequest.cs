using System;
using System.Xml.Serialization;
using Mollie.Api.Extensions;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("request")]
    public class BaseResellerRequest
    {
        public BaseResellerRequest(string partnerId, string profileKey)
        {
            PartnerId = partnerId;
            ProfileKey = profileKey;
        }

        [XmlElement("partner_id")]
        public string PartnerId { get; set; }

        [XmlElement("profile_key")]
        public string ProfileKey { get; set; }

        [XmlElement("timestamp")]
        public long Timestamp => DateTime.Now.ToUnixEpochDate();
    }
}
