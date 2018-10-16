using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("profile")]
    public class WebsiteProfile
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("hash")]
        public string Hash { get; set; }

        [XmlElement("website")]
        public string Website { get; set; }

        [XmlIgnore]
        public long? Sector { get; set; }

        [XmlElement("sector")]
        public string SectorAsText
        {
            get { return Sector.HasValue ? Sector.ToString() : null; }
            set { Sector = !string.IsNullOrEmpty(value) ? long.Parse(value) : default(long?); }
        }

        [XmlIgnore]
        public long? Category { get; set; }

        [XmlElement("category")]
        public string CategoryAsText
        {
            get { return Category.HasValue ? Category.ToString() : null; }
            set { Category = !string.IsNullOrEmpty(value) ? long.Parse(value) : default(long?); }
        }

        [XmlIgnore]
        public bool? Verified { get; set; }

        [XmlElement("verified")]
        public string VerifiedyAsText
        {
            get { return Verified.HasValue ? Verified.ToString() : null; }
            set { Verified = !string.IsNullOrEmpty(value) ? bool.Parse(value) : default(bool?); }
        }

        [XmlElement("phone")]
        public string Phone { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("api_keys", IsNullable = true)]
        public ApiKeys ApiKeys { get; set; }
    }
}