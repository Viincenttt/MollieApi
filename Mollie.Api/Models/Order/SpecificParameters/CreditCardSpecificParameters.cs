namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class CreditCardSpecificParameters : PaymentSpecificParameters
    {

        /// <summary>
        /// The card holder’s address details. We advise to provide these details to improve the credit card 
        /// fraud protection, and thus improve conversion.
        /// </summary>
        public AddressObject BillingAddress { get; set; }

        /// <summary>
        /// The shipping address details. We advise to provide these details to improve the credit card fraud 
        /// protection, and thus improve conversion.
        /// </summary>
        public AddressObject ShippingAddress { get; set; }
    }
}
