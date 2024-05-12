using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Response;
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

        public async Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(string? from = null, int? limit = null, string? profileId = null, bool testmode = false)
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

        private Dictionary<string, string> BuildQueryParameters(string? profileId = null, bool testmode = false)
        {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue(nameof(testmode), testmode);
            result.AddValueIfNotNullOrEmpty(nameof(profileId), profileId);
            return result;
        }
    }
}
