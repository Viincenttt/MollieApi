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
        [InlineData(PaymentMethod.Refund, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaPayLater, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaSliceIt, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaOne, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Przelewy24, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.ApplePay, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.MealVoucher, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.In3, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.PointOfSale, typeof(PointOfSalePaymentResponse))]
        [InlineData(PaymentMethod.Billie, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Trustly, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Twint, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Satispay, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Riverty, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Blik, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.BancomatPay, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.BacsDirectDebit, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Alma, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.GooglePay, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Voucher, typeof(VoucherPaymentResponse))]
        [InlineData(PaymentMethod.MbWay, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.Multibanco, typeof(MultibancoPaymentResponse))]
        [InlineData(PaymentMethod.Bizum, typeof(BizumPaymentResponse))]
        [InlineData(PaymentMethod.Swish, typeof(GenericPaymentResponse))]
        [InlineData(PaymentMethod.KlarnaPayNow, typeof(GenericPaymentResponse))]
        [InlineData("UnknownPaymentMethod", typeof(GenericPaymentResponse))]
        public void Create_CreatesTypeBasedOnPaymentMethod(string paymentMethod, Type expectedType) {
            // Given
            var sut = new PaymentResponseFactory();

            // When
            var result = sut.Create(paymentMethod);

            // Then
            result.ShouldBeOfType(expectedType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Create_WithoutPaymentMethod_CreatesBasePaymentResponse(string? paymentMethod) {
            // Given
            var sut = new PaymentResponseFactory();

            // When
            var result = sut.Create(paymentMethod);

            // Then
            result.ShouldBeOfType<PaymentResponse>();
        }
    }
}
