namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record IngHomePayPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public required IngHomePayPaymentResponseDetails? Details { get; set; }
    }

    public record IngHomePayPaymentResponseDetails : PaymentResponseDetails {
    }
}
