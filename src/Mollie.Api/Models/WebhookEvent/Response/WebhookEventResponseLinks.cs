using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.WebhookEvent.Response;

public class WebhookEventResponseLinks<T> {
    /// <summary>
    /// The API resource URL of the payment method itself.
    /// </summary>
    public required UrlObjectLink<WebhookEventResponse<T>> Self { get; set; }

    /// <summary>
    /// The URL to the webhook documentation.
    /// </summary>
    public required UrlLink Documentation { get; set; }

    /// <summary>
    /// The API resource URL of the entity that this event belongs to.
    /// </summary>
    public required UrlObjectLink<T> Entity { get; set; }
}
