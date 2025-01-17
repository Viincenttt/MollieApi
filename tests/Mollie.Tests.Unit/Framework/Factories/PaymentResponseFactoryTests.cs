using System;
using Shouldly;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Payment.Response.PaymentSpecificParameters;
using Xunit;

namespace Mollie.Tests.Unit.Framework.Factories {
    public class PaymentResponseFactoryTests {
        [Theory]
        [InlineData(PaymentMethod.Bancontact, typeof(BancontactPaymentResponse))]
        [InlineData(PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
        [InlineData(PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
        [InlineData(PaymentMethod.CreditCard, typeof(CreditCardPaymentResponse))]
        [InlineData(PaymentMethod.DirectDebit, typeof(SepaDirectDebitResponse))]
        [InlineData(PaymentMethod.Eps, typeof(EpsPaymentResponse))]
        [InlineData(PaymentMethod.GiftCard, typeof(GiftcardPaymentResponse))]
        [InlineData(PaymentMethod.Giropay, typeof(GiropayPaymentResponse))]
        [InlineData(PaymentMethod.Ideal, typeof(IdealPaymentResponse))]
        [InlineData(PaymentMethod.IngHomePay, typeof(IngHomePayPaymentResponse))]
        [InlineData(PaymentMethod.Kbc, typeof(KbcPaymentResponse))]
        [InlineData(PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
        [InlineData(PaymentMethod.PaySafeCard, typeof(PaySafeCardPaymentResponse))]
        [InlineData(PaymentMethod.Sofort, typeof(SofortPaymentResponse))]
        [InlineData(PaymentMethod.Refund, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.KlarnaPayLater, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.KlarnaSliceIt, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.KlarnaOne, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Przelewy24, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.ApplePay, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.MealVoucher, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.In3, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.PointOfSale, typeof(PointOfSalePaymentResponse))]
        [InlineData(PaymentMethod.Billie, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Trustly, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Twint, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Satispay, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Riverty, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Blik, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.BancomatPay, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.BacsDirectDebit, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Alma, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.GooglePay, typeof(PaymentResponse))]
        [InlineData(PaymentMethod.Voucher, typeof(PaymentResponse))]
        [InlineData("UnknownPaymentMethod", typeof(PaymentResponse))]
        public void Create_CreatesTypeBasedOnPaymentMethod(string paymentMethod, Type expectedType) {
            // Given
            var sut = new PaymentResponseFactory();

            // When
            var result = sut.Create(paymentMethod);

            // Then
            result.ShouldBeOfType(expectedType);
        }
    }
}
