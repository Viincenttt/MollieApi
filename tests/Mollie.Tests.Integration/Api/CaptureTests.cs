using System;
using System.Linq;
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
        _captureClient = new CaptureClient(ApiKey);
        _paymentClient = new PaymentClient(ApiKey);
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl" +
                             " to make the payment, since Mollie can only capture payments that have been authorized")]
    public async Task CanCreateCaptureForPaymentWithManualCaptureMode() {
        // Given: We create a payment with captureMode set to manual
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, 1000.00m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureMode = CaptureMode.Manual
        };
        var payment = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // When: We create a capture for the payment
        var captureRequest = new CaptureRequest
        {
            Amount = new Amount(Currency.EUR, 0.01m),
            Description = "my capture",
            Metadata = "my-metadata string"
        };
        var capture = await _captureClient.CreateCapture(payment.Id, captureRequest);

        // Then: The capture should be created
        capture.Status.Should().Be("pending");
        capture.PaymentId.Should().Be(payment.Id);
        capture.Resource.Should().Be("capture");
        capture.Metadata.Should().Be(captureRequest.Metadata);
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we actually have to use the PaymentUrl" +
                             " to make the payment, since Mollie can only capture payments that have been authorized")]
    public async Task CanRetrieveCaptureListForPayment()
    {
        // Given: we create a payment and capture
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, 1000.00m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureMode = CaptureMode.Manual
        };
        var payment = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var captureRequest = new CaptureRequest
        {
            Amount = new Amount(Currency.EUR, 0.01m),
            Description = "my capture",
            Metadata = "my-metadata string"
        };
        await _captureClient.CreateCapture(payment.Id, captureRequest);

        // When: we retrieve the captures of the payment
        var captureList = await _captureClient.GetCaptureListAsync(payment.Id);

        // Then
        captureList.Count.Should().Be(1);
        var capture = captureList.Items.Single();
        capture.Status.Should().Be("succeeded");
        capture.PaymentId.Should().Be(payment.Id);
        capture.Resource.Should().Be("capture");
        capture.Metadata.Should().Be(captureRequest.Metadata);
    }

    public void Dispose()
    {
        _captureClient?.Dispose();
        _paymentClient?.Dispose();
    }
}
