using System;
using Shouldly;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Request.PaymentSpecificParameters;
using Xunit;

namespace Mollie.Tests.Unit.Models.Payment.Request;

public class PaymentRequestTests {
    [Theory]
    [InlineData(PaymentMethod.CreditCard, typeof(CreditCardPaymentRequest))]
    [InlineData(PaymentMethod.Ideal, typeof(IdealPaymentRequest))]
    public void CreatePaymentRequest(string paymentMethod, Type expectedType) {
        var amount = new Amount(Currency.EUR, 50m);
        var description = "my-description";
        var paymentRequest = new PaymentRequest() {
            Amount = amount,
            Description = description
        };
        switch (paymentMethod) {
            case PaymentMethod.CreditCard:
                paymentRequest = new CreditCardPaymentRequest(paymentRequest) {
                    CardToken = "card-token"
                };
                break;
            case PaymentMethod.Ideal:
                paymentRequest = new IdealPaymentRequest(paymentRequest) {
                    Issuer = "ideal_issuer"
                };
                break;
        }

        paymentRequest.ShouldBeOfType(expectedType);
        paymentRequest.Amount.ShouldBe(amount);
        paymentRequest.Description.ShouldBe(description);
    }
}
