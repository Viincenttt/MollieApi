using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IProfileClient {
        Task<ProfileResponse> CreateProfileAsync(ProfileRequest request);
        Task<ProfileResponse> GetProfileAsync(string profileId);
        Task<ListResponse<ProfileResponse>> GetProfileListAsync(string from = null, int? limit = null);
        Task<ProfileResponse> UpdateProfileAsync(string profileId, ProfileRequest request);
        Task DeleteProfileAsync(string profileId);
        Task<ProfileResponse> GetCurrentProfileAsync();
        Task<PaymentMethodResponse> EnablePaymentMethodAsync(string profileId, string paymentMethod);
        Task<PaymentMethodResponse> EnablePaymentMethodAsync(string paymentMethod);
        Task DisablePaymentMethodAsync(string profileId, string paymentMethod);
        Task DisablePaymentMethodAsync(string paymentMethod);
        Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string profileId, string issuer);
        Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string issuer);
        Task DisableGiftCardIssuerAsync(string profileId, string issuer);
        Task DisableGiftCardIssuerAsync(string issuer);
    }
}