using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.Session.Request;
using Mollie.Api.Models.Session.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class SessionClient : BaseMollieClient, ISessionClient {
        public SessionClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public SessionClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<SessionResponse> GetSessionAsync(
            string sessionId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(sessionId), sessionId);
            var queryParameters = BuildQueryParameters(testmode: testmode);
            return await GetAsync<SessionResponse>(
                    $"sessions/{sessionId}{queryParameters.ToQueryString()}",
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SessionResponse> CreateSessionAsync(
            SessionRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<SessionResponse>(
                    $"sessions", request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

    }
}
