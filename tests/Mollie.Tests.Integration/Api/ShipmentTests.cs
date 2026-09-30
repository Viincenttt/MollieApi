using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Shipment.Request;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class ShipmentTests : BaseMollieApiTestClass, IDisposable {
    private readonly IShipmentClient _shipmentClient;

    public ShipmentTests(IShipmentClient shipmentClient) {
        _shipmentClient = shipmentClient;
    }

    [Fact(Skip = "For manual testing only")]
    public async Task CanCreateShipmentWithOnlyRequiredFields() {
        // the order needs to be autorized to do a shipment on. this can only be done by waiting.
        string validOrderId = "XXXXX";
        ShipmentRequest shipmentRequest = CreateShipmentWithOnlyRequiredFields();
        var result = await _shipmentClient.CreateShipmentAsync(validOrderId, shipmentRequest);
        var shipment = result.Data!;

        // Then: Make sure we get a valid shipment response
        result.Success.ShouldBeTrue();
        shipment.ShouldNotBeNull();
        shipment.CreatedAt.ShouldBeGreaterThan(DateTime.Now);
    }

    [Fact(Skip = "For manual testing only")]
    public async Task CanListShipmentsForOrder(){
        string validOrderId = "XXXXX";
        var result = await _shipmentClient.GetShipmentListAsync(validOrderId);
        var shipments = result.Data!;

        result.Success.ShouldBeTrue();
        shipments.ShouldNotBeNull();
        shipments.Count.ShouldBeGreaterThan(0);
    }

    private ShipmentRequest CreateShipmentWithOnlyRequiredFields() {
        return new ShipmentRequest() {
            Lines = new List<ShipmentLineRequest>()
        };
    }

    public void Dispose()
    {
        _shipmentClient?.Dispose();
    }
}
