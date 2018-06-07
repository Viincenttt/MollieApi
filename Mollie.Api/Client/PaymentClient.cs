using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Client {
    public class PaymentClient : BaseMollieClient, IPaymentClient {

	    public PaymentClient(string apiKey) : base(apiKey) { }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest) {

            if (!string.IsNullOrWhiteSpace(paymentRequest.ProfileId) || paymentRequest.Testmode.HasValue || paymentRequest.ApplicationFee != null) {
                this.ValidateApiKeyIsOauthAccesstoken();
            }

            return await this.PostAsync<PaymentResponse>("payments", paymentRequest).ConfigureAwait(false);
        }

	    public async Task<PaymentResponse> GetPaymentAsync(string paymentId, bool testmode = false) {
	        if (testmode) {
	            this.ValidateApiKeyIsOauthAccesstoken();
            }

		    var testmodeParameter = testmode ? "?testmode=true" : string.Empty;

			return await this.GetAsync<PaymentResponse>($"payments/{paymentId}{testmodeParameter}").ConfigureAwait(false);
		}

		public async Task DeletePaymentAsync(string paymentId) {
		    await this.DeleteAsync($"payments/{paymentId}").ConfigureAwait(false);
		}

	    public async Task<ListResponse<PaymentListData>> GetPaymentListAsync(string from = null, int? limit = null, string profileId = null, bool? testMode = null) {
	        if (!string.IsNullOrWhiteSpace(profileId) || testMode.HasValue) {
	            this.ValidateApiKeyIsOauthAccesstoken();
            }

		    var parameters = new Dictionary<string, string>();

	        if (!string.IsNullOrWhiteSpace(profileId)) {
	            parameters.Add("profileId", profileId);
            }

	        if (testMode.HasValue) {
	            parameters.Add("testmode", testMode.Value.ToString().ToLower());
            }

			return await this.GetListAsync<ListResponse<PaymentListData>>($"payments", from, limit, parameters)
				.ConfigureAwait(false);
		}
    }
}