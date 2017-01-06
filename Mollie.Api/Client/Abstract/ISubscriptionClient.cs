using System.Threading.Tasks;

namespace Mollie.Api.Client.Abstract {
    using Models.Subscription;
    using Models.List;
    public interface ISubscriptionClient {
        Task CancelSubscriptionAsync(string customerId, string subscriptionId);
        Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, SubscriptionRequest request);
        Task<SubscriptionResponse> GetSubscriptionAsync(string customerId, string subscriptionId);
        Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(string customerId, int? offset = default(int?), int? count = default(int?));
    }
}
