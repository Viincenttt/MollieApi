using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund.Response;
using Mollie.Api.Models.Settlement.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class SettlementClient : BaseMollieClient, ISettlementClient {
        public SettlementClient(string oauthAccessToken, HttpClient? httpClient = null) : base(oauthAccessToken, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public SettlementClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<SettlementResponse> GetSettlementAsync(
            string settlementId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetAsync<SettlementResponse>(
                $"settlements/{settlementId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetNextSettlement(CancellationToken cancellationToken = default) {
            return await GetAsync<SettlementResponse>(
                "settlements/next", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetOpenSettlement(CancellationToken cancellationToken = default) {
            return await GetAsync<SettlementResponse>(
                $"settlements/open", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SettlementResponse>> GetSettlementListAsync(
            string? reference = null, string? offset = null, int? count = null, CancellationToken cancellationToken = default) {
            var queryString = !string.IsNullOrWhiteSpace(reference) ? $"?reference={reference}" : string.Empty;
            return await GetListAsync<ListResponse<SettlementResponse>>(
                $"settlements{queryString}", offset, count, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SettlementResponse>> GetSettlementListAsync(
            UrlObjectLink<ListResponse<SettlementResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(
            string settlementId, string? offset = null, int? count = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<PaymentResponse>>(
                $"settlements/{settlementId}/payments", offset, count, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(
            UrlObjectLink<ListResponse<PaymentResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(
            string settlementId, string? offset = null, int? count = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<RefundResponse>>(
                $"settlements/{settlementId}/refunds", offset, count, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(
            UrlObjectLink<ListResponse<RefundResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(
            string settlementId, string? offset = null, int? count = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<ChargebackResponse>>(
                $"settlements/{settlementId}/chargebacks", offset, count, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(
            UrlObjectLink<ListResponse<ChargebackResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(
            string settlementId, string? offset = null, int? count = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<CaptureResponse>>(
                $"settlements/{settlementId}/captures", offset, count, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(
            UrlObjectLink<ListResponse<CaptureResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SettlementResponse> GetSettlementAsync(
            UrlObjectLink<SettlementResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
