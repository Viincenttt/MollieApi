namespace Mollie.Api.Models.Terminal.Request;

public record TerminalPairingCodeRequest {
    /// <summary>
    /// The ID of the profile to pair the terminal with.
    /// </summary>
    public required string ProfileId { get; set; }
}
