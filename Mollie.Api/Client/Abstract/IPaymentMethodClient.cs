using System.Threading.Tasks;

namespace Mollie.Api.Client.Abstract {
    using Models.PaymentMethod;
    using Models.Payment;
    using Models.List;
    public interface IPaymentMethodClient {
        Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod);
        Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(int? offset = default(int?), int? count = default(int?));
    }
}
