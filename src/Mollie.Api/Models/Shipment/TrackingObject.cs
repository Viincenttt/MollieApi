namespace Mollie.Api.Models.Shipment{
    public record TrackingObject {
        /// <summary>
        /// Name of the postal carrier (as specific as possible). For example PostNL.
        /// </summary>
        public required string Carrier { get; init; }

        /// <summary>
        /// The track and trace code of the shipment. For example 3SKABA000000000.
        /// </summary>
        public required string Code { get; init; }

        /// <summary>
        /// The URL where your customer can track the shipment
        /// </summary>
        public string? Url { get; set; }
    }
}
