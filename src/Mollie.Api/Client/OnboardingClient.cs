using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Framework.Authentication.Abstract;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class OnboardingClient : BaseMollieClient, IOnboardingClient {
        public OnboardingClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public OnboardingClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<OnboardingStatusResponse> GetOnboardingStatusAsync(
            CancellationToken cancellationToken = default) {
            return await GetAsync<OnboardingStatusResponse>(
                "onboarding/me", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task SubmitOnboardingDataAsync(
            SubmitOnboardingDataRequest request, CancellationToken cancellationToken = default) {
            await PostAsync<object>(
                "onboarding/me", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
