using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Payment.Response.Specific;
using NUnit.Framework;

namespace Mollie.Tests.Unit.Framework.Factories {
    public class PaymentResponseFactoryTests {
        [TestCase(PaymentMethod.Bancontact, typeof(BancontactPaymentResponse))]
        [TestCase(PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
        [TestCase(PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
        [TestCase(PaymentMethod.CreditCard, typeof(CreditCardPaymentResponse))]
        [TestCase(PaymentMethod.DirectDebit, typeof(SepaDirectDebitResponse))]
        [TestCase(PaymentMethod.Eps, typeof(EpsPaymentResponse))]
        [TestCase(PaymentMethod.GiftCard, typeof(GiftcardPaymentResponse))]
        [TestCase(PaymentMethod.Giropay, typeof(GiropayPaymentResponse))]
        [TestCase(PaymentMethod.Ideal, typeof(IdealPaymentResponse))]
        [TestCase(PaymentMethod.IngHomePay, typeof(IngHomePayPaymentResponse))]
        [TestCase(PaymentMethod.Kbc, typeof(KbcPaymentResponse))]
        [TestCase(PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
        [TestCase(PaymentMethod.PaySafeCard, typeof(PaySafeCardPaymentResponse))]
        [TestCase(PaymentMethod.Sofort, typeof(SofortPaymentResponse))]
        [TestCase(PaymentMethod.Refund, typeof(PaymentResponse))]
        [TestCase(PaymentMethod.KlarnaPayLater, typeof(PaymentResponse))]
        [TestCase(PaymentMethod.KlarnaSliceIt, typeof(PaymentResponse))]
        [TestCase(PaymentMethod.Przelewy24, typeof(PaymentResponse))]
        [TestCase(PaymentMethod.ApplePay, typeof(PaymentResponse))]
        [TestCase(PaymentMethod.MealVoucher, typeof(PaymentResponse))]
        [TestCase(PaymentMethod.In3, typeof(PaymentResponse))]
        [TestCase("UnknownPaymentMethod", typeof(PaymentResponse))]
        public void Create_CreatesTypeBasedOnPaymentMethod(string paymentMethod, Type expectedType) {
            // Given
            var sut = new PaymentResponseFactory();
            
            // When
            var result = sut.Create(paymentMethod);

            // Then
            Assert.AreEqual(expectedType, result.GetType());
        }
    }
}