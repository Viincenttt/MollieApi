namespace Mollie.Api.Models.Order.SpecificParameters
{
    public class ApplePaySpecificParameters : PaymentSpecificParameters
    {
        /// <summary>
        /// Optional - The Apple Pay Payment Token object (encoded as JSON) that is part of the result of authorizing a payment request. 
        /// </summary>
        public string ApplePayPaymentToken { get; set; }
    }
}
