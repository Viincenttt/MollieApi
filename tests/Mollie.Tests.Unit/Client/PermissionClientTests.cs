using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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
        mockHttp.Expect(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}permissions/{permissionId}")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultGetPermissionResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new PermissionsClient("access_abcde", httpClient);

        // Act
        var response = await permissionClient.GetPermissionAsync(permissionId);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        response.Resource.Should().Be("permission");
        response.Id.Should().Be("payments.read");
        response.Description.Should().Be("View your payments");
        response.Granted.Should().BeTrue();
        response.Links.Should().NotBeNull();
        response.Links.Self.Href.Should().Be("https://api.mollie.com/v2/permissions/payments.read");
        response.Links.Self.Type.Should().Be("application/hal+json");
        response.Links.Documentation.Href.Should().Be("https://docs.mollie.com/reference/v2/permissions-api/get-permission");
        response.Links.Documentation.Type.Should().Be("text/html");
    }

    [Fact]
    public async Task GetPermissionAsync_WithoutPermissionId_ThrowsArgumentException()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new PermissionsClient("access_abcde", httpClient);

        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => permissionClient.GetPermissionAsync(string.Empty));

        // Assert
        exception.Message.Should().Be($"Required URL argument 'permissionId' is null or empty");
    }

    [Fact]
    public async Task GetPermissionListAsync_ResponseIsDeserializedInExpectedFormat()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}permissions")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", defaultListPermissionsResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using var permissionClient = new PermissionsClient("access_abcde", httpClient);

        // Act
        var response = await permissionClient.GetPermissionListAsync();

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        response.Count.Should().Be(2);
        response.Should().NotBeNull();
        response.Items.Should().HaveCount(2);
        response.Items[0].Resource.Should().Be("permission");
        response.Items[0].Id.Should().Be("payments.write");
        response.Items[0].Description.Should().Be("Create new payments");
        response.Items[0].Granted.Should().BeFalse();
        response.Items[0].Links.Should().NotBeNull();
        response.Items[0].Links.Self.Href.Should()
            .Be("https://api.mollie.com/v2/permissions/payments.write");
        response.Items[0].Links.Self.Type.Should().Be("application/hal+json");
        response.Items[1].Resource.Should().Be("permission");
        response.Items[1].Id.Should().Be("payments.read");
        response.Items[1].Description.Should().Be("View your payments");
        response.Items[1].Granted.Should().BeTrue();
        response.Items[1].Links.Should().NotBeNull();
        response.Items[1].Links.Self.Href.Should()
            .Be("https://api.mollie.com/v2/permissions/payments.read");
        response.Items[1].Links.Self.Type.Should().Be("application/hal+json");
        response.Links.Should().NotBeNull();
        response.Links.Self.Href.Should().Be("https://api.mollie.com/v2/permissions");
        response.Links.Self.Type.Should().Be("application/hal+json");
        response.Links.Documentation.Href.Should()
            .Be("https://docs.mollie.com/reference/v2/permissions-api/list-permissions");
        response.Links.Documentation.Type.Should().Be("text/html");
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
