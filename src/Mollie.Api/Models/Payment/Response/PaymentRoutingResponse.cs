using System;
using Mollie.Api.JsonConverters;
using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Payment.Response
{
    public record PaymentRoutingResponse
    {

        /// <summary>
        /// Indicates the response contains a routing object. Will always contain route for this endpoint.
        /// </summary>
        public required string Resource { get; set; }

        /// <summary>
        /// The identifier uniquely referring to this route. Mollie assigns this identifier randomly at payment creation
        /// time. For example rt_k6cjd01h. Its ID will always be used by Mollie to refer to a certain route.
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// The mode used to create this route. Mode determines whether a route is real or a test route.
        /// </summary>
        public required Mode Mode { get; set; }

        /// <summary>
        /// The date and time when the route was created, in ISO 8601 format.
        /// </summary>
        public required DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// If more than one routing object is given, the routing objects must indicate what portion of the total payment amount is being routed.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The destination of this portion of the payment.
        /// </summary>
        public required RoutingDestination Destination { get; set; }

        /// <summary>
        /// Optional property you provided to schedule this portion of the payment to be transferred to its destination on a later date.
        /// If no date is given, the funds become available to the balance as soon as the payment succeeds.
        /// </summary>
#if NET8_0_OR_GREATER
        public DateOnly? ReleaseDate { get; set; }
#else
        [JsonConverter(typeof(DateJsonConverter))]
        public DateTime? ReleaseDate { get; set; }
#endif

        /// <summary>
        /// An object with several URL objects relevant to the route.
        /// </summary>
        [JsonPropertyName("_links")]
        public required PaymentRoutingResponseLinks Links { get; set; }
    }
}
