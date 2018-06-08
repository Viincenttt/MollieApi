namespace Mollie.Api.Models.Customer {
    public class CustomerResponseLinks {
        /// <summary>
        /// The API resource URL of the customer itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The URL to the customer retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}