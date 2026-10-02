using System;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Terminal.Response;

/// <summary>
/// Full documentation for this class can be found at https://docs.mollie.com/reference/terminals-get-pairing-code
/// </summary>
public record TerminalPairingCodeResponse : IEntity {
    /// <summary>
    /// Indicates the response contains a terminal pairing code object. Will always contain the string terminal-pairing-code for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The unique identifier of the pairing code. For example termpc_R7gX5Ea9bC4DkFj3G.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Whether this entity was created in live mode or in test mode.
    /// </summary>
    public required Mode Mode { get; set; }

    /// <summary>
    /// The human-readable pairing code to be entered on the terminal. This code is multi-use and will expire after the
    /// time indicated in ExpiresAt.
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    /// The ID of the profile the terminal is being paired with.
    /// </summary>
    public required string ProfileId { get; set; }

    /// <summary>
    /// The status of the pairing code. See the Mollie.Api.Models.Terminal.Response.TerminalPairingCodeStatus class for
    /// a full list of known values.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// Additional pairing code data, present only when requested via the include parameter.
    /// </summary>
    public TerminalPairingCodeDetails? Details { get; set; }

    /// <summary>
    /// The date and time the pairing code expires, in ISO 8601 format. Pairing codes expire 90 days after creation.
    /// After this time, the code can no longer be used to pair a terminal.
    /// </summary>
    public required DateTimeOffset ExpiresAt { get; set; }

    /// <summary>
    /// The date and time the pairing code was revoked, in ISO 8601 format. Null if the code has not been revoked.
    /// </summary>
    public DateTimeOffset? RevokedAt { get; set; }

    /// <summary>
    /// The date and time the pairing code was created, in ISO 8601 format.
    /// </summary>
    public required DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// An object with several URL objects relevant to the pairing code. Every URL object will contain an href and a type field.
    /// </summary>
    [JsonPropertyName("_links")]
    public required TerminalPairingCodeResponseLinks Links { get; set; }
}
