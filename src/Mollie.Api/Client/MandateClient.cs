using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Request;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class MandateClient : BaseMollieClient, IMandateClient {
        public MandateClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public MandateClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) : base(mollieSecretManager, httpClient) {
        }

        public async Task<MandateResponse> GetMandateAsync(
            string customerId, string mandateId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            ValidateRequiredUrlParameter(nameof(mandateId), mandateId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<MandateResponse>(
                $"customers/{customerId}/mandates/{mandateId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateResponse>> GetMandateListAsync(
            string customerId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetListAsync<ListResponse<MandateResponse>>(
                    $"customers/{customerId}/mandates", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MandateResponse> CreateMandateAsync(
            string customerId, MandateRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await PostAsync<MandateResponse>(
                $"customers/{customerId}/mandates", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<MandateResponse>> GetMandateListAsync(
            UrlObjectLink<ListResponse<MandateResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MandateResponse> GetMandateAsync(
            UrlObjectLink<MandateResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task RevokeMandate(
            string customerId, string mandateId, bool testmode = false, CancellationToken cancellationToken = default) {
            var data = TestmodeModel.Create(testmode);
            await DeleteAsync($"customers/{customerId}/mandates/{mandateId}", data, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
