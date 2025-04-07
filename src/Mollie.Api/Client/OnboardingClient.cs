using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Framework.Authentication.Abstract;
using System.Threading;

namespace Mollie.Api.Client {
    public class OnboardingClient : BaseMollieClient, IOnboardingClient {
        public OnboardingClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public OnboardingClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) : base(mollieSecretManager, httpClient) {
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
