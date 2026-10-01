namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record GiropayPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with the consumer bank account details.
        /// </summary>
        public required GiropayPaymentResponseDetails? Details { get; set; }
    }

    public record GiropayPaymentResponseDetails : PaymentResponseDetails {
    }
}
