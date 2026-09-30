using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IProfileClient : IBaseMollieClient {
        Task<MollieResult<ProfileResponse>> CreateProfileAsync(ProfileRequest request, CancellationToken cancellationToken = default);
        Task<MollieResult<ProfileResponse>> GetProfileAsync(string profileId, CancellationToken cancellationToken = default);
        Task<MollieResult<ProfileResponse>> GetProfileAsync(UrlObjectLink<ProfileResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<ProfileResponse>>> GetProfileListAsync(string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<ProfileResponse>>> GetProfileListAsync(UrlObjectLink<ListResponse<ProfileResponse>> url, CancellationToken cancellationToken = default);
        Task<MollieResult<ProfileResponse>> UpdateProfileAsync(string profileId, ProfileRequest request, CancellationToken cancellationToken = default);
        Task<MollieResult> DeleteProfileAsync(string profileId, CancellationToken cancellationToken = default);
        Task<MollieResult<ProfileResponse>> GetCurrentProfileAsync(CancellationToken cancellationToken = default);
        Task<MollieResult<PaymentMethodResponse>> EnablePaymentMethodAsync(string profileId, string paymentMethod, CancellationToken cancellationToken = default);
        Task<MollieResult<PaymentMethodResponse>> EnablePaymentMethodAsync(string paymentMethod, CancellationToken cancellationToken = default);
        Task<MollieResult> DisablePaymentMethodAsync(string profileId, string paymentMethod, CancellationToken cancellationToken = default);
        Task<MollieResult> DisablePaymentMethodAsync(string paymentMethod, CancellationToken cancellationToken = default);
        Task<MollieResult<EnableGiftCardIssuerResponse>> EnableGiftCardIssuerAsync(string profileId, string issuer, CancellationToken cancellationToken = default);
        Task<MollieResult<EnableGiftCardIssuerResponse>> EnableGiftCardIssuerAsync(string issuer, CancellationToken cancellationToken = default);
        Task<MollieResult> DisableGiftCardIssuerAsync(string profileId, string issuer, CancellationToken cancellationToken = default);
        Task<MollieResult> DisableGiftCardIssuerAsync(string issuer, CancellationToken cancellationToken = default);
    }
}
