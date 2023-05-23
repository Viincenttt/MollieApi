using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Subscription; 

public interface ISubscriptionStorageClient {
    Task Create(CreateSubscriptionModel model);
}