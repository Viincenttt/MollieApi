using System.Threading.Tasks;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Refund;
using Mollie.Api.Models.Settlement;

namespace Mollie.Api.Client.Abstract {
    public interface ISettlementsClient {
        /// <summary>
        ///     Get settlement by Id
        /// </summary>
        /// <param name="settlementId">The id of the settlement</param>
        /// <returns>The settlement</returns>
        Task<SettlementResponse> GetSettlementAsync(string settlementId);

        /// <summary>
        ///     Retrieve the details of the current settlement that has not yet been paid out.
        /// </summary>
        /// <returns></returns>
        Task<SettlementResponse> GetNextSettlement();

        /// <summary>
        ///     Retrieve the details of the open balance of the organization. This will return a settlement object representing
        ///     your organization's balance.
        /// </summary>
        /// <returns></returns>
        Task<SettlementResponse> GetOpenBalance();

        /// <summary>
        ///     Retrieve all settlements, ordered from new to old.
        /// </summary>
        /// <param name="reference">
        ///     Optional – Use this parameter to filter for a settlement with a specific reference. The
        ///     reference is visible on your bank statement and in emails. An example reference would be 1182161.1506.02.
        /// </param>
        /// <param name="offset">Optional – The number of objects to skip.</param>
        /// <param name="count">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <returns>A list of settlements</returns>
        Task<ListResponse<SettlementResponse>> GetSettlementsListAsync(string reference = null, int? offset = null,
            int? count = null);

        /// <summary>
        ///     Retrieve all payments included in a settlement.
        /// </summary>
        /// <param name="settlementId">The id of the settlement</param>
        /// <param name="includeSettlements">For each payment, include the settlement this payment belongs to.</param>
        /// <param name="offset">Optional – The number of objects to skip.</param>
        /// <param name="count">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <returns>A list of payments for a specific settlement</returns>
        Task<ListResponse<PaymentResponse>> GetSettlementPaymentsListAsync(string settlementId, int? offset = null,
            int? count = null);

        /// <summary>
        ///     Retrieve all refunds for the given settlement.
        /// </summary>
        /// <param name="settlementId">The id of the settlement</param>
        /// <param name="offset">Optional – The number of objects to skip.</param>
        /// <param name="count">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <returns>A list of refunds for a specific settlement</returns>
        Task<ListResponse<RefundResponse>> GetSettlementRefundsListAsync(string settlementId, int? offset = null,
            int? count = null);

        /// <summary>
        ///     Retrieve all chargebacks for the given settlement.
        /// </summary>
        /// <param name="settlementId">The id of the settlement</param>
        /// <param name="offset">Optional – The number of objects to skip.</param>
        /// <param name="count">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <returns>A list of chargebacks for a specific settlement</returns>
        Task<ListResponse<ChargebackResponse>> GetSettlementChargebacksListAsync(string settlementId,
            int? offset = null, int? count = null);
    }
}