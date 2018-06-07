using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Subscription;

namespace Mollie.Api.Client.Abstract {
    public interface ISubscriptionClient {
        Task CancelSubscriptionAsync(string customerId, string subscriptionId);
        Task<SubscriptionResponse> CreateSubscriptionAsync(string customerId, SubscriptionRequest request);
        Task<SubscriptionResponse> GetSubscriptionAsync(string customerId, string subscriptionId);
        Task<ListResponse<SubscriptionResponse>> GetSubscriptionListAsync(string customerId, string from = null, int? limit = null);
    }
}