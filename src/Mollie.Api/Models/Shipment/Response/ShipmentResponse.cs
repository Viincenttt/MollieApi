using System;
using System.Collections.Generic;
using Mollie.Api.JsonConverters;
using Mollie.Api.Models.Order.Response;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Shipment.Response
{
    public record ShipmentResponse
    {
        /// <summary>
        /// Indicates the response contains a shipment object. Will always contain shipment for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The shipment’s unique identifier, for example shp_3wmsgCJN4U.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The order this shipment was created on, for example ord_8wmqcHMN4U.
        /// </summary>
        public required string OrderId { get; set; }

        /// <summary>
        /// The shipment’s date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// An object containing shipment tracking details. Will be omitted when no tracking details are available.
        /// </summary>
        public TrackingObject? Tracking { get; set; }

        /// <summary>
        /// The optional metadata you provided upon subscription creation. Metadata can for example be used to link a plan to a
        /// subscription.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        /// An array of order line objects
        /// </summary>
        public required IEnumerable<OrderLineResponse> Lines { get; set; }

        /// <summary>
        /// An object with several URL objects relevant to the order. Every URL object will contain an href and a type field.
        /// </summary>
        [JsonProperty("_links")]
        public required ShipmentResponseLinks Links { get; set; }

        public T? GetMetadata<T>(JsonSerializerSettings? jsonSerializerSettings = null) {
            return Metadata != null ? JsonConvert.DeserializeObject<T>(Metadata, jsonSerializerSettings) : default;
        }
    }
}
