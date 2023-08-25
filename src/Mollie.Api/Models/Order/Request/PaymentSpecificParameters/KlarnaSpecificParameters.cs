namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public class KlarnaSpecificParameters<T> : PaymentSpecificParameters where T : class {
        public T ExtraMerchantData { get; set; }
    }
}