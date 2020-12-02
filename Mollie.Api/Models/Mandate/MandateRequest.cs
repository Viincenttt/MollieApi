using System;

namespace Mollie.Api.Models.Mandate {
    public class MandateRequest {
        public MandateRequest() {
            this.Method = Payment.PaymentMethod.DirectDebit;
        }

        /// <summary>
        /// Payment method of the mandate - Possible values: `directdebit` `paypal`
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Required - Name of consumer you add to the mandate
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Required for `directdebit` mandates - Consumer's IBAN account
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// Optional - The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string ConsumerBic { get; set; }

        /// <summary>
        /// Required For Paypal - The consumer's email address.
        /// </summary>
        public string ConsumerEmail { get; set; }

        /// <summary>
        /// Optional - The date when the mandate was signed.
        /// </summary>
        public DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Optional - A custom reference
        /// </summary>
        public string MandateReference { get; set; }

        /// <summary>
        /// Required for `paypal` mandates - The billing agreement ID given by PayPal.
        /// </summary>
        public string PaypalBillingAgreementId { get; set; }
    }
}