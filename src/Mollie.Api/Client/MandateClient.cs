using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Request;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class MandateClient : BaseMollieClient, IMandateClient {
        public MandateClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<MandateResponse> GetMandateAsync(string customerId, string mandateId, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            ValidateRequiredUrlParameter(nameof(mandateId), mandateId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<MandateResponse>($"customers/{customerId}/mandates/{mandateId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateResponse>> GetMandateListAsync(string customerId, string? from = null, int? limit = null, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetListAsync<ListResponse<MandateResponse>>($"customers/{customerId}/mandates", from, limit, queryParameters)
                .ConfigureAwait(false);
        }

        public async Task<MandateResponse> CreateMandateAsync(string customerId, MandateRequest request) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await PostAsync<MandateResponse>($"customers/{customerId}/mandates", request).ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateResponse>> GetMandateListAsync(UrlObjectLink<ListResponse<MandateResponse>> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<MandateResponse> GetMandateAsync(UrlObjectLink<MandateResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task RevokeMandate(string customerId, string mandateId, bool testmode = false) {
            var data = TestmodeModel.Create(testmode);
            await DeleteAsync($"customers/{customerId}/mandates/{mandateId}", data).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
