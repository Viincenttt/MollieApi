namespace Mollie.Api.Models.WebhookEvent.Response;

public record FullWebhookEventResponse<T> : SimpleWebhookEventResponse where T : IEntity {
    /// <summary>
    /// Full payload of the event.
    /// </summary>
    public required T Entity { get; set; }
}

public record FullWebhookEventResponse : SimpleWebhookEventResponse {
    /// <summary>
    /// Full payload of the event.
    /// </summary>
    public required IEntity Entity { get; set; }
}
