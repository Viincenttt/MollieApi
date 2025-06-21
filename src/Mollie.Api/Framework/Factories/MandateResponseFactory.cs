using System;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Mandate.Response.PaymentSpecificParameters;
using Mollie.Api.Models.Payment;

namespace Mollie.Api.Framework.Factories;

internal class MandateResponseFactory : ITypeFactory<MandateResponse> {
    public MandateResponse Create(string? paymentMethod) {
            if (string.IsNullOrEmpty(paymentMethod)) {
                return Activator.CreateInstance<MandateResponse>();
            }

            switch (paymentMethod) {
                case PaymentMethod.PayPal:
                    return Activator.CreateInstance<PayPalMandateResponse>();
                case PaymentMethod.DirectDebit:
                    return Activator.CreateInstance<SepaDirectDebitMandateResponse>();
                case PaymentMethod.CreditCard:
                    return Activator.CreateInstance<CreditCardMandateResponse>();
                default:
                    return Activator.CreateInstance<MandateResponse>();
            }
        }
}
