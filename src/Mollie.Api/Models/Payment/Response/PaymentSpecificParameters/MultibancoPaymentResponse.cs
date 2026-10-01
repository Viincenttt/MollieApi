namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    public record MultibancoPaymentResponse : PaymentResponse {
        /// <summary>
        /// An object with payment details.
        /// </summary>
        public MultibancoPaymentResponseDetails? Details { get; set; }
    }

    public record MultibancoPaymentResponseDetails {
        /// <summary>
        /// Multibanco payment reference of the transaction.
        /// </summary>
        public string? MultibancoReference { get; set; }

        /// <summary>
        /// Multibanco entity reference of the transaction.
        /// </summary>
        public string? MultibancoEntity { get; set; }
    }
}
