using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Terminal.Response
{
    /// <summary>
    /// This Sublass is part of the TerminalResponse Class, Full documentation for this class can be found at https://docs.mollie.com/reference/v2/terminals-api/overview
    /// </summary>
    public record TerminalResponseLinks
    {
        /// <summary>
        /// The API resource URL of the payment method itself.
        /// </summary>
        public required UrlObjectLink<TerminalResponse> Self { get; set; }

        /// <summary>
        /// The URL to the payment method retrieval endpoint documentation.
        /// </summary>
        public UrlLink? Documentation { get; set; }
    }
}
