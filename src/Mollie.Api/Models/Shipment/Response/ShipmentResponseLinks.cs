﻿using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.Shipment.Response {
    public record ShipmentResponseLinks {
        /// <summary>
        /// The API resource URL of the order itself.
        /// </summary>
        public required UrlObjectLink<ShipmentResponse> Self { get; set; }

        /// <summary>
        /// The URL your customer should visit to make the payment for the order.
        /// This is where you should redirect the customer to after creating the order.
        /// </summary>
        public required UrlLink Checkout { get; set; }

        /// <summary>
        /// The URL to the order retrieval endpoint documentation.
        /// </summary>
        public required UrlLink Documentation { get; set; }
    }
}
