namespace Mollie.Api.Models.PaymentMethod.Pricing
{
    public record FixedPricingResponse {
        /// <summary>
        /// The ISO 4217 currency code.
        /// </summary>
        public required string Currency { get; init; }

        /// <summary>
        /// A string containing the exact amount in the given currency.
        /// </summary>
        public required decimal Value { get; init; }
    }
}