namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record IngHomePayPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public required IngHomePayPaymentResponseDetails Details { get; set; }
    }

    public record IngHomePayPaymentResponseDetails {
        /// <summary>
        /// Only available one banking day after the payment has been completed – The consumer’s name.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Only available one banking day after the payment has been completed – The consumer’s IBAN.
        /// </summary>
        public string? ConsumerAccount { get; set; }

        /// <summary>
        /// Only available one banking day after the payment has been completed – BBRUBEBB.
        /// </summary>
        public string? ConsumerBic { get; set; }
    }
}
