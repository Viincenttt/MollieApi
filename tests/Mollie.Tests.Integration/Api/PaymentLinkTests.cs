using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class PaymentLinkTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPaymentLinkClient _paymentLinkClient;

    public PaymentLinkTests() {
        _paymentLinkClient = new PaymentLinkClient(ApiKey);
    }

    [DefaultRetryFact]
    public async Task CanRetrievePaymentLinkList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentLinkResponse> response = await _paymentLinkClient.GetPaymentLinkListAsync();

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task CanCreatePaymentLinkAndRetrieveIt() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.Should().Be(paymentLinkRequest.Amount);
            response.ExpiresAt.Should().Be(expiresAtWithoutMs);
            response.Description.Should().Be(paymentLinkRequest.Description);
            response.RedirectUrl.Should().Be(paymentLinkRequest.RedirectUrl);
            response.Archived.Should().BeFalse();
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }

    [DefaultRetryFact]
    public async Task CanCreatePaymentLinkWithNullAmount() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount =  null,
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a payment link with the expected properties
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.Should().BeNull();
            response.ExpiresAt.Should().Be(expiresAtWithoutMs);
            response.Description.Should().Be(paymentLinkRequest.Description);
            response.RedirectUrl.Should().Be(paymentLinkRequest.RedirectUrl);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }

    [Fact]
    public async Task CanUpdatePaymentLink() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We update the payment link
        PaymentLinkUpdateRequest paymentLinkUpdateRequest = new() {
            Description = "Updated description",
            Archived = true
        };
        var updatedPaymentLinkResponse = await _paymentLinkClient.UpdatePaymentLinkAsync(
            createdPaymentLinkResponse.Id,
            paymentLinkUpdateRequest);

        // Then: We expect the payment link to be updated
        updatedPaymentLinkResponse.Description.Should().Be(paymentLinkUpdateRequest.Description);
        updatedPaymentLinkResponse.Archived.Should().Be(paymentLinkUpdateRequest.Archived);
    }

    [Fact]
    public async Task CanDeletePaymentLink() {
        // Given: We create a new payment link
        PaymentLinkRequest paymentLinkRequest = new() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = DefaultWebhookUrl,
            RedirectUrl = DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await _paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We delete the payment link
        await _paymentLinkClient.DeletePaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect the payment link to be updated
        MollieApiException exception = await Assert.ThrowsAsync<MollieApiException>(() =>
            _paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id));
        exception.Details.Status.Should().Be(404);
        exception.Details.Detail.Should().Be("Payment link does not exists.");
    }

    public void Dispose()
    {
        _paymentLinkClient?.Dispose();
    }
}
