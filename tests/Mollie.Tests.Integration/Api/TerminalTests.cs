using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class TerminalTests : BaseMollieApiTestClass, IDisposable {
    private readonly ITerminalClient _terminalClient;

    public TerminalTests(ITerminalClient terminalClient) {
        _terminalClient = terminalClient;
    }

    [Fact]
    public async Task CanRetrieveTerminalList() {
        // Given

        // When: Retrieve terminal client list
        var result = await _terminalClient.GetTerminalListAsync();
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact(Skip = "Not implemented by Mollie yet")]
    public async Task CanRetrieveSingleTerminal() {
        // Given
        var listResult = await _terminalClient.GetTerminalListAsync();
        var allTerminals = listResult.Data!;
        if (allTerminals.Count > 0) {
            TerminalResponse firstTerminal = allTerminals.Items.First();

            // When: Retrieve terminal client list
            var result = await _terminalClient.GetTerminalAsync(firstTerminal.Id);
            var response = result.Data!;

            // Then
            listResult.Success.ShouldBeTrue();
            result.Success.ShouldBeTrue();
            response.ShouldNotBeNull();
            response.Id.ShouldBe(firstTerminal.Id);
        }
    }

    public void Dispose()
    {
        _terminalClient?.Dispose();
    }
}
