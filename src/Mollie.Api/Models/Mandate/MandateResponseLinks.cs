using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Mandate {
    public class MandateResponseLinks {
        /// <summary>
        /// The API resource URL of the mandate itself.
        /// </summary>
        public required UrlObjectLink<MandateResponse> Self { get; init; }

        /// <summary>
        /// The API resource URL of the customer the mandate is for.
        /// </summary>
        public required UrlObjectLink<CustomerResponse> Customer { get; init; }

        /// <summary>
        /// The URL to the mandate retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}