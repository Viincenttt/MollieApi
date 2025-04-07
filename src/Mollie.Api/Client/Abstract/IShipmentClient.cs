using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Shipment.Request;
using Mollie.Api.Models.Shipment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IShipmentClient : IBaseMollieClient {
        Task<ShipmentResponse> CreateShipmentAsync(string orderId, ShipmentRequest shipmentRequest, CancellationToken cancellationToken = default);
        Task<ShipmentResponse> GetShipmentAsync(string orderId, string shipmentId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ShipmentResponse> GetShipmentAsync(UrlObjectLink<ShipmentResponse> url, CancellationToken cancellationToken = default);
        Task<ListResponse<ShipmentResponse>> GetShipmentListAsync(string orderId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<ShipmentResponse>> GetShipmentListAsync(UrlObjectLink<ListResponse<ShipmentResponse>> url, CancellationToken cancellationToken = default);
        Task<ShipmentResponse> UpdateShipmentAsync(string orderId, string shipmentId, ShipmentUpdateRequest shipmentUpdateRequest, CancellationToken cancellationToken = default);
    }
}
