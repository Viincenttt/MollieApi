using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Payout.Response;
using Mollie.Api.Models.Webhook;
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
    public async Task GetWebhookEventAsync_With_Generic_Parameter_ResponseIsDeserializedInExpectedFormat() {
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
        var result = await webhookClient.GetWebhookEventAsync<PaymentLinkResponse>(webhookEventId);
        var response = result.Data!;

        // Then
        mockHttp.VerifyNoOutstandingRequest();
        result.Success.ShouldBeTrue();
        response.Id.ShouldBe(webhookEventId);
        response.Type.ShouldBe(type);
        response.CreatedAt.ShouldBe(new DateTimeOffset(2024, 12, 16, 15, 57, 04, TimeSpan.Zero));
        response.EntityId.ShouldBe(paymentLinkEntityId);
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/guides/webhooks");
        response.Links.Entity.Href.ShouldBe($"/v2/payment-links/{paymentLinkEntityId}");
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/events/{webhookEventId}");
        response.Entity.ShouldNotBeNull();
        response.Entity.Id.ShouldBe(paymentLinkEntityId);
        response.Entity.Resource.ShouldBe("payment-link");
        response.Entity.ProfileId.ShouldBe("pfl_D96wnsu869");
        response.Entity.Mode.ShouldBe(Mode.Live);
        response.Entity.Description.ShouldBe("Bicycle tires");
        response.Entity.Amount!.Currency.ShouldBe("EUR");
        response.Entity.Amount!.Value.ShouldBe(24.95m);
        response.Entity.MinimumAmount.ShouldBeNull();
        response.Entity.Archived.ShouldBeFalse();
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
        var result = await webhookClient.GetWebhookEventAsync(webhookEventId);
        var response = result.Data!;

        // Then
        mockHttp.VerifyNoOutstandingRequest();
        result.Success.ShouldBeTrue();
        response.Id.ShouldBe(webhookEventId);
        response.Type.ShouldBe(type);
        response.CreatedAt.ShouldBe(new DateTimeOffset(2024, 12, 16, 15, 57, 04, TimeSpan.Zero));
        response.EntityId.ShouldBe(paymentLinkEntityId);
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/guides/webhooks");
        response.Links.Entity.Href.ShouldBe($"/v2/payment-links/{paymentLinkEntityId}");
        response.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/events/{webhookEventId}");
    }

    [Fact]
    public async Task GetWebhookEventAsync_WithPayoutEntity_EntityIsDeserializedAsPayout() {
        // Given
        const string webhookEventId = "webhook-event-id";
        const string payoutId = "payout_j8NvRAM2WNZtsykpLEX8J";
        string entityJson = CreatePayoutJsonResponse(payoutId);
        string jsonToReturnInMockResponse = CreateWebhookEventJsonResponse(webhookEventId, WebhookEventTypes.PayoutCompleted, payoutId, entityJson);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}events/{webhookEventId}",
            jsonToReturnInMockResponse);
        var webhookClient = new WebhookEventClient("abcde", mockHttp.ToHttpClient());

        // When
        var result = await webhookClient.GetWebhookEventAsync(webhookEventId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
        result.Data.Type.ShouldBe(WebhookEventTypes.PayoutCompleted);
        var payout = result.Data.Entity.ShouldBeOfType<PayoutResponse>();
        payout.Id.ShouldBe(payoutId);
        payout.Status.ShouldBe(PayoutStatus.Completed);
    }

    [Fact]
    public async Task GetWebhookEventAsync_WithBalanceTransactionEntity_EntityIsDeserializedAsBalanceTransaction() {
        // Given
        const string webhookEventId = "webhook-event-id";
        const string balanceTransactionId = "baltr_QM24QwzUWR4ev4Xfgyt29d";
        string entityJson = CreatePaymentBalanceTransactionJsonResponse(balanceTransactionId);
        string jsonToReturnInMockResponse = CreateWebhookEventJsonResponse(webhookEventId, WebhookEventTypes.BalanceTransactionCreated, balanceTransactionId, entityJson);
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}events/{webhookEventId}",
            jsonToReturnInMockResponse);
        var webhookClient = new WebhookEventClient("abcde", mockHttp.ToHttpClient());

        // When
        var result = await webhookClient.GetWebhookEventAsync(webhookEventId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Success.ShouldBeTrue();
        result.Data.Type.ShouldBe(WebhookEventTypes.BalanceTransactionCreated);
        var balanceTransaction = result.Data.Entity.ShouldBeOfType<PaymentBalanceTransactionResponse>();
        balanceTransaction.Id.ShouldBe(balanceTransactionId);
        balanceTransaction.Context.PaymentId.ShouldBe("tr_7UhSN1zuXS");
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

    private string CreatePayoutJsonResponse(string payoutId) {
        return $@"{{
      ""resource"": ""payout"",
      ""id"": ""{payoutId}"",
      ""balanceId"": ""bal_gVMhHKqSSRYJyPsuoPNFH"",
      ""amount"": {{
        ""currency"": ""EUR"",
        ""value"": ""100.00""
      }},
      ""status"": ""completed"",
      ""statusReason"": {{
        ""code"": ""completed"",
        ""message"": ""The payout has been completed.""
      }},
      ""createdAt"": ""2024-03-20T09:13:37.0Z"",
      ""mode"": ""live"",
      ""_links"": {{
        ""self"": {{
          ""href"": ""https://api.mollie.com/v2/payouts/{payoutId}"",
          ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
          ""href"": ""https://docs.mollie.com/reference/get-payout"",
          ""type"": ""text/html""
        }}
      }}
    }}";
    }

    private string CreatePaymentBalanceTransactionJsonResponse(string balanceTransactionId) {
        return $@"{{
      ""resource"": ""balance_transaction"",
      ""id"": ""{balanceTransactionId}"",
      ""type"": ""payment"",
      ""resultAmount"": {{
        ""currency"": ""EUR"",
        ""value"": ""9.71""
      }},
      ""initialAmount"": {{
        ""currency"": ""EUR"",
        ""value"": ""10.00""
      }},
      ""deductions"": {{
        ""currency"": ""EUR"",
        ""value"": ""-0.29""
      }},
      ""createdAt"": ""2024-03-20T09:13:37.0Z"",
      ""context"": {{
        ""paymentId"": ""tr_7UhSN1zuXS""
      }}
    }}";
    }
}
