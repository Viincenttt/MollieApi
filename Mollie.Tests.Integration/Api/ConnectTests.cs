using Mollie.Api.Client;
using Mollie.Api.Models.Connect;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class ConnectTests : BaseMollieApiTestClass {
        [Test]
        public void GetAuthorizationUrl_WithSingleScope_GeneratesAuthorizationUrl() {
            // Given: We create a new connect client
            ConnectClient connectClient = new ConnectClient(this.ClientId, this.ClientSecret);

            // When: We get the authorization URL
            string authorizationUrl = connectClient.GetAuthorizationUrl("abcde", new List<string>() { AppPermissions.PaymentsRead });

            // Then: 
            string expectedUrl = $"https://www.mollie.com/oauth2/authorize?client_id={this.ClientId}&state=abcde&scope=payments.read&response_type=code&approval_prompt=auto";
            Assert.AreEqual(expectedUrl, authorizationUrl);
        }

        [Test]
        public void GetAuthorizationUrl_WithMultipleScopes_GeneratesAuthorizationUrl() {
            // Given: We create a new connect client
            ConnectClient connectClient = new ConnectClient(this.ClientId, this.ClientSecret);

            // When: We get the authorization URL
            string authorizationUrl = connectClient.GetAuthorizationUrl("abcde", new List<string>() { AppPermissions.PaymentsRead, AppPermissions.PaymentsWrite });

            // Then: 
            string expectedUrl = $"https://www.mollie.com/oauth2/authorize?client_id={this.ClientId}&state=abcde&scope=payments.read+payments.write&response_type=code&approval_prompt=auto";
            Assert.AreEqual(expectedUrl, authorizationUrl);
        }
        
        [Test]
        [Ignore("We can only test this in debug mode, because we login to the mollie dashboard and login to get the auth token")]
        public async Task GetAccessTokenAsync() {
            // Given: We fetch the authcode from the authorization URL
            string authCode = "abcde"; // Set a valid access token here
            ConnectClient connectClient = new ConnectClient(this.ClientId, this.ClientSecret);
            TokenRequest tokenRequest = new TokenRequest(authCode, "https://www.vincentkok.net");

            // When: We request the auth code
            TokenResponse tokenResponse = await connectClient.GetAccessTokenAsync(tokenRequest);

            // Then: The access token should not be null
            Assert.IsFalse(string.IsNullOrEmpty(tokenResponse.AccessToken));
        }

        private async Task<string> GetAuthorizationCode(string authorizationUrl) {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(authorizationUrl);

            response.EnsureSuccessStatusCode();
            string responseUri = response.RequestMessage.RequestUri.ToString();

            return responseUri;
        }
    }
}
