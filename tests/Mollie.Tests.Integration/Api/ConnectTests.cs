using Mollie.Api.Client;
using Mollie.Tests.Integration.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Shouldly;
using Mollie.Api.Models.Connect.Request;
using Mollie.Api.Models.Connect.Response;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class ConnectTests : BaseMollieApiTestClass {
    private readonly IConnectClient _connectClient;

    public ConnectTests(IConnectClient connectClient) {
        _connectClient = connectClient;
    }

    [Fact]
    public void GetAuthorizationUrl_WithSingleScope_GeneratesAuthorizationUrl() {
        // When: We get the authorization URL
        string authorizationUrl = _connectClient.GetAuthorizationUrl("abcde", new List<string>() { AppPermissions.PaymentsRead });

        // Then:
        string expectedUrl = $"https://my.mollie.com/oauth2/authorize?client_id={ClientId}&state=abcde&scope=payments.read&response_type=code&approval_prompt=auto";
        authorizationUrl.ShouldBe(expectedUrl);
    }

    [Fact]
    public void GetAuthorizationUrl_WithMultipleScopes_GeneratesAuthorizationUrl() {
        // When: We get the authorization URL
        string authorizationUrl = _connectClient.GetAuthorizationUrl("abcdef", new List<string>() {
            AppPermissions.PaymentsRead,
            AppPermissions.PaymentsWrite,
            AppPermissions.ProfilesRead,
            AppPermissions.ProfilesWrite
        });

        // Then:
        string expectedUrl = $"https://my.mollie.com/oauth2/authorize?client_id={ClientId}" +
                             $"&state=abcdef&scope=payments.read+payments.write+profiles.read+profiles.write&response_type=code&approval_prompt=auto";
        authorizationUrl.ShouldBe(expectedUrl);
    }

    [Fact(Skip = "We can only test this in debug mode, because we login to the mollie dashboard and login to get the auth token")]
    public async Task GetAccessTokenAsync_WithValidTokenRequest_ReturnsAccessToken() {
        // Given: We fetch create a token request
        string authCode = "abcde"; // Set a valid access token here
        using ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret);
        TokenRequest tokenRequest = new TokenRequest(authCode, DefaultRedirectUrl);

        // When: We request the auth code
        TokenResponse tokenResponse = await connectClient.GetAccessTokenAsync(tokenRequest);

        // Then: The access token should not be null
        tokenResponse.AccessToken.ShouldNotBeNullOrEmpty();
    }

    [Fact(Skip = "We can only test this in debug mode, because we need a valid access token")]
    public async Task RevokeAccessTokenAsync_WithValidToken_DoesNotThrowError() {
        // Given: We create a revoke token request
        string accessToken = "abcde";
        using ConnectClient connectClient = new ConnectClient(ClientId, ClientSecret);
        RevokeTokenRequest revokeTokenRequest = new RevokeTokenRequest() {
            TokenTypeHint = TokenType.AccessToken,
            Token = accessToken
        };

        // When: we send the request
        await connectClient.RevokeTokenAsync(revokeTokenRequest);
    }
}
