using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Models.Webhook.Request;
using Mollie.Api.Models.Webhook.Response;

namespace Mollie.Api.Client.Abstract;

public interface IWebhookClient : IBaseMollieClient {
    Task<WebhookResponse> CreateWebhookAsync(WebhookRequest request, CancellationToken cancellationToken = default);

    Task<ListResponse<WebhookResponse>> GetWebhookListAsync(string? from = null, int? limit = null,
        bool testmode = false, CancellationToken cancellationToken = default);

    Task<ListResponse<WebhookResponse>> GetWebhookListAsync(
        UrlObjectLink<ListResponse<WebhookResponse>> url, CancellationToken cancellationToken = default);

    Task<WebhookResponse> GetWebhookAsync(string webhookId, bool testmode = false,
        CancellationToken cancellationToken = default);

    Task<WebhookResponse> UpdateWebhookAsync(string webhookId, WebhookRequest request, CancellationToken cancellationToken = default);

    Task DeleteWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default);

    Task TestWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default);
}
