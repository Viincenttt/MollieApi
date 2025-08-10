using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.WebhookEvent.Response;

/// <summary>
/// An object with several relevant URLs. Every URL object will contain an href and a type field.
/// </summary>
public record WebhookEventResponseLinks {
    /// <summary>
    /// In v2 endpoints, URLs are commonly represented as objects with an href and type field.
    /// </summary>
    public required UrlObjectLink<SimpleWebhookEventResponse> Self { get; set; }

    /// <summary>
    /// In v2 endpoints, URLs are commonly represented as objects with an href and type field.
    /// </summary>
    public required UrlLink Documentation { get; set; }

    /// <summary>
    /// The API resource URL of the entity that this event belongs to.
    /// </summary>
    public required UrlObjectLink<IEntity> Entity { get; set; }
}
