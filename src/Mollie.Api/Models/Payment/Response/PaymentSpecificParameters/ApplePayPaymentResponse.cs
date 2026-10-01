namespace Mollie.Api.Models.Payment.Response.PaymentSpecificParameters {
    /// <summary>
    /// Apple Pay payments return the same card details as credit card payments, with details.wallet set to applepay.
    /// </summary>
    public record ApplePayPaymentResponse : CreditCardPaymentResponse {
    }
}
