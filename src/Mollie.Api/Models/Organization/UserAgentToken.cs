using System;

namespace Mollie.Api.Models.Organization;

public record UserAgentToken {
    /// <summary>
    /// The unique User-Agent token.
    /// </summary>
    public required string Token { get; set; }

    /// <summary>
    /// The date from which the token is active, in ISO 8601 format.
    /// </summary>
    public required DateTimeOffset StartsAt { get; set; }

    /// <summary>
    /// The date until when the token will be active, in ISO 8601 format. Will be null if the token does not
    /// have an end date (yet).
    /// </summary>
    public DateTimeOffset? EndsAt { get; set; }
}
