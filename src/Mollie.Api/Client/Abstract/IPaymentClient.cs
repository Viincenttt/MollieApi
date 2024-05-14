using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentClient : IBaseMollieClient {
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest, bool includeQrCode = false);

        /// <summary>
        ///	Retrieve a single payment object by its payment identifier.
        /// </summary>
        /// <param name="paymentId">The payment's ID, for example tr_7UhSN1zuXS.</param>
        /// <param name="testmode">Oauth - Optional – Set this to true to get a payment made in test mode. If you omit
        /// this parameter, you can only retrieve live mode payments.</param>
        /// <param name="includeQrCode">Include a QR code object. Only available for iDEAL, Bancontact and bank transfer
        /// payments.</param>
        /// <param name="includeRemainderDetails">Include the Payment method-specific response parameters of the
        /// ‘remainder payment’ as well. This applies to gift card and voucher payments where only part of the payment
        /// was completed with gift cards or vouchers, and the remainder was completed with a regular payment method.
        /// </param>
        /// <param name="embedRefunds">Include all refunds created for the payment.</param>
        /// <param name="embedChargebacks"> Include all chargebacks issued for the payment.</param>
        /// <returns></returns>
        Task<PaymentResponse> GetPaymentAsync(
            string paymentId,
            bool testmode = false,
            bool includeQrCode = false,
            bool includeRemainderDetails = false,
            bool embedRefunds = false,
            bool embedChargebacks = false);

        /// <summary>
        /// Some payment methods are cancellable for an amount of time, usually until the next day. Or as long as the payment status is open. Payments may be cancelled manually from the Dashboard, or automatically by using this endpoint.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="testmode">Oauth - Optional – Set this to true to cancel a test mode payment.</param>
        /// <returns></returns>
        Task DeletePaymentAsync(string paymentId, bool testmode = false);

        /// <summary>
        /// Retrieve all payments created with the current payment profile, ordered from newest to oldest.
        /// </summary>
        /// <param name="from">Used for pagination. Offset the result set to the payment with this ID. The payment with
        /// this ID is included in the result set as well.</param>
        /// <param name="limit">The number of payments to return (with a maximum of 250).</param>
        /// <param name="profileId">The website profile’s unique identifier, for example pfl_3RkSN1zuPE. Omit this
        /// parameter to retrieve all payments across all profiles.</param>
        /// <param name="testmode">Set this to true to only retrieve payments made in test mode. By default, only live
        /// payments are returned.</param>
        /// <param name="includeQrCode">Include a QR code object for each payment that supports it. Only available for
        /// iDEAL, Bancontact and bank transfer payments.</param>
        /// <param name="embedRefunds">Include any refunds created for the payments.</param>
        /// <param name="embedChargebacks">Include any chargebacks issued for the payments.</param>
        /// <param name="sort">Used for setting the direction of the results based on the from parameter. Can be set
        /// to desc or asc. Default is desc.</param>
        /// <returns></returns>
		Task<ListResponse<PaymentResponse>> GetPaymentListAsync(
            string? from = null,
            int? limit = null,
            string? profileId = null,
            bool testmode = false,
            bool includeQrCode = false,
            bool embedRefunds = false,
            bool embedChargebacks = false,
            SortDirection? sort = null);

        /// <summary>
        /// Retrieve a list of payments by URL
        /// </summary>
        /// <param name="url">The URL from which to retrieve the payments</param>
        /// <returns></returns>
        Task<ListResponse<PaymentResponse>> GetPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url);

        /// <summary>
        /// Retrieve a single payment by URL
        /// </summary>
        /// <param name="url">The URL from which to retrieve the payment</param>
        /// <returns></returns>
        Task<PaymentResponse> GetPaymentAsync(UrlObjectLink<PaymentResponse> url);

        /// <summary>
        /// This endpoint can be used to update some details of a created payment.
        /// </summary>
        /// <param name="paymentId">The payment id to update</param>
        /// <param name="paymentUpdateRequest">The payment parameters to update</param>
        /// <returns></returns>
        Task<PaymentResponse> UpdatePaymentAsync(string paymentId, PaymentUpdateRequest paymentUpdateRequest);
    }
}
