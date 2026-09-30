using Mollie.Api.Models.Terminal.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.UnreferencedRefund.Response;

public record UnreferencedRefundResponseLinks {
    /// <summary>
    /// The API resource URL of the unreferenced refund itself.
    /// </summary>
    public required UrlObjectLink<UnreferencedRefundResponse> Self { get; set; }

    /// <summary>
    /// The API resource URL of the terminal the unreferenced refund was created on.
    /// </summary>
    public UrlObjectLink<TerminalResponse>? Terminal { get; set; }

    /// <summary>
    /// The URL to the unreferenced refund documentation.
    /// </summary>
    public required UrlLink Documentation { get; set; }
}
