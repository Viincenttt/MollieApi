namespace Mollie.Api.Models.Issuer {
    public class IssuerResponse {
        /// <summary>
        /// The issuer's unique identifier, for example ideal_ABNANL2A. When creating a payment, specify this ID as the issuer parameter to forward 
        /// the consumer to their banking environment directly.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The issuer's full name, for example 'ABN AMRO'.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The payment method this issuer belongs to. The Issuers API currently only supports iDEAL.
        /// </summary>
        public string Method { get; set; }
    }
}
