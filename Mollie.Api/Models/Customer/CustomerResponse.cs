using Mollie.Api.Models.Payment;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Mollie.Api.Models.Customer {
    public class CustomerResponse {
        /// <summary>
        /// The customer's unique identifier, for example cst_4pmbK7CqtT. 
        /// Store this identifier for later recurring payments.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The mode used to create this payment. Mode determines whether a payment is real or a test payment.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public PaymentMode Mode { get; set; }

        /// <summary>
        /// Name of your customer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// E-mailaddress of your customer.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Optional metadata.
        /// Use this if you want Mollie to store additional info.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// DateTime when user was created.
        /// </summary>
        public DateTime? CreatedDatetime { get; set; }
    }
}
