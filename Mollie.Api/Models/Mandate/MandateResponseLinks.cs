namespace Mollie.Api.Models.Mandate {
    public class MandateResponseLinks {
        /// <summary>
        /// The API resource URL of the mandate itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The API resource URL of the customer the mandate is for.
        /// </summary>
        public UrlObject Customer { get; set; }

        /// <summary>
        /// The URL to the mandate retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}