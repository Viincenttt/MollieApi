namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record BizumPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public BizumPaymentResponseDetails? Details { get; set; }
    }

    public record BizumPaymentResponseDetails {
        /// <summary>
        /// Bizum payment reference of the transaction.
        /// </summary>
        public string? BizumReference { get; set; }
    }
}
