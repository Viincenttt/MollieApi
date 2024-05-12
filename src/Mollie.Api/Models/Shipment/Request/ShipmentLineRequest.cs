namespace Mollie.Api.Models.Shipment.Request {
    public record ShipmentLineRequest {
        /// <summary>
        /// The API resource token of the order line, for example: odl_jp31jz.
        /// </summary>
        public required string Id { get; init; }

        /// <summary>
        /// The number of items that should be shipped for this order line.
        /// </summary>
        public int? Quantity { get; set; }

        /// <summary>
        /// The amount that you want to ship. In almost all cases, Mollie can determine the amount automatically.
        /// </summary>
        public Amount? Amount { get; set; }
    }
}
