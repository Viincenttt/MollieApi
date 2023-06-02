using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Shipment;
using Mollie.Tests.Integration.Framework;

/*
namespace Mollie.Tests.Integration.Api
{
    public class ShipmentTests : BaseMollieApiTestClass {
        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        [Ignore("For manual testing only")]
        public async Task CanCreateShipmentWithOnlyRequiredFields() {
            // the order needs to be autorized to do a shipment on. this can only be done by waiting.
            string validOrderId = "XXXXX";
            ShipmentRequest shipmentRequest = this.CreateShipmentWithOnlyRequiredFields();
            ShipmentResponse result = await this._shipmentClient.CreateShipmentAsync(validOrderId, shipmentRequest);
            
            // Then: Make sure we get a valid shipment response
            Assert.IsNotNull(result);
            Assert.IsTrue(result.CreatedAt >= DateTime.Now);
        }

        [Test][RetryOnApiRateLimitFailure(BaseMollieApiTestClass.NumberOfRetries)]
        [Ignore("For manual testing only")]
        public async Task CanListShipmentsForOrder(){
            string validOrderId = "XXXXX";
            ListResponse<ShipmentResponse> result = await this._shipmentClient.GetShipmentsListAsync(validOrderId);
            
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        
        private ShipmentRequest CreateShipmentWithOnlyRequiredFields() {
            return new ShipmentRequest() {
                Lines = new List<ShipmentLineRequest>()
            };
        }
    }
}
*/