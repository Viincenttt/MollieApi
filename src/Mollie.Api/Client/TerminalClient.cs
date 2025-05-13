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
            string terminalId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(terminalId), terminalId);
            return await GetAsync<TerminalResponse>(
                $"terminals/{terminalId}", cancellationToken: cancellationToken)
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

        private Dictionary<string, string> BuildQueryParameters(string? profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
