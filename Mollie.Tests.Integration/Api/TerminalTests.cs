using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Terminals;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class TerminalTests : BaseMollieApiTestClass {
        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveTerminalList() {
            // Given

            // When: Retrieve terminal client list
            ListResponse<TerminalResponse> response = await this._terminalClient.GetAllTerminalListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }
    }
}