using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Terminals
{
    public class TerminalResponseLinks
    {
        /// <summary>
        /// The API resource URL of the payment method itself.
        /// </summary>
        public UrlObjectLink<TerminalResponse> Self { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public UrlLink Documentation { get; set; }
    }
}
