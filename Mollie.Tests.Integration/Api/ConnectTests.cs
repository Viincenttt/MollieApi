using Mollie.Api.Client;
using Mollie.Api.Models.Connect;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;
using System.Collections.Generic;
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
            const string expectedUrl = "https://www.mollie.com/oauth2/authorize?client_id=app_VHhvrHWsfuzJxdzHxdk9wfPJ&state=abcde&scope=payments.read&response_type=code&approval_prompt=auto";
            Assert.AreEqual(expectedUrl, authorizationUrl);
        }

        [Test]
        public void GetAuthorizationUrl_WithMultipleScopes_GeneratesAuthorizationUrl() {
            // Given: We create a new connect client
            ConnectClient connectClient = new ConnectClient(this.ClientId, this.ClientSecret);

            // When: We get the authorization URL
            string authorizationUrl = connectClient.GetAuthorizationUrl("abcde", new List<string>() { AppPermissions.PaymentsRead, AppPermissions.PaymentsWrite });

            // Then: 
            const string expectedUrl = "https://www.mollie.com/oauth2/authorize?client_id=app_VHhvrHWsfuzJxdzHxdk9wfPJ&state=abcde&scope=payments.read+payments.write&response_type=code&approval_prompt=auto";
            Assert.AreEqual(expectedUrl, authorizationUrl);
        }
    }
}
