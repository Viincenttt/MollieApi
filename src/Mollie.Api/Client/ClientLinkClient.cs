using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.ClientLink.Request;
using Mollie.Api.Models.ClientLink.Response;
using System.Threading;

namespace Mollie.Api.Client {
    public class ClientLinkClient : OauthBaseMollieClient, IClientLinkClient
    {
        private readonly string _clientId;

        public ClientLinkClient(string clientId, string oauthAccessToken, HttpClient? httpClient = null)
            : base(oauthAccessToken, httpClient)
        {
            _clientId = clientId;
        }

        public ClientLinkClient(string? clientId, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(mollieSecretManager, httpClient) {
            if (string.IsNullOrWhiteSpace(clientId)) {
                throw new ArgumentNullException(nameof(clientId));
            }

            _clientId = clientId!;
        }

        public async Task<ClientLinkResponse> CreateClientLinkAsync(ClientLinkRequest request, CancellationToken cancellationToken = default)
        {
            return await PostAsync<ClientLinkResponse>(
                    "client-links", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public string GenerateClientLinkWithParameters(
            string clientLinkUrl,
            string state,
            List<string> scopes,
            bool forceApprovalPrompt = false)
        {
            var parameters = new Dictionary<string, string> {
                {"client_id", _clientId},
                {"state", state},
                {"scope", string.Join(" ", scopes)},
                {"approval_prompt", forceApprovalPrompt ? "force" : "auto"}
            };

            return clientLinkUrl + parameters.ToQueryString();
        }
    }
}
