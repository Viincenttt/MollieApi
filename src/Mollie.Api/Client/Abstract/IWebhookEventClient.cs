using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.WebhookEvent.Response;

namespace Mollie.Api.Client.Abstract;

public interface IWebhookEventClient : IBaseMollieClient {
    Task<MollieResult<FullWebhookEventResponse<T>>> GetWebhookEventAsync<T>(string webhookEventId, bool testmode = false,
        CancellationToken cancellationToken = default) where T : IEntity;

    Task<MollieResult<FullWebhookEventResponse>> GetWebhookEventAsync(string webhookEventId, bool testmode = false,
        CancellationToken cancellationToken = default);
}
