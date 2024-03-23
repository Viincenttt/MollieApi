using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class BalanceClient : BaseMollieClient, IBalanceClient {
        public BalanceClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }
        
        public async Task<BalanceResponse> GetBalanceAsync(string balanceId) {
            this.ValidateRequiredUrlParameter(nameof(balanceId), balanceId);
            return await this.GetAsync<BalanceResponse>($"balances/{balanceId}").ConfigureAwait(false);
        }
        
        public async Task<BalanceResponse> GetBalanceAsync(UrlObjectLink<BalanceResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }
        
        public async Task<BalanceResponse> GetPrimaryBalanceAsync() {
            return await this.GetAsync<BalanceResponse>("balances/primary").ConfigureAwait(false);
        }
        
        public async Task<ListResponse<BalanceResponse>> ListBalancesAsync(string? from = null, int? limit = null, string? currency = null) {
            var queryParameters = BuildListBalanceQueryParameters(currency);
            return await this.GetListAsync<ListResponse<BalanceResponse>>($"balances", from, limit, queryParameters).ConfigureAwait(false);
        }
        
        public async Task<ListResponse<BalanceResponse>> ListBalancesAsync(UrlObjectLink<ListResponse<BalanceResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<BalanceReportResponse> GetBalanceReportAsync(string balanceId, DateTime from, DateTime until, string? grouping = null) {
            this.ValidateRequiredUrlParameter(nameof(balanceId), balanceId);
            var queryParameters = BuildGetBalanceReportQueryParameters(from, until, grouping);
            return await this.GetAsync<BalanceReportResponse>($"balances/{balanceId}/report{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }
        
        public async Task<BalanceReportResponse> GetPrimaryBalanceReportAsync(DateTime from, DateTime until, string? grouping = null) {
            var queryParameters = BuildGetBalanceReportQueryParameters(from, until, grouping);
            return await this.GetAsync<BalanceReportResponse>($"balances/primary/report{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<BalanceTransactionResponse> ListBalanceTransactionsAsync(string balanceId, string? from = null, int? limit = null) {
            this.ValidateRequiredUrlParameter(nameof(balanceId), balanceId);
            var queryParameters = BuildListBalanceTransactionsQueryParameters(from, limit);
            return await this.GetAsync<BalanceTransactionResponse>($"balances/{balanceId}/transactions{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }
        
        public async Task<BalanceTransactionResponse> ListPrimaryBalanceTransactionsAsync(string? from = null, int? limit = null) {
            var queryParameters = BuildListBalanceTransactionsQueryParameters(from, limit);
            return await this.GetAsync<BalanceTransactionResponse>($"balances/primary/transactions{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildListBalanceTransactionsQueryParameters(string? from, int? limit) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("from", from);
            result.AddValueIfNotNullOrEmpty("limit", limit?.ToString(CultureInfo.InvariantCulture));
            return result;
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