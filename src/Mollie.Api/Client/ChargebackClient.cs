using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Chargeback.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class ChargebackClient : BaseMollieClient, IChargebackClient {
        public ChargebackClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public ChargebackClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            ValidateRequiredUrlParameter(nameof(chargebackId), chargebackId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<ChargebackResponse>(
                    $"payments/{paymentId}/chargebacks/{chargebackId}{queryParameters.ToQueryString()}",
                    cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebackListAsync(string paymentId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetListAsync<ListResponse<ChargebackResponse>>(
                    $"payments/{paymentId}/chargebacks", from, limit, queryParameters, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebackListAsync(string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<ChargebackResponse>>(
                "chargebacks", null, null, queryParameters, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ChargebackResponse>> GetChargebackListAsync(UrlObjectLink<ListResponse<ChargebackResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken)
                .ConfigureAwait(false);
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
