using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration.Api;

public class TerminalTests : BaseMollieApiTestClass, IDisposable {
    private readonly ITerminalClient _terminalClient;

    public TerminalTests() {
        _terminalClient = new TerminalClient(ApiKey);
    }

    [DefaultRetryFact]
    public async Task CanRetrieveTerminalList() {
        // Given

        // When: Retrieve terminal client list
        ListResponse<TerminalResponse> response = await _terminalClient.GetTerminalListAsync();

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [DefaultRetryFact(Skip = "Not implemented by Mollie yet")]
    public async Task CanRetrieveSingleTerminal() {
        // Given
        ListResponse<TerminalResponse> allTerminals = await _terminalClient.GetTerminalListAsync();
        if (allTerminals.Count > 0) {
            TerminalResponse firstTerminal = allTerminals.Items.First();

            // When: Retrieve terminal client list
            TerminalResponse response = await _terminalClient.GetTerminalAsync(firstTerminal.Id);

            // Then
            response.ShouldNotBeNull();
            response.Id.ShouldBe(firstTerminal.Id);
        }
    }

    public void Dispose()
    {
        _terminalClient?.Dispose();
    }
}
