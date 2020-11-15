using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IRefundClient {
        Task CancelRefundAsync(string paymentId, string refundId, bool? testmode = default);
        Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest);
        Task<RefundResponse> GetRefundAsync(string paymentId, string refundId, bool? testmode = default);
        Task<ListResponse<RefundResponse>> GetRefundListAsync(string paymentId, string from = null, int? limit = null, bool? testmode = default);
        Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url);
    }
}