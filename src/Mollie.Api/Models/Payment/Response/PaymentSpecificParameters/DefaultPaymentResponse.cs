namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    /// <summary>
    /// Used for payment methods that do not have method specific details. Only contains the customer details
    /// that are available for all payment methods.
    /// </summary>
    public record DefaultPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public PaymentResponseDetails? Details { get; set; }
    }
}
