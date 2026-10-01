using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record In3PaymentRequest : PaymentRequest {
        public In3PaymentRequest() {
            Method = PaymentMethod.In3;
        }

        [SetsRequiredMembers]
        public In3PaymentRequest(PaymentRequest paymentRequest) : base(paymentRequest) {
            Method = PaymentMethod.In3;
        }

        /// <summary>
        /// The customer's date of birth. If not provided via the API, iDEAL in3 will ask the customer to provide it
        /// during the payment process.
        /// </summary>
#if NET8_0_OR_GREATER
        public DateOnly? ConsumerDateOfBirth { get; set; }
#else
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? ConsumerDateOfBirth { get; set; }
#endif
    }
}
