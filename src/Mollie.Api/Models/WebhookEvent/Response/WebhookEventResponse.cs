using System;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.WebhookEvent.Response;

public class WebhookEventResponse<T> {
    /// <summary>
    /// Indicates the response contains a webhook event object. Will always contain the string event for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The identifier uniquely referring to this event.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// The event's type.
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    /// The entity token that triggered the event
    /// </summary>
    public required string EntityId { get; set; }

    /// <summary>
    /// The event's date time of creation.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Full payload of the event. Contains a null value for simple payloads.
    /// </summary>
    // TODO: Does not work because it should be _embedded.entity
    [JsonPropertyName("_embedded")]
    public T? Embedded { get; set; }

    /// <summary>
    /// An object with several relevant URLs. Every URL object will contain an href and a type field.
    /// </summary>
    /// <returns></returns>
    [JsonPropertyName("_links")]
    public required WebhookEventResponseLinks<T> Links { get; set; }
}
