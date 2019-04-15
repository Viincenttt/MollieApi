using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;

using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentClient {
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest);

		/// <summary>
		///		Retrieve a single payment object by its payment identifier.
		/// </summary>
		/// <param name="paymentId">The payment's ID, for example tr_7UhSN1zuXS.</param>
		/// <param name="testmode">Oauth - Optional – Set this to true to get a payment made in test mode. If you omit this parameter, you can only retrieve live mode payments.</param>
		/// <returns></returns>
		Task<PaymentResponse> GetPaymentAsync(string paymentId, bool testmode = false);

        /// <summary>
        /// Some payment methods are cancellable for an amount of time, usually until the next day. Or as long as the payment status is open. Payments may be cancelled manually from the Dashboard, or automatically by using this endpoint.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
	    Task DeletePaymentAsync(string paymentId);

        /// <summary>
        /// Retrieve all payments created with the current payment profile, ordered from newest to oldest.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="limit"></param>
        /// <param name="profileId"></param>
        /// <param name="testMode"></param>
        /// <returns></returns>
		Task<ListResponse<PaymentResponse>> GetPaymentListAsync(string from = null, int? limit = null, string profileId = null, bool? testMode = null);

        Task<ListResponse<PaymentResponse>> GetPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url);
        Task<PaymentResponse> GetPaymentAsync(UrlObjectLink<PaymentResponse> url);        
    }
}