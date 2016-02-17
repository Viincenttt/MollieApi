using System;
using System.Linq;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class PaymentMethodTests : BaseMollieApiTestClass {
        [Test]
        public void ListPaymentMethodsReturnsAllKnownPaymentMethods() {
            // If: We request all payment methods from the api
            ListResponse<PaymentMethodResponse> paymentMethodList = this._mollieClient.GetPaymentMethodList(0, 100).Result;
            Array paymentMethodEnumValues = Enum.GetValues(typeof(PaymentMethod));

            // Then: Make sure the list of payment methods is the same as our PaymentMethod enum
            Assert.AreEqual(paymentMethodEnumValues.Length - 1, paymentMethodList.TotalCount);
            foreach (PaymentMethod paymentMethodEnum in paymentMethodEnumValues) {
                if (paymentMethodEnum != PaymentMethod.Unknown) {
                    PaymentMethodResponse paymentResponse = paymentMethodList.Data.FirstOrDefault(x => x.Id == paymentMethodEnum.ToString().ToLower());
                    Assert.IsNotNull(paymentResponse);
                }
            }
        }

        [TestCase(PaymentMethod.Ideal)]
        [TestCase(PaymentMethod.CreditCard)]
        [TestCase(PaymentMethod.MisterCash)]
        [TestCase(PaymentMethod.Sofort)]
        [TestCase(PaymentMethod.BankTransfer)]
        [TestCase(PaymentMethod.DirectDebit)]
        [TestCase(PaymentMethod.Belfius)]
        [TestCase(PaymentMethod.Bitcoin)]
        [TestCase(PaymentMethod.PodiumCadeaukaart)]
        [TestCase(PaymentMethod.PayPal)]
        [TestCase(PaymentMethod.PaySafeCard)]
        public void CanRetrievePaymentMethod(PaymentMethod paymentMethod) {
            // If: We request a payment method from the api
            PaymentMethodResponse paymentMethodResponse = this._mollieClient.GetPaymentMethod(paymentMethod).Result;

            // Then: Id should be equal to our enum
            Assert.IsNotNull(paymentMethodResponse);
            Assert.AreEqual(paymentMethod.ToString().ToLower(), paymentMethodResponse.Id);
        }
    }
}
