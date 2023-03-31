using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Terminals;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client
{
    public class TerminalClient : BaseMollieClient, ITerminalClient
    {
        public TerminalClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) { }
        /// <summary>
        /// retrieve the data for one specific terminal
        /// </summary>
        /// <param name="paymentMethod"></param>
        /// <param name="includeIssuers"></param>
        /// <param name="locale"></param>
        /// <param name="includePricing"></param>
        /// <param name="profileId"></param>
        /// <param name="testmode"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public async Task<TerminalResponse> GetTerminalAsync(string id) {
            return await this.GetAsync<TerminalResponse>($"terminals/{id}").ConfigureAwait(false);
        }
        /// <summary>
        /// retrieve all data for all terminals
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="amount"></param>
        /// <param name="includeIssuers"></param>
        /// <param name="includePricing"></param>
        /// <returns></returns>
        public async Task<ListResponse<TerminalResponse>> GetAllTerminalListAsync(string from = null, int? limit = null, string profileId = null) {
            var queryParameters = this.BuildQueryParameters(
                profileId: profileId);
            return await this.GetListAsync<ListResponse<TerminalResponse>>("terminals/", from, limit, queryParameters).ConfigureAwait(false);
        }
        private Dictionary<string, string> BuildQueryParameters(string profileId = null)
        {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            return result;
        }
    }
}