using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Terminal;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration.Api; 

public class TerminalTests : BaseMollieApiTestClass {
    private readonly ITerminalClient _terminalClient;

    public TerminalTests() {
        _terminalClient = new TerminalClient(this.ApiKey);
    }
    
    [DefaultRetryFact]
    public async Task CanRetrieveTerminalList() {
        // Given

        // When: Retrieve terminal client list
        ListResponse<TerminalResponse> response = await this._terminalClient.GetTerminalListAsync();

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryFact(Skip = "Not implemented by Mollie yet")]
    public async Task CanRetrieveSingleTerminal() {
        // Given
        ListResponse<TerminalResponse> allTerminals = await this._terminalClient.GetTerminalListAsync();
        if (allTerminals.Count > 0) {
            TerminalResponse firstTerminal = allTerminals.Items.First();

            // When: Retrieve terminal client list
            TerminalResponse response = await this._terminalClient.GetTerminalAsync(firstTerminal.Id);

            // Then
            response.Should().NotBeNull();
            response.Id.Should().Be(firstTerminal.Id);
        }
    }
}