using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.DelayedRouting.Request;
using Mollie.Api.Models.DelayedRouting.Response;
using Mollie.Api.Models.List.Response;

namespace Mollie.Api.Client.Abstract;

public interface IDelayedRoutingClient : IBaseMollieClient {
    /// <summary>
    /// Creates a route for a specific payment. The routed amount is credited to the account of your customer.
    /// </summary>
    /// <param name="paymentId">The ID of the related payment.</param>
    /// <param name="request">The payload to create a new delayed route for a payment.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The created route object.</returns>
    Task<DelayedRoutingResponse> CreateDelayedRouteAsync(
        string paymentId,
        DelayedRoutingRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single route created for a specific payment.
    /// </summary>
    /// <param name="paymentId">The ID of the related payment.</param>
    /// <param name="routeId">The ID of the route.</param>
    /// <param name="testmode">Set to true to retrieve a test mode route. Only available for OAuth access tokens.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The route object.</returns>
    Task<DelayedRoutingResponse> GetDelayedRouteAsync(
        string paymentId,
        string routeId,
        bool testmode = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of all routes created for a specific payment.
    /// </summary>
    /// <param name="paymentId">The ID of the related payment.</param>
    /// <param name="testmode">Set to true to retrieve test mode routes. Only available for OAuth access tokens.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of route objects.</returns>
    Task<ListResponse<DelayedRoutingResponse>> GetPaymentRouteListAsync(
        string paymentId,
        bool testmode = false,
        CancellationToken cancellationToken = default);
}
