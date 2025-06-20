﻿using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Shipment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Capture.Response {
    public record CaptureResponseLinks {
        /// <summary>
        /// The API resource URL of the capture itself.
        /// </summary>
        public required UrlObjectLink<CaptureResponse> Self { get; set; }

        /// <summary>
        /// The API resource URL of the payment the capture belongs to.
        /// </summary>
        public required UrlObjectLink<PaymentResponse> Payment { get; set; }

        /// <summary>
        /// The API resource URL of the shipment that triggered the capture to be created.
        /// </summary>
        public required UrlObjectLink<ShipmentResponse> Shipment { get; set; }

        /// <summary>
        /// The API resource URL of the settlement this capture has been settled with. Not present if not yet settled.
        /// </summary>
        public required UrlObjectLink<SettlementResponse> Settlement { get; set; }

        /// <summary>
        /// The URL to the order retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
