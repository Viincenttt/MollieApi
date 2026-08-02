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
        Task<MollieResult<PaymentRefundResponse>> CreatePaymentRefundAsync(string paymentId, PaymentRefundRequest paymentRefundRequest, CancellationToken cancellationToken = default);
        Task<MollieResult<PaymentRefundResponse>> GetPaymentRefundAsync(string paymentId, string refundId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult> CancelPaymentRefundAsync(string paymentId, string refundId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<PaymentRefundResponse>>> GetPaymentRefundListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<PaymentRefundResponse>> GetPaymentRefundAsync(UrlObjectLink<PaymentRefundResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<PaymentRefundResponse>>> GetPaymentRefundListAsync(UrlObjectLink<ListResponse<PaymentRefundResponse>> url, CancellationToken cancellationToken = default);

        Task<MollieResult<OrderRefundResponse>> CreateOrderRefundAsync(string orderId, OrderRefundRequest createOrderRefundRequest, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<OrderRefundResponse>>> GetOrderRefundListAsync(string orderId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
    }
}
