using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Shipment;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Capture {
    public record CaptureResponseLinks {
        /// <summary>
        /// The API resource URL of the capture itself.
        /// </summary>
        public required UrlObjectLink<CaptureResponse> Self { get; init; }

        /// <summary>
        /// The API resource URL of the payment the capture belongs to.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Payment { get; init; }

        /// <summary>
        /// The API resource URL of the shipment that triggered the capture to be created.
        /// </summary>
        public required UrlObjectLink<ShipmentResponse> Shipment { get; init; }
        
        /// <summary>
        /// The API resource URL of the settlement this capture has been settled with. Not present if not yet settled.
        /// </summary>
        public required UrlObjectLink<SettlementResponse> Settlement { get; init; }
        
        /// <summary>
        /// The URL to the order retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; init; }
    }
}