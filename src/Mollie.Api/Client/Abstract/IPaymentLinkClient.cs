using System.Threading.Tasks;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentLinkClient : IBaseMollieClient {
        /// <summary>
        /// Create a new payment link
        /// </summary>
        /// <param name="paymentLinkRequest">The payment link request</param>
        /// <returns></returns>
        Task<PaymentLinkResponse> CreatePaymentLinkAsync(PaymentLinkRequest paymentLinkRequest);

        /// <summary>
        ///	Retrieve a single payment link object by its token.
        /// </summary>
        /// <param name="paymentLinkId">The payment link to retrieve</param>
        /// <param name="testmode">Oauth - Optional – Set this to true to get a payment links made in test mode. If you omit
        /// this parameter, you can only retrieve live mode payments.</param>
        Task<PaymentLinkResponse> GetPaymentLinkAsync(string paymentLinkId, bool testmode = false);

        /// <summary>
        /// Retrieve all payment links created with the current payment link profile, ordered from newest to oldest.
        /// </summary>
        /// <param name="from">Used for pagination. Offset the result set to the payment link with this ID. The payment
        /// link with this ID is included in the result set as well.</param>
        /// <param name="limit">The number of payment links to return (with a maximum of 250).</param>
        /// <param name="profileId">The website profile’s unique identifier, for example pfl_3RkSN1zuPE. Omit this
        /// parameter to retrieve the payment links of all profiles of the current organization.</param>
        /// <param name="testmode">Set this to true to only retrieve payment links made in test mode. By default, only
        /// live payment links are returned.</param>
        /// <returns></returns>
		Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(
            string? from = null, int? limit = null, string? profileId = null, bool testmode = false);

        /// <summary>
        /// Retrieve a list of payment links by URL
        /// </summary>
        /// <param name="url">The URL from which to retrieve the payment links</param>
        /// <returns></returns>
        Task<ListResponse<PaymentLinkResponse>> GetPaymentLinkListAsync(UrlObjectLink<ListResponse<PaymentLinkResponse>> url);

        /// <summary>
        /// Retrieve a single payment link by URL
        /// </summary>
        /// <param name="url">The URL from which to retrieve the payment link</param>
        /// <returns></returns>
        Task<PaymentLinkResponse> GetPaymentLinkAsync(UrlObjectLink<PaymentLinkResponse> url);
    }
}
