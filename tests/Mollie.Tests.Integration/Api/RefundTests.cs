using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration.Api;

public class RefundTests : BaseMollieApiTestClass, IDisposable {
    private readonly IRefundClient _refundClient;
    private readonly IPaymentClient _paymentClient;

    public RefundTests() {
        _refundClient = new RefundClient(ApiKey);
        _paymentClient = new PaymentClient(ApiKey);
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
    public async Task CanCreateRefund() {
        // If: We create a payment
        string amount = "100.00";
        PaymentResponse payment = await CreatePayment(amount);

        // We can only test this if you make the payment using the payment.Links.Checkout property.
        // If you don't do this, this test will fail because we can only refund payments that have been paid
        Debugger.Break();

        // When: We attempt to refund this payment
        RefundRequest refundRequest = new RefundRequest() {
            Amount = new Amount(Currency.EUR, amount)
        };
        RefundResponse refundResponse = await _refundClient.CreateRefundAsync(payment.Id, refundRequest);

        // Then
        refundResponse.Should().NotBeNull();
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
    public async Task CanCreatePartialRefund() {
        // If: We create a payment of 250 euro
        PaymentResponse payment = await CreatePayment("250.00");

        // We can only test this if you make the payment using the payment.Links.PaymentUrl property.
        // If you don't do this, this test will fail because we can only refund payments that have been paid
        Debugger.Break();

        // When: We attempt to refund 50 euro
        RefundRequest refundRequest = new RefundRequest() {
            Amount = new Amount(Currency.EUR, "50.00")
        };
        RefundResponse refundResponse = await _refundClient.CreateRefundAsync(payment.Id, refundRequest);

        // Then
        refundResponse.Amount.Should().Be(refundRequest.Amount);
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
    public async Task CanRetrieveSingleRefund() {
        // If: We create a payment
        PaymentResponse payment = await CreatePayment();
        // We can only test this if you make the payment using the payment.Links.PaymentUrl property.
        // If you don't do this, this test will fail because we can only refund payments that have been paid
        Debugger.Break();

        RefundRequest refundRequest = new RefundRequest() {
            Amount = new Amount(Currency.EUR, "50.00")
        };
        RefundResponse refundResponse = await _refundClient.CreateRefundAsync(payment.Id, refundRequest);

        // When: We attempt to retrieve this refund
        RefundResponse result = await _refundClient.GetRefundAsync(payment.Id, refundResponse.Id);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(refundResponse.Id);
        refundResponse.Amount.Should().Be(refundRequest.Amount);
    }

    [DefaultRetryFact]
    public async Task CanRetrieveRefundList() {
        // If: We create a payment
        PaymentResponse payment = await CreatePayment();

        // When: Retrieve refund list for this payment
        ListResponse<RefundResponse> refundList = await _refundClient.GetRefundListAsync(payment.Id);

        // Then
        refundList.Should().NotBeNull();
        refundList.Items.Should().NotBeNull();
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl to make the payment, since Mollie can only refund payments that have been paid")]
    public async Task CanCreateRefundWithMetaData() {
        // If: We create a payment
        string amount = "100.00";
        PaymentResponse payment = await CreatePayment(amount);

        // We can only test this if you make the payment using the payment.Links.Checkout property.
        // If you don't do this, this test will fail because we can only refund payments that have been paid
        Debugger.Break();

        // When: We attempt to refund this payment with meta data.
        var metadata = "this is my metadata";
        RefundRequest refundRequest = new RefundRequest() {
            Amount = new Amount(Currency.EUR, amount),
            Metadata = metadata
        };
        RefundResponse refundResponse = await _refundClient.CreateRefundAsync(payment.Id, refundRequest);

        // Then: Make sure we get the same json result as metadata
        refundResponse.Metadata.Should().Be(metadata);
    }

    private async Task<PaymentResponse> CreatePayment(string amount = "100.00") {
        PaymentRequest paymentRequest = new PayPalPaymentRequest
        {
            Amount = new Amount(Currency.EUR, amount),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        return await _paymentClient.CreatePaymentAsync(paymentRequest);
    }

    public void Dispose()
    {
        _refundClient?.Dispose();
        _paymentClient?.Dispose();
    }
}
