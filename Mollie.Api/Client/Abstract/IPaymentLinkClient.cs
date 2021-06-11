using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentLinkClient {
        /*
            https://docs.mollie.com/reference/v2/payment-links-api/create-payment-link
            https://docs.mollie.com/reference/v2/payment-links-api/list-payment-links
            https://docs.mollie.com/reference/v2/payment-links-api/get-payment-link
         */
        Task<PaymentLinkResponse> CreatePaymentLinkAsync(PaymentLinkRequest paymentLinkRequest);

        /// <summary>
        ///	Retrieve a single payment link object by its token.
        /// </summary>
        /// <param name="paymentLinkId">The payment link's ID, for example pl_4Y0eZitmBnQ6IDoMqZQKh.</param>
        ///// <param name="testmode">Oauth - Optional – Set this to true to get a payment link made in test mode. If you omit this parameter, you can only retrieve live mode paymentLinks.</param>
        /// <returns></returns>
        Task<PaymentLinkResponse> GetPaymentLinkAsync(string paymentLinkId, bool testmode = false);
         
        /// <summary>
        /// Retrieve all payment links created with the current payment link profile, ordered from newest to oldest.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
		Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(string from = null, int? limit = null, string profileId = null, bool testmode = false);
        Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(UrlObjectLink<ListResponse<PaymentLinkResponse>> url);
        Task<PaymentLinkResponse> GetPaymentLinkAsync(UrlObjectLink<PaymentLinkResponse> url); 
    }
}