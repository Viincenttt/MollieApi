using System.Threading.Tasks;

namespace Mollie.Api.Client.Abstract {
    using Models.Payment;
    using Models.Payment.Request;
    using Models.Payment.Response;
    using Models.List;
    public interface IPaymentClient {
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest);
        Task<PaymentResponse> GetPaymentAsync(string paymentId);
        Task<ListResponse<PaymentResponse>> GetPaymentListAsync(int? offset = default(int?), int? count = default(int?));
    }
}
