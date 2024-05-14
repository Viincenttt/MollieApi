namespace Mollie.Api.Models.Payment.Request.PaymentSpecificParameters {
    public record PaySafeCardPaymentRequest : PaymentRequest {
        public PaySafeCardPaymentRequest() {
            Method = PaymentMethod.PaySafeCard;
        }

        /// <summary>
        /// Used for consumer identification. For example, you could use the consumer’s IP address.
        /// </summary>
        public string? CustomerReference { get; set; }
    }
}
