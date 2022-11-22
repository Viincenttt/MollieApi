using System.Threading.Tasks;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract {
    public interface IBalanceClient {
        Task<BalanceResponse> GetBalanceAsync(string balanceId);
        Task<BalanceResponse> GetPrimaryBalanceAsync();
        Task<ListResponse<BalanceResponse>> ListBalancesAsync(string from = null, int? limit = null, string currency = null);
    }
}