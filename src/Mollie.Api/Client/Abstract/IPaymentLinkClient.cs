using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentLinkClient : IBaseMollieClient {
        Task<PaymentLinkResponse> CreatePaymentLinkAsync(PaymentLinkRequest paymentLinkRequest);

        /// <summary>
        ///	Retrieve a single payment link object by its token.
        /// </summary>
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