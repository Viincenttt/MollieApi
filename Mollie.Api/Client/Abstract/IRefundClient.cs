using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Client.Abstract {
    public interface IRefundClient {
        Task CancelRefundAsync(string paymentId, string refundId);
        Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest);
        Task<RefundResponse> GetRefundAsync(string paymentId, string refundId);

        Task<ListResponse<RefundResponse>> GetRefundListAsync(string paymentId, string from = null, int? limit = null);
    }
}