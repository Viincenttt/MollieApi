using System.Threading.Tasks;
using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.List;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class IssuerTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanRetrieveIssuerList() {
            // When: Retrieve payment list with default settings
            ListResponse<IssuerResponse> issuerList = await this._issuerClient.GetIssuerListAsync();

            // Then
            Assert.IsNotNull(issuerList);
        }
    }
}
