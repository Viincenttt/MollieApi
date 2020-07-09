using Mollie.Api.Models.Profile.Response;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class ProfileTests : BaseMollieApiTestClass {
        [Test]
        public async Task GetCurrentProfileAsync_ReturnsCurrentProfile() {
            // Given

            // When: We retrieve the current profile from the mollie API
            ProfileResponse profileResponse = await this._profileClient.GetCurrentProfileAsync();

            // Then: Make sure we get a valid response
            Assert.IsNotNull(profileResponse);
            Assert.IsNotNull(profileResponse.Id);
            Assert.IsNotNull(profileResponse.Email);
            Assert.IsNotNull(profileResponse.Status);
        }
    }
}
