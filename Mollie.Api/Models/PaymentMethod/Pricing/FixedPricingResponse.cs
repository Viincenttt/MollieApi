namespace Mollie.Api.Models.PaymentMethod.Pricing
{
    public class FixedPricingResponse : IResponseObject {
        /// <summary>
        /// The ISO 4217 currency code.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// A string containing the exact amount in the given currency.
        /// </summary>
        public decimal Value { get; set; }
    }
}