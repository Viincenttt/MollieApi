using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IBalanceClient : IBaseMollieClient {
        Task<BalanceResponse> GetBalanceAsync(string balanceId);
        Task<BalanceResponse> GetBalanceAsync(UrlObjectLink<BalanceResponse> url);
        Task<BalanceResponse> GetPrimaryBalanceAsync();
        Task<ListResponse<BalanceResponse>> ListBalancesAsync(string? from = null, int? limit = null, string? currency = null);
        Task<ListResponse<BalanceResponse>> ListBalancesAsync(UrlObjectLink<ListResponse<BalanceResponse>> url);
        Task<BalanceReportResponse> GetBalanceReportAsync(string balanceId, DateTime from, DateTime until, string? grouping = null);
        Task<BalanceReportResponse> GetPrimaryBalanceReportAsync(DateTime from, DateTime until, string? grouping = null);
        Task<BalanceTransactionResponse> ListBalanceTransactionsAsync(string balanceId, string? from = null, int? limit = null);
        Task<BalanceTransactionResponse> ListPrimaryBalanceTransactionsAsync(string? from = null, int? limit = null);
    }
}