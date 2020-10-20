using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IOrderClient {
        Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
        Task<OrderResponse> GetOrderAsync(string orderId, bool embedPayments = false, bool embedRefunds = false, bool embedShipments = false);
        Task<OrderResponse> UpdateOrderAsync(string orderId, OrderUpdateRequest orderUpdateRequest);
        Task<OrderResponse> UpdateOrderLinesAsync(string orderId, string orderLineId, OrderLineUpdateRequest orderLineUpdateRequest);
        Task CancelOrderAsync(string orderId);
        Task<ListResponse<OrderResponse>> GetOrderListAsync(string from = null, int? limit = null);
        Task<ListResponse<OrderResponse>> GetOrderListAsync(UrlObjectLink<ListResponse<OrderResponse>> url);
        Task CancelOrderLinesAsync(string orderId, OrderLineCancellationRequest cancelationRequest);
        Task<PaymentResponse> CreateOrderPaymentAsync(string orderId, OrderPaymentRequest createOrderPaymentRequest);
        Task<OrderRefundResponse> CreateOrderRefundAsync(string orderId, OrderRefundRequest createOrderRefundRequest);
        Task<ListResponse<RefundResponse>> GetOrderRefundListAsync(string orderId, string from = null, int? limit = null);
    }
}