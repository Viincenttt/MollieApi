using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Framework.Authentication.Abstract;

namespace Mollie.Api.Client {
    public class OnboardingClient : BaseMollieClient, IOnboardingClient {
        public OnboardingClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public OnboardingClient(IBearerTokenRetriever bearerTokenRetriever, HttpClient? httpClient = null) : base(bearerTokenRetriever, httpClient) {
        }

        public async Task<OnboardingStatusResponse> GetOnboardingStatusAsync() {
            return await GetAsync<OnboardingStatusResponse>("onboarding/me").ConfigureAwait(false);
        }

        public async Task SubmitOnboardingDataAsync(SubmitOnboardingDataRequest request) {
            await PostAsync<object>("onboarding/me", request).ConfigureAwait(false);
        }
    }
}
