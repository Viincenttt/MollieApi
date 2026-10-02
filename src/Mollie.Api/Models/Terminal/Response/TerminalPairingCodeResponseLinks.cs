using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Terminal.Response;

public record TerminalPairingCodeResponseLinks {
    /// <summary>
    /// The API resource URL of the pairing code itself.
    /// </summary>
    public required UrlObjectLink<TerminalPairingCodeResponse> Self { get; set; }

    /// <summary>
    /// The API resource URL of the profile that the terminal will be paired with.
    /// </summary>
    public required UrlObjectLink<ProfileResponse> Profile { get; set; }

    /// <summary>
    /// The URL to the pairing code endpoint documentation.
    /// </summary>
    public required UrlLink Documentation { get; set; }
}
