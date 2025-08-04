using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Webhook.Response;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class WebhookClientTests : BaseClientTests {
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetWebhookAsync_NoWebhookIdIsGiven_ArgumentExceptionIsThrown(string? terminalId) {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new WebhookClient("api-key", httpClient);

        // When
#pragma warning disable CS8604 // Possible null reference argument.
        var exception =
            await Assert.ThrowsAsync<ArgumentException>(async () => await client.GetWebhookAsync(terminalId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'webhookId' is null or empty");
    }


    [Fact]
    public async Task GetWebhookAsync_WithWebhookId_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string webhookId = "webhook-id";
        const string name = "webhook-name";
        const string url = "https://mollie.com/webhook";
        string[] eventTypes = ["payment-link.paid", "sales-invoice.created"];
        string jsonToReturnInMockResponse = CreateWebhookJsonResponse(webhookId, name, url,
            $"\"{eventTypes[0]}\", \"{eventTypes[1]}\"");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}webhooks/{webhookId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var webhookClient = new WebhookClient("abcde", httpClient);

        // When
        WebhookResponse response = await webhookClient.GetWebhookAsync(webhookId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        response.Id.ShouldBe(webhookId);
        response.Resource.ShouldBe("webhook");
        response.Name.ShouldBe(name);
        response.Url.ShouldBe(url);
        response.EventTypes.ShouldNotBeNull();
        response.EventTypes.ShouldBe(eventTypes);
        response.ProfileId.ShouldBe("pfl_8XcSdLtrNK");
        response.CreatedAt.ShouldBe(DateTime.Parse("2024-12-06T10:09:56+00:00"));
        response.Status.ShouldBe("enabled");
        response.Mode.ShouldBe(Mode.Test);
    }

    [Theory]
    [InlineData(true, "?testmode=true")]
    [InlineData(false, "")]
    public async Task GetWebhookAsync_QueryParameterOptions_CorrectParametersAreAdded(bool testMode, string expectedQueryString) {
        // Given
        const string webhookId = "webhook-id";
        const string name = "webhook-name";
        const string url = "https://mollie.com/webhook";
        string[] eventTypes = ["payment-link.paid", "sales-invoice.created"];
        string jsonToReturnInMockResponse = CreateWebhookJsonResponse(webhookId, name, url,
            $"\"{eventTypes[0]}\", \"{eventTypes[1]}\"");
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}webhooks/{webhookId}{expectedQueryString}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var webhookClient = new WebhookClient("abcde", httpClient);

        // When
        await webhookClient.GetWebhookAsync(webhookId, testMode);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
    }


    [Theory]
    [InlineData(null, null, false, "")]
    [InlineData("from", null, false, "?from=from")]
    [InlineData("from", 50, false, "?from=from&limit=50")]
    public async Task GetWebhookListAsync_QueryParameterOptions_CorrectParametersAreAdded(string? from, int? limit, bool testmode, string expectedQueryString) {
        // Given
        string jsonToReturnInMockResponse = CreateWebhookListJsonResponse();
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}webhooks{expectedQueryString}",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var webhookClient = new WebhookClient("abcde", httpClient);

        // When
        await webhookClient.GetWebhookListAsync(from, limit, testmode);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Fact]
    public async Task GetWebhookListAsync_ResponseIsDeserializedInExpectedFormat() {
        // Given
        string jsonToReturnInMockResponse = CreateWebhookListJsonResponse();
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}webhooks",
            jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var webhookClient = new WebhookClient("abcde", httpClient);

        // When
        ListResponse<WebhookResponse> response = await webhookClient.GetWebhookListAsync();

        // Then
        response.Count.ShouldBe(1);
        response.Items.Count.ShouldBe(response.Count);
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldNotBeNull();
    }

    private string CreateWebhookListJsonResponse() {
        string webhookJson = CreateWebhookJsonResponse("hook_5foxphpBru4xNPCDJJPzH", "Webhook 1",
            "https://mollie.com/webhook1", "\"payment-link.paid\"");

        return $@"{{
  ""_embedded"": {{
    ""webhooks"": [
      {webhookJson}
    ]
  }},
  ""count"": 1,
  ""_links"": {{
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/reference/list-webhook"",
      ""type"": ""text/html""
    }},
    ""self"": {{
      ""href"": ""https://api.mollie.localhost/v2/webhooks?from=hook_yjtBMWDCGw5YFSPQ3HPzH&limit=2"",
      ""type"": ""application/hal+json""
    }},
    ""previous"": {{
      ""href"": ""https://api.mollie.localhost/v2/webhooks?from=hook_5foxphpBru4xNPCDJJPzH&limit=2"",
      ""type"": ""application/hal+json""
    }},
    ""next"": {{
      ""href"": ""https://api.mollie.localhost/v2/webhooks?from=hook_fTqARmWsfs9oXvKbZEPzH&limit=2"",
      ""type"": ""application/hal+json""
    }}
  }}
}}";
    }

    private string CreateWebhookJsonResponse(string webhookId, string name, string url, string eventTypes) {
        return $@"{{
  ""resource"": ""webhook"",
  ""id"": ""{webhookId}"",
  ""url"": ""{url}"",
  ""profileId"": ""pfl_8XcSdLtrNK"",
  ""createdAt"": ""2024-12-06T10:09:56+00:00"",
  ""name"": ""{name}"",
  ""status"": ""enabled"",
  ""mode"": ""test"",
  ""eventTypes"": [{eventTypes}],
  ""_links"": {{
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/reference/get-webhook"",
      ""type"": ""text/html""
    }}
  }}
}}";
    }
}
