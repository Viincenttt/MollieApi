using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration.Api;

public class CaptureTests : BaseMollieApiTestClass, IDisposable {
    private readonly ICaptureClient _captureClient;
    private readonly IPaymentClient _paymentClient;
    
    public CaptureTests() {
        _captureClient = new CaptureClient(this.ApiKey);
        _paymentClient = new PaymentClient(this.ApiKey);
    }
    
    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl" +
                             " to make the payment, since Mollie can only capture payments that have been authorized")]
    public async Task CanCreateCaptureForPaymentWithManualCaptureMode() {
        // Given: We create a payment with captureMode set to manual
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = this.DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureMode = CaptureMode.Manual
        };
        var payment = await _paymentClient.CreatePaymentAsync(paymentRequest);
        
        // When: We create a capture for the payment
        var capture = await _captureClient.CreateCapture(payment.Id, new CaptureRequest
        {
            Amount = paymentRequest.Amount,
            Description = "my capture"
        });

        // Then: The capture should be created
        capture.Status.Should().Be("pending");
        capture.PaymentId.Should().Be(payment.Id);
        capture.Resource.Should().Be("capture");
    }
    
    public void Dispose()
    {
        _captureClient?.Dispose();
        _paymentClient?.Dispose();
    }
}