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
        public ProfileClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ProfileResponse> CreateProfileAsync(ProfileRequest request) {
            return await this.PostAsync<ProfileResponse>("profiles", request).ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetProfileAsync(string profileId) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await this.GetAsync<ProfileResponse>($"profiles/{profileId}").ConfigureAwait(false);
        }
        
        public async Task<ProfileResponse> GetProfileAsync(UrlObjectLink<ProfileResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetCurrentProfileAsync() {
            return await this.GetAsync<ProfileResponse>("profiles/me").ConfigureAwait(false);
        }

        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(string from = null, int? limit = null) {
            return await this.GetListAsync<ListResponse<ProfileResponse>>("profiles", from, limit).ConfigureAwait(false);
        }
        
        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(UrlObjectLink<ListResponse<ProfileResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ProfileResponse> UpdateProfileAsync(string profileId, ProfileRequest request) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await this.PatchAsync<ProfileResponse>($"profiles/{profileId}", request).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> EnablePaymentMethodAsync(string profileId, string paymentMethod) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            this.ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await this.PostAsync<PaymentMethodResponse>($"profiles/{profileId}/methods/{paymentMethod}", null).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> EnablePaymentMethodAsync(string paymentMethod) {
            this.ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await this.PostAsync<PaymentMethodResponse>($"profiles/me/methods/{paymentMethod}", null).ConfigureAwait(false);
        }

        public async Task DisablePaymentMethodAsync(string profileId, string paymentMethod) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            this.ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            await this.DeleteAsync($"profiles/{profileId}/methods/{paymentMethod}").ConfigureAwait(false);
        }

        public async Task DisablePaymentMethodAsync(string paymentMethod) {
            this.ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            await this.DeleteAsync($"profiles/me/methods/{paymentMethod}").ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string profileId) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            await this.DeleteAsync($"profiles/{profileId}").ConfigureAwait(false);
        }

        public async Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string profileId, string issuer) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            this.ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await this.PostAsync<EnableGiftCardIssuerResponse>($"profiles/{profileId}/methods/giftcard/issuers/{issuer}", null).ConfigureAwait(false);
        }

        public async Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string issuer) {
            this.ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await this.PostAsync<EnableGiftCardIssuerResponse>($"profiles/me/methods/giftcard/issuers/{issuer}", null).ConfigureAwait(false);
        }

        public async Task DisableGiftCardIssuerAsync(string profileId, string issuer) {
            this.ValidateRequiredUrlParameter(nameof(profileId), profileId);
            this.ValidateRequiredUrlParameter(nameof(issuer), issuer);
            await this.DeleteAsync($"profiles/{profileId}/methods/giftcard/issuers/{issuer}", null).ConfigureAwait(false);
        }

        public async Task DisableGiftCardIssuerAsync(string issuer) {
            this.ValidateRequiredUrlParameter(nameof(issuer), issuer);
            await this.DeleteAsync($"profiles/me/methods/giftcard/issuers/{issuer}", null).ConfigureAwait(false);
        }
    }
}