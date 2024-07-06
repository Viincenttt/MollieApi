namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record PaySafeCardPaymentResponse : PaymentResponse {
        public required PaySafeCardPaymentResponseDetails? Details { get; set; }
    }

    public record PaySafeCardPaymentResponseDetails {
        /// <summary>
        /// The consumer identification supplied when the payment was created.
        /// </summary>
        public string? CustomerReference { get; set; }
    }
}
