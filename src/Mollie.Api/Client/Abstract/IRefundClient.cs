using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Refund.Request;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IRefundClient : IBaseMollieClient {
        Task<RefundResponse> CreatePaymentRefundAsync(string paymentId, RefundRequest refundRequest);
        Task<RefundResponse> GetPaymentRefundAsync(string paymentId, string refundId, bool testmode = false);
        Task CancelPaymentRefundAsync(string paymentId, string refundId, bool testmode = false);
        Task<ListResponse<RefundResponse>> GetPaymentRefundListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false);
        Task<OrderRefundResponse> CreateOrderRefundAsync(string orderId, OrderRefundRequest createOrderRefundRequest);
        Task<ListResponse<RefundResponse>> GetOrderRefundListAsync(string orderId, string? from = null, int? limit = null, bool testmode = false);
        Task<ListResponse<RefundResponse>> GetRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url);
        Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url);
    }
}
