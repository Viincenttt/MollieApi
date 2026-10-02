using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Models.Terminal.Response;

public record TerminalPairingCodeDetails {
    /// <summary>
    /// The QR code representation of the pairing code. Only available when the QR code is requested via the include parameter.
    /// </summary>
    public QrCode? QrCode { get; set; }
}
