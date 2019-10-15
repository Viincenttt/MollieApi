using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Shipment;

namespace Mollie.Api.Client.Abstract {
    public interface IShipmentClient {
        Task<ShipmentResponse> CreateShipmentAsync(string orderId, ShipmentRequest shipmentRequest);
        Task<ShipmentResponse> GetShipmentAsync(string orderId, string shipmentId);
        Task<ListResponse<ShipmentResponse>> GetShipmentsListAsync(string orderId);
        Task<ShipmentResponse> UpdateOrderAsync(string orderId, string shipmentId, ShipmentUpdateRequest shipmentUpdateRequest);
    }
}