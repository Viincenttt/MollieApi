using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Payment.Response;
using Newtonsoft.Json.Linq;

namespace Mollie.Api.JsonConverters {
    public class PaymentResponseConverter : JsonCreationConverter<PaymentResponse> {
        private readonly PaymentResponseFactory _paymentResponseFactory;

        public PaymentResponseConverter(PaymentResponseFactory paymentResponseFactory) {
            this._paymentResponseFactory = paymentResponseFactory;
        }

        protected override PaymentResponse Create(Type objectType, JObject jObject) {
            string paymentMethod = this.GetPaymentMethod(jObject);

            return this._paymentResponseFactory.Create(paymentMethod);
        }

        private string GetPaymentMethod(JObject jObject) {
            if (this.FieldExists("method", jObject)) {
                string paymentMethodValue = (string) jObject["method"];
                if (!string.IsNullOrEmpty(paymentMethodValue)) {
                    return paymentMethodValue;
                }
            }

            return null;
        }
    }
}