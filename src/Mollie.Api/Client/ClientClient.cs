using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Client.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class ClientClient : OauthBaseMollieClient, IClientClient {
        public ClientClient(string oauthAccessToken, HttpClient? httpClient = null)
            : base(oauthAccessToken, httpClient)
        {
        }

        [ActivatorUtilitiesConstructor]
        public ClientClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient)
        {
        }

        public async Task<ClientResponse> GetClientAsync(
            string clientId,
            bool embedOrganization = false,
            bool embedOnboarding = false,
            bool embedCapabilities = false,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(clientId), clientId);
            var queryParameters = BuildQueryParameters(embedOrganization, embedOnboarding, embedCapabilities);
            return await GetAsync<ClientResponse>(
                $"clients/{clientId}{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ClientResponse>> GetClientListAsync(
            string? from = null,
            int? limit = null,
            bool embedOrganization = false,
            bool embedOnboarding = false,
            bool embedCapabilities = false,
            CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(embedOrganization, embedOnboarding, embedCapabilities);
            return await GetListAsync<ListResponse<ClientResponse>>(
                "clients", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(
            bool embedOrganization = false, bool embedOnboarding = false, bool embedCapabilities = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("embed", BuildEmbedParameter(embedOrganization, embedOnboarding, embedCapabilities));
            return result;
        }

        private string BuildEmbedParameter(
            bool embedOrganization = false, bool embedOnboarding = false, bool embedCapabilities = false) {
            var includeList = new List<string>();
            includeList.AddValueIfTrue("organization", embedOrganization);
            includeList.AddValueIfTrue("onboarding", embedOnboarding);
            includeList.AddValueIfTrue("capabilities", embedCapabilities);
            return includeList.ToIncludeParameter();
        }
    }
}
