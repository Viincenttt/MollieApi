﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.ClientLink.Request;
using Mollie.Api.Models.ClientLink.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class ClientLinkClientTests : BaseClientTests
{
    [Fact]
    public async Task CreateClientLinkAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Given: We create a payment link
        const string clientLinkId = "csr_vZCnNQsV2UtfXxYifWKWH";
        const string clientLinkUrl = "myurl";
        string clientLinkResponseJson = CreateClientLinkResponseJson(clientLinkId, clientLinkUrl);
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When( HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}client-links")
            .Respond("application/json", clientLinkResponseJson);
        HttpClient httpClient = mockHttp.ToHttpClient();
        ClientLinkClient clientLinkClient = new ClientLinkClient("clientId", "access_1234", httpClient);
        var request = new ClientLinkRequest {
            Owner = new ClientLinkOwner {
                Email = "email",
                GivenName = "gicen-name",
                FamilyName = "family-name"
            },
            Address = new AddressObject(),
            Name = "name"
        };

        // When: We send the request
        ClientLinkResponse response = await clientLinkClient.CreateClientLinkAsync(request);

        // Then
        response.Id.ShouldBe(clientLinkId);
        response.Links.ClientLink.Href.ShouldBe(clientLinkUrl);
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GenerateClientLinkWithParameters_GeneratesExpectedUrl(bool forceApprovalPrompt)
    {
        // Arrange
        const string clientId = "app_j9Pakf56Ajta6Y65AkdTtAv";
        const string clientLinkUrl = "https://my.mollie.com/dashboard/client-link/csr_vZCnNQsV2UtfXxYifWKWH";
        const string state = "decafbad";
        var scopes = new List<string>()
        {
            "onboarding.read",
            "organizations.read",
            "payments.write",
            "payments.read",
            "profiles.write",
        };
        ClientLinkClient clientLinkClient = new ClientLinkClient(
            clientId, "access_1234", new MockHttpMessageHandler().ToHttpClient());

        // Act
        string result = clientLinkClient.GenerateClientLinkWithParameters(
            clientLinkUrl, state, scopes, forceApprovalPrompt);

        // Assert
        string expectedApprovalPrompt = forceApprovalPrompt ? "force" : "auto";
        result.ShouldBe("https://my.mollie.com/dashboard/client-link/csr_vZCnNQsV2UtfXxYifWKWH" +
                        $"?client_id={clientId}" +
                        $"&state={state}" +
                        "&scope=onboarding.read+organizations.read+payments.write+payments.read+profiles.write" +
                        $"&approval_prompt={expectedApprovalPrompt}");
    }

    private string CreateClientLinkResponseJson(string id, string clientLinkUrl)
    {
        return $@"{{
    ""id"": ""{id}"",
    ""resource"": ""client-link"",
    ""_links"": {{
        ""clientLink"": {{
            ""href"": ""{clientLinkUrl}"",
            ""type"": ""text/html""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/clients-api/create-client-link"",
            ""type"": ""text/html""
        }}
    }}
}}";
    }
}
