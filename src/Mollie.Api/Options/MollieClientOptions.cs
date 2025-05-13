namespace Mollie.Api.Options;

public class MollieClientOptions {
    /// <summary>
    /// Your API-key or OAuth token
    /// </summary>
    public required string ApiKey { get; init; } = string.Empty;

    /// <summary>
    /// The default user agent is "Mollie.Api.NET {version}". When this property is set, the custom user agent will be
    /// appended to the default user agent.
    /// </summary>
    public string? CustomUserAgent { get; init; }

    /// <summary>
    /// (Optional) ClientId used by Connect API
    /// </summary>
    public string? ClientId { get; init; }

    /// <summary>
    /// (Optional) ClientSecret used by Connect API
    /// </summary>
    /// <returns></returns>
    public string? ClientSecret { get; init; }
}
