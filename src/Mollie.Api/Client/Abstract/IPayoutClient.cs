using System.Threading;
using System.Threading.Tasks;
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
}


