namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record SofortPaymentResponse : PaymentResponse {
        public required SofortPaymentResponseDetails? Details { get; set; }
    }

    public record SofortPaymentResponseDetails : PaymentResponseDetails {
    }
}
