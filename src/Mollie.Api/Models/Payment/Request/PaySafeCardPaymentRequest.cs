namespace Mollie.Api.Models.Payment.Request {
    public class PaySafeCardPaymentRequest : PaymentRequest {
        public PaySafeCardPaymentRequest() {
            this.Method = PaymentMethod.PaySafeCard;
        }

        /// <summary>
        /// Used for consumer identification. For example, you could use the consumer’s IP address.
        /// </summary>
        public string CustomerReference { get; set; }
    }
}