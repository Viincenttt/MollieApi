using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client {
    public class ChargebacksClient : BaseMollieClient, IChargebacksClient {
        public ChargebacksClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId) {
            return await this.GetAsync<ChargebackResponse>($"payments/{paymentId}/chargebacks/{chargebackId}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string paymentId, string from = null, int? limit = null) {
            return await this
                .GetListAsync<ListResponse<ChargebackResponse>>($"payments/{paymentId}/chargebacks", from, limit)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string profileId = null, bool? testmode = null) {
            if (profileId != null || testmode != null) {
                this.ValidateApiKeyIsOauthAccesstoken();
            }

            // Build parameters
            var parameters = new Dictionary<string, string>();
            parameters.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            parameters.AddValueIfNotNullOrEmpty(nameof(testmode), Convert.ToString(testmode).ToLower());

            return await this.GetListAsync<ListResponse<ChargebackResponse>>($"chargebacks", null, null, parameters).ConfigureAwait(false);
        }
    }
}