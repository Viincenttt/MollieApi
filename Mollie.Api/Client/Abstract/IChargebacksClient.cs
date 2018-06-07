using System.Threading.Tasks;
using Mollie.Api.Models.Chargeback;
using Mollie.Api.Models.List;

namespace Mollie.Api.Client.Abstract {
    public interface IChargebacksClient {
        /// <summary>
        ///     Retrieve a single chargeback by its ID. Note the original payment's ID is needed as well.
        /// </summary>
        /// <param name="paymentId">The payment Id</param>
        /// <param name="chargebackId">The chargeback Id</param>
        /// <returns></returns>
        Task<ChargebackResponse> GetChargebackAsync(string paymentId, string chargebackId);

        /// <summary>
        ///     Retrieve all chargebacks for the given payment.
        /// </summary>
        /// <param name="paymentId">The payment Id</param>
        /// <param name="from">Optional – The number of objects to skip.</param>
        /// <param name="limit">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <returns></returns>
        Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string paymentId, string from = null, int? limit = null);

        /// <summary>
        ///     Retrieve all chargebacks for current payment profile (API keys) or the organization (OAuth). Listing the
        ///     chargebacks can be done without knowing anything about the payments.
        /// </summary>
        /// <param name="from">Optional – The number of objects to skip.</param>
        /// <param name="limit">Optional – The number of objects to return (with a maximum of 250).</param>
        /// <param name="oathProfileId">Optional – The payment profile's unique identifier, for example pfl_3RkSN1zuPE.</param>
        /// <param name="oauthTestmode">Optional – Set this to true to only consider chargebacks made in testmode.</param>
        /// <returns></returns>
        Task<ListResponse<ChargebackResponse>> GetChargebacksListAsync(string from = null, int? limit = null, string oathProfileId = null, bool? oauthTestmode = null);
    }
}