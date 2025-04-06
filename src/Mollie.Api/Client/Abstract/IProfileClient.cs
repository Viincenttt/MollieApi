﻿using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Profile.Request;
using Mollie.Api.Models.Profile.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IProfileClient : IBaseMollieClient {
        Task<ProfileResponse> CreateProfileAsync(ProfileRequest request, CancellationToken cancellationToken = default);
        Task<ProfileResponse> GetProfileAsync(string profileId, CancellationToken cancellationToken = default);
        Task<ProfileResponse> GetProfileAsync(UrlObjectLink<ProfileResponse> url, CancellationToken cancellationToken = default);
        Task<ListResponse<ProfileResponse>> GetProfileListAsync(string? from = null, int? limit = null, CancellationToken cancellationToken = default);
        Task<ListResponse<ProfileResponse>> GetProfileListAsync(UrlObjectLink<ListResponse<ProfileResponse>> url, CancellationToken cancellationToken = default);
        Task<ProfileResponse> UpdateProfileAsync(string profileId, ProfileRequest request, CancellationToken cancellationToken = default);
        Task DeleteProfileAsync(string profileId, CancellationToken cancellationToken = default);
        Task<ProfileResponse> GetCurrentProfileAsync(CancellationToken cancellationToken = default);
        Task<PaymentMethodResponse> EnablePaymentMethodAsync(string profileId, string paymentMethod, CancellationToken cancellationToken = default);
        Task<PaymentMethodResponse> EnablePaymentMethodAsync(string paymentMethod, CancellationToken cancellationToken = default);
        Task DisablePaymentMethodAsync(string profileId, string paymentMethod, CancellationToken cancellationToken = default);
        Task DisablePaymentMethodAsync(string paymentMethod, CancellationToken cancellationToken = default);
        Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string profileId, string issuer, CancellationToken cancellationToken = default);
        Task<EnableGiftCardIssuerResponse> EnableGiftCardIssuerAsync(string issuer, CancellationToken cancellationToken = default);
        Task DisableGiftCardIssuerAsync(string profileId, string issuer, CancellationToken cancellationToken = default);
        Task DisableGiftCardIssuerAsync(string issuer, CancellationToken cancellationToken = default);
    }
}
