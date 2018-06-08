using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentMethodClient {
        Task<PaymentMethodResponse> GetPaymentMethodAsync(PaymentMethod paymentMethod, string locale = null);
        Task<ListResponse<PaymentMethodListData>> GetPaymentMethodListAsync(SequenceType? sequenceType = null, string locale = null, Amount amount = null);
    }
}