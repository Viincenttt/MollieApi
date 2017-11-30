using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

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

	    Task DeletePaymentAsync(string paymentId);

		Task<ListResponse<PaymentResponse>>
            GetPaymentListAsync(int? offset = null, int? count = null, string profileId = null, bool? testMode = null);
    }
}