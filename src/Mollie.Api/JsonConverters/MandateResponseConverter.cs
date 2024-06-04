using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Mandate.Response;
using Newtonsoft.Json.Linq;

namespace Mollie.Api.JsonConverters;

internal class MandateResponseConverter : JsonCreationConverter<MandateResponse> {
    private readonly MandateResponseFactory _mandateResponseFactory;

    public MandateResponseConverter(MandateResponseFactory mandateResponseFactory) {
        _mandateResponseFactory = mandateResponseFactory;
    }

    protected override MandateResponse Create(Type objectType, JObject jObject) {
        string? paymentMethod = GetPaymentMethod(jObject);

        return _mandateResponseFactory.Create(paymentMethod);
    }

    private string? GetPaymentMethod(JObject jObject) {
        if (FieldExists("method", jObject)) {
            string paymentMethodValue = (string) jObject["method"]!;
            if (!string.IsNullOrEmpty(paymentMethodValue)) {
                return paymentMethodValue;
            }
        }

        return null;
    }
}
