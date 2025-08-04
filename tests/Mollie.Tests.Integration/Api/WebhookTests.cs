using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Webhook.Request;
using Mollie.Api.Models.Webhook.Response;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class WebhookTests {
    private readonly IWebhookClient _webhookClient;

    public WebhookTests(IWebhookClient webhookClient) {
        _webhookClient = webhookClient;
    }

    [Trait("TestCategory", "LocalIntegrationTests")]
    [Fact]
    public async Task CanCreateAndDeleteWebhook() {
        // Given
        var request = new WebhookRequest {
            Name = "my-webhook",
            Url = "https://github.com/Viincenttt/MollieApi/",
            EventTypes = "payment-link.paid,sales-invoice.created",
            Testmode = true
        };

        // When: The webhook is created
        WebhookResponse response = await _webhookClient.CreateWebhookAsync(request);

        // Then
        response.Name.ShouldBe(request.Name);
        response.Url.ShouldBe(request.Url);
        response.EventTypes.ShouldBe(new [] { "payment-link.paid", "sales-invoice.created" });
        response.Mode.ShouldBe(Mode.Test);
        response.Resource.ShouldBe("webhook");
        response.Id.ShouldNotBeNullOrEmpty();
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
}
