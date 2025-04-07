using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Subscription.Request;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface ISubscriptionClient : IBaseMollieClient {
        Task CancelSubscriptionAsync(string customerId, string subscriptionId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, SubscriptionRequest request, CancellationToken cancellationToken = default);
        Task<SubscriptionResponse> GetSubscriptionAsync(string customerId, string subscriptionId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(string customerId, string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(UrlObjectLink<ListResponse<SubscriptionResponse>> url, CancellationToken cancellationToken = default);
        Task<ListResponse<SubscriptionResponse>> GetAllSubscriptionList(string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<SubscriptionResponse> GetSubscriptionAsync(UrlObjectLink<SubscriptionResponse> url, CancellationToken cancellationToken = default);
        Task<SubscriptionResponse> UpdateSubscriptionAsync(string customerId, string subscriptionId, SubscriptionUpdateRequest request, CancellationToken cancellationToken = default);
        Task<ListResponse<PaymentResponse>> GetSubscriptionPaymentListAsync(string customerId, string subscriptionId, string? from = null, int? limit = null, bool testmode = false, CancellationToken cancellationToken = default);
    }
}
