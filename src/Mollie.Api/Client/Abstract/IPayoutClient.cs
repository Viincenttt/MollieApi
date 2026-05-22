using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payout.Request;
using Mollie.Api.Models.Payout.Response;

namespace Mollie.Api.Client.Abstract;

public interface IPayoutClient : IBaseMollieClient {
    /// <summary>
    /// Request a payout from one of your balances to the balance's configured bank account.
    /// The payout will be executed on the next scheduled business day. If no amount is specified,
    /// the full available balance minus any configured balance reserve is paid out.
    /// </summary>
    Task<PayoutResponse> CreatePayoutAsync(PayoutRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieve a list of all payouts for your organization, including payouts initiated automatically
    /// by the balance's payout schedule and payouts requested via the API or dashboard.
    /// Only payouts created on or after April 1st, 2026 are returned.
    /// </summary>
    Task<ListResponse<PayoutResponse>> GetPayoutListAsync(
        string? balanceId = null, string? from = null, int? limit = null,
        SortDirection? sort = null, bool testmode = false, CancellationToken cancellationToken = default);
}


