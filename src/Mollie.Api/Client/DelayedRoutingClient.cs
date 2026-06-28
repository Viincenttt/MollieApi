using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.DelayedRouting.Request;
using Mollie.Api.Models.DelayedRouting.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Options;

namespace Mollie.Api.Client;

public class DelayedRoutingClient : BaseMollieClient, IDelayedRoutingClient {
    public DelayedRoutingClient(string apiKey, HttpClient? httpClient = null) : base(apiKey, httpClient) {
    }

    [ActivatorUtilitiesConstructor]
    public DelayedRoutingClient(MollieClientOptions options, IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(options, mollieSecretManager, httpClient) {
    }

    /// <inheritdoc />
    public async Task<DelayedRoutingResponse> CreateDelayedRouteAsync(
        string paymentId, DelayedRoutingRequest request, CancellationToken cancellationToken = default) {

        ValidateRequiredUrlParameter(nameof(paymentId), paymentId);

        return await PostAsync<DelayedRoutingResponse>(
                $"payments/{paymentId}/routes", request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<DelayedRoutingResponse> GetDelayedRouteAsync(
        string paymentId,
        string routeId,
        bool testmode = false,
        CancellationToken cancellationToken = default) {

        ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
        ValidateRequiredUrlParameter(nameof(routeId), routeId);
        var queryParameters = BuildQueryParameters(testmode: testmode);
        return await GetAsync<DelayedRoutingResponse>(
                $"payments/{paymentId}/routes/{routeId}{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ListResponse<DelayedRoutingResponse>> GetPaymentRouteListAsync(
        string paymentId,
        bool testmode = false,
        CancellationToken cancellationToken = default) {

        ValidateRequiredUrlParameter(nameof(paymentId), paymentId);
        var queryParameters = BuildQueryParameters(testmode: testmode);
        return await GetAsync<ListResponse<DelayedRoutingResponse>>(
                $"payments/{paymentId}/routes{queryParameters.ToQueryString()}",
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}
