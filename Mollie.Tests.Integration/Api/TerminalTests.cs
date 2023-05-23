using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Terminal;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class TerminalTests : BaseMollieApiTestClass {
        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public async Task CanRetrieveTerminalList() {
            // Given

            // When: Retrieve terminal client list
            ListResponse<TerminalResponse> response = await this._terminalClient.GetTerminalListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        [Ignore("Does not work yet")]
        public async Task CanRetrieveSingleTerminal() {
            // Given
            ListResponse<TerminalResponse> allTerminals = await this._terminalClient.GetTerminalListAsync();
            if (allTerminals.Count == 0) {
                Assert.Inconclusive("No terminals on this account to retrieve");
            }
            TerminalResponse firstTerminal = allTerminals.Items.First();

            // When: Retrieve terminal client list
            TerminalResponse response = await this._terminalClient.GetTerminalAsync(firstTerminal.Id);

            // Then
            Assert.IsNotNull(response);
            Assert.AreEqual(firstTerminal.Id, response.Id);
        }
    }
}