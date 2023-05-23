using Mollie.Api.Models.Subscription;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Subscription; 

public interface ISubscriptionOverviewClient {
    Task<OverviewModel<SubscriptionResponse>> GetList(string customerId);
    Task<OverviewModel<SubscriptionResponse>> GetListByUrl(string url);
}