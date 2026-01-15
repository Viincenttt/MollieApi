namespace Mollie.Api.Models.Shipment.Request
{
    public record ShipmentUpdateRequest : ITestModeRequest {
        public required TrackingObject Tracking { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this shipment a test shipment.
        /// </summary>
        public bool? Testmode { get; set; }
    }
}
