using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Webhook.Response;

/// <summary>
/// An object with several relevant URLs. Every URL object will contain an href and a type field.
/// </summary>
public record WebhookResponseLinks {
    /// <summary>
    /// The API resource URL of the webhook subscription itself.
    /// </summary>
    public required UrlObjectLink<WebhookResponse> Self { get; set; }

    /// <summary>
    /// The URL to the webhook subscription retrieval endpoint documentation.
    /// </summary>
    public required UrlLink Documentation { get; set; }
}
