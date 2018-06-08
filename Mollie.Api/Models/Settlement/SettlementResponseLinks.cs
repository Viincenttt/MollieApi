namespace Mollie.Api.Models.Settlement {
    public class SettlementResponseLinks {
        /// <summary>
        /// The API resource URL of the settlement itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The API resource URL of the payments that are included in this settlement.
        /// </summary>
        public UrlObject Payments { get; set; }

        /// <summary>
        /// The API resource URL of the refunds that are included in this settlement.
        /// </summary>
        public UrlObject Refunds { get; set; }

        /// <summary>
        /// The API resource URL of the chargebacks that are included in this settlement.
        /// </summary>
        public UrlObject Chargebacks { get; set; }

        /// <summary>
        /// The URL to the settlement retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}