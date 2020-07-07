namespace Mollie.Api.Models.Payment.Request {
    public class KbcPaymentRequest : PaymentRequest {
        public KbcPaymentRequest() {
            this.Method = PaymentMethod.Kbc;
        }

        /// <summary>
        /// The issuer to use for the KBC/CBC payment. These issuers are not dynamically available through the Issuers API, 
        /// but can be retrieved by using the issuers include in the Methods API.
        /// </summary>
        public string Issuer { get; set; }
    }
}