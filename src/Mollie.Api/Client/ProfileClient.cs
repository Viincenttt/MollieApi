﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class ProfileClient : BaseMollieClient, IProfileClient {
        public ProfileClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        public ProfileClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null) : base(mollieSecretManager, httpClient) {
        }

        public async Task<ProfileResponse> CreateProfileAsync(
            ProfileRequest request, CancellationToken cancellationToken = default) {
            return await PostAsync<ProfileResponse>(
                "profiles", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetProfileAsync(
            string profileId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await GetAsync<ProfileResponse>(
                $"profiles/{profileId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetProfileAsync(
            UrlObjectLink<ProfileResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ProfileResponse> GetCurrentProfileAsync(CancellationToken cancellationToken = default) {
            return await GetAsync<ProfileResponse>(
                "profiles/me", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(
            string? from = null, int? limit = null, CancellationToken cancellationToken = default) {
            return await GetListAsync<ListResponse<ProfileResponse>>(
                "profiles", from, limit, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<ProfileResponse>> GetProfileListAsync(
            UrlObjectLink<ListResponse<ProfileResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ProfileResponse> UpdateProfileAsync(
            string profileId, ProfileRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            return await PatchAsync<ProfileResponse>(
                $"profiles/{profileId}", request, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> EnablePaymentMethodAsync(
            string profileId, string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await PostAsync<PaymentMethodResponse>(
                $"profiles/{profileId}/methods/{paymentMethod}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> EnablePaymentMethodAsync(
            string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            return await PostAsync<PaymentMethodResponse>(
                $"profiles/me/methods/{paymentMethod}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DisablePaymentMethodAsync(
            string profileId, string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            await DeleteAsync($"profiles/{profileId}/methods/{paymentMethod}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DisablePaymentMethodAsync(string paymentMethod, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentMethod), paymentMethod);
            await DeleteAsync($"profiles/me/methods/{paymentMethod}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteProfileAsync(string profileId, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            await DeleteAsync($"profiles/{profileId}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(
            string profileId, string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await PostAsync<EnableGiftCardIssuerResponse>(
                $"profiles/{profileId}/methods/giftcard/issuers/{issuer}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(
            string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            return await PostAsync<EnableGiftCardIssuerResponse>(
                $"profiles/me/methods/giftcard/issuers/{issuer}", null, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DisableGiftCardIssuerAsync(
            string profileId, string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(profileId), profileId);
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            await DeleteAsync(
                $"profiles/{profileId}/methods/giftcard/issuers/{issuer}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DisableGiftCardIssuerAsync(
            string issuer, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(issuer), issuer);
            await DeleteAsync(
                $"profiles/me/methods/giftcard/issuers/{issuer}", cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
