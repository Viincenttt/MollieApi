using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
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
            string? balanceId = null, int? year = null, int? month = null, IEnumerable<string>? currencies = null,
            string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            var parameters = new Dictionary<string, string>();
            parameters.AddValueIfNotNullOrEmpty(nameof(balanceId), balanceId);
            parameters.AddValueIfNotNullOrEmpty(nameof(year), Convert.ToString(year));
            parameters.AddValueIfNotNullOrEmpty(nameof(month), Convert.ToString(month));
            parameters.AddValueIfNotNullOrEmpty(nameof(currencies), currencies != null ? string.Join(",", currencies) : null);
            return await GetListAsync<ListResponse<SettlementResponse>>(
                "settlements", from, limit, parameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SettlementResponse>> GetSettlementListAsync(
            UrlObjectLink<ListResponse<SettlementResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(
            string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<PaymentResponse>>(
                $"settlements/{settlementId}/payments", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSettlementPaymentListAsync(
            UrlObjectLink<ListResponse<PaymentResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(
            string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<RefundResponse>>(
                $"settlements/{settlementId}/refunds", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<RefundResponse>> GetSettlementRefundListAsync(
            UrlObjectLink<ListResponse<RefundResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(
            string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<ChargebackResponse>>(
                $"settlements/{settlementId}/chargebacks", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetSettlementChargebackListAsync(
            UrlObjectLink<ListResponse<ChargebackResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<CaptureResponse>> GetSettlementCaptureListAsync(
            string settlementId, string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(settlementId), settlementId);
            return await GetListAsync<ListResponse<CaptureResponse>>(
                $"settlements/{settlementId}/captures", from, limit, cancellationToken: cancellationToken)
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
