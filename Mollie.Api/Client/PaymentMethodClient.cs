using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.Api.Client {
    public class PaymentMethodClient : BaseMollieClient, IPaymentMethodClient {
        public PaymentMethodClient(string apiKey) : base (apiKey) { }

        public async Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(int? offset = null, int? count = null) {
            return await this.GetListAsync<ListResponse<PaymentMethodResponse>>("methods", offset, count).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod) {
            return await this.GetAsync<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}").ConfigureAwait(false);
        }
    }
}
