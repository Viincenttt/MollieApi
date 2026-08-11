using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Refund.Request;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IRefundClient : IBaseMollieClient {
        Task<MollieResult<RefundResponse>> CreatePaymentRefundAsync(string paymentId, RefundRequest refundRequest, CancellationToken cancellationToken = default);
        Task<MollieResult<RefundResponse>> GetPaymentRefundAsync(string paymentId, string refundId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult> CancelPaymentRefundAsync(string paymentId, string refundId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<RefundResponse>>> GetPaymentRefundListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<OrderRefundResponse>> CreateOrderRefundAsync(string orderId, OrderRefundRequest createOrderRefundRequest, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<RefundResponse>>> GetOrderRefundListAsync(string orderId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<RefundResponse>>> GetRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url, CancellationToken cancellationToken = default);
        Task<MollieResult<RefundResponse>> GetRefundAsync(UrlObjectLink<RefundResponse> url, CancellationToken cancellationToken = default);
    }
}
