namespace Mollie.Api.Models.Chargeback {
    public class ChargebackResponseLinks {
        /// <summary>
        /// The API resource URL of the chargeback itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The API resource URL of the payment this chargeback belongs to.
        /// </summary>
        public UrlObject Payment { get; set; }

        /// <summary>
        /// The API resource URL of the settlement this payment has been settled with. Not present if not yet settled.
        /// </summary>
        public UrlObject Settlement { get; set; }

        /// <summary>
        /// The URL to the chargeback retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}