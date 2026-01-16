using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class PaymentLinkClient : BaseMollieClient, IPaymentLinkClient {
        public PaymentLinkClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) { }

        [ActivatorUtilitiesConstructor]
        public PaymentLinkClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager,
            HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<PaymentLinkResponse> CreatePaymentLinkAsync(
            PaymentLinkRequest paymentLinkRequest,
            CancellationToken cancellationToken = default) {
            if (!string.IsNullOrWhiteSpace(paymentLinkRequest.ProfileId) || paymentLinkRequest.Testmode.HasValue) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            return await PostAsync<PaymentLinkResponse>(
                $"payment-links",
                paymentLinkRequest,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaymentLinkResponse> UpdatePaymentLinkAsync(
            string paymentLinkId,
            PaymentLinkUpdateRequest paymentLinkUpdateRequest,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);

            if (paymentLinkUpdateRequest.Testmode.HasValue) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            string relativeUri = $"payment-links/{paymentLinkId}";
            return await PatchAsync<PaymentLinkResponse>(
                relativeUri,
                paymentLinkUpdateRequest,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task DeletePaymentLinkAsync(
            string paymentLinkId,
            string? profileId = null,
            bool testmode = false,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            var queryParameters = BuildQueryParameters(profileId);
            var data = CreateTestmodeModel(testmode);
            string relativeUri = $"payment-links/{paymentLinkId}{queryParameters.ToQueryString()}";
            await DeleteAsync(
                relativeUri,
                data,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaymentLinkResponse> GetPaymentLinkAsync(
            string paymentLinkId,
            bool testmode = false,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            if (testmode) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                testmode: testmode);

            return await GetAsync<PaymentLinkResponse>(
                $"payment-links/{paymentLinkId}{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaymentLinkResponse> GetPaymentLinkAsync(
            UrlObjectLink<PaymentLinkResponse> url,
            CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(
            UrlObjectLink<ListResponse<PaymentLinkResponse>> url,
            CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(
            string? from = null,
            int? limit = null,
            string? profileId = null,
            bool testmode = false,
            CancellationToken cancellationToken = default) {
            if (!string.IsNullOrWhiteSpace(profileId) || testmode) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                profileId: profileId,
                testmode: testmode);

            return await GetListAsync<ListResponse<PaymentLinkResponse>>(
                "payment-links",
                from,
                limit,
                queryParameters,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentLinkPaymentListAsync(
            string paymentLinkId,
            string? from = null,
            int? limit = null,
            bool testmode = false,
            SortDirection? sort = null,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            if (testmode) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                testmode: testmode,
                sort: sort);

            return await GetListAsync<ListResponse<PaymentResponse>>(
                $"payment-links/{paymentLinkId}/payments",
                from,
                limit,
                queryParameters,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
