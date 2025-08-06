namespace Mollie.Api.Models.Webhook.Request;

public record WebhookRequest {
    /// <summary>
    /// A name that identifies the webhook.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The URL Mollie will send the events to. This URL must be publicly accessible.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// The list of events to enable for this webhook. You may specify '*' to add all events, except those that require
    /// explicit selection. Separate multiple event types with a comma. See the Mollie.Api.Models.Webhook.WebhookEventTypes
    /// class for a full list of known values
    /// </summary>
    public required string EventTypes { get; set; }

    /// <summary>
    /// Whether to create the entity in test mode or live mode. Most API credentials are specifically created for
    /// either live mode or test mode, in which case this parameter can be omitted. For organization-level credentials
    /// such as OAuth access tokens, you can enable test mode by setting testmode to true.
    /// </summary>
    public bool? Testmode { get; set; }
}
