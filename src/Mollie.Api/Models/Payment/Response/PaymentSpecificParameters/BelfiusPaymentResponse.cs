namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record BelfiusPaymentResponse : PaymentResponse {
        public required BelfiusPaymentResponseDetails? Details { get; set; }
    }

    public record BelfiusPaymentResponseDetails : PaymentResponseDetails {
    }
}
