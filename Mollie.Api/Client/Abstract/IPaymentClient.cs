using System.Threading.Tasks;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentClient {
        Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest);
        Task<PaymentResponse> GetPaymentAsync(string paymentId);

        Task<ListResponse<PaymentResponse>>
            GetPaymentListAsync(int? offset = default(int?), int? count = default(int?));
    }
}