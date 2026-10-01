namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    /// <summary>
    /// The customer details that are available for all payment methods. Payment method specific details
    /// classes inherit from this class.
    /// </summary>
    public record PaymentResponseDetails {
        /// <summary>
        /// Only available if the payment has been completed – The consumer's name.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's account, for example an IBAN or an
        /// email address, depending on the payment method.
        /// </summary>
        public string? ConsumerAccount { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The BIC of the consumer's bank.
        /// </summary>
        public string? ConsumerBic { get; set; }

        /// <summary>
        /// The shipping address details, if available.
        /// </summary>
        public AddressObject? ShippingAddress { get; set; }
    }
}
