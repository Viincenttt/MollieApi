using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;
using Mollie.Api.Models.Webhook.Request;
using Mollie.Api.Models.Webhook.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class WebhookClient : BaseMollieClient, IWebhookClient {
    public WebhookClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
    }

    [ActivatorUtilitiesConstructor]
    public WebhookClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient) {
    }

    public async Task<WebhookResponse> CreateWebhookAsync(WebhookRequest request, CancellationToken cancellationToken = default) {
        return await PostAsync<WebhookResponse>("webhooks", request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ListResponse<WebhookResponse>> GetWebhookListAsync(string? from = null, int? limit = null,
        bool testmode = false, CancellationToken cancellationToken = default) {
        var queryParameters = BuildQueryParameters(testmode: testmode);
        return await GetListAsync<ListResponse<WebhookResponse>>(
                "webhooks", from, limit, queryParameters, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<ListResponse<WebhookResponse>> GetWebhookListAsync(
        UrlObjectLink<ListResponse<WebhookResponse>> url, CancellationToken cancellationToken = default) {
        return await GetAsync(url, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<WebhookResponse> GetWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(webhookId), webhookId);
        var queryParameters = BuildQueryParameters(testmode: testmode);

        return await GetAsync<WebhookResponse>($"webhooks/{webhookId}{queryParameters.ToQueryString()}", cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<WebhookResponse> UpdateWebhookAsync(string webhookId, WebhookRequest request, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(webhookId), webhookId);

        return await PatchAsync<WebhookResponse>($"webhooks/{webhookId}", request, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task DeleteWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(webhookId), webhookId);
        var data = CreateTestmodeModel(testmode);
        await DeleteAsync($"webhooks/{webhookId}", data, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task TestWebhookAsync(string webhookId, bool testmode = false, CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(webhookId), webhookId);
        var queryParameters = BuildQueryParameters(testmode: testmode);

        await PostAsync<object>($"webhooks/{webhookId}/ping{queryParameters.ToQueryString()}", null,
            cancellationToken: cancellationToken);
    }
}
