using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class PermissionClientTests : BaseClientTests
{
    [Fact]
    public async Task GetPermissionAsync_WithPermissionId_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        const string permissionId = "payments.read";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}permissions/{permissionId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultGetPermissionResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new PermissionClient("access_abcde", httpClient);

        // Act
        var response = await permissionClient.GetPermissionAsync(permissionId);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        response.Resource.ShouldBe("permission");
        response.Id.ShouldBe("payments.read");
        response.Description.ShouldBe("View your payments");
        response.Granted.ShouldBeTrue();
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/permissions/payments.read");
        response.Links.Self.Type.ShouldBe("application/hal+json");
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/permissions-api/get-permission");
        response.Links.Documentation.Type.ShouldBe("text/html");
    }

    [Fact]
    public async Task GetPermissionAsync_WithoutPermissionId_ThrowsArgumentException()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new PermissionClient("access_abcde", httpClient);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => permissionClient.GetPermissionAsync(string.Empty));

        // Assert
        exception.Message.ShouldBe($"Required URL argument 'permissionId' is null or empty");
    }

    [Fact]
    public async Task GetPermissionListAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}permissions")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultListPermissionsResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new PermissionClient("access_abcde", httpClient);

        // Act
        var response = await permissionClient.GetPermissionListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        response.Count.ShouldBe(2);
        response.ShouldNotBeNull();
        response.Items.Count.ShouldBe(2);
        response.Items[0].Resource.ShouldBe("permission");
        response.Items[0].Id.ShouldBe("payments.write");
        response.Items[0].Description.ShouldBe("Create new payments");
        response.Items[0].Granted.ShouldBeFalse();
        response.Items[0].Links.ShouldNotBeNull();
        response.Items[0].Links.Self.Href.ShouldBe("https://api.mollie.com/v2/permissions/payments.write");
        response.Items[0].Links.Self.Type.ShouldBe("application/hal+json");
        response.Items[1].Resource.ShouldBe("permission");
        response.Items[1].Id.ShouldBe("payments.read");
        response.Items[1].Description.ShouldBe("View your payments");
        response.Items[1].Granted.ShouldBeTrue();
        response.Items[1].Links.ShouldNotBeNull();
        response.Items[1].Links.Self.Href.ShouldBe("https://api.mollie.com/v2/permissions/payments.read");
        response.Items[1].Links.Self.Type.ShouldBe("application/hal+json");
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/permissions");
        response.Links.Self.Type.ShouldBe("application/hal+json");
        response.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/permissions-api/list-permissions");
        response.Links.Documentation.Type.ShouldBe("text/html");
    }

    private const string defaultGetPermissionResponse = @"{
    ""resource"": ""permission"",
    ""id"": ""payments.read"",
    ""description"": ""View your payments"",
    ""granted"": true,
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/permissions/payments.read"",
            ""type"": ""application/hal+json""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/permissions-api/get-permission"",
            ""type"": ""text/html""
        }
    }
}";

    private const string defaultListPermissionsResponse = @"{
    ""_embedded"": {
        ""permissions"": [
            {
                ""resource"": ""permission"",
                ""id"": ""payments.write"",
                ""description"": ""Create new payments"",
                ""granted"": false,
                ""_links"": {
                    ""self"": {
                        ""href"": ""https://api.mollie.com/v2/permissions/payments.write"",
                        ""type"": ""application/hal+json""
                    }
                }
            },
            {
                ""resource"": ""permission"",
                ""id"": ""payments.read"",
                ""description"": ""View your payments"",
                ""granted"": true,
                ""_links"": {
                    ""self"": {
                        ""href"": ""https://api.mollie.com/v2/permissions/payments.read"",
                        ""type"": ""application/hal+json""
                    }
                }
            }
       ]
    },
    ""count"": 2,
    ""_links"": {
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/permissions-api/list-permissions"",
            ""type"": ""text/html""
        },
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/permissions"",
            ""type"": ""application/hal+json""
        }
    }
}";
}
