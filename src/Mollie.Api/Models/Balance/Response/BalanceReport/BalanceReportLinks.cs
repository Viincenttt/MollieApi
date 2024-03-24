using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Balance.Response.BalanceReport {
    public class BalanceReportLinks {
        /// <summary>
        /// The API resource URL of the balance report itself.
        /// </summary>
        public UrlObjectLink<BalanceReportResponse> Self { get; set; }
        
        /// <summary>
        /// The URL to the order retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}