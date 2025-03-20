using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class ClientClientTests : BaseClientTests {
    [Fact]
    public async Task GetClientAsync_WithoutClientId_ThrowsArgumentException() {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new ClientClient("access_abcde", httpClient);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => permissionClient.GetClientAsync(string.Empty));

        // Assert
        exception.Message.ShouldBe($"Required URL argument 'clientId' is null or empty");
    }

    [Fact]
    public async Task GetClientAsync_WithClientId_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string clientId = "org_12345678";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.ApiEndPoint}clients/{clientId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", DefaultGetClientResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var clientClient = new ClientClient("access_1234", httpClient);

        // Act
        var result = await clientClient.GetClientAsync(clientId);

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        result.Resource.ShouldBe("client");
        result.Id.ShouldBe(clientId);
        result.Commission.ShouldNotBeNull().Count.ShouldBe(0);
        result.OrganizationCreatedAt.ShouldBe(new DateTime(2023, 4, 6, 13, 10, 19, DateTimeKind.Utc));
        result.Links.ShouldNotBeNull();
        result.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/clients/org_12345678");
        result.Links.Self.Type.ShouldBe("application/hal+json");
        result.Links.Organization.Href.ShouldBe("https://api.mollie.com/v2/organizations/org_12345678");
        result.Links.Organization.Type.ShouldBe("application/hal+json");
        result.Links.Onboarding.Href.ShouldBe("https://api.mollie.com/v2/onboarding/org_12345678");
        result.Links.Onboarding.Type.ShouldBe("application/hal+json");
        result.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/clients-api");
        result.Links.Documentation.Type.ShouldBe("text/html");
    }

    [Theory]
    [InlineData(false, false, "")]
    [InlineData(true, true, "?embed=organization,onboarding")]
    [InlineData(true, false, "?embed=organization")]
    [InlineData(false, true, "?embed=onboarding")]
    public async Task GetClientAsync_WithEmbeddedParameters_GeneratesExpectedUrl(bool embedOrganization, bool embedOnboarding, string expectedQueryString)
    {
        // Arrange
        const string clientId = "org_12345678";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.ApiEndPoint}clients/{clientId}{expectedQueryString}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", DefaultGetClientResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var clientClient = new ClientClient("access_1234", httpClient);

        // Act
        await clientClient.GetClientAsync(clientId, embedOrganization, embedOnboarding);

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
    }

    [Theory]
    [InlineData(null, null, false, false, "")]
    [InlineData("from", null, false, false, "?from=from")]
    [InlineData("from", 50, false, false, "?from=from&limit=50")]
    [InlineData("from", 50, true, false, "?from=from&limit=50&embed=organization")]
    [InlineData("from", 50, true, true, "?from=from&limit=50&embed=organization,onboarding")]
    public async Task GetClientListAsync_WithQueryParameters_QueryStringOnlyContainsTestModeParameterIfTrue(
        string from, int? limit, bool embedOrganization, bool embedOnboarding, string expectedQueryString) {
        // Given: We retrieve a list of clients
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}clients{expectedQueryString}")
            .Respond("application/json", DefaultGetClientListResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var clientClient = new ClientClient("access_1234", httpClient);

        // When: We send the request
        var result = await clientClient.GetClientListAsync(from, limit, embedOrganization, embedOnboarding);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetClienListAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get,$"{BaseMollieClient.ApiEndPoint}clients")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", DefaultGetClientListResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var clientClient = new ClientClient("access_1234", httpClient);

        // Act
        var result = await clientClient.GetClientListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingRequest();
        result.Count.ShouldBe(2);
        result.Items.ShouldNotBeNull();
        result.Links.ShouldNotBeNull();
        result.Links.ShouldNotBeNull();
        result.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/clients");
        result.Links.Self.Type.ShouldBe("application/hal+json");
        result.Links.Previous.ShouldBeNull();
        result.Links.Next!.Href.ShouldBe("https://api.mollie.com/v2/clients?from=org_63916732&limit=5");
        result.Links.Next.Type.ShouldBe("application/hal+json");
        result.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/clients-api");
        result.Links.Documentation.Type.ShouldBe("text/html");
    }

    private const string DefaultGetClientResponse = @"{
        ""resource"": ""client"",
        ""id"": ""org_12345678"",
        ""commission"": {
          ""count"": 0
        },
        ""organizationCreatedAt"": ""2023-04-06 13:10:19"",
        ""_links"": {
            ""self"": {
              ""href"": ""https://api.mollie.com/v2/clients/org_12345678"",
              ""type"": ""application/hal+json""
            },
            ""organization"": {
              ""href"": ""https://api.mollie.com/v2/organizations/org_12345678"",
              ""type"": ""application/hal+json""
            },
            ""onboarding"": {
              ""href"": ""https://api.mollie.com/v2/onboarding/org_12345678"",
              ""type"": ""application/hal+json""
            },
            ""documentation"": {
              ""href"": ""https://docs.mollie.com/reference/clients-api"",
              ""type"": ""text/html""
            }
        }
      }";

    private const string DefaultGetClientListResponse = $@"{{
        ""count"": 2,
        ""_embedded"": {{
            ""clients"": [
                {DefaultGetClientResponse},
                {DefaultGetClientResponse},
            ]
        }},
        ""_links"": {{
            ""self"": {{
              ""href"": ""https://api.mollie.com/v2/clients"",
              ""type"": ""application/hal+json""
            }},
            ""previous"": null,
            ""next"": {{
              ""href"": ""https://api.mollie.com/v2/clients?from=org_63916732&limit=5"",
              ""type"": ""application/hal+json""
            }},
            ""documentation"": {{
              ""href"": ""https://docs.mollie.com/reference/clients-api"",
              ""type"": ""text/html""
            }}
          }}
    }}";
}
