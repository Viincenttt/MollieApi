using System.Threading.Tasks;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract {
    public interface IBalanceClient {
        Task<BalanceResponse> GetBalance(string balanceId);
        Task<BalanceResponse> GetPrimaryBalance();
        Task<ListResponse<BalanceResponse>> ListBalances(string from = null, int? limit = null, string currency = null);
    }
}