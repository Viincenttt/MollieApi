using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.PaymentMethod.Response
{
    public record PricingResponse {
        /// <summary>
        /// The area or product-type where the pricing is applied for, translated in the optional locale passed.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// The fixed price per transaction
        /// </summary>
        public required FixedPricingResponse Fixed { get; set; }

        /// <summary>
        /// A string containing the percentage what will be charged over the payment amount besides the fixed price.
        /// </summary>
        [JsonConverter(typeof(StringToDecimalConverter))]
        public required decimal Variable { get; set; }

        /// <summary>
        /// This value is only available for credit card rates. It will correspond with the regions as documented in
        /// the Payments API. See the Mollie.Api.Models.Payment.Response.CreditCardFeeRegion class for a full list of
        /// known values.
        /// </summary>
        public required string? FeeRegion { get; set; }
    }
}
