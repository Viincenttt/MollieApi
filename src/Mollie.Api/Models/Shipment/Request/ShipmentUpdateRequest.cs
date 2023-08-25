namespace Mollie.Api.Models.Shipment
{
    public class ShipmentUpdateRequest{
        public TrackingObject Tracking { get; set; }
        
        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this shipment a test shipment.
        /// </summary>
        public bool? Testmode { get; set; }
    }
}