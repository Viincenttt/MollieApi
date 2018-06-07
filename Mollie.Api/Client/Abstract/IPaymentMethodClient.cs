using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentMethodClient {
        Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod);

        Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(string from = null, int? limit = null);
    }
}