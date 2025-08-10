using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class WebhookEventClientTests : BaseClientTests {
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetWebhookEventAsync_NoWebhookIdIsGiven_ArgumentExceptionIsThrown(string? webhookEventId) {
        // Given
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var client = new WebhookEventClient("api-key", httpClient);

        // When
#pragma warning disable CS8604 // Possible null reference argument.
        var exception =
            await Assert.ThrowsAsync<ArgumentException>(async () => await client.GetWebhookEventAsync(webhookEventId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'webhookEventId' is null or empty");
    }


    [Theory]
    [InlineData(true, "?testmode=true")]
    [InlineData(false, "")]
    public async Task GetWebhookEventAsync_QueryParameterOptions_CorrectParametersAreAdded(bool testMode, string expectedQueryString) {
        // Given
        const string webhookEventId = "webhook-event-id";
        const string type = "payment-link.paid";
        const string paymentLinkEntityId = "pl_qng5gbbv8NAZ5gpM5ZYgx";
        string entityJson = CreatePaymentLinkJsonResponse(paymentLinkEntityId);
        string jsonToReturnInMockResponse = CreateWebhookEventJsonResponse(webhookEventId, type, paymentLinkEntityId, entityJson);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}events/{webhookEventId}{expectedQueryString}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var webhookClient = new WebhookEventClient("abcde", httpClient);

        // When
        await webhookClient.GetWebhookEventAsync(webhookEventId, testMode);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Fact]
    public async Task GetWebhookEventAsync_ResponseIsDeserializedInExpectedFormat() {
        // Given
        const string webhookEventId = "webhook-event-id";
        const string type = "payment-link.paid";
        const string paymentLinkEntityId = "pl_qng5gbbv8NAZ5gpM5ZYgx";
        string entityJson = CreatePaymentLinkJsonResponse(paymentLinkEntityId);
        string jsonToReturnInMockResponse = CreateWebhookEventJsonResponse(webhookEventId, type, paymentLinkEntityId, entityJson);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}events/{webhookEventId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var webhookClient = new WebhookEventClient("abcde", httpClient);

        // When
        var response = await webhookClient.GetWebhookEventAsync(webhookEventId);

        // Then
        mockHttp.VerifyNoOutstandingRequest();
        response.Id.ShouldBe(webhookEventId);
        response.Type.ShouldBe(type);
        response.CreatedAt.ShouldBe(new DateTime(2024, 12, 16, 15, 57, 04, DateTimeKind.Utc));
        response.EntityId.ShouldBe(paymentLinkEntityId);
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/guides/webhooks");
        response.Links.Entity.Href.ShouldBe($"/v2/payment-links/{paymentLinkEntityId}");
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/events/{webhookEventId}");
    }

    private string CreateWebhookEventJsonResponse(string webhookEventId, string type, string entityId, string entityJson) {
        return $@"{{
  ""resource"": ""event"",
  ""id"": ""{webhookEventId}"",
  ""type"": ""{type}"",
  ""entityId"": ""{entityId}"",
  ""createdAt"": ""2024-12-16T15:57:04.0Z"",
  ""_embedded"": {{
    ""entity"": {entityJson}
  }},
  ""_links"": {{
    ""self"": {{
      ""href"": ""https://api.mollie.com/v2/events/{webhookEventId}"",
      ""type"": ""application/hal+json""
    }},
    ""documentation"": {{
      ""href"": ""https://docs.mollie.com/guides/webhooks"",
      ""type"": ""text/html""
    }},
    ""entity"": {{
      ""href"": ""/v2/payment-links/{entityId}"",
      ""type"": ""application/hal+json""
    }}
  }}
}}";
    }

    private string CreatePaymentLinkJsonResponse(string entityId) {
        return $@"{{
      ""resource"": ""payment-link"",
      ""id"": ""{entityId}"",
      ""profileId"": ""pfl_D96wnsu869"",
      ""mode"": ""live"",
      ""description"": ""Bicycle tires"",
      ""amount"": {{
        ""currency"": ""EUR"",
        ""value"": ""24.95""
      }},
      ""minimumAmount"": null,
      ""archived"": false,
      ""redirectUrl"": ""https://webshop.example.org/thanks"",
      ""webhookUrl"": null,
      ""reusable"": true,
      ""createdAt"": ""2021-03-20T09:29:56.0Z"",
      ""paidAt"": null,
      ""expiresAt"": ""2023-06-06T11:00:00.0Z"",
      ""allowedMethods"": null,
      ""applicationFee"": null,
      ""_links"": {{
        ""self"": {{
          ""href"": ""https://api.mollie.com/v2/payment-links/{entityId}"",
          ""type"": ""application/hal+json""
        }},
        ""paymentLink"": {{
          ""href"": ""https://www.mollie.com/paymentscreen/example"",
          ""type"": ""text/html""
        }},
        ""documentation"": {{
          ""href"": ""https://docs.mollie.com/reference/v2/payment-links-api/get-payment-link"",
          ""type"": ""text/html""
        }}
      }}
    }}";
    }
}
