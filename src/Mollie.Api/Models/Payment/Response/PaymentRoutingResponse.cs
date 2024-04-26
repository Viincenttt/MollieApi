using System;

namespace Mollie.Api.Models.Payment.Response
{
    public record PaymentRoutingResponse
    {

        /// <summary>
        /// Indicates the response contains a routing object. Will always contain route for this endpoint.
        /// </summary>
        public required string Resource { get; init; }

        /// <summary>
        /// The identifier uniquely referring to this route. Mollie assigns this identifier randomly at payment creation
        /// time. For example rt_k6cjd01h. Its ID will always be used by Mollie to refer to a certain route.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// If more than one routing object is given, the routing objects must indicate what portion of the total payment amount is being routed.
        /// </summary>
        public required Amount Amount { get; init; }

        /// <summary>
        /// The destination of this portion of the payment.
        /// </summary>
        public required RoutingDestination Destination { get; init; }

        /// <summary>
        /// Optional property you provided to schedule this portion of the payment to be transferred to its destination on a later date.
        /// If no date is given, the funds become available to the balance as soon as the payment succeeds.
        /// </summary>
        public DateTime? ReleaseDate { get; set; }
    }
}
