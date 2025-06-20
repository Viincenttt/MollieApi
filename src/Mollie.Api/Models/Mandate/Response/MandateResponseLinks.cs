using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Mandate.Response {
    public record MandateResponseLinks {
        /// <summary>
        /// The API resource URL of the mandate itself.
        /// </summary>
        public required UrlObjectLink<MandateResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the customer the mandate is for.
        /// </summary>
        public required UrlObjectLink<CustomerResponse> Customer { get; set; }

        /// <summary>
        /// The URL to the mandate retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
