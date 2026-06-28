using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.DelayedRouting.Request {
    public record DelayedRoutingRequest {
        /// <summary>
        /// The amount to be routed.
        /// </summary>
        public required Amount Amount { get; set; }

        /// <summary>
        /// The destination of the route.
        /// </summary>
        public required RoutingDestination Destination { get; set; }

        /// <summary>
        /// Optional – Description shown in reports. Maximum length is 255 characters.
        /// </summary>
        public string? Description { get; set; }
    }
}


