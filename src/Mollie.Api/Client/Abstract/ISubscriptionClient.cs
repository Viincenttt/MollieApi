using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Subscription.Request;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ISubscriptionClient : IBaseMollieClient {
        /// <summary>
        /// Cancel an existing subscription. Canceling a subscription has no effect on the mandates of the customer.
        /// </summary>
        /// <param name="customerId">The customer ID of the customer to which the subscription belongs</param>
        /// <param name="subscriptionId">The subscription ID of the subscription to cancel</param>
        /// <param name="testmode">Indicates whether the subscription is in test mode or not</param>
        /// <param name="cancellationToken">Token to cancel the request. Cancelling the token might not cancel the subscription</param>
        Task CancelSubscriptionAsync(string customerId, string subscriptionId, bool testmode = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create a new subscription for a customer.
        /// </summary>
        /// <param name="customerId">The customer ID of the customer to which the subscription belongs</param>
        /// <param name="request">The subscription request object containing the subscription details</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>The subscription object created by Mollie</returns>
        Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, SubscriptionRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a single subscription by its ID and the ID of its parent customer.
        /// </summary>
        /// <param name="customerId">The customer ID of the customer to which the subscription belongs</param>
        /// <param name="subscriptionId">The subscription ID of the subscription to retrieve</param>
        /// <param name="testmode">Indicates whether the subscription is in test mode or not</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>The subscription object retrieved by Mollie</returns>
        Task<SubscriptionResponse> GetSubscriptionAsync(string customerId, string subscriptionId, bool testmode = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve all subscriptions of a customer.
        /// </summary>
        /// <param name="customerId">The customer ID of the customer to which the subscription belongs</param>
        /// <param name="from">The cursor to start pagination from</param>
        /// <param name="limit">The maximum number of subscriptions to return</param>
        /// <param name="profileId">The profile ID to filter subscriptions by</param>
        /// <param name="testmode">Indicates whether the subscription is in test mode or not</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>A list of paginated subscriptions</returns>
        Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(string customerId, string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(UrlObjectLink<ListResponse<SubscriptionResponse>> url, CancellationToken cancellationToken = default);
        /// <summary>
        /// Get all subscriptions
        /// </summary>
        /// <param name="from">The cursor to start pagination from</param>
        /// <param name="limit">The maximum number of subscriptions to return</param>
        /// <param name="profileId">The profile ID to filter subscriptions by</param>
        /// <param name="testmode">Indicates whether the subscription is in test mode or not</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>A list of paginated subscriptions</returns>
        Task<ListResponse<SubscriptionResponse>> GetAllSubscriptionList(string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<SubscriptionResponse> GetSubscriptionAsync(UrlObjectLink<SubscriptionResponse> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an existing subscription.
        /// </summary>
        /// <param name="customerId">The customer ID of the customer to which the subscription belongs</param>
        /// <param name="subscriptionId">The subscription ID of the subscription to update</param>
        /// <param name="request">The subscription update request</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>The updated subscription object</returns>
        /// <remarks>Canceled subscriptions cannot be updated</remarks>
        Task<SubscriptionResponse> UpdateSubscriptionAsync(string customerId, string subscriptionId, SubscriptionUpdateRequest request, CancellationToken cancellationToken = default);
        /// <summary>
        /// Retrieve all payments of a specific subscription.
        /// </summary>
        /// <param name="customerId">The customer ID of the customer to which the subscription belongs</param>
        /// <param name="subscriptionId">The subscription ID of the subscription to retrieve payments for</param>
        /// <param name="from">The cursor to start pagination from</param>
        /// <param name="limit">The maximum number of payments to return</param>
        /// <param name="testmode">Indicates whether the subscription is in test mode or not</param>
        /// <param name="cancellationToken">Token to cancel the request</param>
        /// <returns>A list of paginated payments</returns>
        Task<ListResponse<PaymentResponse>> GetSubscriptionPaymentListAsync(string customerId, string subscriptionId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
    }
}
