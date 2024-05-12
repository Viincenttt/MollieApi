using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Shipment.Request;
using Mollie.Api.Models.Shipment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IShipmentClient : IBaseMollieClient {
        Task<ShipmentResponse> CreateShipmentAsync(string orderId, ShipmentRequest shipmentRequest);
        Task<ShipmentResponse> GetShipmentAsync(string orderId, string shipmentId, bool testmode = false);
        Task<ShipmentResponse> GetShipmentAsync(UrlObjectLink<ShipmentResponse> url);
        Task<ListResponse<ShipmentResponse>> GetShipmentsListAsync(string orderId, bool testmode = false);
        Task<ListResponse<ShipmentResponse>> GetShipmentsListAsync(UrlObjectLink<ListResponse<ShipmentResponse>> url);
        Task<ShipmentResponse> UpdateShipmentAsync(string orderId, string shipmentId, ShipmentUpdateRequest shipmentUpdateRequest);
    }
}
