using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Api.Models.UnreferencedRefund.Request;
using Mollie.Api.Models.UnreferencedRefund.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class TerminalClient : BaseMollieClient, ITerminalClient
    {
        public TerminalClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) { }

        [ActivatorUtilitiesConstructor]
        public TerminalClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<TerminalResponse> GetTerminalAsync(
            string terminalId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<TerminalResponse>(
                $"terminals/{terminalId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TerminalResponse> GetTerminalAsync(
            UrlObjectLink<TerminalResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<TerminalResponse>> GetTerminalListAsync(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<TerminalResponse>>(
                "terminals", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<TerminalResponse>> GetTerminalListAsync(
            UrlObjectLink<ListResponse<TerminalResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<UnreferencedRefundResponse> CreateUnreferencedRefundAsync(
            string terminalId, UnreferencedRefundRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            if (!string.IsNullOrWhiteSpace(request.ProfileId)) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            return await PostAsync<UnreferencedRefundResponse>(
                $"terminals/{terminalId}/unreferenced-refunds", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<UnreferencedRefundResponse> GetUnreferencedRefundAsync(
            string terminalId, string unreferencedRefundId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            ValidateRequiredUrlParameter(nameof(unreferencedRefundId), unreferencedRefundId);
            return await GetAsync<UnreferencedRefundResponse>(
                $"terminals/{terminalId}/unreferenced-refunds/{unreferencedRefundId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<UnreferencedRefundResponse> GetUnreferencedRefundAsync(
            UrlObjectLink<UnreferencedRefundResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<UnreferencedRefundResponse>> GetUnreferencedRefundListAsync(
            string terminalId, string? from = null, int? limit = null, string? profileId = null, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            if (!string.IsNullOrWhiteSpace(profileId)) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(profileId: profileId);
            // The unreferenced refunds API does not support test mode, so a globally configured testmode is never sent
            queryParameters.Remove("testmode");
            return await GetListAsync<ListResponse<UnreferencedRefundResponse>>(
                $"terminals/{terminalId}/unreferenced-refunds", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<UnreferencedRefundResponse>> GetUnreferencedRefundListAsync(
            UrlObjectLink<ListResponse<UnreferencedRefundResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
