using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client
{
    public class PaymentLinkClient : BaseMollieClient, IPaymentLinkClient
    {
        public PaymentLinkClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) { }

        public async Task<PaymentLinkResponse> CreatePaymentLinkAsync(PaymentLinkRequest paymentLinkRequest)
        {
            if (!string.IsNullOrWhiteSpace(paymentLinkRequest.ProfileId) || paymentLinkRequest.Testmode.HasValue)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }
            return await PostAsync<PaymentLinkResponse>($"payment-links", paymentLinkRequest).ConfigureAwait(false);
        }

        public async Task<PaymentLinkResponse> UpdatePaymentLinkAsync(
            string paymentLinkId,
            PaymentLinkUpdateRequest paymentLinkUpdateRequest,
            bool testmode = false) {

            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            var queryParameters = BuildQueryParameters(testmode);
            string relativeUri = $"payment-links/{paymentLinkId}{queryParameters.ToQueryString()}";
            return await PatchAsync<PaymentLinkResponse>(relativeUri, paymentLinkUpdateRequest).ConfigureAwait(false);
        }

        public async Task DeletePaymentLinkAsync(string paymentLinkId, bool testmode = false) {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            var queryParameters = BuildQueryParameters(testmode);
            string relativeUri = $"payment-links/{paymentLinkId}{queryParameters.ToQueryString()}";
            await DeleteAsync(relativeUri).ConfigureAwait(false);
        }

        public async Task<PaymentLinkResponse> GetPaymentLinkAsync(string paymentLinkId, bool testmode = false)
        {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            if (testmode)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                testmode: testmode);

            return await GetAsync<PaymentLinkResponse>($"payment-links/{paymentLinkId}{queryParameters.ToQueryString()}").ConfigureAwait(false);
        }

        public async Task<PaymentLinkResponse> GetPaymentLinkAsync(UrlObjectLink<PaymentLinkResponse> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(UrlObjectLink<ListResponse<PaymentLinkResponse>> url)
        {
            return await GetAsync(url).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false)
        {
            if (!string.IsNullOrWhiteSpace(profileId) || testmode)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
               profileId: profileId,
               testmode: testmode);

            return await GetListAsync<ListResponse<PaymentLinkResponse>>("payment-links", from, limit, queryParameters).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentLinkPaymentListAsync(
            string paymentLinkId, string? from = null, int? limit = null, bool testmode = false, SortDirection? sort = null)
        {
            ValidateRequiredUrlParameter(nameof(paymentLinkId), paymentLinkId);
            if (testmode)
            {
                ValidateApiKeyIsOauthAccesstoken();
            }

            var queryParameters = BuildQueryParameters(
                testmode: testmode,
                sort: sort);

            return await GetListAsync<ListResponse<PaymentResponse>>($"payment-links/{paymentLinkId}/payments", from, limit, queryParameters).ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }

        private Dictionary<string, string> BuildQueryParameters(string? profileId = null, bool testmode = false, SortDirection? sort = null)
        {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode);
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            result.AddValueIfNotNullOrEmpty(nameof(sort), sort?.ToString()?.ToLowerInvariant());
            return result;
        }
    }
}
