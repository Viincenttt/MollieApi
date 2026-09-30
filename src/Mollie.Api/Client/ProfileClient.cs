using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class ProfileClient : BaseMollieClient, IProfileClient {
        public ProfileClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public ProfileClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<MollieResult<ProfileResponse>> CreateProfileAsync(
            ProfileRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<ProfileResponse>(
                "profiles", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ProfileResponse>> GetProfileAsync(
            string profileId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await GetAsync<ProfileResponse>(
                $"profiles/{profileId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ProfileResponse>> GetProfileAsync(
            UrlObjectLink<ProfileResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ProfileResponse>> GetCurrentProfileAsync(CancellationToken cancellationToken = default) {
            return await GetAsync<ProfileResponse>(
                "profiles/me", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<ProfileResponse>>> GetProfileListAsync(
            string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            return await GetListAsync<ListResponse<ProfileResponse>>(
                "profiles", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ListResponse<ProfileResponse>>> GetProfileListAsync(
            UrlObjectLink<ListResponse<ProfileResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<ProfileResponse>> UpdateProfileAsync(
            string profileId, ProfileRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await PatchAsync<ProfileResponse>(
                $"profiles/{profileId}", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<PaymentMethodResponse>> EnablePaymentMethodAsync(
            string profileId, string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await PostAsync<PaymentMethodResponse>(
                $"profiles/{profileId}/methods/{paymentMethod}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<PaymentMethodResponse>> EnablePaymentMethodAsync(
            string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await PostAsync<PaymentMethodResponse>(
                $"profiles/me/methods/{paymentMethod}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult> DisablePaymentMethodAsync(
            string profileId, string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await DeleteAsync($"profiles/{profileId}/methods/{paymentMethod}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult> DisablePaymentMethodAsync(string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await DeleteAsync($"profiles/me/methods/{paymentMethod}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult> DeleteProfileAsync(string profileId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await DeleteAsync($"profiles/{profileId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<EnableGiftCardIssuerResponse>> EnableGiftCardIssuerAsync(
            string profileId, string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await PostAsync<EnableGiftCardIssuerResponse>(
                $"profiles/{profileId}/methods/giftcard/issuers/{issuer}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult<EnableGiftCardIssuerResponse>> EnableGiftCardIssuerAsync(
            string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await PostAsync<EnableGiftCardIssuerResponse>(
                $"profiles/me/methods/giftcard/issuers/{issuer}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult> DisableGiftCardIssuerAsync(
            string profileId, string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await DeleteAsync(
                $"profiles/{profileId}/methods/giftcard/issuers/{issuer}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<MollieResult> DisableGiftCardIssuerAsync(
            string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await DeleteAsync(
                $"profiles/me/methods/giftcard/issuers/{issuer}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
