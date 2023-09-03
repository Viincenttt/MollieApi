using System;
using Mollie.Api.Models.Onboarding.Request;
using Mollie.Api.Models.Onboarding.Response;
using System.Threading.Tasks;

namespace Mollie.Api.Client.Abstract {
    public interface IOnboardingClient : IDisposable {
        /// <summary>
        /// Get the status of onboarding of the authenticated organization.
        /// </summary>
        Task<OnboardingStatusResponse> GetOnboardingStatusAsync();

        /// <summary>
        /// Submit data that will be prefilled in the merchant’s onboarding. Please note that the data
        /// you submit will only be processed when the onboarding status is needs-data.
        /// </summary>
        Task SubmitOnboardingDataAsync(SubmitOnboardingDataRequest request);
    }
}
