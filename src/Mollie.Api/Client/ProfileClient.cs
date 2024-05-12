using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class ProfileClient : BaseMollieClient, IProfileClient {
        public ProfileClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ProfileResponse> CreateProfileAsync(ProfileRequest request) {
            return await PostAsync<ProfileResponse>("profiles", request).ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetProfileAsync(string profileId) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await GetAsync<ProfileResponse>($"profiles/{profileId}").ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetProfileAsync(UrlObjectLink<ProfileResponse> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetCurrentProfileAsync() {
            return await GetAsync<ProfileResponse>("profiles/me").ConfigureAwait(false);
        }

        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(string? from = null, int? limit = null) {
            return await GetListAsync<ListResponse<ProfileResponse>>("profiles", from, limit).ConfigureAwait(false);
        }

        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(UrlObjectLink<ListResponse<ProfileResponse>> url) {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ProfileResponse> UpdateProfileAsync(string profileId, ProfileRequest request) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await PatchAsync<ProfileResponse>($"profiles/{profileId}", request).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> EnablePaymentMethodAsync(string profileId, string paymentMethod) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await PostAsync<PaymentMethodResponse>($"profiles/{profileId}/methods/{paymentMethod}", null).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> EnablePaymentMethodAsync(string paymentMethod) {
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await PostAsync<PaymentMethodResponse>($"profiles/me/methods/{paymentMethod}", null).ConfigureAwait(false);
        }

        public async Task DisablePaymentMethodAsync(string profileId, string paymentMethod) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            await DeleteAsync($"profiles/{profileId}/methods/{paymentMethod}").ConfigureAwait(false);
        }

        public async Task DisablePaymentMethodAsync(string paymentMethod) {
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            await DeleteAsync($"profiles/me/methods/{paymentMethod}").ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string profileId) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            await DeleteAsync($"profiles/{profileId}").ConfigureAwait(false);
        }

        public async Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string profileId, string issuer) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await PostAsync<EnableGiftCardIssuerResponse>($"profiles/{profileId}/methods/giftcard/issuers/{issuer}", null).ConfigureAwait(false);
        }

        public async Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string issuer) {
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await PostAsync<EnableGiftCardIssuerResponse>($"profiles/me/methods/giftcard/issuers/{issuer}", null).ConfigureAwait(false);
        }

        public async Task DisableGiftCardIssuerAsync(string profileId, string issuer) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            await DeleteAsync($"profiles/{profileId}/methods/giftcard/issuers/{issuer}").ConfigureAwait(false);
        }

        public async Task DisableGiftCardIssuerAsync(string issuer) {
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            await DeleteAsync($"profiles/me/methods/giftcard/issuers/{issuer}").ConfigureAwait(false);
        }
    }
}
