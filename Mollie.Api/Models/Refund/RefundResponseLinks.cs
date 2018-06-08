namespace Mollie.Api.Models.Refund {
    public class RefundResponseLinks {
        /// <summary>
        /// The API resource URL of the refund itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The API resource URL of the payment the refund belongs to.
        /// </summary>
        public UrlObject Payment { get; set; }

        /// <summary>
        /// The API resource URL of the settlement this payment has been settled with. Not present if not yet settled.
        /// </summary>
        public UrlObject Settlement { get; set; }

        /// <summary>
        /// The URL to the refund retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}