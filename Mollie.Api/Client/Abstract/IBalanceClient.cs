using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract {
    public interface IBalanceClient {
        Task<BalanceResponse> GetBalanceAsync(string balanceId);
        Task<BalanceResponse> GetPrimaryBalanceAsync();
        Task<ListResponse<BalanceResponse>> ListBalancesAsync(string from = null, int? limit = null, string currency = null);
        Task<BalanceReportResponse> GetBalanceReportAsync(string balanceId, DateTime from, DateTime until, string grouping = null);
        Task<BalanceReportResponse> GetPrimaryBalanceReportAsync(DateTime from, DateTime until, string grouping = null);
    }
}