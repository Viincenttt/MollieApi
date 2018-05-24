namespace Mollie.Api.Models.Payment.Response {
    public class PaymentResponseLinks {
        /// <summary>
        ///     The URL the consumer should visit to make the payment. This is where you redirect the consumer to.
        /// </summary>
        public string PaymentUrl { get; set; }

        /// <summary>
        ///     The API resource URL of the settlement this payment belongs to.
        /// </summary>
        public string Settlement { get; set; }
    }
}