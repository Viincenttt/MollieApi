namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record KlarnaSpecificParameters<T> : PaymentSpecificParameters where T : class {
        public T? ExtraMerchantData { get; set; }
    }
}