namespace Mollie.Api.Models.Shipment.Request
{
    public record ShipmentUpdateRequest {
        public required TrackingObject Tracking { get; init; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this shipment a test shipment.
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
