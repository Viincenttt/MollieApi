namespace Mollie.Api.Options;

public class MollieClientOptions {
    /// <summary>
    /// Your API-key or OAuth token
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// (Optional) ClientId used by Connect API
    /// </summary>
    public string? ClientId { get; set; } = string.Empty;

    /// <summary>
    /// (Optional) ClientSecret used by Connect API
    /// </summary>
    /// <returns></returns>
    public string? ClientSecret { get; set; } = string.Empty;
}
