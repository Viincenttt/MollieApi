using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Settlement;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class SettlementsClient : BaseMollieClient, ISettlementsClient {
        public SettlementsClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        public async Task<SettlementResponse> GetSettlementAsync(string settlementId) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetAsync<SettlementResponse>($"settlements/{settlementId}").ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetNextSettlement() {
            return await GetAsync<SettlementResponse>($"settlements/next").ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetOpenSettlement() {
            return await GetAsync<SettlementResponse>($"settlements/open").ConfigureAwait(false);
        }

        public async Task<ListResponse<SettlementResponse>> GetSettlementsListAsync(string? reference = null, string? offset = null, int? count = null) {
            var queryString = !string.IsNullOrWhiteSpace(reference) ? $"?reference={reference}" : string.Empty;
            return await GetListAsync<ListResponse<SettlementResponse>>($"settlements{queryString}", offset, count).ConfigureAwait(false);
        }

        public async Task<ListResponse<SettlementResponse>> GetSettlementsListAsync(UrlObjectLink<ListResponse<SettlementResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(string settlementId, string? offset = null, int? count = null) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<PaymentResponse>>($"settlements/{settlementId}/payments", offset, count).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(string settlementId, string? offset = null, int? count = null) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<RefundResponse>>($"settlements/{settlementId}/refunds", offset, count).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(UrlObjectLink<ListResponse<RefundResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(string settlementId, string? offset = null, int? count = null) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<ChargebackResponse>>($"settlements/{settlementId}/chargebacks", offset, count).ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetSettlementCapturesListAsync(string settlementId, string? offset = null, int? count = null)
        {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<CaptureResponse>>($"settlements/{settlementId}/captures", offset, count).ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetSettlementCapturesListAsync(UrlObjectLink<ListResponse<CaptureResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetSettlementAsync(UrlObjectLink<SettlementResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> ListSettlementCapturesAsync(string settlementId)
        {
            return await GetSettlementCapturesListAsync(settlementId);
        }
    }
}
