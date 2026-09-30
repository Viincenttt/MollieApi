using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.UnreferencedRefund.Response;

/// <summary>
/// An unreferenced refund is a refund that is not linked to a payment. The refund is paid out to the customer through a
/// point-of-sale terminal.
/// </summary>
public record UnreferencedRefundResponse : IEntity {
    /// <summary>
    /// Indicates the response contains an unreferenced refund object. Will always contain the string unreferenced-refund
    /// for this endpoint.
    /// </summary>
    public required string Resource { get; set; }

    /// <summary>
    /// The identifier uniquely referring to this unreferenced refund. Example: unref_vytxeTZskVKR7C7WgdSP3d.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Whether this entity was created in live mode or in test mode.
    /// </summary>
    public required Mode Mode { get; set; }

    /// <summary>
    /// The description of the unreferenced refund.
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// The amount refunded to the customer.
    /// </summary>
    public required Amount Amount { get; set; }

    /// <summary>
    /// The status of the unreferenced refund.
    /// Refer to the <see cref="UnreferencedRefundStatus"/> class for a full list of known values.
    /// </summary>
    public required string Status { get; set; }

    /// <summary>
    /// The identifier of the terminal the unreferenced refund was created on. Example: term_7MgL4wea46qkRcoTZjWEH.
    /// </summary>
    public required string TerminalId { get; set; }

    /// <summary>
    /// The optional metadata you provided upon creation of the unreferenced refund.
    /// </summary>
    [JsonConverter(typeof(RawJsonConverter))]
    public string? Metadata { get; set; }

    /// <summary>
    /// The entity's date and time of creation, in ISO 8601 format.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// An object with several URL objects relevant to the unreferenced refund. Every URL object will contain an href and
    /// a type field.
    /// </summary>
    [JsonPropertyName("_links")]
    public required UnreferencedRefundResponseLinks Links { get; set; }

    public T? GetMetadata<T>(JsonSerializerOptions? jsonSerializerOptions = null) {
        return Metadata != null ? JsonSerializer.Deserialize<T>(Metadata, jsonSerializerOptions) : default;
    }
}
