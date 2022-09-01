using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Shipment;

namespace Mollie.Api.Client.Abstract {
    public interface IShipmentClient {
        Task<ShipmentResponse> CreateShipmentAsync(string orderId, ShipmentRequest shipmentRequest);
        Task<ShipmentResponse> GetShipmentAsync(string orderId, string shipmentId, bool testmode = false);
        Task<ListResponse<ShipmentResponse>> GetShipmentsListAsync(string orderId, bool testmode = false);
        Task<ShipmentResponse> UpdateShipmentAsync(string orderId, string shipmentId, ShipmentUpdateRequest shipmentUpdateRequest);
    }
}