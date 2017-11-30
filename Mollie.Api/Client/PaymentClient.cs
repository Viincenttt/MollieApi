using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Client {
    public class PaymentClient : BaseMollieClient, IPaymentClient {

	    public PaymentClient(string apiKey) : base(apiKey) { }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest) {

			if (!string.IsNullOrWhiteSpace(paymentRequest.ProfileId) || paymentRequest.Testmode.HasValue || paymentRequest.ApplicationFee != null)
				ValidateApiKeyIsOauthAccesstoken();

            return await PostAsync<PaymentResponse>("payments", paymentRequest).ConfigureAwait(false);
        }

	    public async Task<PaymentResponse> GetPaymentAsync(string paymentId, bool testmode = false)
	    {
		    if (testmode)
			    ValidateApiKeyIsOauthAccesstoken();

		    var testmodeParameter = testmode ? "?testmode=true" : string.Empty;

			return await GetAsync<PaymentResponse>($"payments/{paymentId}{testmodeParameter}").ConfigureAwait(false);
		}

		public async Task DeletePaymentAsync(string paymentId)
	    {
		    await DeleteAsync($"payments/{paymentId}").ConfigureAwait(false);
		}

	    public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(int? offset = null, int? count = null, string profileId = null, bool? testMode = null)
	    {
			if (!string.IsNullOrWhiteSpace(profileId) || testMode.HasValue)
				ValidateApiKeyIsOauthAccesstoken();

		    var parameters = new Dictionary<string, string>();
			
			if (!string.IsNullOrWhiteSpace(profileId))
				parameters.Add("profileId", profileId);

		    if (testMode.HasValue)
			    parameters.Add("testmode", testMode.Value.ToString().ToLower());

			return await GetListAsync<ListResponse<PaymentResponse>>($"payments{parameters.ToQueryString()}", offset, count)
				.ConfigureAwait(false);
		}
    }
}