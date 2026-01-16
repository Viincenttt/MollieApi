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
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class PaymentClient : BaseMollieClient, IPaymentClient {

	    public PaymentClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) { }

        [ActivatorUtilitiesConstructor]
        public PaymentClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<PaymentResponse> CreatePaymentAsync(
            PaymentRequest paymentRequest,
            bool includeQrCode = false,
            CancellationToken cancellationToken = default) {
            if (!string.IsNullOrWhiteSpace(paymentRequest.ProfileId) || paymentRequest.Testmode.HasValue || paymentRequest.ApplicationFee != null) {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                includeQrCode: includeQrCode);

            return await PostAsync<PaymentResponse>(
                $"payments{queryParameters.ToQueryString()}",
                paymentRequest,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaymentResponse> GetPaymentAsync(
            string paymentId,
            bool testmode = false,
            bool includeQrCode = false,
            bool includeRemainderDetails = false,
            bool embedRefunds = false,
            bool embedChargebacks = false,
            CancellationToken cancellationToken = default) {

	        if (testmode) {
	            ValidateApiKeyIsOauthAccesstoken();
            }

            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            var queryParameters = BuildQueryParameters(
                testmode: testmode,
                includeQrCode: includeQrCode,
                includeRemainderDetails: includeRemainderDetails,
                embedRefunds: embedRefunds,
                embedChargebacks: embedChargebacks
            );
			return await GetAsync<PaymentResponse>(
                $"payments/{paymentId}{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken).ConfigureAwait(false);
		}

		public async Task CancelPaymentAsync(
            string paymentId,
            bool testmode = false,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            var data = CreateTestmodeModel(testmode);
		    await DeleteAsync(
                $"payments/{paymentId}",
                data,
                cancellationToken: cancellationToken).ConfigureAwait(false);
		}

        public async Task ReleasePaymentAuthorization(
            string paymentId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            var queryParameters = BuildQueryParameters(testmode: testmode);
            await PostAsync<object>(
                $"payments/{paymentId}/release-authorization{queryParameters.ToQueryString()}",
                null,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<PaymentResponse> GetPaymentAsync(
            UrlObjectLink<PaymentResponse> url,
            CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(
            UrlObjectLink<ListResponse<PaymentResponse>> url,
            CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(
            string? from = null,
            int? limit = null,
            string? profileId = null,
            bool testmode = false,
            bool includeQrCode = false,
            bool embedRefunds = false,
            bool embedChargebacks = false,
            SortDirection? sort = null,
            CancellationToken cancellationToken = default) {

	        if (!string.IsNullOrWhiteSpace(profileId) || testmode) {
	            ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                profileId: profileId,
                testmode: testmode,
                includeQrCode: includeQrCode,
                embedRefunds: embedRefunds,
                embedChargebacks: embedChargebacks,
                sort: sort);

            return await GetListAsync<ListResponse<PaymentResponse>>(
                $"payments",
                from,
                limit,
                queryParameters,
                cancellationToken: cancellationToken).ConfigureAwait(false);
		}

        public async Task<PaymentResponse> UpdatePaymentAsync(
            string paymentId,
            PaymentUpdateRequest paymentUpdateRequest,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

            return await PatchAsync<PaymentResponse>(
                $"payments/{paymentId}",
                paymentUpdateRequest,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(
            string? profileId = null,
            bool testmode = false,
            bool includeQrCode = false,
            bool includeRemainderDetails = false,
            bool embedRefunds = false,
            bool embedChargebacks = false,
            SortDirection? sort = null) {

            var result = base.BuildQueryParameters(profileId, testmode);
            result.AddValueIfNotNullOrEmpty("include", BuildIncludeParameter(includeQrCode, includeRemainderDetails));
            result.AddValueIfNotNullOrEmpty("embed", BuildEmbedParameter(embedRefunds, embedChargebacks));
            result.AddValueIfNotNullOrEmpty(nameof(sort), sort?.ToString()?.ToLowerInvariant());
            return result;
        }

        private string BuildIncludeParameter(bool includeQrCode = false, bool includeRemainderDetails = false) {
            var includeList = new List<string>();
            includeList.AddValueIfTrue("details.qrCode", includeQrCode);
            includeList.AddValueIfTrue("details.remainderDetails", includeRemainderDetails);
            return includeList.ToIncludeParameter();
        }

        private string BuildEmbedParameter(bool embedRefunds = false, bool embedChargebacks = false)
        {
            var embedList = new List<string>();
            embedList.AddValueIfTrue("refunds", embedRefunds);
            embedList.AddValueIfTrue("chargebacks", embedChargebacks);
            return embedList.ToIncludeParameter();
        }
    }
}
