using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IProfileClient : IBaseMollieClient {
        Task<ProfileResponse> CreateProfileAsync(ProfileRequest request);
        Task<ProfileResponse> GetProfileAsync(string profileId);
        Task<ProfileResponse> GetProfileAsync(UrlObjectLink<ProfileResponse> url);
        Task<ListResponse<ProfileResponse>> GetProfileListAsync(string? from = null, int? limit = null);
        Task<ListResponse<ProfileResponse>> GetProfileListAsync(UrlObjectLink<ListResponse<ProfileResponse>> url);
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