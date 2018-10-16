using System.Xml.Serialization;

namespace Mollie.Api.Models.Reseller
{
    [XmlRoot("request")]
    public class CreateAccountRequest : BaseResellerRequest
    {
        public CreateAccountRequest(string partnerId, string profileKey, bool testModus) : base(partnerId, profileKey) 
        {
            Testmode = testModus ? 1 : 0;
        }

        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("company_name")]
        public string CompanyName { get; set; }
        [XmlElement("email")]
        public string Email { get; set; }
        [XmlElement("address")]
        public string Address { get; set; }
        [XmlElement("zipcode")]
        public string Zipcode { get; set; }
        [XmlElement("city")]
        public string City { get; set; }
        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("registration_number")]
        public string RegistrationNumber { get; set; }

        [XmlElement("vat_number")]
        public string VatNumber { get; set; }

        [XmlElement("representative")]
        public string Representative { get; set; }


        [XmlElement("bankaccount_iban")]
        public string BankaccountIban { get; set; }

        [XmlElement("bankaccount_bic")]
        public string BankaccountBic { get; set; }

        [XmlElement("bankaccount_location")]
        public string BankaccountLocation { get; set; }

        [XmlElement("Testmode")]
        public int Testmode { get; set; }

    }
}
