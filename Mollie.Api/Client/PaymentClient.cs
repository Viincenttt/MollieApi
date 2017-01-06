using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Client {
    public class PaymentClient : BaseMollieClient, IPaymentClient {
        public PaymentClient(string apiKey) : base (apiKey) { }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest) {
            return await this.PostAsync<PaymentResponse>("payments", paymentRequest).ConfigureAwait(false);
        }

        public async Task<ListResponse<PaymentResponse>> GetPaymentListAsync(int? offset = null, int? count = null) {
            return await this.GetListAsync<ListResponse<PaymentResponse>>("payments", offset, count).ConfigureAwait(false);
        }

        public async Task<PaymentResponse> GetPaymentAsync(string paymentId) {
            return await this.GetAsync<PaymentResponse>($"payments/{paymentId}").ConfigureAwait(false);
        }
    }
}
