using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models.Connect.Request;
using Mollie.Api.Models.Connect.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class ConnectClientTests : BaseClientTests
{
    private const string ClientId = "client-id";
    private const string ClientSecret = "client-secret";

    [Fact]
    public void GetAuthorizationUrl_WithSingleScope_GeneratesAuthorizationUrl()
    {
        // Arrange
        HttpClient httpClient = new HttpClient();
        ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret, httpClient);
        var scopes = new List<string> {AppPermissions.PaymentsRead};

        // Act
        string authorizationUrl = connectClient.GetAuthorizationUrl("abcde", scopes);

        // Assert
        string expectedUrl = $"https://my.mollie.com/oauth2/authorize?client_id={ClientId}&state=abcde&scope=payments.read&response_type=code&approval_prompt=auto";
        authorizationUrl.ShouldBe(expectedUrl);
    }

    [Fact]
    public void GetAuthorizationUrl_WithUserSuppliedValue_ReturnsSuppliedValue()
    {
        // Arrange
        var userSuppliedUrl = "https://test.com/oauth2/authorize";
        HttpClient httpClient = new HttpClient();
        ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret, httpClient, authorizeEndPoint: userSuppliedUrl);
        var scopes = new List<string> { AppPermissions.PaymentsRead };

        // Act
        string authorizationUrl = connectClient.GetAuthorizationUrl("abcde", scopes);

        // Assert
        string expectedUrl = $"{userSuppliedUrl}?client_id={ClientId}&state=abcde&scope=payments.read&response_type=code&approval_prompt=auto";
        authorizationUrl.ShouldBe(expectedUrl);
    }

    [Fact]
    public async Task GetAccessTokenAsync_WithUserSuppliedEndpoint_CallsTheCorrectEndpoint()
    {
        // Arrange
        var userSuppliedUrl = "https://test.com/oauth2/";

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"{userSuppliedUrl}tokens")
            .Respond("application/json", defaultGetTokenResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret, httpClient, tokenEndPoint: userSuppliedUrl);
        var tokenRequest = new TokenRequest("refresh_abcde", DefaultRedirectUrl);

        // Act
        await connectClient.GetAccessTokenAsync(tokenRequest);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData("refresh_abcde", "refresh_token")]
    [InlineData("abcde", "authorization_code")]
    public async Task GetAccessTokenAsync_WithRefreshToken_ResponseIsDeserializedInExpectedFormat(string refreshToken, string expectedGrantType)
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, "https://api.mollie.com/oauth2/tokens")
            .Respond("application/json", defaultGetTokenResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret, httpClient);
        var tokenRequest = new TokenRequest(refreshToken, DefaultRedirectUrl);

        // Act
        TokenResponse tokenResponse = await connectClient.GetAccessTokenAsync(tokenRequest);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
        tokenRequest.GrantType.ShouldBe(expectedGrantType);
        tokenResponse.ShouldNotBeNull();
        tokenResponse.AccessToken.ShouldBe("access_46EUJ6x8jFJZZeAvhNH4JVey6qVpqR");
        tokenResponse.RefreshToken.ShouldBe("refresh_FS4xc3Mgci2xQ5s5DzaLXh3HhaTZOP");
        tokenResponse.ExpiresIn.ShouldBe(3600);
        tokenResponse.TokenType.ShouldBe("bearer");
        tokenResponse.Scope.ShouldBe("payments.read organizations.read");
    }

    [Fact]
    public async Task RevokeTokenAsync_SendsRequest()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Delete, "https://api.mollie.com/oauth2/tokens")
            .Respond(HttpStatusCode.NoContent);
        HttpClient httpClient = mockHttp.ToHttpClient();
        ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret, httpClient);
        var revokeTokenRequest = new RevokeTokenRequest
        {
            Token = "access_46EUJ6x8jFJZZeAvhNH4JVey6qVpqR",
            TokenTypeHint = "refresh_token"
        };

        // Act
        await connectClient.RevokeTokenAsync(revokeTokenRequest);

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    private const string defaultGetTokenResponse = @"
{
    ""access_token"": ""access_46EUJ6x8jFJZZeAvhNH4JVey6qVpqR"",
    ""refresh_token"": ""refresh_FS4xc3Mgci2xQ5s5DzaLXh3HhaTZOP"",
    ""expires_in"": 3600,
    ""token_type"": ""bearer"",
    ""scope"": ""payments.read organizations.read""
}";
}
