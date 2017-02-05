namespace Mollie.Api.Models.Payment.Response {
    public class PayPalPaymentResponse : PaymentResponse {
        public PayPalPaymentResponseDetails Details { get; set; }
    }

    public class PayPalPaymentResponseDetails {
        /// <summary>
        /// The consumer's first and last name.
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// PayPal's reference for the transaction, for instance 9AL35361CF606152E.
        /// </summary>
        public string PayPalReference { get; set; }

        /// <summary>
        /// The consumer's email address.
        /// </summary>
        public string ConsumerAccount { get; set; }
    }
}