using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client {
    public class BalanceClient : BaseMollieClient, IBalanceClient {
        public BalanceClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }
        
        public async Task<BalanceResponse> GetBalanceAsync(string balanceId) {
            return await this.GetAsync<BalanceResponse>($"balances/{balanceId}").ConfigureAwait(false);
        }
        
        public async Task<BalanceResponse> GetPrimaryBalanceAsync() {
            return await this.GetAsync<BalanceResponse>("balances/primary").ConfigureAwait(false);
        }
        
        public async Task<ListResponse<BalanceResponse>> ListBalancesAsync(string from = null, int? limit = null, string currency = null) {
            var queryParameters = BuildQueryParameters(currency);
            return await this.GetListAsync<ListResponse<BalanceResponse>>($"balances", from, limit, queryParameters).ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(string currency) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("currency", currency);
            return result;
        }
    }
}