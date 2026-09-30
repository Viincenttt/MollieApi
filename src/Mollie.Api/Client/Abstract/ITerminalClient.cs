using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Api.Models.UnreferencedRefund.Request;
using Mollie.Api.Models.UnreferencedRefund.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract
{    /// <summary>
     /// Calls in this class are documented in https://docs.mollie.com/reference/v2/terminals-api/overview
     /// </summary>
    public interface ITerminalClient : IBaseMollieClient {
        Task<TerminalResponse> GetTerminalAsync(string terminalId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<TerminalResponse> GetTerminalAsync(UrlObjectLink<TerminalResponse> url, CancellationToken cancellationToken = default);
        Task<ListResponse<TerminalResponse>> GetTerminalListAsync(string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<ListResponse<TerminalResponse>> GetTerminalListAsync(UrlObjectLink<ListResponse<TerminalResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Create an unreferenced refund on a terminal. An unreferenced refund is a refund that is not linked to a payment.
        /// The customer receives the refund by presenting their card on the terminal. Test mode is not supported.
        /// </summary>
        /// <param name="terminalId">The terminal ID, for example term_7MgL4wea46qkRcoTZjWEH.</param>
        /// <param name="request">The unreferenced refund request. ProfileId can only be set when using an OAuth access token.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The created unreferenced refund.</returns>
        Task<UnreferencedRefundResponse> CreateUnreferencedRefundAsync(string terminalId, UnreferencedRefundRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a single unreferenced refund of a terminal.
        /// </summary>
        /// <param name="terminalId">The terminal ID, for example term_7MgL4wea46qkRcoTZjWEH.</param>
        /// <param name="unreferencedRefundId">The unreferenced refund ID, for example unref_vytxeTZskVKR7C7WgdSP3d.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The unreferenced refund.</returns>
        Task<UnreferencedRefundResponse> GetUnreferencedRefundAsync(string terminalId, string unreferencedRefundId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a single unreferenced refund using a URL object, for example the self link of a response.
        /// </summary>
        /// <param name="url">The URL object of the unreferenced refund.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The unreferenced refund.</returns>
        Task<UnreferencedRefundResponse> GetUnreferencedRefundAsync(UrlObjectLink<UnreferencedRefundResponse> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve all unreferenced refunds of a terminal, ordered from newest to oldest.
        /// </summary>
        /// <param name="terminalId">The terminal ID, for example term_7MgL4wea46qkRcoTZjWEH.</param>
        /// <param name="from">Used for pagination. Offset the result set to the unreferenced refund with this ID. The unreferenced refund with this ID is included in the result set as well.</param>
        /// <param name="limit">The number of unreferenced refunds to return (with a maximum of 250).</param>
        /// <param name="profileId">Oauth only - The profile ID the unreferenced refunds belong to.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A list of unreferenced refunds.</returns>
        Task<ListResponse<UnreferencedRefundResponse>> GetUnreferencedRefundListAsync(string terminalId, string? from = null, int? limit = null, string? profileId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a list of unreferenced refunds using a URL object, for example the next or previous link of a list response.
        /// </summary>
        /// <param name="url">The URL object of the list.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A list of unreferenced refunds.</returns>
        Task<ListResponse<UnreferencedRefundResponse>> GetUnreferencedRefundListAsync(UrlObjectLink<ListResponse<UnreferencedRefundResponse>> url, CancellationToken cancellationToken = default);
     }
}
