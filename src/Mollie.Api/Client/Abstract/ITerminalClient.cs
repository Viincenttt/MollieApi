using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Terminal.Request;
using Mollie.Api.Models.Terminal.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract
{    /// <summary>
     /// Calls in this class are documented in https://docs.mollie.com/reference/v2/terminals-api/overview
     /// </summary>
    public interface ITerminalClient : IBaseMollieClient {
        Task<MollieResult<TerminalResponse>> GetTerminalAsync(string terminalId, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<TerminalResponse>> GetTerminalAsync(UrlObjectLink<TerminalResponse> url, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<TerminalResponse>>> GetTerminalListAsync(string? from = null, int? limit = null, string? profileId = null, bool testmode = false, CancellationToken cancellationToken = default);
        Task<MollieResult<ListResponse<TerminalResponse>>> GetTerminalListAsync(UrlObjectLink<ListResponse<TerminalResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Request a pairing code to onboard a point-of-sale terminal. Pairing codes expire after 90 days and can be used multiple times.
        /// This endpoint does not support test mode.
        /// </summary>
        /// <param name="request">The request containing the ID of the profile to pair the terminal with.</param>
        /// <param name="includeQrCode">Include a QR code of the pairing code in the response.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The requested terminal pairing code.</returns>
        Task<MollieResult<TerminalPairingCodeResponse>> CreateTerminalPairingCodeAsync(TerminalPairingCodeRequest request, bool includeQrCode = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve all terminal pairing codes, including active, expired and revoked codes. This endpoint does not support test mode.
        /// </summary>
        /// <param name="from">Used for pagination. Offset the result set to the pairing code with this ID.</param>
        /// <param name="limit">The number of pairing codes to return (with a maximum of 250).</param>
        /// <param name="profileId">The ID of the profile you wish to retrieve pairing codes for.</param>
        /// <param name="sort">Used for setting the direction of the result set. Defaults to descending order.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A list of terminal pairing codes.</returns>
        Task<MollieResult<ListResponse<TerminalPairingCodeResponse>>> GetTerminalPairingCodeListAsync(string? from = null, int? limit = null, string? profileId = null, SortDirection? sort = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a list of terminal pairing codes using a URL object, for example the next or previous page of a list response.
        /// </summary>
        /// <param name="url">The URL object of the list to retrieve.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>A list of terminal pairing codes.</returns>
        Task<MollieResult<ListResponse<TerminalPairingCodeResponse>>> GetTerminalPairingCodeListAsync(UrlObjectLink<ListResponse<TerminalPairingCodeResponse>> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a single terminal pairing code by its ID. This endpoint does not support test mode.
        /// </summary>
        /// <param name="pairingCodeId">The ID of the terminal pairing code.</param>
        /// <param name="includeQrCode">Include a QR code of the pairing code in the response.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The terminal pairing code.</returns>
        Task<MollieResult<TerminalPairingCodeResponse>> GetTerminalPairingCodeAsync(string pairingCodeId, bool includeQrCode = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieve a single terminal pairing code using a URL object.
        /// </summary>
        /// <param name="url">The URL object of the pairing code to retrieve.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The terminal pairing code.</returns>
        Task<MollieResult<TerminalPairingCodeResponse>> GetTerminalPairingCodeAsync(UrlObjectLink<TerminalPairingCodeResponse> url, CancellationToken cancellationToken = default);

        /// <summary>
        /// Revoke a terminal pairing code, preventing the onboarding of new point-of-sale terminals. Terminals that have already
        /// paired with this code are not affected. This endpoint does not support test mode.
        /// </summary>
        /// <param name="pairingCodeId">The ID of the terminal pairing code to revoke.</param>
        /// <param name="cancellationToken">Token to cancel the request.</param>
        /// <returns>The revoked terminal pairing code.</returns>
        Task<MollieResult<TerminalPairingCodeResponse>> RevokeTerminalPairingCodeAsync(string pairingCodeId, CancellationToken cancellationToken = default);
     }
}
