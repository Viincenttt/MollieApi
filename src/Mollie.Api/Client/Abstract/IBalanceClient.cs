using System;
using System.Threading.Tasks;
using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IBalanceClient : IBaseMollieClient {
        /// <summary>
        /// Retrieve a single balance object by its balance identifier.
        /// </summary>
        /// <param name="balanceId">The balance identifier to retrieve</param>
        /// <returns></returns>
        Task<BalanceResponse> GetBalanceAsync(string balanceId);

        /// <summary>
        /// Retrieve a single balance object using an URL
        /// </summary>
        /// <param name="url">The URL of the balance object</param>
        /// <returns></returns>
        Task<BalanceResponse> GetBalanceAsync(UrlObjectLink<BalanceResponse> url);

        /// <summary>
        /// Retrieve the primary balance. This is the balance of your account’s primary currency, where all payments are
        /// settled to by default.
        /// </summary>
        /// <returns></returns>
        Task<BalanceResponse> GetPrimaryBalanceAsync();

        /// <summary>
        /// Retrieve all the organization’s balances, including the primary balance, ordered from newest to oldest.
        /// </summary>
        /// <param name="from">Offset the result set to the balance with this ID. The balance with this ID is included
        /// in the result set as well.</param>
        /// <param name="limit">The number of balances to return (with a maximum of 250).</param>
        /// <param name="currency">Currency filter that will make it so only balances in given currency are returned.
        /// For example EUR.</param>
        /// <returns></returns>
        Task<ListResponse<BalanceResponse>> GetBalanceListAsync(string? from = null, int? limit = null, string? currency = null);

        /// <summary>
        /// Retrieve all the organization’s balances by URL
        /// </summary>
        /// <param name="url">The URL of the balance objects</param>
        /// <returns></returns>
        Task<ListResponse<BalanceResponse>> GetBalanceListAsync(UrlObjectLink<ListResponse<BalanceResponse>> url);

        /// <summary>
        /// With the Get balance report endpoint you can retrieve a summarized report for all movements on a given
        /// balance within a given timeframe.
        /// </summary>
        /// <param name="balanceId">The balance id for which to retrieve a report</param>
        /// <param name="from">he start date of the report, in YYYY-MM-DD format. The from date is ‘inclusive’, and in
        /// Central European Time. This means a report with for example from: 2020-01-01 will include movements of
        /// 2020-01-01 0:00:00 CET and onwards.</param>
        /// <param name="until">The end date of the report, in YYYY-MM-DD format. The until date is ‘exclusive’, and
        /// in Central European Time. This means a report with for example until: 2020-02-01 will include movements up
        /// until 2020-01-31 23:59:59 CET.</param>
        /// <param name="grouping">You can retrieve reports in two different formats: status-balances and
        /// transaction-categories</param>
        /// <returns></returns>
        Task<BalanceReportResponse> GetBalanceReportAsync(string balanceId, DateTime from, DateTime until, string? grouping = null);

        /// <summary>
        /// With the Get primary balance report endpoint you can retrieve a summarized report for all movements on your
        /// primary balance within a given timeframe.
        /// </summary>
        /// <param name="from">The start date of the report, in YYYY-MM-DD format. The from date is ‘inclusive’, and in
        /// Central European Time. This means a report with for example from: 2020-01-01 will include movements of
        /// 2020-01-01 0:00:00 CET and onwards.</param>
        /// <param name="until">The end date of the report, in YYYY-MM-DD format. The until date is ‘exclusive’, and in
        /// Central European Time. This means a report with for example until: 2020-02-01 will include movements up
        /// until 2020-01-31 23:59:59 CET.</param>
        /// <param name="grouping">You can retrieve reports in two different formats: status-balances and
        /// transaction-categories</param>
        /// <returns></returns>
        Task<BalanceReportResponse> GetPrimaryBalanceReportAsync(DateTime from, DateTime until, string? grouping = null);

        /// <summary>
        /// With the List balance transactions endpoint you can retrieve a list of all the movements on your balance.
        /// This includes payments, refunds, chargebacks, and settlements.
        /// </summary>
        /// <param name="balanceId">The balance id for which to retrieve a report</param>
        /// <param name="from">Offset the result set to the balance transactions with this ID. The balance transaction
        /// with this ID is included in the result set as well.</param>
        /// <param name="limit">The number of balance transactions to return (with a maximum of 250).</param>
        /// <returns></returns>
        Task<ListResponse<BalanceTransactionResponse>> GetBalanceTransactionListAsync(string balanceId, string? from = null, int? limit = null);

        /// <summary>
        /// With the List primary balance transactions endpoint you can retrieve a list of all the movements on your
        /// primary balance. This includes payments, refunds, chargebacks, and settlements.
        /// </summary>
        /// <param name="from">Offset the result set to the balance transactions with this ID. The balance transaction
        /// with this ID is included in the result set as well.</param>
        /// <param name="limit">The number of balance transactions to return (with a maximum of 250).</param>
        /// <returns></returns>
        Task<ListResponse<BalanceTransactionResponse>> GetPrimaryBalanceTransactionListAsync(string? from = null, int? limit = null);
    }
}
