using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Request.ManageOrderLines;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrderClient : IBaseMollieClient {
        Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken = default);
        Task<OrderResponse> GetOrderAsync(
            string orderId, bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false, bool testmode = false, CancellationToken cancellationToken = default);
        Task<OrderResponse> GetOrderAsync(UrlObjectLink<OrderResponse> url, CancellationToken cancellationToken = default);
        Task<OrderResponse> UpdateOrderAsync(string orderId, OrderUpdateRequest orderUpdateRequest, CancellationToken cancellationToken = default);
        Task<OrderResponse> UpdateOrderLinesAsync(string orderId, string orderLineId, OrderLineUpdateRequest orderLineUpdateRequest, CancellationToken cancellationToken = default);
        Task<OrderResponse> ManageOrderLinesAsync(string orderId, ManageOrderLinesRequest manageOrderLinesRequest, CancellationToken cancellationToken = default);
        Task CancelOrderAsync(string orderId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<OrderResponse>> GetOrderListAsync(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false, SortDirection? sort = null, CancellationToken cancellationToken = default);
        Task<ListResponse<OrderResponse>> GetOrderListAsync(UrlObjectLink<ListResponse<OrderResponse>> url, CancellationToken cancellationToken = default);
        Task CancelOrderLinesAsync(string orderId, OrderLineCancellationRequest cancelationRequest, CancellationToken cancellationToken = default);
        Task<PaymentResponse> CreateOrderPaymentAsync(string orderId, OrderPaymentRequest createOrderPaymentRequest, CancellationToken cancellationToken = default);
    }
}
