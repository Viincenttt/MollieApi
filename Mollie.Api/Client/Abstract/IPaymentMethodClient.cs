using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentMethodClient {
		Task<PaymentMethodResponse> GetPaymentMethodAsync(string paymentMethod, bool includeIssuers = false, string locale = null, bool includePricing = false, string profileId = null, bool testmode = false, string currency = null);
        Task<ListResponse<PaymentMethodResponse>> GetAllPaymentMethodListAsync(string locale = null, Amount amount = null, bool includeIssuers = false, bool includePricing = false);
        Task<ListResponse<PaymentMethodResponse>> GetPaymentMethodListAsync(string sequenceType = null, string locale = null, Amount amount = null, bool includeIssuers = false, bool includePricing = false, string profileId = null, bool testmode = false, Resource? resource = null);
        Task<PaymentMethodResponse> GetPaymentMethodAsync(UrlObjectLink<PaymentMethodResponse> url);
    }
}