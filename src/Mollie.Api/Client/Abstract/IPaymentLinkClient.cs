using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment.Response;
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
        /// Update a payment link
        /// </summary>
        /// <param name="paymentLinkId">Provide the ID of the item you want to perform this operation on.</param>
        /// <param name="paymentLinkUpdateRequest">The request body</param>
        /// <param name="testmode">Most API credentials are specifically created for either live mode or test mode.
        /// In those cases the testmode query parameter can be omitted. For organization-level credentials such as
        /// OAuth access tokens, you can enable test mode by setting the testmode query parameter to true.</param>
        /// <returns>The updated payment link response</returns>
        Task<PaymentLinkResponse> UpdatePaymentLinkAsync(
            string paymentLinkId,
            PaymentLinkUpdateRequest paymentLinkUpdateRequest,
            bool testmode = false);

        /// <summary>
        /// Payment links for which no payments have been made yet can be deleted entirely. This can be useful for
        /// removing payment links that have been incorrectly configured or that are no longer relevant.
        /// </summary>
        /// <param name="paymentLinkId">Provide the ID of the item you want to perform this operation on.</param>
        /// <param name="testmode">Most API credentials are specifically created for either live mode or test mode.
        /// In those cases the testmode query parameter can be omitted. For organization-level credentials such as
        /// OAuth access tokens, you can enable test mode by setting the testmode query parameter to true.</param>
        /// <returns></returns>
        Task DeletePaymentLinkAsync(string paymentLinkId, bool testmode = false);

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

        /// <summary>
        /// Retrieve the list of payments for a specific payment link.
        /// </summary>
        /// <param name="paymentLinkId">Provide the ID of the item you want to perform this operation on.</param>
        /// <param name="from">Provide an ID to start the result set from the item with the given ID and onwards. This
        /// allows you to paginate the result set.</param>
        /// <param name="limit">The maximum number of items to return. Defaults to 50 items.</param>
        /// <param name="testmode">Most API credentials are specifically created for either live mode or test mode. In
        /// those cases the testmode query parameter can be omitted. For organization-level credentials such as OAuth access
        /// tokens, you can enable test mode by setting the testmode query parameter to true. Test entities cannot be
        /// retrieved when the endpoint is set to live mode, and vice versa.</param>
        /// <param name="sort">Used for setting the direction of the result set. Defaults to descending order, meaning
        /// the results are ordered from newest to oldest.</param>
        /// <returns></returns>
        Task<ListResponse<PaymentResponse>> GetPaymentLinkPaymentListAsync(
            string paymentLinkId, string? from = null, int? limit = null, bool testmode = false, SortDirection? sort = null);
    }
}
