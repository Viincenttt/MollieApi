using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Models.Chargeback;
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
        mockHttp.When( HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}client-links")
            .Respond("application/json", clientLinkResponseJson);
        HttpClient httpClient = mockHttp.ToHttpClient();
        ClientLinkClient clientLinkClient = new ClientLinkClient("access_1234", httpClient); 

        // When: We send the request
        ClientLinkResponse response = await clientLinkClient.CreateClientLinkAsync(new ClientLinkRequest());

        // Then
        response.Id.Should().Be(clientLinkId);
        response.Links.ClientLink.Href.Should().Be(clientLinkUrl);
        mockHttp.VerifyNoOutstandingRequest();
    }

    public string CreateClientLinkResponseJson(string id, string clientLinkUrl)
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