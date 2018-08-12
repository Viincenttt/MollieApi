using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IRefundClient {
        Task CancelRefundAsync(string paymentId, string refundId);
        Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest);
        Task<RefundResponse> GetRefundAsync(string paymentId, string refundId);
        Task<ListResponse<RefundListData>> GetRefundListAsync(string paymentId, string from = null, int? limit = null);
        Task<RefundResponse> GetRefundAsync(UrlObjectLink<RefundResponse> url);
    }
}