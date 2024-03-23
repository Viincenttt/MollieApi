using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentClient : IBaseMollieClient {
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest, bool includeQrCode = false);

        /// <summary>
        ///		Retrieve a single payment object by its payment identifier.
        /// </summary>
        /// <param name="paymentId">The payment's ID, for example tr_7UhSN1zuXS.</param>
        /// <param name="testmode">Oauth - Optional – Set this to true to get a payment made in test mode. If you omit this parameter, you can only retrieve live mode payments.</param>
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
        /// <returns></returns>
        Task DeletePaymentAsync(string paymentId, bool testmode = false);

        /// <summary>
        /// Retrieve all payments created with the current payment profile, ordered from newest to oldest.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="limit"></param>
        /// <param name="profileId"></param>
        /// <param name="testmode"></param>
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
        
        Task<ListResponse<PaymentResponse>> GetPaymentListAsync(UrlObjectLink<ListResponse<PaymentResponse>> url);
        
        Task<PaymentResponse> GetPaymentAsync(UrlObjectLink<PaymentResponse> url);
        
        Task<PaymentResponse> UpdatePaymentAsync(string paymentId, PaymentUpdateRequest paymentUpdateRequest);
    }
}