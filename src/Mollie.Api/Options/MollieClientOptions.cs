using Mollie.Api.Client;

namespace Mollie.Api.Options;

public class MollieClientOptions {
    /// <summary>
    /// Your API-key or OAuth token
    /// </summary>
    public required string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// (Optional) The default user agent is "Mollie.Api.NET {version}". When this property is set, the custom user
    /// agent will be appended to the default user agent.
    /// </summary>
    public string? CustomUserAgent { get; set; }

    /// <summary>
    /// (Optional) ClientId used by Connect API
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// (Optional) ClientSecret used by Connect API
    /// </summary>
    /// <returns></returns>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// The base URL for all API requests. Can be overridden for testing purposes.
    /// </summary>
    public string ApiBaseUrl { get; set; } = BaseMollieClient.DefaultBaseApiEndPoint;

    /// <summary>
    /// The authorize endpoint for the Connect client. Can be overridden for testing purposes.
    /// </summary>
    public string ConnectOAuthAuthorizeEndPoint { get; set; } = ConnectClient.DefaultAuthorizeEndpoint;

    /// <summary>
    /// The token endpoint for the Connect client. Can be overridden for testing purposes.
    /// </summary>
    public string ConnectTokenEndPoint { get; set; } = ConnectClient.DefaultTokenEndpoint;
}
