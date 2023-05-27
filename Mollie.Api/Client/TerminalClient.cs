using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Terminal;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class TerminalClient : BaseMollieClient, ITerminalClient
    {
        public TerminalClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) { }
        
        public async Task<TerminalResponse> GetTerminalAsync(string terminalId) {
            this.ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            return await this.GetAsync<TerminalResponse>($"terminals/{terminalId}").ConfigureAwait(false);
        }
        
        public async Task<TerminalResponse> GetTerminalAsync(UrlObjectLink<TerminalResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }
        
        public async Task<ListResponse<TerminalResponse>> GetTerminalListAsync(string from = null, int? limit = null, string profileId = null, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(profileId, testmode);
            return await this.GetListAsync<ListResponse<TerminalResponse>>("terminals", from, limit, queryParameters).ConfigureAwait(false);
        }
        
        public async Task<ListResponse<TerminalResponse>> GetTerminalListAsync(UrlObjectLink<ListResponse<TerminalResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(string profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}