using System.Collections.Generic;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.Api.Client {
    public class PaymentMethodClient : BaseMollieClient, IPaymentMethodClient {
        public PaymentMethodClient(string apiKey) : base(apiKey) {
        }

        public async Task<ListResponse<PaymentMethodListData>> GetPaymentMethodListAsync(SequenceType? sequenceType = null, string locale = null, Amount amount = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                {nameof(sequenceType).ToLower(), sequenceType.ToString().ToLower()},
                {nameof(locale), locale},
                {"amount[value]", amount?.Value},
                {"amount[currency]", amount?.Currency}
            };

            return await this.GetListAsync<ListResponse<PaymentMethodListData>>("methods", null, null, parameters).ConfigureAwait(false);
        }

        public async Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod, string locale = null) {
            Dictionary<string, string> parameters = new Dictionary<string, string>() {
                {nameof(locale), locale}
            };
            string queryString = parameters.ToQueryString();

            return await this.GetAsync<PaymentMethodResponse>($"methods/{paymentMethod.ToString().ToLower()}{queryString}").ConfigureAwait(false);
        }
    }
}