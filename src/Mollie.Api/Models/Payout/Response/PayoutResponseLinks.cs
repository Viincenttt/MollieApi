using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Payout.Response;

public record PayoutResponseLinks {
    /// <summary>
    /// The URL to the current resource.
    /// </summary>
    public required UrlObjectLink<PayoutResponse> Self { get; set; }

    /// <summary>
    /// The URL to the documentation of the current resource.
    /// </summary>
    public required UrlLink Documentation { get; set; }
}

