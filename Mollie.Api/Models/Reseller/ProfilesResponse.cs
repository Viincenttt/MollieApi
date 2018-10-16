using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot(ElementName = "response")]
    public class AvailablePaymentsResponse
    {
        [XmlElement(ElementName = "services")]
        public Services Services { get; set; }
    }

    [XmlRoot(ElementName = "services")]
    public class Services
    {
        [XmlElement(ElementName = "ivr")]
        public bool Ivr { get; set; }

        [XmlElement(ElementName = "psms")]
        public bool Psms { get; set; }

        [XmlElement(ElementName = "ideal")]
        public bool Ideal { get; set; }

        [XmlElement(ElementName = "paysafecard")]
        public bool Paysafecard { get; set; }

        [XmlElement(ElementName = "creditcard")]
        public bool Creditcard { get; set; }

        [XmlElement(ElementName = "mistercash")]
        public bool Mistercash { get; set; }
    }


    [XmlRoot(ElementName = "response")]
    public class ProfilesResponse
    {
        [XmlElement(ElementName = "items")]
        public Items Items { get; set; }
    }

    [XmlRoot(ElementName = "profile")]
    public class Profile
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "hash")]
        public string Hash { get; set; }

        [XmlElement(ElementName = "website")]
        public string Website { get; set; }

        [XmlElement(ElementName = "sector")]
        public string Sector { get; set; }

        [XmlElement(ElementName = "category")]
        public string Category { get; set; }

        [XmlElement(ElementName = "verified")]
        public string Verified { get; set; }

        [XmlElement(ElementName = "phone")]
        public string Phone { get; set; }

        [XmlElement(ElementName = "email")]
        public string Email { get; set; }

        [XmlElement(ElementName = "api_keys")]
        public ApiKeys Api_keys { get; set; }
    }

    [XmlRoot(ElementName = "items")]
    public class Items
    {
        [XmlElement(ElementName = "profile")]
        public Profile Profile { get; set; }
    }
}
