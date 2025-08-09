using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.WebhookEvent.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class WebhookEventClient : BaseMollieClient, IWebhookEventClient {
    public WebhookEventClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
    }

    [ActivatorUtilitiesConstructor]
    public WebhookEventClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient) {
    }

    public async Task<WebhookEventResponse> GetWebhookEventAsync(string webhookEventId, bool testmode = false,
        CancellationToken cancellationToken = default) {
        ValidateRequiredUrlParameter(nameof(webhookEventId), webhookEventId);
        var queryParameters = BuildQueryParameters(testmode);

        return await GetAsync<WebhookEventResponse>($"events/{webhookEventId}{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    private Dictionary<string, string> BuildQueryParameters(bool testmode = false) {
        var result = new Dictionary<string, string>();
        result.AddValueIfTrue("testmode", testmode);
        return result;
    }
}
