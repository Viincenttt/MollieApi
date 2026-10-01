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
        [InlineData(PaymentMethod.Refund, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaPayLater, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaSliceIt, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaOne, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Przelewy24, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.ApplePay, typeof(ApplePayPaymentResponse))]
        [InlineData(PaymentMethod.MealVoucher, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.In3, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.PointOfSale, typeof(PointOfSalePaymentResponse))]
        [InlineData(PaymentMethod.Billie, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Trustly, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Twint, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Satispay, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Riverty, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Blik, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.BancomatPay, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.BacsDirectDebit, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Alma, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.GooglePay, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Voucher, typeof(VoucherPaymentResponse))]
        [InlineData(PaymentMethod.MbWay, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.Multibanco, typeof(MultibancoPaymentResponse))]
        [InlineData(PaymentMethod.Bizum, typeof(BizumPaymentResponse))]
        [InlineData(PaymentMethod.Swish, typeof(DefaultPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaPayNow, typeof(DefaultPaymentResponse))]
        [InlineData("UnknownPaymentMethod", typeof(DefaultPaymentResponse))]
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
