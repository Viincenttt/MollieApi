using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.PaymentMethod.Response
{
    public record FixedPricingResponse {
        /// <summary>
        /// The ISO 4217 currency code.
        /// </summary>
        public required string Currency { get; set; }

        /// <summary>
        /// A string containing the exact amount in the given currency.
        /// </summary>
        [JsonConverter(typeof(StringToDecimalConverter))]
        public required decimal Value { get; set; }
    }
}
