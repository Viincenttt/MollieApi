namespace Mollie.Api.Models.PaymentMethod {
    public class PaymentResponseLinks {
        /// <summary>
        /// The API resource URL of the payment method itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}