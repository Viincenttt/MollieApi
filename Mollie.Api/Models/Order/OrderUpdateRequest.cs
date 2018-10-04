namespace Mollie.Api.Models.Order {
    public class OrderUpdateRequest {
        /// <summary>
        /// The billing person and address for the order. See Order address details for the
        /// exact fields needed.
        /// </summary>
        public OrderAddressDetails BillingAddress { get; set; }

        /// <summary>
        /// The shipping address for the order. See Order address details for the exact
        /// fields needed.
        /// </summary>
        public OrderAddressDetails ShippingAddress { get; set; }
    }
}