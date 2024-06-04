using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Balance.Response {
    public class BalanceResponseLinks {
        /// <summary>
        /// The API resource URL of the balance itself.
        /// </summary>
        public required UrlObjectLink<BalanceResponse> Self { get; set; }

        /// <summary>
        /// The URL to the order retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
