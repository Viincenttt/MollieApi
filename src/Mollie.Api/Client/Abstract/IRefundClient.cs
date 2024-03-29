using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IRefundClient : IBaseMollieClient {
        Task CancelRefundAsync(string paymentId, string refundId, bool testmode = false);
        Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest);
        Task<RefundResponse> GetRefundAsync(string paymentId, string refundId, bool testmode = false);
        Task<ListResponse<RefundResponse>> GetRefundListAsync(string paymentId, string from = null, int? limit = null, bool testmode = false);
        Task<ListResponse<RefundResponse>> GetRefundListAsync(UrlObjectLink<ListResponse<RefundResponse>> url);
        Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url);
    }
}