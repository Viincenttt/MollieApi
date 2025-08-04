using System;

namespace Mollie.Api.Models.Webhook.Response;

public record WebhookResponse {
    /// <summary>
    /// Indicates the response contains a webhook subscription object. Will always contain the string webhook for this
    /// endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The identifier uniquely referring to this subscription.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The subscription's events destination.
    /// </summary>
    public required string Url { get; set; }

    /// <summary>
    /// The identifier uniquely referring to the profile that created the subscription.
    /// </summary>
    public required string ProfileId { get; set; }

    /// <summary>
    /// The subscription's date time of creation.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// The subscription's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The events types that are subscribed.
    /// </summary>
    public required string EventTypes { get; set; }

    /// <summary>
    /// The subscription's current status.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The subscription's mode.
    /// </summary>
    public required string Mode { get; set; }
}
