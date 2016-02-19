using System.Diagnostics;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class RefundTests : BaseMollieApiTestClass {
        [Test]
        [Ignore("We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
        public void CanCreateRefund() {
            // If: We create a payment
            PaymentResponse payment = this.CreatePayment();

            // We can only test this if you make the payment using the payment.Links.PaymentUrl property. 
            // If you don't do this, this test will fail because we can only refund payments that have been paid
            Debugger.Break(); 

            // When: We attempt to refund this payment
            RefundResponse refundResponse = this._mollieClient.CreateRefundAsync(payment.Id, 100).Result;

            // Then
            Assert.IsNotNull(refundResponse);
        }

        [Test]
        [Ignore("We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
        public void CanRetrieveSingleRefund() {
            // If: We create a payment
            PaymentResponse payment = this.CreatePayment();
            // We can only test this if you make the payment using the payment.Links.PaymentUrl property. 
            // If you don't do this, this test will fail because we can only refund payments that have been paid
            Debugger.Break();
            RefundResponse refundResponse = this._mollieClient.CreateRefundAsync(payment.Id, 100).Result;

            // When: We attempt to retrieve this refund
            RefundResponse result = this._mollieClient.GetRefundAsync(payment.Id, refundResponse.Id).Result;

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(refundResponse.Id, result.Id);
            Assert.AreEqual(refundResponse.AmountRefunded, result.AmountRefunded);
            Assert.AreEqual(refundResponse.AmountRemaining, result.AmountRemaining);
        }

        [Test]
        public void CanRetrieveRefundList() {
            // If: We create a payment
            PaymentResponse payment = this.CreatePayment();

            // When: Retrieve refund list for this payment
            ListResponse<RefundResponse> refundList = this._mollieClient.GetRefundListAsync(payment.Id).Result;

            // Then
            Assert.IsNotNull(refundList);
        }

        private PaymentResponse CreatePayment() {
            PaymentRequest paymentRequest = new CreditCardPaymentRequest();
            paymentRequest.Amount = 100;
            paymentRequest.Description = "Description";
            paymentRequest.RedirectUrl = this.DefaultRedirectUrl;

            return this._mollieClient.CreatePaymentAsync(paymentRequest).Result;
        }
    }
}
