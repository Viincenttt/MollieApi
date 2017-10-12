using System.Diagnostics;
using System.Threading.Tasks;
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
        public async Task CanCreateRefund() {
            // If: We create a payment
            PaymentResponse payment = await this.CreatePayment();

            // We can only test this if you make the payment using the payment.Links.PaymentUrl property. 
            // If you don't do this, this test will fail because we can only refund payments that have been paid
            Debugger.Break(); 

            // When: We attempt to refund this payment
            RefundResponse refundResponse = await this._refundClient.CreateRefundAsync(payment.Id);

            // Then
            Assert.IsNotNull(refundResponse);
        }

        [Test]
        [Ignore("We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
        public async Task CanCreatePartialRefund() {
            // If: We create a payment of 250 euro
            PaymentResponse payment = await this.CreatePayment(250);

            // We can only test this if you make the payment using the payment.Links.PaymentUrl property. 
            // If you don't do this, this test will fail because we can only refund payments that have been paid
            Debugger.Break();

            // When: We attempt to refund 50 euro
            RefundRequest refundRequest = new RefundRequest() {
                Amount = 50
            };
            RefundResponse refundResponse = await this._refundClient.CreateRefundAsync(payment.Id, refundRequest);

            // Then
            Assert.AreEqual(50, refundResponse.Payment.AmountRefunded);
            Assert.AreEqual(200, refundResponse.Payment.AmountRemaining);
        }

        [Test]
        [Ignore("We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
        public async Task CanRetrieveSingleRefund() {
            // If: We create a payment
            PaymentResponse payment = await this.CreatePayment();
            // We can only test this if you make the payment using the payment.Links.PaymentUrl property. 
            // If you don't do this, this test will fail because we can only refund payments that have been paid
            Debugger.Break();
            RefundResponse refundResponse = await this._refundClient.CreateRefundAsync(payment.Id);

            // When: We attempt to retrieve this refund
            RefundResponse result = await this._refundClient.GetRefundAsync(payment.Id, refundResponse.Id);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(refundResponse.Id, result.Id);
            Assert.AreEqual(refundResponse.Payment.AmountRefunded, result.Payment.AmountRefunded);
            Assert.AreEqual(refundResponse.Payment.AmountRemaining, result.Payment.AmountRemaining);
        }

        [Test]
        public async Task CanRetrieveRefundList() {
            // If: We create a payment
            PaymentResponse payment = await this.CreatePayment();

            // When: Retrieve refund list for this payment
            ListResponse<RefundResponse> refundList = await this._refundClient.GetRefundListAsync(payment.Id);

            // Then
            Assert.IsNotNull(refundList);
        }

        private async Task<PaymentResponse> CreatePayment(decimal amount = 100) {
            PaymentRequest paymentRequest = new CreditCardPaymentRequest();
            paymentRequest.Amount = amount;
            paymentRequest.Description = "Description";
            paymentRequest.RedirectUrl = this.DefaultRedirectUrl;

            return await this._paymentClient.CreatePaymentAsync(paymentRequest);
        }
    }
}
