using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Settlement;

namespace Mollie.Api.Client {
    public class SettlementsClient : OauthBaseMollieClient, ISettlementsClient {
        public SettlementsClient(string oauthAccessToken) : base(oauthAccessToken) {
        }

        public async Task<SettlementResponse> GetSettlementAsync(string settlementId) {
            return await this.GetAsync<SettlementResponse>($"settlements/{settlementId}").ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetNextSettlement() {
            return await this.GetAsync<SettlementResponse>($"settlements/next").ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetOpenBalance() {
            return await this.GetAsync<SettlementResponse>($"settlements/open").ConfigureAwait(false);
        }

        public async Task<ListResponse<SettlementResponse>> GetSettlementsListAsync(string reference = null, string from = null, int? limit = null) {
            var queryString = !string.IsNullOrWhiteSpace(reference) ? $"?reference={reference}" : string.Empty;
            return await this.GetListAsync<ListResponse<SettlementResponse>>($"settlements{queryString}", from, limit)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(string settlementId, string from = null, int? limit = null) {
            return await this
                .GetListAsync<ListResponse<PaymentResponse>>($"settlements/{settlementId}/payments", from, limit)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(string settlementId, string from = null, int? limit = null) {
            return await this
                .GetListAsync<ListResponse<RefundResponse>>($"settlements/{settlementId}/refunds", from, limit)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(string settlementId, string from = null, int? limit = null) {
            return await this
                .GetListAsync<ListResponse<ChargebackResponse>>($"settlements/{settlementId}/chargebacks", from, limit).ConfigureAwait(false);
        }
    }
}