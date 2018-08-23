using System.Threading.Tasks;
using Mollie.Api.Models.Subscription;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Subscription {
    public interface ISubscriptionOverviewClient {
        Task<OverviewModel<SubscriptionResponse>> GetList(string customerId);
        Task<OverviewModel<SubscriptionResponse>> GetListByUrl(string url);
    }
}