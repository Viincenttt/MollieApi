using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IOnboardingClient : IBaseMollieClient {
        Task<MollieResult<OnboardingStatusResponse>> GetOnboardingStatusAsync(CancellationToken cancellationToken = default);

        Task<MollieResult> SubmitOnboardingDataAsync(SubmitOnboardingDataRequest request, CancellationToken cancellationToken = default);
    }
}
