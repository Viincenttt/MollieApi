using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IOnboardingClient : IBaseMollieClient {
        Task<OnboardingStatusResponse> GetOnboardingStatusAsync(CancellationToken cancellationToken = default);

        Task SubmitOnboardingDataAsync(SubmitOnboardingDataRequest request, CancellationToken cancellationToken = default);
    }
}
