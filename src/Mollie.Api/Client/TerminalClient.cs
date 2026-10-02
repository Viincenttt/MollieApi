using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Request;
using Mollie.Api.Models.Terminal.Response;
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

        public async Task<MollieResult<TerminalResponse>> GetTerminalAsync(
            string terminalId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<TerminalResponse>(
                $"terminals/{terminalId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<TerminalResponse>> GetTerminalAsync(
            UrlObjectLink<TerminalResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<TerminalResponse>>> GetTerminalListAsync(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<TerminalResponse>>(
                "terminals", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<TerminalResponse>>> GetTerminalListAsync(
            UrlObjectLink<ListResponse<TerminalResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<TerminalPairingCodeResponse>> CreateTerminalPairingCodeAsync(
            TerminalPairingCodeRequest request, bool includeQrCode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildPairingCodeQueryParameters(includeQrCode);
            return await PostAsync<TerminalPairingCodeResponse>(
                $"terminals/pairing-codes{queryParameters.ToQueryString()}", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<TerminalPairingCodeResponse>>> GetTerminalPairingCodeListAsync(
            string? from = null, int? limit = null, string? profileId = null, SortDirection? sort = null, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(profileId: profileId, sort: sort);
            return await GetListAsync<ListResponse<TerminalPairingCodeResponse>>(
                "terminals/pairing-codes", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<TerminalPairingCodeResponse>>> GetTerminalPairingCodeListAsync(
            UrlObjectLink<ListResponse<TerminalPairingCodeResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<TerminalPairingCodeResponse>> GetTerminalPairingCodeAsync(
            string pairingCodeId, bool includeQrCode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(pairingCodeId), pairingCodeId);
            var queryParameters = BuildPairingCodeQueryParameters(includeQrCode);
            return await GetAsync<TerminalPairingCodeResponse>(
                $"terminals/pairing-codes/{pairingCodeId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<TerminalPairingCodeResponse>> GetTerminalPairingCodeAsync(
            UrlObjectLink<TerminalPairingCodeResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<MollieResult<TerminalPairingCodeResponse>> RevokeTerminalPairingCodeAsync(
            string pairingCodeId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(pairingCodeId), pairingCodeId);
            return await DeleteAsync<TerminalPairingCodeResponse>(
                $"terminals/pairing-codes/{pairingCodeId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildPairingCodeQueryParameters(bool includeQrCode) {
            var includeList = new List<string>();
            includeList.AddValueIfTrue("details.qrCode", includeQrCode);
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("include", includeList.ToIncludeParameter());
            return result;
        }
    }
}
