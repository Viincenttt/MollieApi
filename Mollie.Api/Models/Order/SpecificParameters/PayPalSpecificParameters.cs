namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class PayPalSpecificParameters : PaymentSpecificParameters
    {
        /// <summary>
        /// Description for Paypal order. Use keywords Cart, Order, Invoice or Payment followed by order ID.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Optional - The shipping address details.
        /// </summary>
        public AddressObject ShippingAddress { get; set; }
    }
}
