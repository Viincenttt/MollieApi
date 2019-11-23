using System.Collections.Generic;
namespace Mollie.Api.Models.Shipment{
    public class ShipmentRequest {
        /// <summary>
        /// The total amount of the order, including VAT and discounts. This is the amount that will be charged
        /// to your customer.
        /// </summary>
        public TrackingObject Tracking { get; set; }

        /// <summary>
        /// The lines in the order. Each line contains details such as a description of the item ordered, its
        /// price et cetera. If you send an empty array, the entire order will be shipped
        /// </summary>
        public IEnumerable<ShipmentLineRequest> Lines { get; set; }

    }
}