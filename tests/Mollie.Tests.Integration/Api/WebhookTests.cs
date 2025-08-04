using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Webhook.Request;
using Mollie.Api.Models.Webhook.Response;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class WebhookTests : IAsyncLifetime {
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
            EventTypes = "payment-link.paid,sales-invoice.created",
            Testmode = true
        };

        // When: The webhook is created
        WebhookResponse created = await _webhookClient.CreateWebhookAsync(request);

        // Then
        created.Name.ShouldBe(request.Name);
        created.Url.ShouldBe(request.Url);
        created.EventTypes.ShouldBe(new [] { "payment-link.paid", "sales-invoice.created" });
        created.Mode.ShouldBe(Mode.Test);
        created.Resource.ShouldBe("webhook");
        created.Id.ShouldNotBeNullOrEmpty();

        // Then: The webhook can be retrieved
        WebhookResponse retrieved = await _webhookClient.GetWebhookAsync(created.Id);
        retrieved.ShouldBeEquivalentTo(created);

        // Then: The webhook can be deleted
        await _webhookClient.DeleteWebhookAsync(created.Id, testmode: true);
    }

    [Fact]
    public async Task CanRetrieveWebhookList() {
        // Given

        // When: Retrieve webhook list
        ListResponse<WebhookResponse> response = await _webhookClient.GetWebhookListAsync(testmode: true);

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanUpdateWebhook() {
        // Given: Create a webhook
        var createRequest = new WebhookRequest {
            Name = "my-webhook",
            Url = "https://github.com/Viincenttt/MollieApi/",
            EventTypes = "payment-link.paid,sales-invoice.created",
            Testmode = true
        };
        WebhookResponse created = await _webhookClient.CreateWebhookAsync(createRequest);
        var updateRequest = new WebhookRequest {
            Name = "my-webhook-updated",
            Url = "https://github.com/Viincenttt/MollieApi/-updated",
            EventTypes = "payment-link.paid",
            Testmode = true
        };

        // When: The webhook is updated
        WebhookResponse updated = await _webhookClient.UpdateWebhookAsync(created.Id, updateRequest);

        // Then
        updated.Name.ShouldBe(updateRequest.Name);
        updated.Url.ShouldBe(updateRequest.Url);
        updated.EventTypes.ShouldBe(new[] { "payment-link.paid" });
        updated.Mode.ShouldBe(Mode.Test);
    }

    public async Task InitializeAsync() {
        ListResponse<WebhookResponse> webhooks = await _webhookClient.GetWebhookListAsync(testmode: true);
        foreach (var webhook in webhooks.Items) {
            await _webhookClient.DeleteWebhookAsync(webhook.Id, testmode: true);
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;
}
