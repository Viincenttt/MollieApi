using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class ChargebacksClient : BaseMollieClient, IChargebacksClient {
        public ChargebacksClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(chargebackId), chargebackId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<ChargebackResponse>($"payments/{paymentId}/chargebacks/{chargebackId}{queryParameters.ToQueryString()}")
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await this
                .GetListAsync<ListResponse<ChargebackResponse>>($"payments/{paymentId}/chargebacks", from, limit, queryParameters)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string? profileId = null, bool testmode = false) {
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<ChargebackResponse>>($"chargebacks", null, null, queryParameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(string? profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            result.AddValueIfTrue(nameof(testmode), testmode);
            return result;
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode);
            return result;
        }
    }
}
