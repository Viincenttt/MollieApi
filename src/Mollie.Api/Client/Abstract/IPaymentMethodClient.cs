using System.Threading;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Url;

namespace Mollie.Api.Client.Abstract {
    public interface IPaymentMethodClient : IBaseMollieClient {
        Task<MollieResult<PaymentMethodResponse>> GetPaymentMethodAsync(
            string paymentMethod,
            bool includeIssuers = false,
            string? locale = null,
            string? profileId = null,
            bool testmode = false,
            string? currency = null,
            CancellationToken cancellationToken = default);

        Task<MollieResult<ListResponse<PaymentMethodResponse>>> GetAllPaymentMethodListAsync(
            string? locale = null,
            Amount? amount = null,
            bool includeIssuers = false,
            bool includePricing = false,
            string? profileId = null,
            bool testmode = false,
            CancellationToken cancellationToken = default);

        Task<MollieResult<ListResponse<PaymentMethodResponse>>> GetPaymentMethodListAsync(
            string? sequenceType = null,
            string? locale = null,
            Amount? amount = null,
            bool includeIssuers = false,
            string? profileId = null,
            bool testmode = false,
            Resource? resource = null,
            string? billingCountry = null,
            string? includeWallets = null,
            CancellationToken cancellationToken = default);

        Task<MollieResult<PaymentMethodResponse>> GetPaymentMethodAsync(UrlObjectLink<PaymentMethodResponse> url,
            CancellationToken cancellationToken = default);
    }
}
