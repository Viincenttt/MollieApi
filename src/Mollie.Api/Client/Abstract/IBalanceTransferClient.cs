using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.BalanceTransfer.Request;
using Mollie.Api.Models.BalanceTransfer.Response;
using Mollie.Api.Models.List.Response;

namespace Mollie.Api.Client.Abstract;

public interface IBalanceTransferClient {
    /// <summary>
    /// This API endpoint allows you to create a balance transfer from your organization's balance to a connected
    /// organization's balance, or vice versa. You can also create a balance transfer between two connected
    /// organizations. To create a balance transfer, you must be authenticated as the source organization, and the
    /// destination organization must be a connected organization that has authorized the balance-transfers.write
    /// scope for your organization.
    /// </summary>
    Task<BalanceTransferResponse> CreateBalanceTransferAsync(
        BalanceTransferRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paginated list of balance transfers associated with your organization. These may be a balance transfer
    /// that was received or sent from your balance, or a balance transfer that you initiated on behalf of your clients.
    /// If no balance transfers are available, the resulting array will be empty. This request should never throw an error.
    /// </summary>
    Task<ListResponse<BalanceTransferResponse>> GetBalanceTransferListAsync(
        string? from = null, int? limit = null, SortDirection? sort = null, bool testmode = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a single Connect balance transfer object by its ID.
    /// </summary>
    Task<BalanceTransferResponse> GetBalanceTransferAsync(
        string balanceTransferId, bool testmode = false, CancellationToken cancellationToken = default);
}
