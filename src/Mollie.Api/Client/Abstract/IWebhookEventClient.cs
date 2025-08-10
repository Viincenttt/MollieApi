using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.WebhookEvent.Response;

namespace Mollie.Api.Client.Abstract;

public interface IWebhookEventClient {
    Task<SimpleWebhookEventResponse> GetWebhookEventAsync(string webhookId, bool testmode = false,
        CancellationToken cancellationToken = default);
}
