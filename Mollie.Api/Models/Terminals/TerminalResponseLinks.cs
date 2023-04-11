using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Terminals
{
    /// <summary>
    /// This Sublass is part of the TerminalResponse Class, Full documentation for this class can be found at https://docs.mollie.com/reference/v2/terminals-api/overview
    /// </summary>
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
