namespace Mollie.Api.Models.Organization {
    public class OrganizationResponseLinks {
        /// <summary>
        /// The API resource URL of the organization itself.
        /// </summary>
        public UrlObject Self { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public UrlObject Documentation { get; set; }
    }
}