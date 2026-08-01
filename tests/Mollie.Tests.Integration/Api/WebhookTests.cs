using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Webhook;
using Mollie.Api.Models.Webhook.Request;
using Mollie.Tests.Integration.Framework;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

[Trait("TestCategory", "LocalIntegrationTests")]
public class WebhookTests : BaseMollieApiTestClass, IDisposable, IAsyncLifetime {
    private readonly IWebhookClient _webhookClient;

    public WebhookTests(IWebhookClient webhookClient) {
        _webhookClient = webhookClient;
    }

    [Fact]
    public async Task CanCreateRetrieveAndDeleteWebhook() {
        // Given
        var request = new WebhookRequest {
            Name = "my-webhook",
            Url = "https://github.com/Viincenttt/MollieApi/",
            EventTypes = [WebhookEventTypes.PaymentLinkPaid, WebhookEventTypes.SalesInvoiceCreated],
            Testmode = true
        };

        // When: The webhook is created
        var createResult = await _webhookClient.CreateWebhookAsync(request);
        var created = createResult.Data!;

        // Then
        createResult.Success.ShouldBeTrue();
        created.Name.ShouldBe(request.Name);
        created.Url.ShouldBe(request.Url);
        created.EventTypes.ShouldBe([WebhookEventTypes.PaymentLinkPaid, WebhookEventTypes.SalesInvoiceCreated]);
        created.Mode.ShouldBe(Mode.Test);
        created.Resource.ShouldBe("webhook");
        created.Id.ShouldNotBeNullOrEmpty();

        // Then: The webhook can be retrieved
        var retrieveResult = await _webhookClient.GetWebhookAsync(created.Id, testmode: true);
        var retrieved = retrieveResult.Data!;
        retrieveResult.Success.ShouldBeTrue();
        retrieved.ShouldBeEquivalentTo(created);

        // Then: The webhook can be deleted
        await _webhookClient.DeleteWebhookAsync(created.Id, testmode: true);
    }

    [Fact]
    public async Task CanRetrieveWebhookList() {
        // Given

        // When: Retrieve webhook list
        var result = await _webhookClient.GetWebhookListAsync(testmode: true);
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanUpdateWebhook() {
        // Given: Create a webhook
        var createRequest = new WebhookRequest {
            Name = "my-webhook",
            Url = "https://github.com/Viincenttt/MollieApi/",
            EventTypes = [WebhookEventTypes.PaymentLinkPaid],
            Testmode = true
        };
        var createResult = await _webhookClient.CreateWebhookAsync(createRequest);
        var created = createResult.Data!;
        var updateRequest = new WebhookRequest {
            Name = "my-webhook-updated",
            Url = "https://github.com/Viincenttt/MollieApi/-updated",
            EventTypes = [WebhookEventTypes.PaymentLinkPaid, WebhookEventTypes.SalesInvoiceCreated],
            Testmode = true
        };

        // When: The webhook is updated
        var updateResult = await _webhookClient.UpdateWebhookAsync(created.Id, updateRequest);
        var updated = updateResult.Data!;

        // Then
        createResult.Success.ShouldBeTrue();
        updateResult.Success.ShouldBeTrue();
        updated.Name.ShouldBe(updateRequest.Name);
        updated.Url.ShouldBe(updateRequest.Url);
        updated.EventTypes.ShouldBe([WebhookEventTypes.PaymentLinkPaid, WebhookEventTypes.SalesInvoiceCreated]);
        updated.Mode.ShouldBe(Mode.Test);
    }

    [Fact]
    public async Task CanTestWebhook() {
        // Given: Create a webhook
        var createRequest = new WebhookRequest {
            Name = "my-webhook",
            Url = "https://github.com/Viincenttt/MollieApi/",
            EventTypes = [WebhookEventTypes.PaymentLinkPaid],
            Testmode = true
        };
        var createResult = await _webhookClient.CreateWebhookAsync(createRequest);
        var created = createResult.Data!;

        // When: The webhook is updated
        var result = await _webhookClient.TestWebhookAsync(created.Id, testmode: true);

        // Then: An error result is returned as the URL can't be reached
        createResult.Success.ShouldBeTrue();
        result.Success.ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.ToString().ShouldBe("Unprocessable Entity - Failed to ping the webhook subscription.");
    }

    public async Task InitializeAsync() {
        var result = await _webhookClient.GetWebhookListAsync(testmode: true);
        var webhooks = result.Data!;
        foreach (var webhook in webhooks.Items) {
            await _webhookClient.DeleteWebhookAsync(webhook.Id, testmode: true);
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public void Dispose() => _webhookClient.Dispose();
}
