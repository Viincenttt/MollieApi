using Mollie.Api.Models.Payment;

namespace Mollie.Api.Models.DelayedRouting.Request {
    public record DelayedRoutingRequest : ITestModeRequest {
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

        /// <summary>
        /// Optional – Set this to true to make this a test mode route.
        /// Only available for OAuth access tokens.
        /// </summary>
        public bool? Testmode { get; set; }
    }
}


