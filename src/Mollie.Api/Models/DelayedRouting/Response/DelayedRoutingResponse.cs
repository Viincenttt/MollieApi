using System;
using System.Text.Json.Serialization;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.DelayedRouting.Response {
    public record DelayedRoutingResponse : IEntity {
        /// <summary>
        /// Indicates the response contains a route object. Will always contain the string route for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The identifier uniquely referring to this route. Example: crt_dyARQ3JzCgtPDhU2Pbq3J.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The unique identifier of the payment this route belongs to. For example: tr_5B8cwPMGnU6qLbRvo7qEZo.
        /// </summary>
        public required string PaymentId { get; set; }

        /// <summary>
        /// The amount of the route that will be routed to the specified destination.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The description of the route as shown in reports.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The destination of the route.
        /// </summary>
        public required RoutingDestination Destination { get; set; }

        /// <summary>
        /// The entity's date and time of creation, in ISO 8601 format.
        /// </summary>
        public required DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Useful URLs to related resources.
        /// </summary>
        [JsonPropertyName("_links")]
        public required DelayedRoutingResponseLinks Links { get; set; }
    }
}



