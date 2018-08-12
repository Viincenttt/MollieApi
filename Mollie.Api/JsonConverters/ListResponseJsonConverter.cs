using System;

namespace Mollie.Api.JsonConverters {

    using Framework.Factories;

    using Models.Payment;
    using Models.Payment.Response;

    using Newtonsoft.Json.Linq;
    /*
    public class ListResponseJsonConverter : JsonCreationConverter<PaymentResponse> {
        private readonly PaymentResponseFactory _paymentResponseFactory;

        public PaymentResponseConverter(PaymentResponseFactory paymentResponseFactory)
        {
            this._paymentResponseFactory = paymentResponseFactory;
        }

        protected override PaymentResponse Create(Type objectType, JObject jObject)
        {
            var paymentMethod = this.GetPaymentMethod(jObject);

            return this._paymentResponseFactory.Create(paymentMethod);
        }

        private PaymentMethod? GetPaymentMethod(JObject jObject)
        {
            if (this.FieldExists("method", jObject))
            {
                var paymentMethodValue = (string)jObject["method"];
                if (!string.IsNullOrEmpty(paymentMethodValue))
                {
                    return (PaymentMethod)Enum.Parse(typeof(PaymentMethod), paymentMethodValue, true);
                }
            }

            return null;
        }
    }*/
}
