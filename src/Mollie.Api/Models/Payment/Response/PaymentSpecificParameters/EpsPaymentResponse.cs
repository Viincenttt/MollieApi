namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record EpsPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with the consumer bank account details.
        /// </summary>
        public required EpsPaymentResponseDetails? Details { get; set; }
    }

    public record EpsPaymentResponseDetails : PaymentResponseDetails {
    }
}
