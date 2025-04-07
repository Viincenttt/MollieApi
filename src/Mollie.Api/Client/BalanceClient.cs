using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class BalanceClient : BaseMollieClient, IBalanceClient {
        public BalanceClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public BalanceClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) : base(mollieSecretManager, httpClient) {
        }

        public async Task<BalanceResponse> GetBalanceAsync(string balanceId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(balanceId), balanceId);
            return await GetAsync<BalanceResponse>($"balances/{balanceId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<BalanceResponse> GetBalanceAsync(UrlObjectLink<BalanceResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<BalanceResponse> GetPrimaryBalanceAsync(CancellationToken cancellationToken = default) {
            return await GetAsync<BalanceResponse>("balances/primary", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<BalanceResponse>> GetBalanceListAsync(
            string? from = null, int? limit = null, string? currency = null, CancellationToken cancellationToken = default) {
            var queryParameters = BuildListBalanceQueryParameters(currency);
            return await GetListAsync<ListResponse<BalanceResponse>>(
                $"balances", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<BalanceResponse>> GetBalanceListAsync(
            UrlObjectLink<ListResponse<BalanceResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<BalanceReportResponse> GetBalanceReportAsync(
            string balanceId, DateTime from, DateTime until, string? grouping = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(balanceId), balanceId);
            var queryParameters = BuildGetBalanceReportQueryParameters(from, until, grouping);
            return await GetAsync<BalanceReportResponse>(
                $"balances/{balanceId}/report{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<BalanceReportResponse> GetPrimaryBalanceReportAsync(
            DateTime from, DateTime until, string? grouping = null, CancellationToken cancellationToken = default) {
            var queryParameters = BuildGetBalanceReportQueryParameters(from, until, grouping);
            return await GetAsync<BalanceReportResponse>(
                $"balances/primary/report{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<BalanceTransactionResponse>> GetBalanceTransactionListAsync(
            string balanceId, string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(balanceId), balanceId);
            return await GetListAsync<ListResponse<BalanceTransactionResponse>>(
                    $"balances/{balanceId}/transactions", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<BalanceTransactionResponse>> GetPrimaryBalanceTransactionListAsync(
            string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            return await GetListAsync<ListResponse<BalanceTransactionResponse>>(
                    $"balances/primary/transactions", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<BalanceTransactionResponse>> GetBalanceTransactionListAsync(
            UrlObjectLink<ListResponse<BalanceTransactionResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildGetBalanceReportQueryParameters(DateTime from, DateTime until, string? grouping = null) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("from", from.ToString("yyyy-MM-dd"));
            result.AddValueIfNotNullOrEmpty("until", until.ToString("yyyy-MM-dd"));
            result.AddValueIfNotNullOrEmpty("grouping", grouping);
            return result;
        }

        private Dictionary<string, string> BuildListBalanceQueryParameters(string? currency) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("currency", currency);
            return result;
        }
    }
}
