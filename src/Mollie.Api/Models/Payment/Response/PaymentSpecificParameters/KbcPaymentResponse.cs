namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record KbcPaymentResponse : PaymentResponse {
        public required KbcPaymentResponseDetails? Details { get; set; }
    }

    public record KbcPaymentResponseDetails : PaymentResponseDetails {
    }
}
