using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Subscription;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client {
    public class SubscriptionClient : BaseMollieClient, ISubscriptionClient {
        public SubscriptionClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }

        public async Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(string customerId, string from = null, int? limit = null, string profileId = null, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(profileId, testmode);
            return await this.GetListAsync<ListResponse<SubscriptionResponse>>($"customers/{customerId}/subscriptions", from, limit, queryParameters)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SubscriptionResponse>> GetAllSubscriptionList(string from = null, int? limit = null, string profileId = null, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(profileId, testmode);
            return await this.GetListAsync<ListResponse<SubscriptionResponse>>($"subscriptions", from, limit, queryParameters)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(UrlObjectLink<ListResponse<SubscriptionResponse>> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> GetSubscriptionAsync(UrlObjectLink<SubscriptionResponse> url) {
            return await this.GetAsync(url).ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> GetSubscriptionAsync(string customerId, string subscriptionId, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(testmode);
            return await this.GetAsync<SubscriptionResponse>($"customers/{customerId}/subscriptions/{subscriptionId}{queryParameters.ToQueryString()}")
                .ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, SubscriptionRequest request) {
            return await this.PostAsync<SubscriptionResponse>($"customers/{customerId}/subscriptions", request)
                .ConfigureAwait(false);
        }

        public async Task CancelSubscriptionAsync(string customerId, string subscriptionId, bool testmode = false) {
            var data = TestmodeModel.Create(testmode);
            await this.DeleteAsync($"customers/{customerId}/subscriptions/{subscriptionId}", data).ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> UpdateSubscriptionAsync(string customerId, string subscriptionId, SubscriptionUpdateRequest request) {
            return await this.PatchAsync<SubscriptionResponse>($"customers/{customerId}/subscriptions/{subscriptionId}", request).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSubscriptionPaymentListAsync(string customerId, string subscriptionId, string from = null, int? limit = null, bool testmode = false) {
            var queryParameters = this.BuildQueryParameters(testmode);
            return await this.GetListAsync<ListResponse<PaymentResponse>>($"customers/{customerId}/subscriptions/{subscriptionId}/payments", from, limit, queryParameters)
                .ConfigureAwait(false);
        }
        
        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
        
        private Dictionary<string, string> BuildQueryParameters(string profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}