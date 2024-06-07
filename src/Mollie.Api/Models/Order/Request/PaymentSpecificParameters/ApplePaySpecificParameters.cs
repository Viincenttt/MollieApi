namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record ApplePaySpecificParameters : PaymentSpecificParameters {
        /// <summary>
        /// Optional - The Apple Pay Payment Token object (encoded as JSON) that is part of the result of authorizing a payment
        /// request. The token contains the payment information needed to authorize the payment.
        /// </summary>
        public string? ApplePayPaymentToken { get; set; }
    }
}