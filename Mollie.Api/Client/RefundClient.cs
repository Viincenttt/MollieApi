using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Refund;

namespace Mollie.Api.Client {
    public class RefundClient : BaseMollieClient, IRefundClient {
        public RefundClient(string apiKey) : base(apiKey) {
        }

        public async Task<RefundResponse> CreateRefundAsync(string paymentId) {
            return await CreateRefundAsync(paymentId, new RefundRequest()).ConfigureAwait(false);
        }

        public async Task<RefundResponse> CreateRefundAsync(string paymentId, RefundRequest refundRequest) {
            return await PostAsync<RefundResponse>($"payments/{paymentId}/refunds", refundRequest)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetRefundListAsync(string paymentId, int? offset = null,
            int? count = null) {
            return await GetListAsync<ListResponse<RefundResponse>>($"payments/{paymentId}/refunds", offset, count)
                .ConfigureAwait(false);
        }

        public async Task<RefundResponse> GetRefundAsync(string paymentId, string refundId) {
            return await GetAsync<RefundResponse>($"payments/{paymentId}/refunds/{refundId}").ConfigureAwait(false);
        }

        public async Task CancelRefundAsync(string paymentId, string refundId) {
            await DeleteAsync($"payments/{paymentId}/refunds/{refundId}").ConfigureAwait(false);
        }
    }
}