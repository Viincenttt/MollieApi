using Mollie.Api.Models.Issuer;
using Mollie.Api.Models.List;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class IssuerTests : BaseMollieApiTestClass {
        [Test]
        public void CanRetrieveIssuerList() {
            // When: Retrieve payment list with default settings
            ListResponse<IssuerResponse> issuerList = this._mollieClient.GetIssuerList().Result;

            // Then
            Assert.IsNotNull(issuerList);
        }
    }
}
