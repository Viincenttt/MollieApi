namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record KlarnaSpecificParameters<T> : OrderPaymentParameters where T : class {
        public T? ExtraMerchantData { get; set; }
    }
}
