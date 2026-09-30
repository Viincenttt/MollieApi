using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Models.Webhook.Request;
using Mollie.Api.Models.Webhook.Response;

namespace Mollie.Api.Client.Abstract;

public interface IWebhookClient : IBaseMollieClient {
    Task<MollieResult<WebhookResponse>> CreateWebhookAsync(WebhookRequest request, CancellationToken cancellationToken = default);

    Task<MollieResult<ListResponse<WebhookResponse>>> GetWebhookListAsync(string? from = null, int? limit = null,
        bool testmode = false, CancellationToken cancellationToken = default);

    Task<MollieResult<ListResponse<WebhookResponse>>> GetWebhookListAsync(
        UrlObjectLink<ListResponse<WebhookResponse>> url, CancellationToken cancellationToken = default);

    Task<MollieResult<WebhookResponse>> GetWebhookAsync(string webhookId, bool testmode = false,
        CancellationToken cancellationToken = default);

    Task<MollieResult<WebhookResponse>> UpdateWebhookAsync(string webhookId, WebhookRequest request, CancellationToken cancellationToken = default);

    Task<MollieResult> DeleteWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default);

    Task<MollieResult> TestWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default);
}
