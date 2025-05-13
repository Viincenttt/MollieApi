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
using Mollie.Api.Models.Subscription.Request;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Options;

namespace Mollie.Api.Client {
    public class SubscriptionClient : BaseMollieClient, ISubscriptionClient {
        public SubscriptionClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
        }

        [ActivatorUtilitiesConstructor]
        public SubscriptionClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
            : base(options, mollieSecretManager, httpClient) {
        }

        public async Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(
            string customerId, string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<SubscriptionResponse>>(
                    $"customers/{customerId}/subscriptions", from, limit, queryParameters,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SubscriptionResponse>> GetAllSubscriptionList(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default) {
            var queryParameters = BuildQueryParameters(profileId, testmode);
            return await GetListAsync<ListResponse<SubscriptionResponse>>(
                    "subscriptions", from, limit, queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(
            UrlObjectLink<ListResponse<SubscriptionResponse>> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> GetSubscriptionAsync(
            UrlObjectLink<SubscriptionResponse> url, CancellationToken cancellationToken = default) {
            return await GetAsync(url, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> GetSubscriptionAsync(
            string customerId, string subscriptionId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            ValidateRequiredUrlParameter(nameof(subscriptionId), subscriptionId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetAsync<SubscriptionResponse>(
                    $"customers/{customerId}/subscriptions/{subscriptionId}{queryParameters.ToQueryString()}",
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> CreateSubscriptionAsync(
            string customerId, SubscriptionRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            return await PostAsync<SubscriptionResponse>(
                    $"customers/{customerId}/subscriptions", request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task CancelSubscriptionAsync(
            string customerId, string subscriptionId, bool testmode = false, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            ValidateRequiredUrlParameter(nameof(subscriptionId), subscriptionId);
            var data = TestmodeModel.Create(testmode);
            await DeleteAsync(
                $"customers/{customerId}/subscriptions/{subscriptionId}", data,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<SubscriptionResponse> UpdateSubscriptionAsync(
            string customerId, string subscriptionId, SubscriptionUpdateRequest request, CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            ValidateRequiredUrlParameter(nameof(subscriptionId), subscriptionId);
            return await PatchAsync<SubscriptionResponse>(
                $"customers/{customerId}/subscriptions/{subscriptionId}", request,
                cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetSubscriptionPaymentListAsync(
            string customerId, string subscriptionId, string? from = null, int? limit = null, bool testmode = false,
            CancellationToken cancellationToken = default) {
            ValidateRequiredUrlParameter(nameof(customerId), customerId);
            ValidateRequiredUrlParameter(nameof(subscriptionId), subscriptionId);
            var queryParameters = BuildQueryParameters(testmode);
            return await GetListAsync<ListResponse<PaymentResponse>>(
                    $"customers/{customerId}/subscriptions/{subscriptionId}/payments", from, limit,
                    queryParameters, cancellationToken: cancellationToken)
                .ConfigureAwait(false);
        }

        private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
            var result = new Dictionary<string, string>();
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }

        private Dictionary<string, string> BuildQueryParameters(string? profileId, bool testmode) {
            var result = new Dictionary<string, string>();
            result.AddValueIfNotNullOrEmpty("profileId", profileId);
            result.AddValueIfTrue("testmode", testmode);
            return result;
        }
    }
}
