using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Mandate {
    public class MandateRequest {
        public MandateRequest() {
            this.Method = Payment.PaymentMethod.DirectDebit;
        }

        /// <summary>
        /// Payment method of the mandate.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Required - Name of consumer you add to the mandate
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Required - Consumer's IBAN account
        /// </summary>
        public string ConsumerAccount { get; set; }

        /// <summary>
        /// Optional - The consumer's bank's BIC / SWIFT code.
        /// </summary>
        public string ConsumerBic { get; set; }
        
        /// <summary>
        /// Optional - The date when the mandate was signed.
        /// </summary>
        public DateTime? SignatureDate { get; set; }

        /// <summary>
        /// Optional - A custom reference
        /// </summary>
        public string MandateReference { get; set; }
    }
}