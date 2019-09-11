using Mollie.Api.Models.Payment.Request;

namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class KbcSpecificParameters : PaymentSpecificParameters
    {
        /// <summary>
        /// The issuer to use for the KBC/CBC payment. These issuers are not dynamically available through the Issuers API, 
        /// but can be retrieved by using the issuers include in the Methods API.
        /// </summary>
        public KbcIssuer Issuer { get; set; }
    }
}
