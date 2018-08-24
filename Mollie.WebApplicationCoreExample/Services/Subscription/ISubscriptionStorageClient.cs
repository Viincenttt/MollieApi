using System.Threading.Tasks;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Subscription {
    public interface ISubscriptionStorageClient {
        Task Create(CreateSubscriptionModel model);
    }
}