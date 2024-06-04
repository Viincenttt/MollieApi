namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record BelfiusPaymentResponse : PaymentResponse {
        public required BelfiusPaymentResponseDetails Details { get; set; }
    }

    public record BelfiusPaymentResponseDetails {
        /// <summary>
        /// Only available if the payment has been completed – The consumer's name.
        /// </summary>
        public string? ConsumerName { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's IBAN.
        /// </summary>
        public string? ConsumerAccount { get; set; }

        /// <summary>
        /// Only available if the payment has been completed – The consumer's bank's BIC.
        /// </summary>
        public string? ConsumerBic { get; set; }
    }
}
