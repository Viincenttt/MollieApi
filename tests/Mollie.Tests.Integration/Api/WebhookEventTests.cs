using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Tests.Integration.Framework;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

[Trait("TestCategory", "LocalIntegrationTests")]
public class WebhookEventTests : BaseMollieApiTestClass, IDisposable {
    private readonly IWebhookEventClient _webhookEventClient;

    public WebhookEventTests(IWebhookEventClient webhookEventClient) {
        _webhookEventClient = webhookEventClient;
    }

    [Fact]
    public async Task CanRetrieveWebhookEvent() {
        // Arrange
        const string webhookEventIdToRetrieve = "event_GvJ8WHrp5isUdRub9CJyH";

        // Act
        var webhookEvent = await _webhookEventClient.GetWebhookEventAsync(webhookEventIdToRetrieve);

        // Assert
        webhookEvent.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanRetrieveGenericWebhookEvent() {
        // Arrange
        const string webhookEventIdToRetrieve = "event_GvJ8WHrp5isUdRub9CJyH";

        // Act
        var webhookEvent = await _webhookEventClient.GetWebhookEventAsync<PaymentLinkResponse>(webhookEventIdToRetrieve);

        // Assert
        webhookEvent.ShouldNotBeNull();
    }

    public void Dispose() {
        _webhookEventClient.Dispose();
    }
}
